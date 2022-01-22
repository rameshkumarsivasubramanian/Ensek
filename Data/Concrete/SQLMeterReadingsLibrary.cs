using Ensek.Data.Abstract;
using System;
using System.Linq;

namespace Ensek.Data.Concrete
{
    public class SQLMeterReadingsLibrary : IMeterReadingsLibrary
    {
        private readonly EnsekData _db;
        public SQLMeterReadingsLibrary(EnsekData db)
        {
            _db = db;
        }

        public bool HasOlderReading(string AccountId, string MeterReadingDateTime)
        {
            int iAccountId = int.Parse(AccountId);
            DateTime dtMeterReadingDateTime = DateTime.Parse(MeterReadingDateTime);
            int cntOlder = _db.MeterReadings.Count(a => a.AccountId == iAccountId && a.MeterReadingDateTime > dtMeterReadingDateTime);

            return (cntOlder > 0);
        }
    }
}
