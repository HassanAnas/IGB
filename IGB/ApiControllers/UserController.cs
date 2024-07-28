using IGB.Data;
using IGB.DTOs;
using IGB.Models;
using IGB.Models.Users;
using IGB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IGB.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly EmailService emailservice;
        private readonly JwtTokenGeneratorService JwtTokenGeneratorService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration _configuration;
        private readonly string? BaseUrl;
        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, EmailService emailservice, SignInManager<IdentityUser> signInManager, IConfiguration configuration, JwtTokenGeneratorService jwtTokenGeneratorService)
        {
            this.context = context;
            this.userManager = userManager;
            this.emailservice = emailservice;
            this.signInManager = signInManager;
            _configuration = configuration;
            BaseUrl = _configuration["BaseUrl:BaseUrl"];
            JwtTokenGeneratorService = jwtTokenGeneratorService;
        }

        [HttpPost("UserCreate")]
        public IActionResult UserCreate([FromBody] UserCreateDto UserCreateDto)
        {
            if (UserCreateDto.RoleId == null || UserCreateDto.Email == null || UserCreateDto.Password == null)
            {
                var Response = new
                {
                    Response = "0",
                    Error = "Invalid request"
                };

                string jsonResponse = JsonConvert.SerializeObject(Response);
                return BadRequest(jsonResponse);
            }
            else
            {
                try
                {

                    var ApplicationUser = new ApplicationUser();
                    var hashedPassword = userManager.PasswordHasher.HashPassword(ApplicationUser, UserCreateDto.Password);
                    DateTime Date = DateTime.Now;
                    string? RoleName = "";

                    if (UserCreateDto.RoleId == "de8dc073-416f-4716-89be-b1c48c9f9de1")
                    {
                        ApplicationUser.Tag = "A" + Date.ToString("yyyyMMddHHmmss");
                        ApplicationUser.IsAdmin = true;
                        RoleName = "Admin";
                    }
                    else if (UserCreateDto.RoleId == "2c98e431-4a03-4999-807d-d7ac6ebedac9")
                    {
                        ApplicationUser.Tag = "T" + Date.ToString("yyyyMMddHHmmss");
                        ApplicationUser.IsTutor = true;
                        RoleName = "Tutor";
                    }
                    else if (UserCreateDto.RoleId == "feb14f16-e7ac-43bf-9bd2-e7de8d1ba71d")
                    {
                        ApplicationUser.Tag = "S" + Date.ToString("yyyyMMddHHmmss");
                        ApplicationUser.IsStudent = true;
                        RoleName = "Student";
                    }

                    var user = new ApplicationUser
                    {
                        Email = UserCreateDto.Email,
                        UserName = UserCreateDto.Email,
                        NormalizedUserName = UserCreateDto.Email.ToUpper(),
                        NormalizedEmail = UserCreateDto.Email.ToUpper(),
                        PasswordHash = hashedPassword,
                        Tag = ApplicationUser.Tag,
                        IsAdmin = ApplicationUser.IsAdmin,
                        IsTutor = ApplicationUser.IsTutor,
                        IsStudent = ApplicationUser.IsStudent,
                        RoleName = RoleName
                    };

                    context.ApplicationUsers.Add(user);

                    var obj1 = new ApplicationUserRole
                    {
                        RoleId = UserCreateDto.RoleId,
                        UserId = user.Id
                    };

                    context.ApplicationUserRoles.Add(obj1);

                    var notification = new AllNotification
                    {
                        NewUserId = user.Id,
                        ForAdmin = true,
                        Notification = "A New User Account Registration Need To Be Approved"
                    };

                    context.AllNotifications.Add(notification);
                    context.SaveChanges();

                    string title = "Confirm Your IGB Account Registration";
                    string msg = $"Please <a href=\"{BaseUrl}/UserApprove/{user.Id}/{notification.Id}\">click here</a> to approve the user.";
                    emailservice.SendEmail(UserCreateDto.Email, title, msg);

                    var Response = new
                    {
                        Response = "1",
                        Message = "User Has Been Created Successfully And Email Has Been Sent To You To Verify Your Account"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return Ok(jsonResponse);
                }
                catch (Exception ex)
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "Email Already Exists"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);
                }
            }
        }


        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginDto UserLoginDto)
        {
            if (UserLoginDto.Email == null || UserLoginDto.Password == null)
            {
                var Response = new
                {
                    Response = "0",
                    Error = "Invalid request"
                };

                string jsonResponse = JsonConvert.SerializeObject(Response);
                return BadRequest(jsonResponse);
            }
            else
            {
                try
                {
                    var LoginUser = context.ApplicationUsers.Where(x => x.Email == UserLoginDto.Email).FirstOrDefault();
               

                    long? DateOfBirth = null;
                    if (LoginUser.DateOfBirth.HasValue)
                    {
                        DateOfBirth = (long)(DateTime.UtcNow - LoginUser.DateOfBirth.Value).TotalMilliseconds;
                    }

                    if (LoginUser == null)
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "User not found"
                        };

                        string jsonResponse = JsonConvert.SerializeObject(Response);
                        return BadRequest(jsonResponse);
                    }
                    else
                    {
                        // Use UserManager to verify the password
                        var result = await signInManager.CheckPasswordSignInAsync(LoginUser, UserLoginDto.Password, false);

                        string? Token = JwtTokenGeneratorService.GenerateToken(LoginUser);

                        if (result.Succeeded)
                        {
                            var RoleId = context.ApplicationUserRoles.Where(x => x.UserId == LoginUser.Id).Select(x => x.RoleId).FirstOrDefault();
                            var RoleName = context.ApplicationRoles.Where(x => x.Id == RoleId).Select(x => x.Name).FirstOrDefault();


                            if (RoleName == "Tutor")
                            {
                                var Response = new
                                {
                                    Response = "1",
                                    Token= Token,
                                    Id = LoginUser.Id,
                                    Email = LoginUser.Email,
                                    Tag = LoginUser.Tag,
                                    RoleName = LoginUser.RoleName,
                                    FirstName = LoginUser.FirstName,
                                    LastName = LoginUser.LastName,
                                    DateOfBirth = DateOfBirth,
                                    Gender = LoginUser.Gender,
                                    Nationality = LoginUser.Nationality,
                                    ResidingCountry = LoginUser.ResidingCountry,
                                    TimeZone = LoginUser.TimeZone,
                                    LocalNumber = LoginUser.LocalNumber,
                                    WhatsappNumber = LoginUser.WhatsappNumber,
                                    HomeAddress = LoginUser.HomeAddress,
                                    Image = LoginUser.Image,
                                    ProfileLink = LoginUser.ProfileLink,
                                    Type = LoginUser.EmployeeType,
                                    TypeOther = LoginUser.OtherEmployeeType,
                                    Contract = LoginUser.Contract,
                                    IsActive = LoginUser.IsActive,
                                    IsUpdated = LoginUser.IsUpdated,
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return Ok(jsonResponse);
                            }
                            else if (RoleName == "Student")
                            {
                                var Response = new
                                {
                                    Token = Token,
                                    Response = "1",
                                    Id = LoginUser.Id,
                                    Email = LoginUser.Email,
                                    Tag = LoginUser.Tag,
                                    RoleName = LoginUser.RoleName,
                                    FirstName = LoginUser.FirstName,
                                    LastName = LoginUser.LastName,
                                    DateOfBirth = DateOfBirth,
                                    Gender = LoginUser.Gender,
                                    Nationality = LoginUser.Nationality,
                                    ResidingCountry = LoginUser.ResidingCountry,
                                    TimeZone = LoginUser.TimeZone,
                                    LocalNumber = LoginUser.LocalNumber,
                                    WhatsappNumber = LoginUser.WhatsappNumber,
                                    CurriculumId = LoginUser.CurriculumId,
                                    GradeId = LoginUser.GradeId,
                                    HomeAddress = LoginUser.HomeAddress,
                                    Image = LoginUser.Image,
                                    RegularIrregular = LoginUser.RegularIrregular,
                                    ExtraCaring = LoginUser.ExtraCaring,
                                    IsActive = LoginUser.IsActive,
                                    IsUpdated = LoginUser.IsUpdated,
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return Ok(jsonResponse);
                            }
                            else if (RoleName == "Guardian")
                            {
                                var Response = new
                                {
                                    Token = Token,
                                    Response = "1",
                                    Id = LoginUser.Id,
                                    Email = LoginUser.Email,
                                    Tag = LoginUser.Tag,
                                    RoleName = LoginUser.RoleName,
                                    FirstName = LoginUser.FirstName,
                                    LocalNumber = LoginUser.LocalNumber,
                                    WhatsaapNumber = LoginUser.WhatsappNumber,
                                    HomeAddress = LoginUser.HomeAddress,
                                    StudentId = LoginUser.StudentId,
                                    RelationshipWithStudent = LoginUser.RelationshipWithStudent,
                                    IsActive = LoginUser.IsActive,
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return Ok(jsonResponse);
                            }
                            else
                            {
                                var Response = new
                                {
                                    Response = "0",
                                    Error = "Role Is Not Correct"

                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return BadRequest(jsonResponse);
                            }

                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Invalid credentials"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return BadRequest(jsonResponse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "Something Went Wrong"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);
                }
            }
        }


        [HttpGet("UserStatus/{UserId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> UserStatus(string UserId)
        {
            if (UserId == null)
            {
                var Response = new
                {
                    Response = "0",
                    Error = "Invalid request"
                };

                string jsonResponse = JsonConvert.SerializeObject(Response);
                return BadRequest(jsonResponse);
            }
            else
            {
                try
                {
                    bool UserStatus = context.ApplicationUsers.Where(x => x.Id == UserId).Select(x => x.IsActive).FirstOrDefault();

                    if (UserStatus != null)
                    {
                        var Response = new
                        {
                            Response = "1",
                            UserStatus = UserStatus
                        };

                        string jsonResponse = JsonConvert.SerializeObject(Response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "Invalid credentials"
                        };

                        string jsonResponse = JsonConvert.SerializeObject(Response);
                        return BadRequest(jsonResponse);
                    }


                }
                catch (Exception ex)
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "Something Went Wrong"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);
                }
            }
        }


    }

}


