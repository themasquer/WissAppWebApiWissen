using System;
using System.Transactions;

namespace AppCore.Transactions
{
    // Source: https://www.codeproject.com/Articles/690136/All-About-TransactionScope
    // NOT: Projede Entity'ler için ReadUncommited ve Required, Log'lar için ise ReadUncommited ve Suppress kullanacağız!
    public class Transaction
    {
        private TransactionScope transactionScope;
        // Supporting transactions from a code block is the main responsibility of this class. 
        //  We often use this class for managing local as well as distributed transactions from our code.

        private TransactionOptions transactionOptions;
        // Contains isolation level and timeout values.

        private IsolationLevel isolationLevel;
        // It defines the locking mechanism and policy to read data inside another transaction.
        /*
            Serializable: Highest level of isolation. It locks data exclusively when read and write occurs. It acquires range locks so that phantom rows are not created.
            Repeatable Read: Second highest level of isolation. Same as serializable except it does not acquire range locks so phantom rows may be created.
            Read Committed: It allow shared locks and read only committed data. That means never read changed data that are in the middle of any transaction.
            Read Un-Committed: It is the lowest level of Isolation. It allows dirty read. 
         */
        /*
            Dirty Read: One transaction reads changed data of anohter tranaction but that data is still not committed. 
               You may take decission/action based on that data. A problem will arise when data is rolled-back later. 
               If rollback happens then your decision/action will be wrong and it produces a bug in your application.
             Non Repeatable Read: A transaction reads the same data from same table multiple times. A problem will arise when for each read, data is different.
             Phantom Read: Suppose a transaction will read a table first and it finds 100 rows. 
               A problem will arise when the same tranaction goes for another read and it finds 101 rows. The extra row is called a phantom row. 
        */

        private TimeSpan defaultTimeout;
        // How much time object will wait for a transaction to be completed. Can be set in web.config.

        private TimeSpan maximumTimeout;
        // How much maximum time object will wait for a transaction to be completed. Can be set in web.config.

        private TransactionScopeOption transactionScopeOption;
        // It is an enumeration. There are three options available in this enumeration:
        /*
            Required: It is default value for TransactionScope. If any already exists any transaction then it will join with that transaciton otherwise create new one.
            RequiredNew: When select this option a new transaction is always created. This transaction is independent with its outer transaction.
            Suppress: When select this option, no transaction will be created. Even if it already.
        */

        public Transaction()
        {
            isolationLevel = IsolationLevel.ReadCommitted;
            defaultTimeout = TransactionManager.DefaultTimeout; // 1 minute
            maximumTimeout = TransactionManager.MaximumTimeout;
            transactionScopeOption = TransactionScopeOption.Required;
            transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = isolationLevel;
            transactionOptions.Timeout = defaultTimeout;
            transactionScope = new TransactionScope(transactionScopeOption, transactionOptions);
        }

        public Transaction(int timeOutSeconds, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            defaultTimeout = TimeSpan.FromSeconds(timeOutSeconds);
            maximumTimeout = TransactionManager.MaximumTimeout;
            this.isolationLevel = isolationLevel;
            this.transactionScopeOption = transactionScopeOption;
            transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = isolationLevel;
            transactionOptions.Timeout = defaultTimeout;
            transactionScope = new TransactionScope(transactionScopeOption, transactionOptions);
        }

        public Transaction(bool log, int timeOutSeconds = 60)
        {
            defaultTimeout = TimeSpan.FromSeconds(timeOutSeconds);
            maximumTimeout = TransactionManager.MaximumTimeout;
            isolationLevel = IsolationLevel.ReadCommitted;
            transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = isolationLevel;
            transactionOptions.Timeout = defaultTimeout;
            if (!log)
                transactionScopeOption = TransactionScopeOption.Required;
            else
                transactionScopeOption = TransactionScopeOption.Suppress;
            transactionScope = new TransactionScope(transactionScopeOption, transactionOptions);
        }

        public void Complete()
        {
            // If this method is not called, the transaction will automatically be rolled back.
            transactionScope.Complete();
            Dispose();
        }

        public void Dispose()
        {
            // If an explicit rollback needs to be called, this method can be called.
            transactionScope.Dispose();
        }
    }
}
