﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Identity.Contexts
{
    public class StoreIdentityDbContext : IdentityDbContext<AppUser>
    {

        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) :base(options)
        {
            
        }

    }
}