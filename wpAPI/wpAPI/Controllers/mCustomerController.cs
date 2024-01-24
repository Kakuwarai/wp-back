using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wpAPI.Models;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mCustomerController : ControllerBase
    {

        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        public mCustomerController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("customerList")]
        public async Task<ActionResult> customerList(int page, int size, string? search)
        {
            try
            {



                int customerCount = _context.Customers.Where(x => x.IsDelete == false && (search == null ? x.Name.ToLower().Contains("") : x.Name.ToLower().Contains(search))).
                        Count();

                var customerList = _context.Customers.Where(x => x.IsDelete == false && (search == null ? x.Name.ToLower().Contains("") : x.Name.ToLower().Contains(search))).
                    Select(x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name,
                        x.CompanyName,
                        x.CompanyAddress1,
                        x.Salutation,
                        x.Position,
                        CreatedByUserId =_context.Users.Where(index => index.Id == x.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                        CreatedDate = x.CreatedDate.ToString("d MMMM yyyy"),
                        x.Status

                    }).
                    Skip(page * size).
                    Take(size).
                    OrderBy(x => x.Code).ToList();

                return Ok(new { customerList , customerCount });
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



        [HttpPost("customerAdd")]
        public async Task<ActionResult> customerAdd(Customer customer)
        {
            try
            {
                string code = (int.Parse(_context.Customers.Where(x => x.IsDelete == false).OrderBy(x => x.Code).Select(x => x.Code).LastOrDefault()) + 1).ToString();

                customer.Code = code;
                customer.CreatedDate = DateTime.Now;

                _context.Customers.Add(customer);
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

        [HttpPost("customerEdit")]
        public async Task<ActionResult> customerEdit(Customer customer)
        {
            try
            {


                Customer editCustomer = _context.Customers.Where(x => x.IsDelete == false && x.Code == customer.Code).FirstOrDefault();

                editCustomer.Status = customer.Status;
                editCustomer.Name = customer.Name;
                editCustomer.Position = customer.Position;
                editCustomer.CompanyName = customer.CompanyName;
                editCustomer.CompanyAddress1 = customer.CompanyAddress1;

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
