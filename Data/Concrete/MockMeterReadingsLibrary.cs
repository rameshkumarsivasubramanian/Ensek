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
                new MeterReading() //To fail validation
                {
                    //Row #3
                    AccountId = 2233, 
                    MeterReadingDateTime = new DateTime(2020, 3, 13, 15, 45, 0), //After current reading date
                    MeterReadValue = 1576
                },
                new MeterReading() //To pass validation
                {
                    //Row #4
                    AccountId = 8766, 
                    //Before current reading date
                    MeterReadingDateTime = new DateTime(2019, 3, 13, 15, 45, 0), 
                    MeterReadValue = 1576
                }
            };
        }

        public bool HasOlderReading(MeterReading mr)
        {
            int cntOlder = _mockData.Count(a => a.AccountId == mr.AccountId && a.MeterReadingDateTime >= mr.MeterReadingDateTime);

            return (cntOlder > 0);
        }

        public void InsertReading(MeterReading mr)
        {
            _mockData.Add(mr);
        }

        public void SaveChanges()
        {
            //Do nothing
        }
    }
}
