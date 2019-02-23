using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserProfileManager.Data.DbContexts;

namespace UserProfileManager.Data.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext DbContext = null;

        #region Repositories

        public IUserProfileRepository UserProfileRepository { get; private set; }

        #endregion

        public UnitOfWork(
                ApplicationDbContext dbContext,
                IUserProfileRepository userProfileRepository
            )
        {
            this.DbContext = dbContext;
            this.UserProfileRepository = userProfileRepository;
        }

        public void Commit()
        {
            this.DbContext.SaveChanges();
        }

        public void Reject()
        {
            foreach (var entry in this.DbContext.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Detached:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
