﻿using AutoMapper;
using grandelivery.WebUI.Models;
using grandelivery.WebUI.ViewModels;
using SX.WebCore.ViewModels;

namespace grandelivery.WebUI
{
    public class AutoMapperConfig
    {
        public static void Register(IMapperConfigurationExpression cfg)
        {
            //order
            cfg.CreateMap<Order, VMOrder>();
            cfg.CreateMap<VMOrder, Order>();

            //register model
            cfg.CreateMap<VMRegister, SxVMRegister>();
            cfg.CreateMap<SxVMRegister, VMRegister>();

            ////article
            //cfg.CreateMap<Article, VMArticle>();
            //cfg.CreateMap<VMArticle, Article>();
        }
    }
}