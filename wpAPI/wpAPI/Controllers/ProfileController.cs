using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wpAPI.Models;
using static wpAPI.Controllers.mUsersController;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        //private readonly IConverter _pdfConverter;

        public ProfileController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }

        public partial class profileUser
        {
            public long Id { get; set; }

            public string? Username { get; set; }

            public string? Password { get; set; }

            public string? Lastname { get; set; }

            public string? Firstname { get; set; }

            public string? Middlename { get; set; }

            public string? Nickname { get; set; }

            public string? EmailAddress { get; set; }

            public string? ContactNumber { get; set; }
        }


            [HttpPost("saveProfile")]
        public async Task<ActionResult> saveProfile(profileUser user)
        {
            try
            {

                User checker = _context.Users.Where( x => x.Id == user.Id && x.Password == user.Password ).FirstOrDefault();

                if (checker == null)
                {
                    return StatusCode(202, "Invalid Password");
                }

            
                checker.Firstname = user.Firstname;
                checker.Lastname = user.Lastname;
                checker.Nickname = user.Nickname;
                checker.EmailAddress = user.EmailAddress;
                checker.ModifiedByUserId = user.Id;
                checker.ModifiedDate = DateTime.Now;
                checker.Fullname = user.Firstname + " " + user.Middlename[0] + ". " + user.Lastname;
                checker.Fullname2 = user.Lastname + ", " + user.Firstname + " " + user.Middlename;
                checker.ContactNumber = user.ContactNumber;
                _context.SaveChanges();


             /*   var send = _context.Users.Where(x => x.Id == user.Id && x.Password == user.Password).Select
                    (
                    x => new
                    {
                        x.Id,
                        x.Username,
                        x.Firstname,
                        x.Lastname,
                        x.Middlename,
                        x.EmailAddress,
                        x.Sbu,
                        x.Nickname,
                        x.Status,
                        x.Fullname,
                        x.Fullname2,
                        x.Type,
                        x.ContactNumber,
                        x.CreatedByUserId,
                        x.CreatedDate,
                    }).FirstOrDefault();*/

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

        [HttpPost("changePassword")]
        public async Task<ActionResult> changePassword(long userId,string currentPassword, string newPassword)
        {
            try
            {

                User users = _context.Users.Where(x => x.Password == currentPassword && x.Id == userId).FirstOrDefault();

                if (users == null)
                {
                    return StatusCode(202, "Invalid Password");
                }

                users.Password = newPassword;
                users.ModifiedByUserId = users.Id;
                users.ModifiedDate = DateTime.Now;

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
