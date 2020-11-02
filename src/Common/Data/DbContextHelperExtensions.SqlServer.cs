using System;
using Microsoft.EntityFrameworkCore;

namespace Common.Data
{
    public static partial class DbContextHelperExtensions
    {
        public static DbContextHelper AddSupportSqlServer(this DbContextHelper helper, Action<DbContextOptionsBuilder, string> setupAction = null)
        {
            setupAction ??= (builder, connString) => { builder.UseSqlServer(connString); };
            helper.AddSupport(DbContextSetupAction.Create("SqlServer", setupAction));
            return helper;
        }
    }
}
