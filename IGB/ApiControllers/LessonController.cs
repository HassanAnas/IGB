using IGB.Data;
using IGB.DTOs;
using IGB.Models;
using IGB.Models.Users;
using IGB.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using IGB.Models.Admin;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using IGB.Models.Tutor;

namespace IGB.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly EmailService emailservice;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly TimeConversionService TimeConversionService;

        public LessonController(ApplicationDbContext context, UserManager<IdentityUser> userManager, EmailService emailservice, SignInManager<IdentityUser> signInManager, TimeConversionService TimeConversionService)
        {
            this.context = context;
            this.userManager = userManager;
            this.emailservice = emailservice;
            this.signInManager = signInManager;
            this.TimeConversionService = TimeConversionService;
        }

        [HttpPost("LessonBookingCreate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult LessonBookingCreateUpdate([FromBody] LessonBookingCreateListDto LessonBookingCreateListDtos)
        {
            if (LessonBookingCreateListDtos == null)
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
                    CourseBooking CourseBooking = new CourseBooking();

                    CourseBooking = context.CourseBookings
                       .Include(x => x.StudentApplicationUsers)
                       .Include(x => x.TutorApplicationUsers)
                       .Include(x => x.AdminApplicationUsers)
                       .Include(x => x.Courses)
                       .Include(x => x.LessonBookings)
                       .Where(x => x.Id == LessonBookingCreateListDtos.CourseBookingId).FirstOrDefault();

                    ApplicationUser ApplicationUser = new ApplicationUser();

                    if (LessonBookingCreateListDtos.LessonBookingInitiatedBy == "Student")
                    {
                        ApplicationUser = context.ApplicationUsers.Where(x => x.Id == CourseBooking.StudentApplicationUsers.Id).FirstOrDefault();
                    }
                    else if (LessonBookingCreateListDtos.LessonBookingInitiatedBy == "Tutor")
                    {
                        ApplicationUser = context.ApplicationUsers.Where(x => x.Id == CourseBooking.TutorApplicationUsers.Id).FirstOrDefault();
                    }

                    foreach (var item in LessonBookingCreateListDtos.LessonBookingCreateDtos)
                    {
                        DateTime FirstTimeSlot = new DateTime(1970, 1, 1).AddMilliseconds(item.FirstTimeSlot.Value);
                        DateTime SecondTimeSlot = new DateTime(1970, 1, 1).AddMilliseconds(item.SecondTimeSlot.Value);
                        DateTime ThirdTimeSlot = new DateTime(1970, 1, 1).AddMilliseconds(item.ThirdTimeSlot.Value);

                        var obj = new LessonBooking
                        {
                            CourseBookingId = LessonBookingCreateListDtos.CourseBookingId,
                            LessonBookingInitiatedBy = LessonBookingCreateListDtos.LessonBookingInitiatedBy,
                            LessonName = item.LessonName,
                            TopicCovered = item.TopicCovered,
                            FirstTimeSlot = FirstTimeSlot,
                            SecondTimeSlot = SecondTimeSlot,
                            ThirdTimeSlot = ThirdTimeSlot,
                            Duration = item.Duration,
                            IsSubmited = true,
                            Status = "Pending",
                            StudentRemarks = item.StudentRemarks,
                            TutorRemarks = item.TutorRemarks,
                        };
                        context.LessonBookings.Add(obj);
                        context.SaveChanges();

                    }

                    var obj1 = new AllNotification
                    {
                        CourseBookingIdForLesson = LessonBookingCreateListDtos.CourseBookingId,
                        ForAdmin = true,
                        Notification = $"Lessons Booking Of {CourseBooking.Courses.Name} Course Need To Be Approved"
                    };
                    context.AllNotifications.Add(obj1);
                    context.SaveChanges();

                    var Response = new
                    {
                        Response = "1",
                        Message = "Lesson Booking Created"
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

        [HttpPost("LessonBookingUpdate/{lessonBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult LessonBookingUpdate(long lessonBookingId, [FromBody] LessonBookingUpdateDto LessonBookingUpdateDto)
        {
            if (lessonBookingId == null || LessonBookingUpdateDto == null)
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
                    LessonBooking LessonBooking = new LessonBooking();
                    LessonBooking = context.LessonBookings.Where(x => x.Id == lessonBookingId)
                        .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                        .FirstOrDefault();

                    if (LessonBooking != null)
                    {
                        if (LessonBooking.IsBooked != true && LessonBooking.IsDone != true)
                        {

                            var LessonNotification = context.AllNotifications
                                    .Where(x => x.LessonBookingId == lessonBookingId)
                                      .ToList();

                            context.AllNotifications.RemoveRange(LessonNotification);

                            CourseBooking CourseBooking = new CourseBooking();

                            CourseBooking = context.CourseBookings
                               .Include(x => x.StudentApplicationUsers)
                               .Include(x => x.TutorApplicationUsers)
                               .Include(x => x.AdminApplicationUsers)
                               .Include(x => x.Courses)
                               .Include(x => x.LessonBookings)
                               .Where(x => x.Id == LessonBookingUpdateDto.CourseBookingId).FirstOrDefault();

                            ApplicationUser ApplicationUser = new ApplicationUser();

                            if (LessonBookingUpdateDto.LessonBookingUpdatedBy == "Student")
                            {
                                ApplicationUser = context.ApplicationUsers.Where(x => x.Id == CourseBooking.StudentApplicationUsers.Id).FirstOrDefault();
                            }
                            else if (LessonBookingUpdateDto.LessonBookingUpdatedBy == "Tutor")
                            {
                                ApplicationUser = context.ApplicationUsers.Where(x => x.Id == CourseBooking.TutorApplicationUsers.Id).FirstOrDefault();
                            }


                            DateTime FirstTimeSlot = new DateTime(1970, 1, 1).AddMilliseconds(LessonBookingUpdateDto.FirstTimeSlot.Value);
                            DateTime SecondTimeSlot = new DateTime(1970, 1, 1).AddMilliseconds(LessonBookingUpdateDto.SecondTimeSlot.Value);
                            DateTime ThirdTimeSlot = new DateTime(1970, 1, 1).AddMilliseconds(LessonBookingUpdateDto.ThirdTimeSlot.Value);


                            LessonBooking.CourseBookingId = LessonBookingUpdateDto.CourseBookingId;
                            LessonBooking.LessonBookingInitiatedBy = LessonBookingUpdateDto.LessonBookingUpdatedBy;
                            LessonBooking.LessonName = LessonBookingUpdateDto.LessonName;
                            LessonBooking.TopicCovered = LessonBookingUpdateDto.TopicCovered;
                            LessonBooking.FirstTimeSlot = FirstTimeSlot;
                            LessonBooking.SecondTimeSlot = SecondTimeSlot;
                            LessonBooking.ThirdTimeSlot = ThirdTimeSlot;
                            LessonBooking.Duration = LessonBookingUpdateDto.Duration;
                            LessonBooking.IsSubmited = true;
                            LessonBooking.Status = "Pending";
                            LessonBooking.StudentRemarks = LessonBookingUpdateDto.StudentRemarks;
                            LessonBooking.TutorRemarks = LessonBookingUpdateDto.TutorRemarks;

                            context.SaveChanges();



                            var obj1 = new AllNotification
                            {
                                CourseBookingIdForLesson = LessonBookingUpdateDto.CourseBookingId,
                                ForAdmin = true,
                                Notification = $"Lessons Booking Of {CourseBooking.Courses.Name} Course Need To Be Approved"
                            };
                            context.AllNotifications.Add(obj1);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Lesson Booking Updated"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Lesson Booking Cannt Be Updated After Approval"
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
                            Error = "Record Not Found"
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

        [HttpGet("LessonBookingDelete/{lessonBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult LessonBookingDelete(long lessonBookingId)
        {
            if (lessonBookingId == null)
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
                    LessonBooking LessonBooking = new LessonBooking();
                    LessonBooking = context.LessonBookings.Find(lessonBookingId);

                    if (LessonBooking != null)
                    {
                        if (LessonBooking.IsBooked != true && LessonBooking.IsDone != true)
                        {

                            var LessonNotification = context.AllNotifications
                                    .Where(x => x.LessonBookingId == lessonBookingId)
                                      .ToList();

                            context.AllNotifications.RemoveRange(LessonNotification);

                            context.LessonBookings.Remove(LessonBooking);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Lesson Booking Deleted"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Lesson Booking Cannt Be Deleted After Approval"
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
                            Error = "Record Not Found"
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

        [HttpGet("LessonBookingRejectByTutor/{lessonBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult LessonBookingRejectByTutor(long lessonBookingId)
        {
            if (lessonBookingId == null)
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
                    LessonBooking LessonBooking = new LessonBooking();
                    AllNotification AllNotification = new AllNotification();

                    LessonBooking = context.LessonBookings
         .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
         .Include(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
         .Include(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
        .Where(x => x.Id == Convert.ToInt64(lessonBookingId))
        .FirstOrDefault();

                    if (LessonBooking != null)
                    {
                        if (LessonBooking.IsAdminApproved == true)
                        {
                            LessonBooking.Status = "Rejected By Tutor";
                            LessonBooking.IsSubmited = false;
                            LessonBooking.IsAdminApproved = false;
                            LessonBooking.IsAdminRejected = false;
                            LessonBooking.IsTutorApproved = false;
                            LessonBooking.IsTutorRejected = true;
                            LessonBooking.IsStudentApproved = false;
                            LessonBooking.IsStudentRejected = false;
                            LessonBooking.IsBooked = false;
                            context.SaveChanges();

                            AllNotification = context.AllNotifications
                            .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                            .Where(x => x.LessonBookingId == lessonBookingId).FirstOrDefault();

                            AllNotification.IsReadByTutor = true;
                            context.SaveChanges();


                            var ForStudent = new AllNotification
                            {
                                Notification = $"{LessonBooking.LessonName} Lesson Booking Request Is Rejected By Tutor",
                                ForStudent = true,
                                ForStudentId = LessonBooking.CourseBookings.StudentApplicationUsers.Id
                            };
                            context.AllNotifications.Add(ForStudent);
                            context.SaveChanges();

                            var ForAdmin = new AllNotification
                            {
                                Notification = $"{LessonBooking.LessonName} Lesson Booking Request Is Rejected By Tutor",
                                ForTutor = true,
                            };
                            context.AllNotifications.Add(ForAdmin);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Lesson Booking Rejected By Tutor"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "1",
                                Message = "Lesson Booking Cann't Be Rejected After Approval"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "Credential Error"
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


        [HttpGet("LessonBookingRejectByStudent/{lessonBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult LessonBookingRejectByStudent(long lessonBookingId)
        {
            if (lessonBookingId == null)
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
                    LessonBooking LessonBooking = new LessonBooking();
                    AllNotification AllNotification = new AllNotification();

                    LessonBooking = context.LessonBookings
                     .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                     .Include(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                     .Include(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                    .Where(x => x.Id == Convert.ToInt64(lessonBookingId))
                    .FirstOrDefault();

                    if (LessonBooking != null)
                    {
                        if (LessonBooking.IsAdminApproved == true)
                        {
                            LessonBooking.Status = "Rejected By Student";
                            LessonBooking.IsSubmited = false;
                            LessonBooking.IsAdminApproved = false;
                            LessonBooking.IsAdminRejected = false;
                            LessonBooking.IsTutorApproved = false;
                            LessonBooking.IsTutorRejected = false;
                            LessonBooking.IsStudentApproved = false;
                            LessonBooking.IsStudentRejected = true;
                            LessonBooking.IsBooked = false;
                            context.SaveChanges();

                            AllNotification = context.AllNotifications
                            .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
  .Where(x => x.LessonBookingId == lessonBookingId).FirstOrDefault();

                            AllNotification.IsReadByStudent = true;
                            context.SaveChanges();


                            var ForTutor = new AllNotification
                            {
                                Notification = $"{LessonBooking.LessonName} Lesson Booking Request Is Rejected By Student",
                                ForTutor = true,
                                ForTutorId = LessonBooking.CourseBookings.TutorApplicationUsers.Id
                            };
                            context.AllNotifications.Add(ForTutor);
                            context.SaveChanges();

                            var ForAdmin = new AllNotification
                            {
                                Notification = $"{LessonBooking.LessonName} Lesson Booking Request Is Rejected By Student",
                                ForTutor = true,
                            };
                            context.AllNotifications.Add(ForAdmin);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Lesson Booking Rejected By Student"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "1",
                                Message = "Lesson Booking Cann't Be Rejected After Approval"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "Credential Error"
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


        //[HttpPost("LessonBookingApprovetByTutor/{lessonBookingId}")]
        //[Authorize(Policy = "ApiUser")]
        //public IActionResult LessonBookingApprovetByTutor(long lessonBookingId, [FromBody] LessonBookingApproveByTutorDto LessonBookingApproveByTutorDto)
        //{
        //    if (lessonBookingId == null || LessonBookingApproveByTutorDto == null)
        //    {
        //        var Response = new
        //        {
        //            Response = "0",
        //            Error = "Invalid request"
        //        };

        //        string jsonResponse = JsonConvert.SerializeObject(Response);
        //        return BadRequest(jsonResponse);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            LessonBooking LessonBooking = new LessonBooking();
        //            AllNotification AllNotification = new AllNotification();

        //            LessonBooking = context.LessonBookings
        // .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
        // .Include(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
        // .Include(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
        //.Where(x => x.Id == Convert.ToInt64(lessonBookingId))
        //.FirstOrDefault();

        //            if (LessonBooking != null)
        //            {
        //                if (LessonBooking.IsAdminApproved == true)
        //                {
        //                    LessonBooking.Status = "Rejected By Tutor";
        //                    LessonBooking.IsSubmited = false;
        //                    LessonBooking.IsAdminApproved = false;
        //                    LessonBooking.IsAdminRejected = false;
        //                    LessonBooking.IsTutorApproved = false;
        //                    LessonBooking.IsTutorRejected = true;
        //                    LessonBooking.IsStudentApproved = false;
        //                    LessonBooking.IsStudentRejected = false;
        //                    LessonBooking.IsBooked = false;
        //                    context.SaveChanges();

        //                    AllNotification = context.AllNotifications
        //                    .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
        //                    .Where(x => x.LessonBookingId == lessonBookingId).FirstOrDefault();

        //                    AllNotification.IsReadByTutor = true;
        //                    context.SaveChanges();


        //                    var ForStudent = new AllNotification
        //                    {
        //                        Notification = $"{LessonBooking.LessonName} Lesson Booking Request Is Rejected By Tutor",
        //                        ForStudent = true,
        //                        ForStudentId = LessonBooking.CourseBookings.StudentApplicationUsers.Id
        //                    };
        //                    context.AllNotifications.Add(ForStudent);
        //                    context.SaveChanges();

        //                    var ForAdmin = new AllNotification
        //                    {
        //                        Notification = $"{LessonBooking.LessonName} Lesson Booking Request Is Rejected By Tutor",
        //                        ForTutor = true,
        //                    };
        //                    context.AllNotifications.Add(ForAdmin);
        //                    context.SaveChanges();

        //                    var Response = new
        //                    {
        //                        Response = "1",
        //                        Message = "Lesson Booking Rejected By Tutor"
        //                    };

        //                    string jsonResponse = JsonConvert.SerializeObject(Response);
        //                    return Ok(jsonResponse);
        //                }
        //                else
        //                {
        //                    var Response = new
        //                    {
        //                        Response = "1",
        //                        Message = "Lesson Booking Cann't Be Rejected After Approval"
        //                    };

        //                    string jsonResponse = JsonConvert.SerializeObject(Response);
        //                    return Ok(jsonResponse);
        //                }
        //            }
        //            else
        //            {
        //                var Response = new
        //                {
        //                    Response = "0",
        //                    Error = "Credential Error"
        //                };

        //                string jsonResponse = JsonConvert.SerializeObject(Response);
        //                return BadRequest(jsonResponse);

        //            }
        //        }


        //        catch (Exception ex)
        //        {
        //            var Response = new
        //            {
        //                Response = "0",
        //                Error = "Something Went Wrong"
        //            };

        //            string jsonResponse = JsonConvert.SerializeObject(Response);
        //            return BadRequest(jsonResponse);
        //        }
        //    }
        //}


    }
}