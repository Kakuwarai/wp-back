using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wpAPI.Models;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mPositionController : ControllerBase
    {
        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        public mPositionController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("positionList")]
        public async Task<ActionResult> positionList(int page, int size, string? search)
        {
            try
            {

                int positionCount = _context.Positions.Where(x => (search == null ? x.Name.ToLower().Contains("") : x.Name.ToLower().Contains(search.ToLower()))).
                        Count();

                var positionList = _context.Positions.Where(x => (search == null ? x.Name.ToLower().Contains("") : x.Name.ToLower().Contains(search.ToLower()))).
                    Select(x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name,
                        CreatedByUserId = _context.Users.Where(index => index.Id == x.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                        CreatedDate = x.CreatedDate.Value.ToString("d MMMM yyyy"),
                        x.Status

                    }).
                    Skip(page * size).
                    Take(size).
                    OrderBy(x => x.Code).ToList();

                return Ok(new { positionList, positionCount });
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

        [HttpPost("positionAdd")]
        public async Task<ActionResult> positionAdd(Position position)
        {
            try
            {
                Position positionChecker = _context.Positions.Where(x => x.Name!.ToLower() == position.Name!.ToLower() || x.Code == position.Code).FirstOrDefault();
                if (positionChecker == null)
                {
                    position.CreatedDate = DateTime.Now;

                    _context.Positions.Add(position);
                }
                else
                {
                    return StatusCode(202, "Name or Code already Exist!");
                }
                

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

        [HttpPost("positionEdit")]
        public async Task<ActionResult> positionEdit(Position position)
        {
            try
            {
                Position positionChecker = _context.Positions.Where(x => x.Id != position.Id && x.Name!.ToLower() == position.Name!.ToLower()).FirstOrDefault();

              if(positionChecker == null)
                {
                    Position currentPosition = _context.Positions.Where(x => x.Id == position.Id).FirstOrDefault()!;

                    currentPosition.Name = position.Name;
                    currentPosition.Status = position.Status;
                    currentPosition.ModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                }
                else
                {
                    return StatusCode(202, "Name already Exist!");
                }

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
