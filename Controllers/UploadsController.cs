using Ensek.Data.Abstract;
using Ensek.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ensek.Controllers
{
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IAccountsLibrary _accountsRepo;
        private readonly IMeterReadingsLibrary _meterReadingsRepo;
        public UploadsController(IAccountsLibrary AccountsRepo, IMeterReadingsLibrary MeterReadingsRepo)
        {
            _accountsRepo = AccountsRepo;
            _meterReadingsRepo = MeterReadingsRepo;
        }

        //01. Create the following endpoint:
        //POST => /meter-reading-uploads
        [Route("meter-reading-uploads")]
        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files.First();
                string fileExtension = Path.GetExtension(file.FileName).ToUpper();
                //02. The endpoint should be able to process a CSV of meter readings
                if (fileExtension == ".CSV")
                {
                    //Check if file has any data
                    if (file.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                        {
                            //Check if the file has any header
                            if (!reader.EndOfStream)
                            {
                                //Read header
                                string headerData = reader.ReadLine();

                                List<MeterReadingRead> fileContents = new List<MeterReadingRead>();
                                int RowNum = 0;
                                while (!reader.EndOfStream)
                                {
                                    //Read rows
                                    string rowData = reader.ReadLine();

                                    MeterReadingRead newMeterReading = new MeterReadingRead(++RowNum, headerData, rowData);
                                    //Check if the row has any loading issues
                                    if (newMeterReading.IsValid)
                                    {
                                        //06. Check if the AccountId is valid
                                        bool isValidAccountId = _accountsRepo.IsValidAccountId(newMeterReading.AccountId);
                                        if (!isValidAccountId)
                                        {
                                            newMeterReading.ValidationResults.Add(string.Format("Row#{0} {1}", RowNum, "Invalid AccountId"));
                                        }
                                        else
                                        {
                                            //05. Check for duplicate entries
                                            int cntDuplicate = fileContents.Count(r => r.AccountId == newMeterReading.AccountId && r.IsValid);
                                            if (cntDuplicate > 0)
                                            {
                                                newMeterReading.ValidationResults.Add(string.Format("Row#{0} {1}", RowNum, "Duplicate Entry"));
                                            }
                                            else
                                            {
                                                //09. When an account has an existing read, ensure the new read isn't older than the existing read
                                                bool hasOlderReading = _meterReadingsRepo.HasOlderReading(newMeterReading.AccountId, newMeterReading.MeterReadingDateTime);
                                                if (hasOlderReading)
                                                {
                                                    newMeterReading.ValidationResults.Add(string.Format("Row#{0} {1}", RowNum, "Has older readings"));
                                                }
                                                else
                                                {
                                                    _meterReadingsRepo.InsertReading(newMeterReading.AccountId, newMeterReading.MeterReadingDateTime, newMeterReading.MeterReadValue);
                                                }
                                            }
                                        }
                                    }

                                    //Add to the collection valid or not
                                    fileContents.Add(newMeterReading);
                                }

                                MeterReadingLoadStatus retVal = new MeterReadingLoadStatus()
                                {
                                    TotalRecords = fileContents.Count,
                                    Successful = fileContents.Count(r => r.IsValid),
                                    Failed = fileContents.Count(r => !r.IsValid),
                                    ValidationResults = fileContents.SelectMany(r => r.ValidationResults)
                                };

                                if (retVal.Successful > 0)
                                    _meterReadingsRepo.SaveChanges();

                                //Check if the file has any row data excluding the header
                                if (retVal.TotalRecords > 0)
                                {
                                    //04. After processing, the number of sucessful/failed readings should be returned
                                    return Ok(retVal);
                                }
                                else
                                {
                                    return BadRequest("No data");
                                }
                            }
                            else
                            {
                                return BadRequest("No header");
                            }
                        }
                    }
                    else
                    {
                        return BadRequest("File empty");
                    }
                }
                else
                {
                    return BadRequest($"*{fileExtension} not a valid format");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}