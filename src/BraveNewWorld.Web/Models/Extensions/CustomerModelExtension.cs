using AutoMapper;
using BraveNewWorld.Dal;

namespace BraveNewWorld.Web.Models.Extensions
{
    public static class CustomerModelExtension
    {
        public static Customer ModelToEntity(this CustomerModel model)
        {
            return Mapper.Map<Customer>(model);
        }
    }
}