using System;
using System.Transactions;

using NUnit.Framework;

namespace YatORM.Tests.Attributes
{
    public class RollbackAttribute : Attribute, ITestAction
    {
        private TransactionScope _transaction;

        public void BeforeTest(TestDetails testDetails)
        {
            this._transaction = new TransactionScope();
        }

        public void AfterTest(TestDetails testDetails)
        {
            this._transaction.Dispose();
        }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }
    }
}