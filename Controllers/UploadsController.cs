using AutoMapper;
using Ensek.Data.Abstract;
using Ensek.DTO;
using Ensek.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Ensek.Controllers
{
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountsLibrary _accountsRepo;
        private readonly IMeterReadingsLibrary _meterReadingsRepo;
        
        public UploadsController(IMapper Mapper, IAccountsLibrary AccountsRepo, IMeterReadingsLibrary MeterReadingsRepo)
        {
            _mapper = Mapper;
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
                FileHandler fileHandler = new FileHandler(_mapper, _accountsRepo, _meterReadingsRepo, file);

                FileHandler.ValidationStatus vs = fileHandler.Validate();
                switch (vs)
                {
                    case FileHandler.ValidationStatus.InvalidExtension:
                        return BadRequest("Not a valid format");
                    case FileHandler.ValidationStatus.HasNoData:
                        return BadRequest("File empty");
                    case FileHandler.ValidationStatus.HasNoHeader:
                        return BadRequest("No header");
                    case FileHandler.ValidationStatus.HasNoRows:
                        return BadRequest("No data");
                    default:
                        MeterReadingLoadStatus retVal = fileHandler.GetMeterReadingStatus();

                        if (retVal.Successful > 0)
                            _meterReadingsRepo.SaveChanges();

                        //04. After processing, the number of sucessful/failed readings should be returned
                        return Ok(retVal);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}