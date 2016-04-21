using AutoMapper;
using AutoMapper.Mappers;

namespace BraveNewWorld.Web
{
    public static class MappingRegistrations
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<CollectionProfile>();
                x.AddProfile<CustomerProfile>();
            });
        }
    }
}