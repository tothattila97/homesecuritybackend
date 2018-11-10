using HomeSecurity.Bll.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurity.Bll.Database
{
    public class HomeSecurityDbContext: IdentityDbContext<User, IdentityRole<int>, int>
    {
        public HomeSecurityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
        }
    }
}
