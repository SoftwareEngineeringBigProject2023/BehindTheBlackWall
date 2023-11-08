using Microsoft.EntityFrameworkCore;
using SEServer.Statements.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.EntityFrameworkCore
{
    internal static class ModelBuilderExtensions
    {
        public static ModelBuilder AddUser(this ModelBuilder builder)
        {
            var user = builder.Entity<Player>();
            user.HasKey(entity => entity.Id);
            user.Property(entity => entity.UserName).HasMaxLength(20);
            user.Property(entity => entity.Password).HasMaxLength(30);
            user.Property(entity => entity.ImagePath).HasMaxLength(30);
            return builder;
        }
    }
}
