using IGB.Data;
using IGB.DTOs;
using IGB.Models.Users;
using IGB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace IGB.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentProfileController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public StudentProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        [HttpGet("StudentProfileTab1Get/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentProfileTab1Get(string studentId)
        {
            if (studentId == null)
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
                    long? DateOfBirth=null;

                    ApplicationUser Student = context.ApplicationUsers.Where(x => x.Id == studentId).FirstOrDefault();

                    var CurriculumName = context.Curriculums.Where(x => x.Id == Student.CurriculumId).Select(x=>x.Name).FirstOrDefault();
                    var GradeName = context.Grades.Where(x => x.Id == Student.GradeId).Select(x => x.Name).FirstOrDefault();

                    if (Student.DateOfBirth.HasValue)
                    {
                        DateOfBirth = (long)(DateTime.UtcNow - Student.DateOfBirth.Value).TotalMilliseconds;
             
                    }

                    if (Student != null)
                    {
                        var StudentTab1 = new
                        {
                            Id=Student.Id,
                            FirstName = Student.FirstName,
                            LastName = Student.LastName,
                            Email = Student.Email,
                            DateOfBirth = DateOfBirth,
                            Gender = Student.Gender,
                            Nationality = Student.Nationality,
                            ResidingCountry = Student.ResidingCountry,
                            TimeZone = Student.TimeZone,
                            LocalNumber = Student.LocalNumber,
                            WhatsappNumber = Student.WhatsappNumber,
                            CurriculumId = Student.CurriculumId,
                            CurriculumName = CurriculumName,
                            GradeId = Student.GradeId,
                            GradeName = GradeName,
                            HomeAddress = Student.HomeAddress,
                            Image = Student.Image,
                        };

                        var Response = new
                        {
                            Response = "1",
                            StudentTab1 = StudentTab1
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

        [HttpPost("StudentProfileTab1CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult StudentProfileTab1CreateUpdate([FromBody] StudentProfileTab1Dto StudentProfileTab1Dto)
        {
            if (StudentProfileTab1Dto.StudentId == null)
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
                    ApplicationUser = context.ApplicationUsers.Find(StudentProfileTab1Dto.StudentId);
                    var hashedPassword = userManager.PasswordHasher.HashPassword(ApplicationUser, StudentProfileTab1Dto.Password);

                    DateTime dob = new DateTime(1970, 1, 1).AddMilliseconds(StudentProfileTab1Dto.DateOfBirth.Value);
                    string formattedDate = dob.ToString("yyyyMMdd");

                    ApplicationUser.FirstName = StudentProfileTab1Dto.FirstName;
                    ApplicationUser.LastName = StudentProfileTab1Dto.LastName;

                    
                        //ApplicationUser.Email = StudentProfileTab1Dto.Email;
                        //ApplicationUser.UserName = StudentProfileTab1Dto.Email;
                        //ApplicationUser.NormalizedUserName = StudentProfileTab1Dto.Email.ToUpper();
                        //ApplicationUser.NormalizedEmail = StudentProfileTab1Dto.Email.ToUpper();
                       

                    ApplicationUser.PasswordHash = hashedPassword;
                    ApplicationUser.DateOfBirth = dob;
                    ApplicationUser.Gender = StudentProfileTab1Dto.Gender;
                    ApplicationUser.Nationality = StudentProfileTab1Dto.Nationality;
                    ApplicationUser.ResidingCountry = StudentProfileTab1Dto.ResidingCountry;
                    ApplicationUser.TimeZone = StudentProfileTab1Dto.TimeZone;
                    ApplicationUser.LocalNumber = StudentProfileTab1Dto.LocalNumber;
                    ApplicationUser.WhatsappNumber = StudentProfileTab1Dto.WhatsappNumber;
                    ApplicationUser.CurriculumId = StudentProfileTab1Dto.CurriculumId;
                    ApplicationUser.GradeId = StudentProfileTab1Dto.GradeId;
                    ApplicationUser.HomeAddress = StudentProfileTab1Dto.HomeAddress;
                    ApplicationUser.Image = StudentProfileTab1Dto.Image;
                    ApplicationUser.IsProfileUpdated = true;

                    context.SaveChanges();

                    var Response = new
                    {
                        Response = "1",
                        Message = "Student Profile Tab 1 Has Been Updated Successfully"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return Ok(jsonResponse);
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



        [HttpGet("StudentProfileTab2Get/{StudentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentProfileTab2Get(string StudentId)
        {
            if (StudentId == null)
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
                    ApplicationUser Student = context.ApplicationUsers.Where(x => x.Id == StudentId).FirstOrDefault();
                    ApplicationUser Guardian = context.ApplicationUsers.Where(x => x.StudentId == StudentId && x.IsFirstGuardian == true).FirstOrDefault();

                    if (Guardian != null && Student !=null)
                    {
                        var StudentTab2 = new
                        {
                            Id = StudentId,
                            Name = Guardian.FirstName,
                            Tag = Guardian.Tag,
                            RelationshipWithStudent = Guardian.RelationshipWithStudent,
                            Email = Guardian.Email,
                            LocalNumber = Guardian.LocalNumber,
                            WhatsappNumber = Guardian.WhatsappNumber,
                            HomeAddress = Guardian.HomeAddress,
                        };

                        var Response = new
                        {
                            Response = "1",
                            StudentTab2 = StudentTab2
                        };

                        string jsonResponse = JsonConvert.SerializeObject(Response);
                        return Ok(jsonResponse);

            
                    }
                    else if(Student!=null && Guardian==null)
                    {
                        var StudentTab2 = new
                        {
                            Id = StudentId,
                            Name = (string)null,
                            Tag = (string)null,
                            RelationshipWithStudent = (string)null,
                            Email = (string)null,
                            LocalNumber = (string)null,
                            WhatsappNumber = (string)null,
                            HomeAddress = (string)null
                        };

                        var Response = new
                        {
                            Response = "1",
                            StudentTab2 = StudentTab2
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

        [HttpPost("StudentProfileTab2CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult StudentProfileTab2CreateUpdate([FromBody] StudentProfileTab2Dto StudentProfileTab2Dto)
        {
            if (StudentProfileTab2Dto == null)
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
                    ApplicationUser = context.ApplicationUsers.Find(StudentProfileTab2Dto.StudentId);

                    var hashedPassword = userManager.PasswordHasher.HashPassword(ApplicationUser, StudentProfileTab2Dto.Password);
                    DateTime Date = DateTime.Now;
                    if (ApplicationUser != null)
                    {
                        if (ApplicationUser.IsFirstGuardianUpdated == false)
                        {
                            ApplicationUser.IsFirstGuardianUpdated = true;

                            var obj = new ApplicationUser
                            {
                                IsFirstGuardian = true,
                                StudentId = StudentProfileTab2Dto.StudentId,
                                FirstName = StudentProfileTab2Dto.Name,
                                RelationshipWithStudent = StudentProfileTab2Dto.RelationshipWithStudent,
                                LocalNumber = StudentProfileTab2Dto.LocalNumber,
                                WhatsappNumber = StudentProfileTab2Dto.WhatsappNumber,
                                HomeAddress = StudentProfileTab2Dto.HomeAddress,
                                UserName = StudentProfileTab2Dto.Email,
                                Email = StudentProfileTab2Dto.Email,
                                NormalizedUserName = StudentProfileTab2Dto.Email.ToUpper(),
                                NormalizedEmail = StudentProfileTab2Dto.Email.ToUpper(),
                                PasswordHash = hashedPassword,
                                Tag = "G" + Date.ToString("yyyyMMddHHmmss"),
                                RoleName = "First Guardian",
                                Update = "No Need To Update"
                            };
                            context.ApplicationUsers.Add(obj);
                            context.SaveChanges();

                            var obj1 = new ApplicationUserRole
                            {
                                UserId = obj.Id,
                                RoleId = "a2204359-8a88-49c9-867f-a2dd15e3f7c8",
                            };
                            context.ApplicationUserRoles.Add(obj1);
                            context.SaveChanges();


                            var Response = new
                            {
                                Response = "1",
                                Message = "Student Profile Tab 2 Has Been Created Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);

                        }
                        else
                        {
                            var data = context.ApplicationUsers.Where(x => x.StudentId == StudentProfileTab2Dto.StudentId && x.IsFirstGuardian == true).FirstOrDefault();
                            if (data != null)
                            {

                                data.FirstName = StudentProfileTab2Dto.Name;
                                data.RelationshipWithStudent = StudentProfileTab2Dto.RelationshipWithStudent;
                                data.LocalNumber = StudentProfileTab2Dto.LocalNumber;
                                data.WhatsappNumber = StudentProfileTab2Dto.WhatsappNumber;
                                data.HomeAddress = StudentProfileTab2Dto.HomeAddress;
                                //data.UserName = StudentProfileTab2Dto.Email;
                                //data.Email = StudentProfileTab2Dto.Email;
                                //data.NormalizedUserName = StudentProfileTab2Dto.Email.ToUpper();
                                //data.NormalizedEmail = StudentProfileTab2Dto.Email.ToUpper();
                                data.PasswordHash = hashedPassword;

                                context.ApplicationUsers.Update(data);
                                context.SaveChanges();

                                var Response = new
                                {
                                    Response = "1",
                                    Message = "Student Profile Tab 2 Has Been Updated Successfully"
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return Ok(jsonResponse);
                            }
                            else
                            {
                                var Response = new
                                {
                                    Response = "0",
                                    Error = "Invalid Credential"
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return BadRequest(jsonResponse);
                            }
                        }
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "Invalid Credentials"
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
                        Error = "Email Already Exists"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);
                }
            }
        }



        [HttpGet("StudentProfileTab3Get/{StudentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentProfileTab3Get(string StudentId)
        {
            if (StudentId == null)
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
                    ApplicationUser Student = context.ApplicationUsers.Where(x => x.Id == StudentId).FirstOrDefault();
                    ApplicationUser Guardian = context.ApplicationUsers.Where(x => x.StudentId == StudentId && x.IsSecondGuardian == true).FirstOrDefault();

                    if (Guardian != null && Student != null)
                    {
                        var StudentTab3 = new
                        {
                            Id = Guardian.StudentId,
                            Name = Guardian.FirstName,
                            Tag = Guardian.Tag,
                            RelationshipWithStudent = Guardian.RelationshipWithStudent,
                            Email = Guardian.Email,
                            LocalNumber = Guardian.LocalNumber,
                            WhatsappNumber = Guardian.WhatsappNumber,
                            HomeAddress = Guardian.HomeAddress,
                        };

                        var Response = new
                        {
                            Response = "1",
                            StudentTab3 = StudentTab3
                        };

                        string jsonResponse = JsonConvert.SerializeObject(Response);
                        return Ok(jsonResponse);
                    }
                    else if (Student != null && Guardian == null)
                    {
                        var StudentTab3 = new
                        {
                            Id = StudentId,
                            Name = (string)null,
                            Tag = (string)null,
                            RelationshipWithStudent = (string)null,
                            Email = (string)null,
                            LocalNumber = (string)null,
                            WhatsappNumber = (string)null,
                            HomeAddress = (string)null
                        };

                        var Response = new
                        {
                            Response = "1",
                            StudentTab3 = StudentTab3
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

        [HttpPost("StudentProfileTab3CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult StudentProfileTab3CreateUpdate([FromBody] StudentProfileTab3Dto StudentProfileTab3Dto)
        {
            if (StudentProfileTab3Dto == null)
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
                    ApplicationUser = context.ApplicationUsers.Find(StudentProfileTab3Dto.StudentId);
                    var hashedPassword = userManager.PasswordHasher.HashPassword(ApplicationUser, StudentProfileTab3Dto.Password);
                    DateTime Date = DateTime.Now;

                    if (ApplicationUser != null)
                    {
                        if (ApplicationUser.IsSecondGuardian == false)
                        {
                            ApplicationUser.IsSecondGuardianUpdated = true;

                            var obj = new ApplicationUser
                            {
                                IsSecondGuardian = true,
                                StudentId = StudentProfileTab3Dto.StudentId,
                                FirstName = StudentProfileTab3Dto.Name,
                                RelationshipWithStudent = StudentProfileTab3Dto.RelationshipWithStudent,
                                LocalNumber = StudentProfileTab3Dto.LocalNumber,
                                WhatsappNumber = StudentProfileTab3Dto.WhatsappNumber,
                                HomeAddress = StudentProfileTab3Dto.HomeAddress,
                                UserName = StudentProfileTab3Dto.Email,
                                Email = StudentProfileTab3Dto.Email,
                                NormalizedUserName = StudentProfileTab3Dto.Email.ToUpper(),
                                NormalizedEmail = StudentProfileTab3Dto.Email.ToUpper(),
                                PasswordHash = hashedPassword,
                                Tag = "G" + Date.ToString("yyyyMMddHHmmss"),
                                RoleName = "Second Guardian",
                                Update = "No Need To Update"
                            };
                            context.ApplicationUsers.Add(obj);
                            context.SaveChanges();

                            var obj1 = new ApplicationUserRole
                            {
                                UserId = obj.Id,
                                RoleId = "a2204359-8a88-49c9-867f-a2dd15e3f7c8",
                            };
                            context.ApplicationUserRoles.Add(obj1);
                            context.SaveChanges();


                            var Response = new
                            {
                                Response = "1",
                                Message = "Student Profile Tab 3 Has Been Created Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var data = context.ApplicationUsers.Where(x => x.StudentId == StudentProfileTab3Dto.StudentId && x.IsSecondGuardian == true).FirstOrDefault();
                            if (data != null)
                            {
                                data.FirstName = StudentProfileTab3Dto.Name;
                                data.RelationshipWithStudent = StudentProfileTab3Dto.RelationshipWithStudent;
                                data.LocalNumber = StudentProfileTab3Dto.LocalNumber;
                                data.WhatsappNumber = StudentProfileTab3Dto.WhatsappNumber;
                                data.HomeAddress = StudentProfileTab3Dto.HomeAddress;
                                //data.UserName = StudentProfileTab3Dto.Email;
                                //data.Email = StudentProfileTab3Dto.Email;
                                //data.NormalizedUserName = StudentProfileTab3Dto.Email.ToUpper();
                                //data.NormalizedEmail = StudentProfileTab3Dto.Email.ToUpper();
                                data.PasswordHash = hashedPassword;

                                context.ApplicationUsers.Update(data);
                                context.SaveChanges();

                                var Response = new
                                {
                                    Response = "1",
                                    Message = "Student Profile Tab 3 Has Been Updated Successfully"
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return Ok(jsonResponse);
                            }
                            else
                            {
                                var Response = new
                                {
                                    Response = "0",
                                    Error = "Invalid Credential"
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return BadRequest(jsonResponse);
                            }
                        }
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "Invalid Credentials"
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
                        Error = "Email Already Exists"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);
                }
            }
        }


    }
}
