using Ensek.Data.Abstract;
using Ensek.Models;
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

        public bool HasOlderReading(MeterReading mr)
        {
            int cntOlder = _db.MeterReadings.Count(a => a.AccountId == mr.AccountId && a.MeterReadingDateTime >= mr.MeterReadingDateTime);

            return (cntOlder > 0);
        }

        public void InsertReading(MeterReading mr)
        {
            _db.MeterReadings.Add(mr);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
