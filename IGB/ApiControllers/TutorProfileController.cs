using IGB.Data;
using IGB.DTOs;
using IGB.Models.Admin;
using IGB.Models.Tutor;
using IGB.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;

namespace IGB.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorProfileController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public TutorProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("TutorProfileTab1Get/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorProfileTab1Get(string tutorId)
        {
            if (tutorId == null)
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
                    long? DateOfBirth = null;

                    ApplicationUser Tutor = context.ApplicationUsers.Where(x => x.Id == tutorId).FirstOrDefault();


                    if (Tutor.DateOfBirth.HasValue)
                    {
                        DateOfBirth = (long)(DateTime.UtcNow - Tutor.DateOfBirth.Value).TotalMilliseconds;

                    }

                    if (Tutor != null)
                    {
                        var TutorTab1 = new
                        {
                            Id = Tutor.Id,
                            FirstName = Tutor.FirstName,
                            LastName = Tutor.LastName,
                            Email = Tutor.Email,
                            TutorType = Tutor.EmployeeType,
                            OtherTutorType = Tutor.OtherEmployeeType,
                            ProfileLink = Tutor.ProfileLink,
                            DateOfBirth = DateOfBirth,
                            Gender = Tutor.Gender,
                            Nationality = Tutor.Nationality,
                            ResidingCountry = Tutor.ResidingCountry,
                            TimeZone = Tutor.TimeZone,
                            LocalNumber = Tutor.LocalNumber,
                            WhatsappNumber = Tutor.WhatsappNumber,
                            HomeAddress = Tutor.HomeAddress,
                            Image = Tutor.Image,
                        };

                        var Response = new
                        {
                            Response = "1",
                            TutorTab1 = TutorTab1
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

        [HttpPost("TutorProfileTab1CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab1CreateUpdate([FromBody] TutorProfileTab1Dto TutorProfileTab1Dto)
        {
            if (TutorProfileTab1Dto == null)
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
                    ApplicationUser = context.ApplicationUsers.Find(TutorProfileTab1Dto.TutorId);
                    var hashedPassword = userManager.PasswordHasher.HashPassword(ApplicationUser, TutorProfileTab1Dto.Password);

                    DateTime dob = new DateTime(1970, 1, 1).AddMilliseconds(TutorProfileTab1Dto.DateOfBirth.Value);
                    string formattedDate = dob.ToString("yyyyMMdd");

                    ApplicationUser.FirstName = TutorProfileTab1Dto.FirstName;
                    ApplicationUser.LastName = TutorProfileTab1Dto.LastName;

                    //ApplicationUser.Email = TutorProfileTab1Dto.Email;
                    //ApplicationUser.UserName = TutorProfileTab1Dto.Email;
                    //ApplicationUser.NormalizedUserName = TutorProfileTab1Dto.Email.ToUpper();
                    //ApplicationUser.NormalizedEmail = TutorProfileTab1Dto.Email.ToUpper();

                    ApplicationUser.PasswordHash = hashedPassword;
                    ApplicationUser.EmployeeType = TutorProfileTab1Dto.TutorType;
                    if (TutorProfileTab1Dto.TutorType == "Other")
                    {
                        ApplicationUser.OtherEmployeeType = TutorProfileTab1Dto.OtherTutorType;
                    }
                    else
                    {
                        ApplicationUser.OtherEmployeeType = null;
                    }
                    ApplicationUser.ProfileLink = TutorProfileTab1Dto.ProfileLink;
                    ApplicationUser.DateOfBirth = dob;
                    ApplicationUser.Gender = TutorProfileTab1Dto.Gender;
                    ApplicationUser.Nationality = TutorProfileTab1Dto.Nationality;
                    ApplicationUser.ResidingCountry = TutorProfileTab1Dto.ResidingCountry;
                    ApplicationUser.TimeZone = TutorProfileTab1Dto.TimeZone;
                    ApplicationUser.LocalNumber = TutorProfileTab1Dto.LocalNumber;
                    ApplicationUser.WhatsappNumber = TutorProfileTab1Dto.WhatsappNumber;
                    ApplicationUser.HomeAddress = TutorProfileTab1Dto.HomeAddress;
                    ApplicationUser.Image = TutorProfileTab1Dto.Image;
                    ApplicationUser.IsProfileUpdated = true;
                    context.SaveChanges();

                    var Response = new
                    {
                        Response = "1",
                        Message = "Tutor Profile Tab 1 Has Been Updated Successfully"
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



        [HttpGet("TutorProfileTab2Get/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorProfileTab2Get(string tutorId)
        {
            if (tutorId == null)
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
                    List<TutorSpecialityDto> TutorTab2 = context.TutorSpecialitys
         .Where(x => x.TutorId == tutorId)
         .Select(x => new TutorSpecialityDto
         {
             Id = x.Id,
             CurriculumId = x.CurriculumId,
             CurriculumName = x.CurriculumName,
             GradeId = x.GradeId,
             GradeName = x.GradeName,
             CourseId = x.CourseId,
             CourseName = x.CourseName,
             ExpertiseLevel = x.ExpLevel,
         })
         .ToList();


                    if (TutorTab2.Any())
                    {
                        var response = new
                        {
                            Response = "1",
                            Id = tutorId,
                            TutorTab2 = TutorTab2
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Record Found"
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

        [HttpPost("TutorProfileTab2CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab2CreateUpdate([FromBody] TutorProfileTab2Dto TutorProfileTab2Dto)
        {
            if (TutorProfileTab2Dto == null)
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
                    if (TutorProfileTab2Dto.Id == null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(TutorProfileTab2Dto.TutorId);

                        if (ApplicationUser != null)
                        {
                            ApplicationUser.IsSpecialiitiesUpdated = true;

                            string? CurriculumName = context.Curriculums.Where(x => x.Id == TutorProfileTab2Dto.CurriculumId).Select(x => x.Name).FirstOrDefault();
                            string? GradeName = context.Grades.Where(x => x.Id == TutorProfileTab2Dto.GradeId).Select(x => x.Name).FirstOrDefault();
                            string? CourseName = context.Courses.Where(x => x.Id == TutorProfileTab2Dto.CourseId).Select(x => x.Name).FirstOrDefault();

                            var Speciality = new TutorSpeciality
                            {
                                TutorId = TutorProfileTab2Dto.TutorId,
                                CurriculumId = TutorProfileTab2Dto.CurriculumId,
                                CurriculumName = CurriculumName,
                                GradeId = TutorProfileTab2Dto.GradeId,
                                GradeName = GradeName,
                                CourseId = TutorProfileTab2Dto.CourseId,
                                CourseName = CourseName,
                                ExpLevel = TutorProfileTab2Dto.ExpertiseLevel
                            };
                            context.TutorSpecialitys.Add(Speciality);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 2 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return BadRequest(jsonResponse);
                        }
                    }
                    else
                    {
                        TutorSpeciality TutorSpeciality = new TutorSpeciality();
                        TutorSpeciality = context.TutorSpecialitys.Find(TutorProfileTab2Dto.Id);

                        if (TutorSpeciality != null)
                        {
                            string? CurriculumName = context.Curriculums.Where(x => x.Id == TutorProfileTab2Dto.CurriculumId).Select(x => x.Name).FirstOrDefault();
                            string? GradeName = context.Grades.Where(x => x.Id == TutorProfileTab2Dto.GradeId).Select(x => x.Name).FirstOrDefault();
                            string? CourseName = context.Courses.Where(x => x.Id == TutorProfileTab2Dto.CourseId).Select(x => x.Name).FirstOrDefault();

                            TutorSpeciality.CurriculumId = TutorProfileTab2Dto.CurriculumId;
                            TutorSpeciality.CurriculumName = CurriculumName;
                            TutorSpeciality.GradeId = TutorProfileTab2Dto.GradeId;
                            TutorSpeciality.GradeName = GradeName;
                            TutorSpeciality.CourseId = TutorProfileTab2Dto.CourseId;
                            TutorSpeciality.CourseName = CourseName;
                            TutorSpeciality.ExpLevel = TutorProfileTab2Dto.ExpertiseLevel;

                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 2 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Record Not Found"
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

        [HttpPost("TutorProfileTab2CreateInList/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab2CreateInList(string tutorId, [FromBody] List<TutorProfileTab2DtoList> TutorProfileTab2Dto)
        {
            if (!TutorProfileTab2Dto.Any())
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
                    if (tutorId != null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(tutorId);

                        if (ApplicationUser != null)
                        {
                            ApplicationUser.IsSpecialiitiesUpdated = true;

                            foreach (var item in TutorProfileTab2Dto)
                            {
                                string? CurriculumName = context.Curriculums.Where(x => x.Id == item.CurriculumId).Select(x => x.Name).FirstOrDefault();
                                string? GradeName = context.Grades.Where(x => x.Id == item.GradeId).Select(x => x.Name).FirstOrDefault();
                                string? CourseName = context.Courses.Where(x => x.Id == item.CourseId).Select(x => x.Name).FirstOrDefault();

                                var Speciality = new TutorSpeciality
                                {
                                    TutorId = tutorId,
                                    CurriculumId = item.CurriculumId,
                                    CurriculumName = CurriculumName,
                                    GradeId = item.GradeId,
                                    GradeName = GradeName,
                                    CourseId = item.CourseId,
                                    CourseName = CourseName,
                                    ExpLevel = item.ExpertiseLevel
                                };
                                context.TutorSpecialitys.Add(Speciality);
                                context.SaveChanges();
                            }

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 2 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
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
                            Error = "Credentials Error"
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







        [HttpGet("TutorProfileTab3Get/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentProfileTab3Get(string tutorId)
        {
            if (tutorId == null)
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
                    List<TutorEducationDto> TutorTab3 = context.TutorEducations
        .Where(x => x.TutorId == tutorId)
        .Select(x => new TutorEducationDto
        {
            Id = x.Id,
            UniversityLevel = x.UniversityLevel,
            IntermediateLevel = x.IntermediateLevel,
            OtherIntermediateLevel = x.OtherIntermediateLevel,
            MatriculationLevel = x.MatriculationLevel,
            OtherMatriculationLevel = x.OtherMatriculationLevel,
            Faculty = x.Faculty,
            StartYear = x.StartYear,
            EndYear = x.EndYear
        })
        .ToList();
                    if (TutorTab3.Any())
                    {
                        var response = new
                        {
                            Response = "1",
                            Id = tutorId,
                            TutorTab3 = TutorTab3
                        };


                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Record Found"
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

        [HttpPost("TutorProfileTab3CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab3CreateUpdate([FromBody] TutorProfileTab3Dto TutorProfileTab3Dto)
        {
            if (TutorProfileTab3Dto == null)
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
                    if (TutorProfileTab3Dto.Id == null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(TutorProfileTab3Dto.TutorId);

                        if (ApplicationUser != null)
                        {
                            ApplicationUser.IsEducationUpdated = true;

                            var Education = new TutorEducation
                            {
                                TutorId = TutorProfileTab3Dto.TutorId,
                                UniversityLevel = TutorProfileTab3Dto.UniversityLevel,
                                IntermediateLevel = TutorProfileTab3Dto.IntermediateLevel,
                                OtherIntermediateLevel = TutorProfileTab3Dto.OtherIntermediateLevel,
                                MatriculationLevel = TutorProfileTab3Dto.MatriculationLevel,
                                OtherMatriculationLevel = TutorProfileTab3Dto.OtherMatriculationLevel,
                                Faculty = TutorProfileTab3Dto.Faculty,
                                StartYear = TutorProfileTab3Dto.StartYear,
                                EndYear = TutorProfileTab3Dto.EndYear,

                            };
                            context.TutorEducations.Add(Education);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 3 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return BadRequest(jsonResponse);
                        }
                    }
                    else
                    {
                        TutorEducation TutorEducation = new TutorEducation();
                        TutorEducation = context.TutorEducations.Find(TutorProfileTab3Dto.Id);

                        if (TutorEducation != null)
                        {
                            TutorEducation.UniversityLevel = TutorProfileTab3Dto.UniversityLevel;
                            TutorEducation.IntermediateLevel = TutorProfileTab3Dto.IntermediateLevel;
                            TutorEducation.OtherIntermediateLevel = TutorProfileTab3Dto.OtherIntermediateLevel;
                            TutorEducation.MatriculationLevel = TutorProfileTab3Dto.MatriculationLevel;
                            TutorEducation.OtherMatriculationLevel = TutorProfileTab3Dto.OtherMatriculationLevel;
                            TutorEducation.Faculty = TutorProfileTab3Dto.Faculty;
                            TutorEducation.StartYear = TutorProfileTab3Dto.StartYear;
                            TutorEducation.EndYear = TutorProfileTab3Dto.EndYear;

                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 3 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Record Not Found"
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

        [HttpPost("TutorProfileTab3CreateInList/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab3CreateInList(string tutorId, [FromBody] List<TutorProfileTab3DtoList> TutorProfileTab3Dto)
        {
            if (TutorProfileTab3Dto == null)
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
                    if (tutorId != null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(tutorId);

                        if (ApplicationUser != null)
                        {
                            ApplicationUser.IsEducationUpdated = true;

                            foreach (var item in TutorProfileTab3Dto)
                            {
                                var Education = new TutorEducation
                                {
                                    TutorId = tutorId,
                                    UniversityLevel = item.UniversityLevel,
                                    IntermediateLevel = item.IntermediateLevel,
                                    OtherIntermediateLevel = item.OtherIntermediateLevel,
                                    MatriculationLevel = item.MatriculationLevel,
                                    OtherMatriculationLevel = item.OtherMatriculationLevel,
                                    Faculty = item.Faculty,
                                    StartYear = item.StartYear,
                                    EndYear = item.EndYear,

                                };
                                context.TutorEducations.Add(Education);
                                context.SaveChanges();
                            }
                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 3 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
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
                            Error = "Credentials Error"
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





        [HttpGet("TutorProfileTab4Get/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorProfileTab4Get(string tutorId)
        {
            if (tutorId == null)
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
                    List<TutorExperienceDto> TutorTab4 = context.TutorExperiences
.Where(x => x.TutorId == tutorId)
.Select(x => new TutorExperienceDto
{
    Id = x.Id,
    InstituteName = x.InstituteName,
    Designation = x.Designation,
    CurriculumId = x.CurriculumId,
    CurriculumName = x.CurriculumName,
    CourseId = x.CourseId,
    CourseName = x.CourseName,
    StartYear = x.StartYear,
    EndYear = x.EndYear
})
.ToList();



                    if (TutorTab4.Count > 0)
                    {
                        var response = new
                        {
                            Response = "1",
                            Id = tutorId,
                            TutorTab4 = TutorTab4
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Record Found"
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

        [HttpPost("TutorProfileTab4CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab4CreateUpdate([FromBody] TutorProfileTab4Dto TutorProfileTab4Dto)
        {
            if (TutorProfileTab4Dto == null)
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
                    if (TutorProfileTab4Dto.Id == null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(TutorProfileTab4Dto.TutorId);

                        if (ApplicationUser != null)
                        {
                            string? CurriculumName = context.Curriculums.Where(x => x.Id == TutorProfileTab4Dto.CurriculumId).Select(x => x.Name).FirstOrDefault();
                            string? CourseName = context.Courses.Where(x => x.Id == TutorProfileTab4Dto.CourseId).Select(x => x.Name).FirstOrDefault();


                            ApplicationUser.IsExperienceUpdated = true;

                            var Experience = new TutorExperience
                            {
                                TutorId = TutorProfileTab4Dto.TutorId,
                                InstituteName = TutorProfileTab4Dto.InstituteName,
                                Designation = TutorProfileTab4Dto.Designation,
                                CurriculumId = TutorProfileTab4Dto.CurriculumId,
                                CurriculumName = CurriculumName,
                                CourseId = TutorProfileTab4Dto.CourseId,
                                CourseName = CourseName,
                                StartYear = TutorProfileTab4Dto.StartYear,
                                EndYear = TutorProfileTab4Dto.EndYear,

                            };
                            context.TutorExperiences.Add(Experience);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 4 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return BadRequest(jsonResponse);
                        }
                    }
                    else
                    {
                        TutorExperience TutorExperience = new TutorExperience();
                        TutorExperience = context.TutorExperiences.Find(TutorProfileTab4Dto.Id);

                        if (TutorExperience != null)
                        {
                            string? CurriculumName = context.Curriculums.Where(x => x.Id == TutorProfileTab4Dto.CurriculumId).Select(x => x.Name).FirstOrDefault();
                            string? CourseName = context.Courses.Where(x => x.Id == TutorProfileTab4Dto.CourseId).Select(x => x.Name).FirstOrDefault();


                            TutorExperience.InstituteName = TutorProfileTab4Dto.InstituteName;
                            TutorExperience.InstituteName = TutorProfileTab4Dto.InstituteName;
                            TutorExperience.Designation = TutorProfileTab4Dto.Designation;
                            TutorExperience.CurriculumId = TutorProfileTab4Dto.CurriculumId;
                            TutorExperience.CurriculumName = CurriculumName;
                            TutorExperience.CourseId = TutorProfileTab4Dto.CourseId;
                            TutorExperience.CourseName = CourseName;
                            TutorExperience.StartYear = TutorProfileTab4Dto.StartYear;
                            TutorExperience.EndYear = TutorProfileTab4Dto.EndYear;

                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 4 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Record Not Found"
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

        [HttpPost("TutorProfileTab4CreateInList/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab4CreateInList(string tutorId, [FromBody] List<TutorProfileTab4DtoList> TutorProfileTab4Dto)
        {
            if (TutorProfileTab4Dto == null)
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
                    if (tutorId != null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(tutorId);

                        if (ApplicationUser != null)
                        {
                            ApplicationUser.IsExperienceUpdated = true;

                            foreach (var item in TutorProfileTab4Dto)
                            {

                                string? CurriculumName = context.Curriculums.Where(x => x.Id == item.CurriculumId).Select(x => x.Name).FirstOrDefault();
                                string? CourseName = context.Courses.Where(x => x.Id == item.CourseId).Select(x => x.Name).FirstOrDefault();

                                var Experience = new TutorExperience
                                {
                                    TutorId = tutorId,
                                    InstituteName = item.InstituteName,
                                    Designation = item.Designation,
                                    CurriculumId = item.CurriculumId,
                                    CurriculumName = CurriculumName,
                                    CourseId = item.CourseId,
                                    CourseName = CourseName,
                                    StartYear = item.StartYear,
                                    EndYear = item.EndYear,

                                };
                                context.TutorExperiences.Add(Experience);
                                context.SaveChanges();
                            }
                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 4 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }

                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
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
                            Error = "Credentials Error"
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




        [HttpGet("TutorProfileTab5Get/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorProfileTab5Get(string tutorId)
        {
            if (tutorId == null)
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
                    List<TutorDocumentDto> TutorTab5 = context.TutorDocuments
.Where(x => x.TutorId == tutorId)
.Select(x => new TutorDocumentDto
{
    Id = x.Id,
    Information = x.Information,
    FileType = x.FileType,
    FileName = x.FileName,
    File = x.File
})
.ToList();



                    if (TutorTab5.Count > 0)
                    {
                        var response = new
                        {
                            Response = "1",
                            Id = tutorId,
                            TutorTab5 = TutorTab5
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Record Found"
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

        [HttpPost("TutorProfileTab5CreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab5CreateUpdate([FromBody] TutorProfileTab5Dto TutorProfileTab5Dto)
        {
            if (TutorProfileTab5Dto == null)
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
                    if (TutorProfileTab5Dto.Id == null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(TutorProfileTab5Dto.TutorId);

                        if (ApplicationUser != null)
                        {
                            ApplicationUser.IsDocumentUpdated = true;

                            var Document = new TutorDocument
                            {
                                TutorId = TutorProfileTab5Dto.TutorId,
                                Information = TutorProfileTab5Dto.Information,
                                FileType = TutorProfileTab5Dto.FileType,
                                FileName = TutorProfileTab5Dto.FileName,
                                File = TutorProfileTab5Dto.File,
                            };
                            context.TutorDocuments.Add(Document);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 5 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return BadRequest(jsonResponse);
                        }
                    }
                    else
                    {
                        TutorDocument TutorDocument = new TutorDocument();
                        TutorDocument = context.TutorDocuments.Find(TutorProfileTab5Dto.Id);

                        if (TutorDocument != null)
                        {
                            TutorDocument.Information = TutorProfileTab5Dto.Information;
                            TutorDocument.FileType = TutorProfileTab5Dto.FileType;
                            TutorDocument.FileName = TutorProfileTab5Dto.FileName;
                            TutorDocument.File = TutorProfileTab5Dto.File;

                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 5 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Record Not Found"
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

        
        [HttpPost("TutorProfileTab5CreatInList/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult TutorProfileTab5CreatInList(string tutorId , [FromBody] List<TutorProfileTab5DtoList> TutorProfileTab5Dto)
        {
            if (TutorProfileTab5Dto == null)
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
                    if (tutorId != null)
                    {
                        ApplicationUser? ApplicationUser = new ApplicationUser();
                        ApplicationUser = context.ApplicationUsers.Find(tutorId);

                        if (ApplicationUser != null)
                        {
                            ApplicationUser.IsDocumentUpdated = true;

                            foreach (var item in TutorProfileTab5Dto)
                            {
                                var Document = new TutorDocument
                                {
                                    TutorId = tutorId,
                                    Information = item.Information,
                                    FileType = item.FileType,
                                    FileName = item.FileName,
                                    File = item.File,
                                };
                                context.TutorDocuments.Add(Document);
                                context.SaveChanges();
                            }
                            var Response = new
                            {
                                Response = "1",
                                Message = "Tutor Profile Tab 5 Has Been Updated Successfully"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Tutor Not Found"
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
                            Error = "Credentials Error"
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