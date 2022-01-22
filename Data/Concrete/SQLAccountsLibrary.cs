using Ensek.Data.Abstract;
using Ensek.Models;
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

        public bool IsValidAccountId(MeterReading mr)
        {
            int cntAccountId = _db.Accounts.Count(a => a.AccountId == mr.AccountId);

            return (cntAccountId > 0);
        }
    }
}