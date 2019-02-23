using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Data.Repositories.Base
{
    public interface IUnitOfWork
    {
        IUserProfileRepository UserProfileRepository { get; }

        /// <summary>
        /// Commits all changes
        /// </summary>
        void Commit();

        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void Reject();
    }
}
