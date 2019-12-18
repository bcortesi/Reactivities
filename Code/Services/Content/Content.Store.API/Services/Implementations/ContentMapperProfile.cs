using AutoMapper;
using Content.Models;
using Content.Store.API.Models;

namespace Content.Store.API.Services.Implementations
{
    public class ContentMapperProfile : Profile
    {
        public ContentMapperProfile()
        {
            CreateMap<ContentDBModel, ContentModel>().ReverseMap();
        }
    }
}
