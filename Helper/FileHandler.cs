using AutoMapper;
using Ensek.Data.Abstract;
using Ensek.DTO;
using Ensek.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ensek.Helper
{
    public class FileHandler
    {
        public enum ValidationStatus
        {
            AllGood,
            InvalidExtension,
            HasNoData,
            HasNoHeader,
            HasNoRows
        }

        private readonly IMapper _mapper;
        private readonly IAccountsLibrary _accountsRepo;
        private readonly IMeterReadingsLibrary _meterReadingsRepo;
        private IFormFile _file;
        private string[] _fileContents;

        public FileHandler(IMapper Mapper, IAccountsLibrary AccountsRepo, IMeterReadingsLibrary MeterReadingsRepo, IFormFile file)
        {
            _mapper = Mapper;
            _accountsRepo = AccountsRepo;
            _meterReadingsRepo = MeterReadingsRepo;
            _file = file;

            this.ReadData();
        }

        public ValidationStatus Validate()
        {
            ValidationStatus retVal = ValidationStatus.AllGood;

            if (!this.HasValidExtension)
                retVal = ValidationStatus.InvalidExtension;
            else if (!this.HasData)
                retVal = ValidationStatus.HasNoData;
            else if (!this.HasHeader)
                retVal = ValidationStatus.HasNoHeader;
            else if (!this.HasRows)
                retVal = ValidationStatus.HasNoRows;

            return retVal;
        }

        public MeterReadingLoadStatus GetMeterReadingStatus()
        {
            List<MeterReadingRead> meterReadings = this.MeterReadings;
            foreach (MeterReadingRead newMeterReading in meterReadings)
            {
                //Check if the row has any loading issues
                if (newMeterReading.IsValid)
                {
                    MeterReading mr = _mapper.Map<MeterReading>(newMeterReading);

                    //06. Check if the AccountId is valid
                    bool isValidAccountId = _accountsRepo.IsValidAccountId(mr);
                    if (isValidAccountId)
                    {
                        //05. Check for duplicate entries
                        int cntDuplicate = meterReadings.Count(r => r.AccountId == newMeterReading.AccountId && r.IsValid);
                        if (cntDuplicate == 1)
                        {
                            //09. When an account has an existing read, ensure the new read isn't older than the existing read
                            bool hasOlderReading = _meterReadingsRepo.HasOlderReading(mr);
                            if (!hasOlderReading)
                            {
                                _meterReadingsRepo.InsertReading(mr);
                            }
                            else
                            {
                                newMeterReading.ValidationResults.Add($"Row#{newMeterReading.RowNum} Has older readings");
                            }
                        }
                        else
                        {
                            newMeterReading.ValidationResults.Add($"Row#{newMeterReading.RowNum} Duplicate Entry");
                        }
                    }
                    else
                    {
                        newMeterReading.ValidationResults.Add($"Row#{newMeterReading.RowNum} Invalid AccountId");
                    }
                }
            }

            MeterReadingLoadStatus retVal = new MeterReadingLoadStatus()
            {
                TotalRecords = meterReadings.Count,
                Successful = meterReadings.Count(r => r.IsValid),
                Failed = meterReadings.Count(r => !r.IsValid),
                ValidationResults = meterReadings.SelectMany(r => r.ValidationResults)
            };

            return retVal;
        }

        private void ReadData()
        {
            List<string> retVal = new List<string>();
            using (StreamReader reader = new StreamReader(_file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    string rowData = reader.ReadLine();
                    retVal.Add(rowData);
                }   
            }

            _fileContents = retVal.ToArray();
        }

        private bool HasValidExtension
        {
            get
            {
                //02. The endpoint should be able to process a CSV of meter readings
                string fileExtension = Path.GetExtension(_file.FileName).ToUpper();

                return (fileExtension == ".CSV");
            }
        }

        private bool HasData
        {
            get
            {
                //Check if file has any data
                return (_file.Length > 0);
            }
        }

        private bool HasHeader
        {
            get
            {
                //Check if the file has any header
                return (_fileContents.Length > 0);
            }
        }

        private bool HasRows
        {
            get
            {
                //Check if the file has any row data excluding the header
                return (_fileContents.Length > 1);
            }
        }

        private string FileHeader
        {
            get
            {
                return this.HasHeader ? _fileContents[0] : "";
            }
        }

        private string[] FileContents
        {
            get
            {
                string[] retVal = { };

                if (this.HasRows)
                {
                    retVal = new string[_fileContents.Length - 1];
                    Array.Copy(_fileContents, 1, retVal, 0, _fileContents.Length - 1);
                }

                return retVal;
            }
        }

        private List<MeterReadingRead> MeterReadings
        {
            get
            {
                List<MeterReadingRead> retVal = new List<MeterReadingRead>();

                //Read header
                string headerData = this.FileHeader;

                for (int RowNum = 1; RowNum < this.FileContents.Length; RowNum++)
                {
                    //Read row
                    string rowData = this.FileContents[RowNum];

                    MeterReadingRead newMeterReading = new MeterReadingRead(RowNum, headerData, rowData);
                    //Add to the collection valid or not
                    retVal.Add(newMeterReading);
                }

                return retVal;
            }
        }
    }
}