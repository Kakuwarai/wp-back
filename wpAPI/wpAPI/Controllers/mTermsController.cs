using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wpAPI.Models;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mTermsController : ControllerBase
    {
        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        //private readonly IConverter _pdfConverter;

        public mTermsController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }



        [HttpPost("temrList")]
        public async Task<ActionResult> temrList(int page, int size, string? search)
        {
            try
            {



                List<TermsAndCondition> termList = _context.TermsAndConditions.Where(x => x.IsDelete == 0 && (search == null ? x.Code.Contains("") : (x.Code.ToLower().Contains(search) || x.Description.Contains(search)))).
                     Skip(page * size).
                     Take(10).
                     ToList();

                int termCount = _context.TermsAndConditions.Where(x => x.IsDelete == 0 && (search == null ? x.Code.Contains("") : (x.Code.ToLower().Contains(search) || x.Description.Contains(search)))).
                 Count();


                return Ok(new { termList , termCount });
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

        [HttpPost("addTerms")]
        public async Task<ActionResult> addTerms(TermsAndCondition termsAndCondition)
        {
            try
            {
                string latestCode = _context.TermsAndConditions.OrderBy(x => x.Code).Select(x => x.Code).LastOrDefault();
                string temp = (int.Parse(latestCode) + 1).ToString();
                if (temp.Length == 1)
                {
                    temp = "00" + temp;
                }
                if (temp.Length == 2)
                {
                    temp = "0" + temp;
                }
                termsAndCondition.Code = temp;
                termsAndCondition.CreatedDate = DateTime.Now;

                _context.TermsAndConditions.Add(termsAndCondition);
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


        [HttpPost("editTerms")]
        public async Task<ActionResult> editTerms(TermsAndCondition termsAndCondition)
        {
            try
            {

                TermsAndCondition currentTerms = _context.TermsAndConditions.Where(x => x.Code == termsAndCondition.Code).FirstOrDefault();

                currentTerms.Description = termsAndCondition.Description;
                currentTerms.ModifiedDate = DateTime.Now;
                currentTerms.Status = termsAndCondition.Status;

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
