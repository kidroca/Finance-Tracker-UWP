namespace FinanceTracker.WebApi.AspApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AutoMapper.QueryableExtensions;
    using Data.DbModels;
    using Data.DbModels.Transactions;
    using Data.Handlers.Repositories;
    using Models;

    [Authorize]
    public class TransactionsController : Common
    {
        private readonly IRepository<Transaction> transactionsRepo;
        private readonly IRepository<Balance> balancesRepo;
        private readonly IRepository<Category> categoriesRepo;

        public TransactionsController(
            IRepository<Transaction> transactionsRepository,
            IRepository<Balance> balancesRepository,
            IRepository<Category> categoriesRepository)
        {
            this.transactionsRepo = transactionsRepository;
            this.balancesRepo = balancesRepository;
            this.categoriesRepo = categoriesRepository;
        }

        public async Task<IHttpActionResult> Get(int page, int size = 10, string category = null)
        {
            if (page <= 0 || size <= 0)
            {
                return this.BadRequest("Page and Size must be greater than 0");
            }

            IQueryable<Transaction> transactions = this.transactionsRepo.All()
                .Where(t => t.IsDeleted == false
                            && t.BalanceId == this.UserId);

            if (!string.IsNullOrEmpty(category))
            {
                category = base.FormatCategoryName(category);
                transactions = transactions.Where(t => t.Category.Name == category);
            }

            var filtered = await transactions
                .OrderByDescending(t => t.DateTime)
                .Skip((page - 1) * size)
                .Take(size)
                .ProjectTo<TransactionBindingModel>()
                .ToListAsync();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            responseMessage.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            responseMessage.Content = new ObjectContent(
                typeof(List<TransactionBindingModel>), filtered, new JsonMediaTypeFormatter());

            var response = this.ResponseMessage(responseMessage);
            return (response);
        }

        // Todo Check for api endpoint problems because method is overloaded
        public async Task<IHttpActionResult> Get(DateRanageModel range, string category = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var transactions = this.transactionsRepo.All()
                .Where(t => range.FromDate <= t.DateTime && t.DateTime <= range.ToDate);

            if (category != null)
            {
                category = base.FormatCategoryName(category);
                transactions = transactions.Where(t => t.Category.Name == category);
            }

            var filtered = await transactions
                .OrderByDescending(t => t.DateTime)
                .ProjectTo<TransactionBindingModel>()
                .ToListAsync();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            responseMessage.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            responseMessage.Content = new ObjectContent(
                typeof(List<TransactionBindingModel>), filtered, new JsonMediaTypeFormatter());

            var response = this.ResponseMessage(responseMessage);
            return (response);
        }

        public IHttpActionResult Post(TransactionBindingModel transaction)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newTransaction = new Transaction
            {
                Amount = transaction.Amount,
                Balance = this.balancesRepo.GetById(this.UserId),
                DateTime = transaction.DateTime,
                Type = transaction.Type
            };

            var dbCategory = this.categoriesRepo.All()
                .FirstOrDefault(c => c.Name == transaction.Category);

            if (dbCategory == null)
            {
                newTransaction.Category = new Category { Name = transaction.Category };
            }
            else
            {
                newTransaction.Category = dbCategory;
            }

            decimal balance = this.ApplyTransactionToBalance(newTransaction);

            try
            {
                this.transactionsRepo.Add(newTransaction)
                    .SaveChanges();
            }
            catch (Exception)
            {
                this.RevertTransactionFromBalance(newTransaction);
                throw;
            }

            return this.Created(
                $"api/Transactions/{newTransaction.Id}",
                $"Balance: {balance}");
        }

        public async Task<IHttpActionResult> Put(TransactionBindingModel transaction)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var modifiedTransaction = this.transactionsRepo.GetById(transaction.Id);
            if (modifiedTransaction == null)
            {
                return this.NotFound();
            }

            var responseDelete = this.Delete(modifiedTransaction.Id);
            var content = await responseDelete
                .ExecuteAsync(CancellationToken.None);

            if (!content.IsSuccessStatusCode)
            {
                return responseDelete;
            }

            return this.Post(transaction);
        }

        public IHttpActionResult Delete(int id, bool revertTransaction = true)
        {
            var transaction = this.transactionsRepo.GetById(id);
            if (transaction == null)
            {
                return this.NotFound();
            }

            if (transaction.BalanceId != this.UserId)
            {
                return this.NotFound();
            }

            IHttpActionResult result;
            if (revertTransaction)
            {
                decimal balance = this.RevertTransactionFromBalance(transaction);
                result = this.Ok($"Balance: {balance}");
            }
            else
            {
                result = this.Ok("Transaction deleted.");
            }

            try
            {
                transaction.IsDeleted = true;
                this.transactionsRepo.Update(transaction)
                    .SaveChanges();
            }
            catch (Exception)
            {
                this.ApplyTransactionToBalance(transaction);
                throw;
            }

            return result;
        }

        public IHttpActionResult Delete(string category, bool revertTransactions = true)
        {
            category = base.FormatCategoryName(category);

            var transactions = this.transactionsRepo.All()
                .Where(t => t.IsDeleted == false
                            && t.BalanceId == this.UserId
                            && t.Category.Name == category)
                .ToList();

            foreach (Transaction t in transactions)
            {
                t.IsDeleted = true;
                this.RevertTransactionFromBalance(t);
                this.transactionsRepo.Update(t);
            }

            try
            {
                this.transactionsRepo.SaveChanges();
            }
            catch (Exception)
            {
                foreach (Transaction t in transactions)
                {
                    t.IsDeleted = false;
                    this.ApplyTransactionToBalance(t);
                }

                throw;
            }

            return this.Ok("All transaction deleted successfully.");
        }

        private decimal RevertTransactionFromBalance(Transaction transaction)
        {
            decimal amount = transaction.Amount;

            if (transaction.Type == TransactionType.Deposit)
            {
                amount *= -1;
            }

            var balance = transaction.Balance;
            balance.CurrentAmount += amount;

            return balance.CurrentAmount;
        }

        private decimal ApplyTransactionToBalance(Transaction transaction)
        {
            decimal amount = transaction.Amount;

            if (transaction.Type == TransactionType.Withdraw)
            {
                amount *= -1;
            }

            var balance = this.balancesRepo.GetById(this.UserId);
            balance.CurrentAmount += amount;

            return balance.CurrentAmount;
        }
    }
}