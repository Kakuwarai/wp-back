using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wpAPI.Models;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mBranchesController : ControllerBase
    {
        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        public mBranchesController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("branchList")]
        public async Task<ActionResult> branchList(long id, int page, int size, String? search)
        {
            try
            {
                User userInfo = _context.Users.Where(x => x.Id == id).FirstOrDefault();

                if (userInfo.Type == "Super")
                {

                    int branchCount = _context.Branches.Where(
                        x =>
                        x.IsDelete == 0 &&
                        (search == null ? x.Code.Contains("") : (x.Code.Contains(search) || x.Name.ToLower().Contains(search)))
                        ).Count();

                   var branchList = _context.Branches.Where(
                        x => 
                        x.IsDelete == 0 &&
                        (search == null? x.Code.Contains(""):(x.Code.Contains(search) || x.Name.ToLower().Contains(search)))
                        ).Select(x => new
                        {
                            x.Id,
                            x.Code,
                            x.Name,
                            x.Address1,
                            x.PostalCode,
                            x.Area,
                            x.Region,
                            x.Status,
                            CreatedByUserId = _context.Users.Where(index => index.Id == x.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                            CreatedDate = x.CreatedDate.ToString("d MMMM yyyy"),
                        }).
                        OrderBy(x => x.Name).
                        Skip(page * size).
                        Take(size).
                       ToList();

                    return Ok(new { branchList, totalPage = branchCount });

                }
                else if (userInfo.Type == "Admin")
                {

                    int branchCount = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == id && x.Branch.IsDelete == 0 &&
                        (search == null ? x.Branch.Code.Contains("") : (x.Branch.Code.Contains(search) || x.Branch.Name.ToLower().Contains(search)))
                      ).AsEnumerable().Distinct().Count();


                    var branchList = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == id && x.Branch.IsDelete == 0 &&
                        (search == null ? x.Branch.Code.Contains("") : (x.Branch.Code.Contains(search) || x.Branch.Name.ToLower().Contains(search)))
                      ).AsEnumerable().Reverse().OrderBy(x => x.Branch.Name).Skip(page * size).
                        Take(size).Select(x => new
                      {
                          x.Branch.Id,
                          x.Branch.Code,
                          x.Branch.Name,
                          x.Branch.Address1,
                          x.Branch.PostalCode,
                          x.Branch.Area,
                          x.Branch.Region,
                          x.Branch.Status,
                          CreatedByUserId = _context.Users.Where(index => index.Id == x.Branch.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                          CreatedDate = x.Branch.CreatedDate.ToString("d MMMM yyyy"),
                        }).Distinct().ToList();

                    return Ok(new { branchList, totalPage = branchCount });
                }
                else
                {
                    int branchCount = _context.Branches.Join(_context.UserBranches,
                   branch => branch.Code,
                   userbranch => userbranch.BranchCode,
                    (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                    x.UserBranch.UserId == id && x.Branch.IsDelete == 0 &&
                        (search == null ? x.Branch.Code.Contains("") : (x.Branch.Code.Contains(search) || x.Branch.Name.ToLower().Contains(search)))
                    ).AsEnumerable().Distinct().Count();



                    var branchList = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == id && x.Branch.IsDelete == 0 &&
                        (search == null ? x.Branch.Code.Contains("") : (x.Branch.Code.Contains(search) || x.Branch.Name.ToLower().Contains(search)))
                      ).AsEnumerable().Reverse().OrderBy(x => x.Branch.Name).Skip(page * size).
                        Take(size).Select(x => new
                      {
                          x.Branch.Id,
                          x.Branch.Code,
                          x.Branch.Name,
                          x.Branch.Address1,
                          x.Branch.PostalCode,
                          x.Branch.Area,
                          x.Branch.Region,
                          x.Branch.Status,
                          CreatedByUserId = _context.Users.Where(index => index.Id == x.Branch.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                          CreatedDate = x.Branch.CreatedDate.ToString("d MMMM yyyy"),
                        }).Distinct().ToList();

                    return Ok(new { branchList, totalPage = branchCount });

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

        [HttpPost("branchAdd")]
        public async Task<ActionResult> branchAdd(Branch branch)
        {
            try
            {

               Branch checker = _context.Branches.Where(x => x.Code.ToLower() == branch.Code.ToLower() && x.IsDelete == 0).FirstOrDefault();

                if(checker != null)
                {
                    return StatusCode(208, "Branch code already exist!");
                }
                else
                {
                    _context.Branches.Add(branch);
                    _context.SaveChanges();

                    _context.UserBranches.Add(new UserBranch
                    {

                        UserId = branch.CreatedByUserId,
                        BranchCode = branch.Code,
                        IsDefault = 0,
                        CreatedByUserId = branch.CreatedByUserId,
                        Status = 1

                    });
                    _context.SaveChanges();
                    return Ok();
                }

                


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

        [HttpPost("branchEdit")]
        public async Task<ActionResult> branchEdit(Branch branch)
        {
            try
            {

                Branch checker = _context.Branches.Where(x => x.Code.ToLower() == branch.Code.ToLower() && x.IsDelete == 0).FirstOrDefault();
                Branch newBranch = _context.Branches.Where(x => x.Id == branch.Id).FirstOrDefault();

                if (checker != null && checker.Code != branch.Code)
                {
                    return StatusCode(208, "Branch code already exist!");
                }
                else
                {

                    newBranch.Code = branch.Code;
                    newBranch.Name = branch.Name;
                    newBranch.Address1 = branch.Address1;
                    newBranch.PostalCode = branch.PostalCode;
                    newBranch.Area = newBranch.Area;
                    newBranch.Region = branch.Region;
                    newBranch.Status = branch.Status;

                    _context.SaveChanges();
                    return Ok();
                }




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
