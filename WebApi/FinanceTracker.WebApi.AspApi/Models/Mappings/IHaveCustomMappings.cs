namespace FinanceTracker.WebApi.AspApi.Models.Mappings
{
    using AutoMapper;

    interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration config);
    }
}
