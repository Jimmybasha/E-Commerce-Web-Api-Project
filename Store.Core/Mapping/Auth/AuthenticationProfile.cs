using AutoMapper;
using Store.Core.Dtos.Auth;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Auth
{
    public class AuthenticationProfile :Profile
    {

        public AuthenticationProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }


    }
}
