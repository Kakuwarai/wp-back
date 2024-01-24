using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wpAPI.Models;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mChargesController : ControllerBase
    {
        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        //private readonly IConverter _pdfConverter;

        public mChargesController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }



        [HttpPost("chargesList")]
        public async Task<ActionResult> chargesList(int page, int size, string? search)
        {
            try
            {

             

                List<Rate> chargesList = _context.Rates.Where(x=>x.IsDelete == 0 && (search == null? x.ChargeName.Contains(""): (x.ChargeName.ToLower().Contains(search) || x.ChargeCode.Contains(search)) )).
                     Skip(page * size).
                     Take(10).
                     ToList();

                int chargesCount = _context.Rates.Where(x => x.IsDelete == 0 && (search == null ? x.ChargeName.Contains("") : (x.ChargeName.ToLower().Contains(search) || x.ChargeCode.Contains(search)))).
                 Count();


                return Ok(new { chargesList , chargesCount });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {
                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {
                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("addCharges")]
        public async Task<ActionResult> addCharges(Rate rate)
        {
            try
            {

               string latestCode = _context.Rates.OrderBy(x => x.ChargeCode).Select(x => x.ChargeCode).LastOrDefault();
                rate.ChargeCode = (int.Parse(latestCode) + 1).ToString();
                rate.CreatedDate = DateTime.Now;

               _context.Rates.Add(rate);
                _context.SaveChanges();

                return Ok();
   
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {
                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }


        [HttpPost("editCharges")]
        public async Task<ActionResult> editCharges(Rate rate)
        {
            try
            {

                Rate currentRate = _context.Rates.Where(x => x.ChargeCode == rate.ChargeCode).FirstOrDefault();

                currentRate.ChargeName = rate.ChargeName;
                currentRate.ChargeCategory = rate.ChargeCategory;
                currentRate.Currency = rate.Currency;
                currentRate.Amount = rate.Amount;
                currentRate.Uom = rate.Uom;
                currentRate.DefaultRemarks = rate.DefaultRemarks;
                currentRate.Status = rate.Status;
                currentRate.ModifiedByUserId = rate.ModifiedByUserId;
                currentRate.ModifiedDate = DateTime.Now;

       
                _context.SaveChanges();

                return Ok();

            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {
                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

    }
}
