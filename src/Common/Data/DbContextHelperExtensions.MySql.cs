using System;
using Microsoft.EntityFrameworkCore;

namespace Common.Data
{
    public static partial class DbContextHelperExtensions
    {
        public static DbContextHelper AddSupportMySql(this DbContextHelper helper, Action<DbContextOptionsBuilder, string> setupAction = null)
        {
            setupAction ??= (builder, connString) => { builder.UseMySQL(connString); };
            helper.AddSupport(DbContextSetupAction.Create("MySql", setupAction));
            return helper;
        }
    }
}
