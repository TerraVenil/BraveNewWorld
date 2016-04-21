using AutoMapper;
using AutoMapper.EquivilencyExpression;
using BraveNewWorld.Dal;
using BraveNewWorld.Web.Models;

namespace BraveNewWorld.Web
{
    public class CustomerProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "CustomerProfile";
            }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<OrderModel, Order>()
                  .EqualityComparision((src, dest) => src.OrderID == dest.OrderID)
                  .ForMember(dest => dest.Customer, src => src.Ignore())
                  .ForMember(dest => dest.Employee, src => src.Ignore())
                  .ForMember(dest => dest.Order_Details, src => src.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}