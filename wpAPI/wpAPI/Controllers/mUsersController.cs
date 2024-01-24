using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wpAPI.Models;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mUsersController : ControllerBase
    {
        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        public mUsersController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        [HttpPost("userList")]
        public async Task<ActionResult> userList(long id,int page, String? search)
        {
            try
            {
                string[] searchSplitted = search == null ? null : search.Split(" ");
                User userInfo = _context.Users.Where(x => x.Id == id).FirstOrDefault();

                if (userInfo.Type == "Super")
                {

                    int usersListCount = _context.Users.Where(
                        x => x.Id != userInfo.Id &&
                        (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                        x.IsDeactivated == 0 &&
                        x.IsVerified == 1
                        ).Count();

                    var usersList = _context.Users.Where(
                       x => x.Id != userInfo.Id &&
                       (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                       x.IsDeactivated == 0 &&
                       x.IsVerified == 1
                       ).
                       Select( x => new
                       {
                           x.Id,
                           x.Firstname,
                           x.Middlename,
                           x.Lastname,
                           x.Nickname,
                           x.Username,
                           x.ContactNumber,
                           x.EmailAddress,
                           x.Type,
                           x.PositionCode,
                           x.UserBranchUsers,
                           x.Status,
                           CreatedByUserId = _context.Users.Where(index => index.Id == x.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                           CreatedDate = x.CreatedDate.ToString("d MMMM yyyy"),
                       }).
                        OrderBy(x => x.Firstname).
                        Skip(page * 10).
                        Take(10).
                       ToList();

                    return Ok(new { usersList, totalPage = usersListCount });

                }
                else if(userInfo.Type == "Admin")
                {

                    int usersListCount = _context.Users.Where(
                        x => x.Id != userInfo.Id &&
                        (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                        x.Type != "Super" &&
                        x.IsDeactivated == 0 &&
                        x.IsVerified == 1
                        ).Count();

                    var usersList = _context.Users.Where(
                       x => x.Id != userInfo.Id &&
                       (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                        x.Type != "Super" &&
                       x.IsDeactivated == 0 &&
                       x.IsVerified == 1
                       ).
                        Select(x => new
                        {
                            x.Id,
                            x.Firstname,
                            x.Middlename,
                            x.Lastname,
                            x.Nickname,
                            x.Username,
                            x.ContactNumber,
                            x.EmailAddress,
                            x.Type,
                            x.PositionCode,
                            x.UserBranchUsers,
                            x.Status,
                            CreatedByUserId = _context.Users.Where(index => index.Id == x.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                            CreatedDate = x.CreatedDate.ToString("d MMMM yyyy"),
                        }).
                        OrderBy(x => x.Firstname).
                        Skip(page * 10).
                        Take(10).
                       ToList();

                    return Ok(new { usersList, totalPage = usersListCount });
                }
                else
                {
                    int usersListCount = _context.Users.Where(
                        x => x.Id != userInfo.Id &&
                        (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                        x.Type != "Super" &&
                        x.Type != "Admin" &&
                        x.IsDeactivated == 0 &&
                        x.IsVerified == 1
                        ).Count();

                    var usersList = _context.Users.Where(
                       x => x.Id != userInfo.Id &&
                       (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                        x.Type != "Super" &&
                         x.Type != "Admin" &&
                       x.IsDeactivated == 0 &&
                       x.IsVerified == 1
                       ).
                        Select(x => new
                        {
                            x.Id,
                            x.Firstname,
                            x.Middlename,
                            x.Lastname,
                            x.Nickname,
                            x.Username,
                            x.ContactNumber,
                            x.EmailAddress,
                            x.Type,
                            x.PositionCode,
                            x.UserBranchUsers,
                            x.Status,
                            CreatedByUserId = _context.Users.Where(index => index.Id == x.CreatedByUserId).Select(index => index.Fullname).FirstOrDefault(),
                            CreatedDate = x.CreatedDate.ToString("d MMMM yyyy"),
                        }).
                        OrderBy(x => x.Firstname).
                        Skip(page * 10).
                        Take(10).
                       ToList();

                    return Ok(new { usersList, totalPage = usersListCount });

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


        [HttpPost("userBranchEdit")]
        public async Task<ActionResult> userBranchEdit(long selectedUserId, long userId)
        {
            try
            {

                String userType = _context.Users.Where(x =>
             x.Id == userId
             ).Select(x => x.Type).FirstOrDefault();
                List<Position> position = _context.Positions.Where(x => x.Status == true).ToList();
                if (userType == "Super")
                {

                    var availableBranches = _context.Branches.Where(x => x.Status == 1 && x.IsDelete == 0).Select(x => new { x.Code, x.Name }).OrderBy(x => x.Name).ToList();

                    var selectedUserBranches = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == selectedUserId && x.UserBranch.Status == 1
                      ).AsEnumerable().Reverse().OrderByDescending(x => x.Branch.Name).Select(x => new
                      {
                          x.Branch.Code,
                          x.Branch.Name
                      }).Distinct().ToList();



                    return Ok(new { availableBranches, selectedUserBranches, position });
                }
                else
                {


                    var availableBranches = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == userId && x.UserBranch.Status == 1
                      ).AsEnumerable().Reverse().OrderByDescending(x => x.Branch.Name).Select(x => new
                      {
                          x.Branch.Code,
                          x.Branch.Name
                      }).Distinct().ToList();

                    var selectedUserBranches = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == selectedUserId && x.UserBranch.Status == 1
                      ).AsEnumerable().Reverse().OrderByDescending(x => x.Branch.Name).Select(x => new
                      {
                          x.Branch.Code,
                          x.Branch.Name
                      }).Distinct().ToList();



                    return Ok(new { availableBranches, selectedUserBranches, position });
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


        public class userTableEditSaveUser
        {
            public long userId { get; set; }
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string? MiddleName { get; set; }
            public string LastName { get; set; }
            public string? Nickname { get; set; }
            public string Username { get; set; }
            public string ContactNumber { get; set; }
            public string EmailAddress { get; set; }
            public string UserType { get; set; }
            public string branches { get; set; }
            public string PositionCode { get; set; }
        }
        [HttpPost("userTableEditSave")]
        public async Task<ActionResult> userTableEditSave(userTableEditSaveUser userTableEditSaveUsers)
        {
            try
            {

                List<UserBranch> allUserBranches = _context.UserBranches.Where(x => x.UserId == userTableEditSaveUsers.Id).ToList();

                if (allUserBranches.Count != 0)
                {
                    foreach (var branchId in allUserBranches)
                    {
                        branchId.Status = 0;
                    }
                }

                var branchIdList = userTableEditSaveUsers.branches.Split(',');

                User userDetails = _context.Users.Where(x => x.Id == userTableEditSaveUsers.Id).FirstOrDefault();

                if (userDetails == null)
                {
                    return StatusCode(202, "Error no data");
                }
                else
                {
                    userDetails.Firstname = userTableEditSaveUsers.FirstName;
                    userDetails.Middlename = userTableEditSaveUsers.MiddleName;
                    userDetails.Lastname = userTableEditSaveUsers.LastName;
                    userDetails.Nickname = userTableEditSaveUsers.Nickname;
                    userDetails.Username = userTableEditSaveUsers.Username;
                    userDetails.EmailAddress = userTableEditSaveUsers.EmailAddress;
                    userDetails.ContactNumber = userTableEditSaveUsers.ContactNumber;
                    userDetails.Type = userTableEditSaveUsers.UserType;
                    userDetails.PositionCode = userTableEditSaveUsers.PositionCode;
                }


                foreach (string branchId in branchIdList)
                {
                    UserBranch userBranch = _context.UserBranches.Where(x => x.BranchCode == branchId && x.UserId == userTableEditSaveUsers.Id).FirstOrDefault();

                    if (userBranch == null)
                    {
                        _context.UserBranches.Add(new UserBranch
                        {
                            UserId = userTableEditSaveUsers.Id,
                            BranchCode = branchId,
                            IsDefault = 0,
                            Status = 1,
                            CreatedByUserId = userTableEditSaveUsers.userId,

                        });
                    }
                    else
                    {
                        userBranch.Status = 1;
                    }

                }
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


        [HttpPost("userBranchAdd")]
        public async Task<ActionResult> userBranchAdd(long selectedUserId)
        {
            try
            {

                String userType = _context.Users.Where(x =>
                x.Id == selectedUserId
                ).Select(x=>x.Type).FirstOrDefault();

                List<Position> position = _context.Positions.Where(x => x.Status == true).ToList();

                if (userType == "Super")
                {
                    var availableBranches = _context.Branches.Where(x => x.Status == 1 && x.IsDelete == 0).Select(x => new { x.Code, x.Name }).OrderBy(x => x.Name).ToList();

                    return Ok(new { availableBranches , position});
                }
                else
                {
                    

                    var availableBranches = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == selectedUserId && x.UserBranch.Status == 1
                      ).AsEnumerable().Reverse().OrderByDescending(x => x.Branch.Name).Select(x => new
                      {
                          x.Branch.Code,
                          x.Branch.Name
                      }).Distinct().ToList();



                    return Ok(new { availableBranches, position });
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

        [HttpPost("userTableAddSave")]
        public async Task<ActionResult> userTableAddSave(User userTableEditSaveUsers,String branches,long userId)
        {
            try
            {


                User checker = _context.Users.Where(x => x.Username == userTableEditSaveUsers.Username).FirstOrDefault();

                if (checker != null)
                {

                    return StatusCode(202, "Username already exist!");
                }

                var branchIdList = branches.Split(',');

                userTableEditSaveUsers.CreatedByUserId = userId;
                userTableEditSaveUsers.CreatedDate = DateTime.Now;
                userTableEditSaveUsers.Status = 1;

                if(userTableEditSaveUsers.Middlename == null || userTableEditSaveUsers.Middlename == "")
                {
                    userTableEditSaveUsers.Fullname = userTableEditSaveUsers.Firstname + userTableEditSaveUsers.Lastname;
                    userTableEditSaveUsers.Fullname2 = userTableEditSaveUsers.Lastname + ", " + userTableEditSaveUsers.Firstname ;
                }
                else
                {
                    userTableEditSaveUsers.Fullname = userTableEditSaveUsers.Firstname + " " + userTableEditSaveUsers.Middlename[0] + ". " + userTableEditSaveUsers.Lastname;
                    userTableEditSaveUsers.Fullname2 = userTableEditSaveUsers.Lastname + ", " + userTableEditSaveUsers.Firstname + " " + userTableEditSaveUsers.Middlename;
                }

                userTableEditSaveUsers.IsVerified = 1;
                userTableEditSaveUsers.Password = userTableEditSaveUsers.Username;

               _context.Users.Add(userTableEditSaveUsers);

                _context.SaveChanges();
            
                foreach (string branchId in branchIdList)
                {
                  
                        _context.UserBranches.Add(new UserBranch
                        {
                            UserId = userTableEditSaveUsers.Id,
                            BranchCode = branchId,
                            IsDefault = 0,
                            Status = 1,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now


                        });
                  

                }
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
