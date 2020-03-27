using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Library.DataRepository
{
    public interface IInternalTransaction : ITransaction
    {
        void BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void RollbackTransaction();
    }
}
