﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
                if (file.ContentType == "text/csv")
                {
                    if (file.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                        {
                            if (!reader.EndOfStream)
                            {
                                string header = reader.ReadLine();

                                List<string> strContent = new List<string>();
                                while (!reader.EndOfStream)
                                {
                                    string line = reader.ReadLine();
                                    strContent.Add(line);
                                }

                                if (strContent.Count > 0)
                                {
                                    return Ok(strContent.Count);
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