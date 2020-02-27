using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WissAppEntities.Entities;
using WissAppWebApi.Models;

namespace WissAppWebApi
{
    public class MappingConfig
    {
        public static readonly MapperConfiguration mapperConfiguration;

        static MappingConfig()
        {
            mapperConfiguration = new MapperConfiguration(c =>
            {
                c.AddProfile<UsersProfile>();
                c.AddProfile<UsersModelProfile>();
                c.AddProfile<RolesProfile>();
                c.AddProfile<UsersMessagesProfile>();
            });
        }
    }

    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<Users, UsersModel>()
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.Role, o => o.MapFrom(s => s.Roles.Name));
        }
    }

    public class UsersModelProfile : Profile
    {
        public UsersModelProfile()
        {
            CreateMap<UsersModel, Users>();
        }
    }

    public class RolesProfile : Profile
    {
        public RolesProfile()
        {
            CreateMap<Roles, RolesModel>().ReverseMap();
        }
    }

    public class UsersMessagesProfile : Profile
    {
        public UsersMessagesProfile()
        {
            CreateMap<UsersMessages, UsersMessagesModel>()
                .ForMember(d => d.Message, o => o.MapFrom(s => s.Messages.Message))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Messages.Date))
                .ForMember(d => d.Sender, o => o.MapFrom(s => s.Senders.UserName))
                .ForMember(d => d.Receiver, o => o.MapFrom(s => s.Receivers.UserName));
        }
    }
}