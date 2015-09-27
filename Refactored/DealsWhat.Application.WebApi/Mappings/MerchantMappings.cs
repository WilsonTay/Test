using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Model;

namespace DealsWhat.Application.WebApi.Mappings
{
    public class MerchantMappings
    {
        public static void CreateMerchantInfoMapping()
        {
            AutoMapper.Mapper.CreateMap<MerchantModel, MerchantInfoViewModel>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.BusinessRegistrationNumber, opt => opt.MapFrom(src => src.BusinessRegNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About));
        }
    }
}