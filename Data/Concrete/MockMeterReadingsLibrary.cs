using Ensek.Data.Abstract;
using Ensek.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ensek.Data.Concrete
{
    public class MockMeterReadingsLibrary : IMeterReadingsLibrary
    {
        private List<MeterReading> _mockData;

        public MockMeterReadingsLibrary()
        {
            _mockData = new List<MeterReading>()
            {
                new MeterReading() 
                {
                    AccountId = 2233, 
                    MeterReadingDateTime = new DateTime(2021, 3, 13, 15, 45, 0), 
                    MeterReadValue = 1576
                }
            };
        }

        public bool HasOlderReading(string AccountId, string MeterReadingDateTime)
        {
            int iAccountId = int.Parse(AccountId);
            DateTime dtMeterReadingDateTime = DateTime.Parse(MeterReadingDateTime);
            int cntOlder = _mockData.Count(a => a.AccountId == iAccountId && a.MeterReadingDateTime > dtMeterReadingDateTime);

            return (cntOlder > 0);
        }
    }
}
