namespace FinanceTracker.WebApi.AspApi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Data.DbModels.Transactions;
    using Mappings;

    public class TransactionBindingModel : IMapFrom<Transaction>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Range(0, Double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(2)]
        public string Category { get; set; }

        public DateTime DateTime { get; set; }

        public TransactionType Type { get; set; }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Transaction, TransactionBindingModel>()
                .ForMember(tran => tran.Category, map => map
                    .MapFrom(tran => tran.Category.Name));
        }
    }
}