using Ensek.Data.Abstract;
using System.Linq;

namespace Ensek.Data.Concrete
{
    public class SQLAccountsLibrary : IAccountsLibrary
    {
        private readonly EnsekData _db;
        public SQLAccountsLibrary(EnsekData db)
        {
            _db = db;
        }

        public bool IsValidAccountId(string AccountId)
        {
            int iAccountId = int.Parse(AccountId);
            int cntAccountId = _db.Accounts.Count(a => a.AccountId == iAccountId);

            return (cntAccountId > 0);
        }
    }
}