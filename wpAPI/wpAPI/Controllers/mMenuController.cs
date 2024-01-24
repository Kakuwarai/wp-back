using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wpAPI.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mMenuController : ControllerBase
    {
        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;


        public mMenuController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("menuUserList")]
        public async Task<ActionResult> menuUserList(long userId,int page, int size, String? search, string branch)
        {
            try
            {

                string[] searchSplitted = search == null ? null : search.Split(" ");


                    int usersListCount = _context.Users.Join(_context.UserBranches,
                   userId => userId.Id,
                   userBranchId => userBranchId.UserId,
                    (userIds, userBranchId) => new { UserId = userIds, UserBranchId = userBranchId }).Where(x =>
                    (search == null ? x.UserId.Fullname2.Contains("") : (x.UserId.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.UserId.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                        x.UserBranchId.BranchCode == branch &&
                        x.UserId.IsDeactivated == 0 &&
                        x.UserId.IsVerified == 1 &&
                        x.UserId.Status == 1 &&
                        x.UserId.Type != "Super" &&
                        x.UserBranchId.Status == 1 
                    ).AsEnumerable().Reverse().OrderBy(x => x.UserId.Firstname).Select(x => new
                    {
                        x.UserId.Id,
                        x.UserId.Firstname,
                        x.UserId.Middlename,
                        x.UserId.Lastname,
                        x.UserId.Type
                    }).Distinct().Count();

                    var usersList = _context.Users.Join(_context.UserBranches,
                  userId => userId.Id,
                  userBranchId => userBranchId.UserId,
                   (userIds, userBranchId) => new { UserId = userIds, UserBranchId = userBranchId }).Where(x =>
                       (search == null ? x.UserId.Fullname2.Contains("") : (x.UserId.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.UserId.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower()))) &&
                       x.UserBranchId.BranchCode == branch &&
                       x.UserId.IsDeactivated == 0 &&
                       x.UserId.IsVerified == 1 &&
                       x.UserId.Status == 1 &&
                        x.UserId.Type != "Super" &&
                       x.UserBranchId.Status == 1
                   ).AsEnumerable().Reverse().OrderBy(x => x.UserId.Firstname).Select(x => new
                   {
                       x.UserId.Id,
                       x.UserId.Firstname,
                       x.UserId.Middlename,
                       x.UserId.Lastname,
                       x.UserId.Type
                   }).OrderBy(x => x.Firstname).Skip(page * size).Take(size).Distinct().ToList();

                    return Ok(new { usersList, totalPage = usersListCount });

             

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


        [HttpPost("branchMenuList")]
        public async Task<ActionResult> branchMenuList(long id)
        {
            try
            {
                User userInfo = _context.Users.Where(x => x.Id == id).FirstOrDefault();

                if (userInfo.Type == "Super")
                {

                  

                    List<Branch> branchList = _context.Branches.Where(
                        x =>
                        x.IsDelete == 0 &&
                        x.Status == 1
                        ).
                        OrderBy(x => x.Name).
                       ToList();

                    return Ok(branchList);

                }
                else if (userInfo.Type == "Admin")
                {

                    var branchList = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == id && x.Branch.IsDelete == 0 && x.Branch.Status == 1 && x.UserBranch.Status == 1
                      ).AsEnumerable().Reverse().OrderBy(x => x.Branch.Name).Select(x => new
                      {
                          x.Branch.Id,
                          x.Branch.Code,
                          x.Branch.Name,
                          x.Branch.Address1,
                          x.Branch.PostalCode,
                          x.Branch.Area,
                          x.Branch.Region,
                          x.Branch.Status,
                      }).Distinct().ToList();

                    return Ok( branchList);
                }
                else
                {

                    var branchList = _context.Branches.Join(_context.UserBranches,
                     branch => branch.Code,
                     userbranch => userbranch.BranchCode,
                      (branch, userbranch) => new { Branch = branch, UserBranch = userbranch }).Where(x =>
                      x.UserBranch.UserId == id && x.Branch.IsDelete == 0 && x.Branch.Status == 1 && x.UserBranch.Status == 1
                      ).AsEnumerable().Reverse().OrderBy(x => x.Branch.Name).Select(x => new
                      {
                          x.Branch.Id,
                          x.Branch.Code,
                          x.Branch.Name,
                          x.Branch.Address1,
                          x.Branch.PostalCode,
                          x.Branch.Area,
                          x.Branch.Region,
                          x.Branch.Status
                      }).Distinct().ToList();

                    return Ok(branchList);

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

        [HttpPost("userMenus")]
        public async Task<ActionResult> userMenus(long id,string branchCode)
        {
            try
            {

                List<UserMenu> userMenu = _context.UserMenus.Where(x => x.UserId == id && x.BranchCode == branchCode && x.Status == 1).ToList();

                return Ok(userMenu);


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
        public class menuAccess
        {

            public long? userId { get; set; }
            public int? selectedUserId { get; set; }
            public bool? transaction { get; set; }
            public bool? users { get; set; }
            public bool? branches { get; set; }
            public bool? menus { get; set; }
            public bool? customer { get; set; }
            public bool? charges { get; set; }
            public bool? terms { get; set; }
            public string? branchCode { get; set; }
            public bool? position { get; set; }
            

        }
        [HttpPost("userMenusSave")]
        public async Task<ActionResult> userMenusSave(menuAccess menuAccess)
        {
            try
            {

                List<UserMenu> userMenu = _context.UserMenus.Where(x => x.UserId == menuAccess.selectedUserId && x.BranchCode == menuAccess.branchCode).ToList();


                if (userMenu != null)
                {

                    if (menuAccess.transaction == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 3);

                        if(temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 3,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 3);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                    }


                    if (menuAccess.users == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 4);

                        if (temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 4,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 4);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                    }

                    if (menuAccess.branches == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 5);

                        if (temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 5,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 5);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                    }

                    if (menuAccess.menus == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 6);

                        if (temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 6,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 6);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                       
                    }
                    if (menuAccess.customer == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 7);

                        if (temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 7,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 7);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }

                    }

                    //
                    if (menuAccess.charges == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 8);

                        if (temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 8,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 8);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                    }
                    //
                    //
                    if (menuAccess.charges == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 9);

                        if (temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 9,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 9);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                    }

                    if (menuAccess.position == true)
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 10);

                        if (temp != null)
                        {
                            temp.Status = 1;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            _context.UserMenus.Add(new UserMenu
                            {
                                UserId = menuAccess.selectedUserId,
                                BranchCode = menuAccess.branchCode,
                                MenuId = 10,
                                Status = 1,
                                CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                                CreatedDate = DateTime.Now

                            });
                        }
                    }
                    else
                    {
                        UserMenu temp = userMenu.FirstOrDefault(x => x.MenuId == 10);
                        if (temp != null)
                        {
                            temp.Status = 0;
                            temp.ModifiedByUserId = int.Parse(menuAccess.userId.ToString());
                            temp.ModifiedDate = DateTime.Now;
                        }
                    }

                }
                else
                {
                    if(menuAccess.transaction == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 3,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now

                        });
                    }

                    if (menuAccess.users == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 4,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now

                        });
                    }

                    if (menuAccess.branches == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 5,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now

                        });
                    }

                    if (menuAccess.menus == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 6,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now
                        });
                    }
                    if (menuAccess.customer == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 7,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now
                        });
                    }
                    if (menuAccess.customer == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 8,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now
                        });
                    }
                    if (menuAccess.customer == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 9,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now
                        });
                    }
                    if (menuAccess.position == true)
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            UserId = menuAccess.selectedUserId,
                            BranchCode = menuAccess.branchCode,
                            MenuId = 10,
                            Status = 1,
                            CreatedByUserId = int.Parse(menuAccess.userId.ToString()),
                            CreatedDate = DateTime.Now
                        });
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

        [HttpPost("userMenusSidebar")]
        public async Task<ActionResult> userMenusSidebar(long id, string branchCode)
        {
            try
            {

                List<UserMenu> userMenu = _context.UserMenus.Where(x => x.UserId == id && x.BranchCode == branchCode && x.Status == 1).ToList();

                return Ok(userMenu);


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

        [HttpPost("dynamicMenu")]
        public async Task<ActionResult> dynamicMenu(long id, string branchCode)
        {
            try
            {

                List<String> userMenu = _context.UserMenus.Where(x => x.UserId == id && x.BranchCode == branchCode && x.Status == 1).Select(x => x.MenuId.ToString()).ToList();

                List<Menu> menuList = _context.Menus.Where(x => userMenu.Contains(x.Id.ToString())).ToList();

                List<Menu> parent = _context.Menus.Where(x => menuList.Select(s => s.ParentMenuId.ToString()).Contains(x.Id.ToString())).ToList();

                return Ok(new {menuList, parent});


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
