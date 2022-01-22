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
        //01. Create the following endpoint:
        //POST => /meter-reading-uploads
        [Route("meter-reading-uploads")]
        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files.First();
                //02. The endpoint should be able to process a CSV of meter readings
                if (file.ContentType == "text/csv")
                {
                    if (file.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                        {
                            if (!reader.EndOfStream)
                            {
                                string headerData = reader.ReadLine();

                                List<MeterReadingRead> fileContents = new List<MeterReadingRead>();
                                int RowNum = 0;
                                while (!reader.EndOfStream)
                                {
                                    string rowData = reader.ReadLine();

                                    MeterReadingRead newMeterReading = new MeterReadingRead(++RowNum, headerData, rowData);
                                    fileContents.Add(newMeterReading);
                                }

                                if (fileContents.Count > 0)
                                {
                                    //04. After processing, the number of sucessful/failed readings should be returned
                                    return Ok(new MeterReadingLoadStatus()
                                    { 
                                        TotalRecords = fileContents.Count,
                                        Successful = fileContents.Count(r => r.IsValid),
                                        Failed = fileContents.Count(r => !r.IsValid),
                                        ValidationResults = fileContents.SelectMany(r => r.ValidationResults)
                                    });
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
                    return BadRequest(file.ContentType + " not a valid format");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}