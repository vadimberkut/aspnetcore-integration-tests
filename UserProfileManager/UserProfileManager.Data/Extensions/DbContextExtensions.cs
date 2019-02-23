using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Data.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Checks if all migrations were applied to DB
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsAllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Count;

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Count;

            return applied == total;
        }
    }
}
