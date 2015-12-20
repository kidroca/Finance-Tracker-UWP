namespace FinanceTracker.WebApi.AspApi.Controllers
{
    using System;
    using System.Linq;
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

        public IHttpActionResult Get(string category = null, int page = 1, int size = 10)
        {
            if (page <= 0 || size <= 0)
            {
                return this.BadRequest("Page and Size must be greater than 0");
            }

            IQueryable<Transaction> transactions = this.transactionsRepo.All()
                .Where(t => t.IsDeleted == false
                            && t.BalanceId == this.UserId);

            if (!string.IsNullOrEmpty(category.Trim()))
            {
                category = base.FormatCategoryName(category);
                transactions.Where(t => t.Category.Name == category);
            }

            var filtered = transactions
                .Skip((page - 1) * size)
                .Take(size)
                .ProjectTo<TransactionBindingModel>()
                .ToList();

            return this.Ok(filtered);
        }

        public IHttpActionResult Post(TransactionBindingModel transaction)
        {
            var newTransaction = new Transaction
            {
                Amount = transaction.Amount,
                Balance = this.balancesRepo.GetById(this.UserId),
                DateTime = transaction.DateTime,
                Type = transaction.Type,
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

        public IHttpActionResult Delete(int id, bool revertTransaction = true)
        {
            var transaction = this.transactionsRepo.GetById(id);
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