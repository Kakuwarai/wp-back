using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using wpAPI.Models;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<ActionResult> test(String userNE, String password)
        {
            try
            {
               

                var logChecker = _context.Users.Where(x => (x.Username == userNE || x.EmailAddress == userNE) && x.Password == password).Select(
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
                    }).FirstOrDefault();

                if(logChecker == null)
                {

                    return StatusCode(202, "invalid username or password");
                }

                if (logChecker.Type != "Super")
                {
                    var selectedUserBranches = _context.Branches.Join(_context.UserBranches,
                  branch => branch.Code,
                  userbranch => userbranch.BranchCode,
                   (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                   x.UserBranch.UserId == logChecker.Id && x.UserBranch.Status == 1
                   ).AsEnumerable().Reverse().OrderByDescending(x => x.Branch.Name).Select(x => new
                   {
                       x.Branch.Code,
                       x.Branch.Name,
                       x.UserBranch.IsDefault
                   }).Distinct().ToList();

                    var tempBranch = selectedUserBranches.Where(x => x.IsDefault == 1).FirstOrDefault();

                    if(tempBranch == null)
                    {
                        UserBranch toDefault = _context.UserBranches.Where( x => x.UserId == logChecker.Id && x.BranchCode == selectedUserBranches[0].Code && x.Status == 1).FirstOrDefault();

                        toDefault.IsDefault = 1;

                        _context.SaveChanges();

                        var selectedUserBranches1 = _context.Branches.Join(_context.UserBranches,
                 branch => branch.Code,
                 userbranch => userbranch.BranchCode,
                  (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                  x.UserBranch.UserId == logChecker.Id && x.UserBranch.Status == 1
                  ).AsEnumerable().Reverse().OrderByDescending(x => x.Branch.Name).Select(x => new
                  {
                      x.Branch.Code,
                      x.Branch.Name,
                      x.UserBranch.IsDefault
                  }).Distinct().ToList();



                        List<String> userMenuTemp = _context.UserMenus.Where(x => x.BranchCode == tempBranch.Code && x.UserId == logChecker.Id && x.Status == 1).Select(x => x.MenuId.ToString()).ToList();

                        List<Menu> menuListTemp = _context.Menus.Where(x => userMenuTemp.Contains(x.Id.ToString())).ToList();

                        List<Menu> parentTemp = _context.Menus.Where(x => menuListTemp.Select(s => s.ParentMenuId.ToString()).Contains(x.Id.ToString())).ToList();

                        return Ok(new { logChecker, selectedUserBranches1, menuList = menuListTemp, parent = parentTemp });

 

                    }

                    List<String> userMenuTemp1 = _context.UserMenus.Where(x => x.BranchCode == tempBranch.Code && x.UserId == logChecker.Id && x.Status == 1).Select(x => x.MenuId.ToString()).ToList();
                   
                    List<Menu> menuListTemp1 = _context.Menus.Where(x => userMenuTemp1.Contains(x.Id.ToString())).ToList();

                    List<Menu> parentTemp1 = _context.Menus.Where(x => menuListTemp1.Select(s => s.ParentMenuId.ToString()).Contains(x.Id.ToString())).ToList();

                    return Ok(new { logChecker , selectedUserBranches, menuList = menuListTemp1, parent = parentTemp1 });
                }
                else
                {

                    List<Menu> menuListTemp = _context.Menus.Where(x => x.ParentMenuId != 0).ToList();

                    List<Menu> parentTemp = _context.Menus.Where(x => x.ParentMenuId == 0).ToList();

                    var selectedUserBranches = _context.Branches.Where(x => x.IsDelete == 0 && x.Status == 1).Select(x => new {x.Code, x.Name}).ToList();

                    return Ok(new { logChecker, selectedUserBranches, menuList = menuListTemp, parent = parentTemp });
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
