using Ensek.Data.Abstract;
using Ensek.Models;
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

        public void InsertReading(string AccountId, string MeterReadingDateTime, string MeterReadValue)
        {
            int iAccountId = int.Parse(AccountId);
            DateTime dtMeterReadingDateTime = DateTime.Parse(MeterReadingDateTime);
            int iMeterReadValue = int.Parse(MeterReadValue);

            _db.MeterReadings.Add(new MeterReading()
            {
                AccountId = iAccountId,
                MeterReadingDateTime = dtMeterReadingDateTime,
                MeterReadValue = iMeterReadValue
            });
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
