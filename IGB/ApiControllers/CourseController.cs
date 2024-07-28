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
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly EmailService emailservice;
        private readonly SignInManager<IdentityUser> signInManager;

        public CourseController(ApplicationDbContext context, UserManager<IdentityUser> userManager, EmailService emailservice, SignInManager<IdentityUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.emailservice = emailservice;
            this.signInManager = signInManager;
        }

        [HttpPost("CourseBookingCreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult CourseBookingCreateUpdate([FromBody] CourseBookingDto CourseBookingDto)
        {
            if (CourseBookingDto == null)
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
                    if (CourseBookingDto.Id == null)
                    {
                        var obj = new CourseBooking
                        {
                            StudentId = CourseBookingDto.StudentId,
                            CourseId = CourseBookingDto.CourseId,
                            StudentRemarks = CourseBookingDto.StudentRemarks,
                            IsSubmited = true,
                            Status = "Pending"
                        };
                        context.CourseBookings.Add(obj);
                        context.SaveChanges();

                        var CourseName = context.CourseBookings.Where(x => x.Id == Convert.ToInt64(obj.Id)).Include(x => x.Courses).Select(x => x.Courses.Name).FirstOrDefault();

                        var obj1 = new AllNotification
                        {
                            CourseBookingId = obj.Id,
                            ForAdmin = true,
                            Notification = $"{CourseName} Course Booking Need To Be Approved"
                        };
                        context.AllNotifications.Add(obj1);
                        context.SaveChanges();

                        var Response = new
                        {
                            Response = "1",
                            Message = "Course Booking Created"
                        };

                        string jsonResponse = JsonConvert.SerializeObject(Response);
                        return Ok(jsonResponse);

                    }
                    else
                    {
                        CourseBooking CourseBooking = new CourseBooking();
                        CourseBooking = context.CourseBookings.Find(CourseBookingDto.Id);

                        if (CourseBooking != null)
                        {
                            if (CourseBooking.IsBooked != true && CourseBooking.IsCompleted != true)
                            {
                                CourseBooking.StudentId = CourseBookingDto.StudentId;
                                CourseBooking.StudentRemarks = CourseBookingDto.StudentRemarks;
                                CourseBooking.Status = "Pending";
                                CourseBooking.IsSubmited = true;
                                CourseBooking.IsAdminApproved = false;
                                CourseBooking.IsAdminRejected = false;
                                CourseBooking.IsTutorRejected = false;

                                context.SaveChanges();

                                var notification = context.AllNotifications.Where(x => x.CourseBookingId == CourseBooking.Id).ToList();
                                context.AllNotifications.RemoveRange(notification);
                                context.SaveChanges();

                                var CourseName = context.CourseBookings.Where(x => x.Id == Convert.ToInt64(CourseBooking.Id)).Include(x => x.Courses).Select(x => x.Courses.Name).FirstOrDefault();

                                var obj1 = new AllNotification
                                {
                                    CourseBookingId = Convert.ToInt64(CourseBookingDto.Id),
                                    ForAdmin = true,
                                    Notification = $"{CourseName} Course Booking Need To Be Approved Again"

                                };
                                context.AllNotifications.Add(obj1);
                                context.SaveChanges();
                                var Response = new
                                {
                                    Response = "1",
                                    Message = "Course Booking Updated"
                                };

                                string jsonResponse = JsonConvert.SerializeObject(Response);
                                return Ok(jsonResponse);
                            }
                            else
                            {
                                var Response = new
                                {
                                    Response = "0",
                                    Error = "Course Booking Cannt Be Edited After Approval"
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

        [HttpGet("CourseBookingDelete/{courseBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult CourseBookingDelete(long courseBookingId)
        {
            if (courseBookingId == null)
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
                    CourseBooking = context.CourseBookings.Find(courseBookingId);

                    if (CourseBooking != null)
                    {
                        if (CourseBooking.IsBooked != true && CourseBooking.IsCompleted != true)
                        {

                            var CourseNotification = context.AllNotifications
                                    .Where(x => x.CourseBookingId == courseBookingId)
                                      .ToList();

                            context.AllNotifications.RemoveRange(CourseNotification);

                            context.CourseBookings.Remove(CourseBooking);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Course Booking Deleted"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Course Booking Cannt Be Deleted After Approval"
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

        [HttpGet("CourseBookingComplete/{courseBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult CourseBookingComplete(long courseBookingId)
        {
            if (courseBookingId == null)
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
                    CourseBooking = context.CourseBookings.Include(x => x.LessonBookings)
     .Include(x => x.StudentApplicationUsers)
     .Where(x => x.Id == courseBookingId).FirstOrDefault();

                    if (CourseBooking != null)
                    {
                        bool lessons = CourseBooking.LessonBookings.Any(x => x.IsBooked == true || x.IsSubmited == true || x.IsAdminApproved == true || x.IsTutorApproved == true);

                        if (lessons)
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Course Booking Cannt Be Completed , Some Lessons Are Remaining To Be Attend"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return BadRequest(jsonResponse);
                        }
                        else
                        {
                            CourseBooking.IsSubmited = false;
                            CourseBooking.IsAdminApproved = false;
                            CourseBooking.IsAdminRejected = false;
                            CourseBooking.IsTutorApproved = false;
                            CourseBooking.IsTutorRejected = false;
                            CourseBooking.IsBooked = false;

                            CourseBooking.IsCompleted = true;

                            CourseBooking.Status = "Completed";

                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Course Booking Completed"
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

        [HttpGet("CourseBookingReject/{courseBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult CourseBookingReject(long courseBookingId)
        {
            if (courseBookingId == null)
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
         .Include(x => x.Courses)
        .Include(x => x.StudentApplicationUsers).ThenInclude(x => x.Curriculums)
        .Include(x => x.StudentApplicationUsers).ThenInclude(x => x.Grades)
        .Where(x => x.Id == Convert.ToInt64(courseBookingId))
        .FirstOrDefault();

                    if (CourseBooking != null)
                    {
                        if (CourseBooking.IsBooked != true && CourseBooking.IsCompleted != true)
                        {

                            CourseBooking.Status = "Rejected By Tutor";
                            CourseBooking.IsSubmited = false;
                            CourseBooking.IsAdminApproved = false;
                            CourseBooking.IsAdminRejected = false;
                            CourseBooking.IsTutorApproved = false;
                            CourseBooking.IsTutorRejected = true;
                            CourseBooking.IsBooked = false;

                            var notification = context.AllNotifications.Where(x => x.CourseBookingId == CourseBooking.Id).ToList();
                            context.AllNotifications.RemoveRange(notification);
                            context.SaveChanges();

                            var ForStudent = new AllNotification
                            {
                                Notification = $"Your {CourseBooking.Courses.Name} Course Booking Request Is Rejected By Tutor",
                                ForStudent = true,
                                ForStudentId = CourseBooking.StudentApplicationUsers.Id
                            };
                            context.AllNotifications.Add(ForStudent);
                            context.SaveChanges();

                            var ForAdmin = new AllNotification
                            {
                                Notification = $"{CourseBooking.Courses.Name} Course Booking Request Is Rejected By Tutor",
                                ForAdmin = true,
                            };
                            context.AllNotifications.Add(ForAdmin);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Course Booking Rejected"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Course Booking Cannt Be Rejected After Approval"
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

        [HttpGet("CourseBookingApprove/{courseBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult CourseBookingApprove(long courseBookingId)
        {
            if (courseBookingId == null)
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
         .Include(x => x.Courses)
        .Include(x => x.StudentApplicationUsers).ThenInclude(x => x.Curriculums)
        .Include(x => x.StudentApplicationUsers).ThenInclude(x => x.Grades)
        .Where(x => x.Id == Convert.ToInt64(courseBookingId))
        .FirstOrDefault();

                    if (CourseBooking != null)
                    {
                        if (CourseBooking.IsBooked != true && CourseBooking.IsCompleted != true)
                        {

                            CourseBooking.Status = "Booked";
                            CourseBooking.IsSubmited = false;
                            CourseBooking.IsAdminApproved = false;
                            CourseBooking.IsAdminRejected = false;
                            CourseBooking.IsTutorApproved = true;
                            CourseBooking.IsTutorRejected = false;
                            CourseBooking.IsBooked = true;

                            var notification = context.AllNotifications.Where(x => x.CourseBookingId == CourseBooking.Id).ToList();
                            context.AllNotifications.RemoveRange(notification);
                            context.SaveChanges();


                            var ForStudent = new AllNotification
                            {
                                Notification = $"Your {CourseBooking.Courses.Name} Course Booking Request Is Approved By Tutor",
                                ForStudent = true,
                                ForStudentId = CourseBooking.StudentApplicationUsers.Id
                            };
                            context.AllNotifications.Add(ForStudent);
                            context.SaveChanges();

                            var ForAdmin = new AllNotification
                            {
                                Notification = $"{CourseBooking.Courses.Name} Course Booking Request Is Approved By Tutor",
                                ForAdmin = true,
                            };
                            context.AllNotifications.Add(ForAdmin);
                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Course Booking Approved"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Course Booking Cannt Be Approved Again After Approval"
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


        [HttpGet("StudentEnrolledCourses/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentEnrolledCourses(
        string studentId,
[FromQuery] string? globalSearch,
[FromQuery] long? bookingDate,
[FromQuery] string? tutorName,
[FromQuery] string? courseName,
[FromQuery] string? status,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)

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

                    var query = context.CourseBookings
    .Include(x => x.StudentApplicationUsers)
    .Include(x => x.TutorApplicationUsers)
    .Include(x => x.AdminApplicationUsers)
    .Include(x => x.Courses)
    .Where(x => x.StudentApplicationUsers.Id == studentId)
    .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.Courses.Name.Contains(globalSearch) || p.TutorApplicationUsers.FirstName.Contains(globalSearch) || p.Status.Contains(globalSearch));
                    }

                    if (!string.IsNullOrEmpty(courseName))
                    {
                        query = query.Where(p => p.Courses.Name.Contains(courseName));
                    }

                    if (!string.IsNullOrEmpty(tutorName))
                    {
                        query = query.Where(p => p.TutorApplicationUsers.FirstName.Contains(tutorName));
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    if (bookingDate != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(bookingDate.Value);
                        query = query.Where(p => p.CourseBookingDate >= dateTime);
                    }


                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "coursename":
                            query = sortDescending ? query.OrderByDescending(p => p.Courses.Name) : query.OrderBy(p => p.Courses.Name);
                            break;
                        case "tutorname":
                            query = sortDescending ? query.OrderByDescending(p => p.TutorApplicationUsers.FirstName) : query.OrderBy(p => p.TutorApplicationUsers.FirstName);
                            break;
                        case "status":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                            break;
                        case "coursebookingdate":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookingDate) : query.OrderBy(p => p.CourseBookingDate);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookingDate) : query.OrderBy(p => p.CourseBookingDate);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            x.Id,
                            CourseBookingDate = ((DateTimeOffset)x.CourseBookingDate).ToUnixTimeMilliseconds(),
                            CourseName = x.Courses.Name,
                            TutorName = x.TutorApplicationUsers.FirstName,
                            x.Status
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            StudentId = studentId,
                            EnrolledCourses = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Course Found"
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




        [HttpGet("TutorEnrolledCourses/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorEnrolledCourses(
    string tutorId,
    [FromQuery] string? globalSearch,
    [FromQuery] long? bookingDate,
    [FromQuery] string? studentName,
    [FromQuery] string? courseName,
    [FromQuery] string? status,
    [FromQuery] int? skip,
    [FromQuery] int? take,
    [FromQuery] string? sortBy,
    [FromQuery] bool? sortDesc = null)

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

                    var query = context.CourseBookings
    .Include(x => x.StudentApplicationUsers)
    .Include(x => x.TutorApplicationUsers)
    .Include(x => x.AdminApplicationUsers)
    .Include(x => x.Courses)
    .Where(x => x.TutorApplicationUsers.Id == tutorId)
    .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.Courses.Name.Contains(globalSearch) || p.StudentApplicationUsers.FirstName.Contains(globalSearch) || p.Status.Contains(globalSearch));
                    }

                    if (!string.IsNullOrEmpty(courseName))
                    {
                        query = query.Where(p => p.Courses.Name.Contains(courseName));
                    }

                    if (!string.IsNullOrEmpty(studentName))
                    {
                        query = query.Where(p => p.StudentApplicationUsers.FirstName.Contains(studentName));
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    if (bookingDate != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(bookingDate.Value);
                        query = query.Where(p => p.CourseBookingDate >= dateTime);
                    }


                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "coursename":
                            query = sortDescending ? query.OrderByDescending(p => p.Courses.Name) : query.OrderBy(p => p.Courses.Name);
                            break;
                        case "studentname":
                            query = sortDescending ? query.OrderByDescending(p => p.StudentApplicationUsers.FirstName) : query.OrderBy(p => p.StudentApplicationUsers.FirstName);
                            break;
                        case "status":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                            break;
                        case "coursebookingdate":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookingDate) : query.OrderBy(p => p.CourseBookingDate);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookingDate) : query.OrderBy(p => p.CourseBookingDate);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            x.Id,
                            CourseBookingDate = ((DateTimeOffset)x.CourseBookingDate).ToUnixTimeMilliseconds(),
                            CourseName = x.Courses.Name,
                            StudentName = x.StudentApplicationUsers.FirstName,
                            x.Status
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            TutorId = tutorId,
                            EnrolledCourses = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Course Found"
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


        [HttpGet("CourseBookingLedger/{courseBookingId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> CourseBookingLedger(
long courseBookingId,
[FromQuery] string? globalSearch,
[FromQuery] long? bookingDate,
[FromQuery] long? startTime,
[FromQuery] long? endTime,
[FromQuery] long? duration,
[FromQuery] string? lessonName,
[FromQuery] string? studentName,
[FromQuery] string? tutorName,
[FromQuery] long? creditCharged,
[FromQuery] long? estimatedCreditCharged,
[FromQuery] string? status,
[FromQuery] string? classLink,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)

        {
            if (courseBookingId == null)
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

                    var CourseBooking = context.CourseBookings
                            .Include(x => x.StudentApplicationUsers).ThenInclude(x => x.StudentApplicationUsers)
                            .Include(x => x.TutorApplicationUsers)
                            .Include(x => x.AdminApplicationUsers)
                            .Include(x => x.Courses)
                            .Include(x => x.LessonBookings)
                            .Where(x => x.Id == Convert.ToInt64(courseBookingId)).FirstOrDefault();

                    var query = context.LessonBookings
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.StudentApplicationUsers).ThenInclude(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.AdminApplicationUsers)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.TutorApplicationUsers)
                    .Where(x => x.CourseBookingId == Convert.ToInt64(courseBookingId))
                    .OrderBy(x => x.LessonBookingDate)
                    .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p =>
                            p.LessonName.Contains(globalSearch) ||
                            p.CourseBookings.StudentApplicationUsers.FirstName.Contains(globalSearch) ||
                            p.CourseBookings.TutorApplicationUsers.FirstName.Contains(globalSearch) ||
                            p.Status.Contains(globalSearch) ||
                            p.ClassLink.Contains(globalSearch) ||
                            p.CreditCharged.ToString().Contains(globalSearch) ||
                            p.EstimatedCreditCharged.ToString().Contains(globalSearch) ||
                            p.Duration.ToString().Contains(globalSearch)
                        );
                    }

                    if (bookingDate != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(bookingDate.Value);
                        query = query.Where(p => p.LessonBookingDate >= dateTime);
                    }

                    if (startTime != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(startTime.Value);
                        query = query.Where(p => p.StartTime >= dateTime);
                    }

                    if (endTime != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(endTime.Value);
                        query = query.Where(p => p.EndTime >= dateTime);
                    }

                    if (duration != null)
                    {
                        query = query.Where(p => p.Duration == duration);
                    }

                    if (!string.IsNullOrEmpty(lessonName))
                    {
                        query = query.Where(p => p.LessonName.Contains(lessonName));
                    }

                    if (!string.IsNullOrEmpty(studentName))
                    {
                        query = query.Where(p => p.CourseBookings.StudentApplicationUsers.FirstName.Contains(studentName));
                    }

                    if (!string.IsNullOrEmpty(tutorName))
                    {
                        query = query.Where(p => p.CourseBookings.TutorApplicationUsers.FirstName.Contains(tutorName));
                    }

                    if (creditCharged != null)
                    {
                        query = query.Where(p => p.CreditCharged == creditCharged);
                    }

                    if (estimatedCreditCharged != null)
                    {
                        query = query.Where(p => p.EstimatedCreditCharged == estimatedCreditCharged);
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    if (!string.IsNullOrEmpty(classLink))
                    {
                        query = query.Where(p => p.ClassLink.Contains(classLink));
                    }


                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "bookingdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookingDate) : query.OrderBy(p => p.LessonBookingDate);
                            break;

                        case "starttime,":
                            query = sortDescending ? query.OrderByDescending(p => p.StartTime) : query.OrderBy(p => p.StartTime);
                            break;

                        case "endtime,":
                            query = sortDescending ? query.OrderByDescending(p => p.EndTime) : query.OrderBy(p => p.EndTime);
                            break;

                        case "duration,":
                            query = sortDescending ? query.OrderByDescending(p => p.Duration) : query.OrderBy(p => p.Duration);
                            break;

                        case "lessonName,,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonName) : query.OrderBy(p => p.LessonName);
                            break;

                        case "studentname,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.StudentApplicationUsers.FirstName) : query.OrderBy(p => p.CourseBookings.StudentApplicationUsers.FirstName);
                            break;

                        case "tutorname,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.TutorApplicationUsers.FirstName) : query.OrderBy(p => p.CourseBookings.TutorApplicationUsers.FirstName);
                            break;

                        case "creditcharged,":
                            query = sortDescending ? query.OrderByDescending(p => p.CreditCharged) : query.OrderBy(p => p.CreditCharged);
                            break;

                        case "estimatedcreditcharged,":
                            query = sortDescending ? query.OrderByDescending(p => p.EstimatedCreditCharged) : query.OrderBy(p => p.EstimatedCreditCharged);
                            break;

                        case "status,":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                            break;

                        case "classlink,":
                            query = sortDescending ? query.OrderByDescending(p => p.ClassLink) : query.OrderBy(p => p.ClassLink);
                            break;

                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookingDate) : query.OrderBy(p => p.LessonBookingDate);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            Id=x.Id,
                            LessonBookingDate = ((DateTimeOffset)x.LessonBookingDate).ToUnixTimeMilliseconds(),
                            StartTime = ((DateTimeOffset)x.StartTime).ToUnixTimeMilliseconds(),
                            EndTime = ((DateTimeOffset)x.EndTime).ToUnixTimeMilliseconds(),
                            Duration=x.Duration,
                            LessonName = x.LessonName,
                            TutorName =x.CourseBookings.TutorApplicationUsers.FirstName,
                            StudentName=x.CourseBookings.StudentApplicationUsers.FirstName,
                            CreditCharged=x.CreditCharged,
                            EstimatedCreditCharged=x.EstimatedCreditCharged,
                            Status=x.Status,
                            ClassLink=x.ClassLink
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            Lessons = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Lesson Found"
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



        [HttpGet("StudentSchedule/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentSchedule(
string studentId,
[FromQuery] string? globalSearch,
[FromQuery] long? bookingDate,
[FromQuery] long? startTime,
[FromQuery] long? endTime,
[FromQuery] long? duration,
[FromQuery] string? courseName,
[FromQuery] string? lessonName,
[FromQuery] string? studentName,
[FromQuery] string? tutorName,
[FromQuery] long? creditCharged,
[FromQuery] long? estimatedCreditCharged,
[FromQuery] string? status,
[FromQuery] string? classLink,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)

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
                    var query = context.LessonBookings
                       .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.AdminApplicationUsers)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.TutorApplicationUsers)
                .Where(x => x.CourseBookings.StudentApplicationUsers.Id == studentId && x.IsBooked == true)
                   .OrderBy(n => n.StartTime)
                    .AsQueryable();

                   var lessonBooking = context.LessonBookings
                    .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.AdminApplicationUsers)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                    .Where(x => x.CourseBookings.StudentApplicationUsers.Id == studentId && x.IsBooked == true)
                    .OrderBy(n => n.StartTime)
                    .FirstOrDefault();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p =>
                            p.CourseBookings.Courses.Name.Contains(globalSearch) ||
                            p.LessonName.Contains(globalSearch) ||
                            p.CourseBookings.StudentApplicationUsers.FirstName.Contains(globalSearch) ||
                            p.CourseBookings.TutorApplicationUsers.FirstName.Contains(globalSearch) ||
                            p.Status.Contains(globalSearch) ||
                            p.ClassLink.Contains(globalSearch) ||
                            p.CreditCharged.ToString().Contains(globalSearch) ||
                            p.EstimatedCreditCharged.ToString().Contains(globalSearch) ||
                            p.Duration.ToString().Contains(globalSearch)
                        );
                    }

                    if (bookingDate != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(bookingDate.Value);
                        query = query.Where(p => p.LessonBookingDate >= dateTime);
                    }

                    if (startTime != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(startTime.Value);
                        query = query.Where(p => p.StartTime >= dateTime);
                    }

                    if (endTime != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(endTime.Value);
                        query = query.Where(p => p.EndTime >= dateTime);
                    }

                    if (duration != null)
                    {
                        query = query.Where(p => p.Duration == duration);
                    }

                    if (!string.IsNullOrEmpty(courseName))
                    {
                        query = query.Where(p => p.CourseBookings.Courses.Name.Contains(courseName));
                    }

                    if (!string.IsNullOrEmpty(lessonName))
                    {
                        query = query.Where(p => p.LessonName.Contains(lessonName));
                    }

                    if (!string.IsNullOrEmpty(studentName))
                    {
                        query = query.Where(p => p.CourseBookings.StudentApplicationUsers.FirstName.Contains(studentName));
                    }

                    if (!string.IsNullOrEmpty(tutorName))
                    {
                        query = query.Where(p => p.CourseBookings.TutorApplicationUsers.FirstName.Contains(tutorName));
                    }

                    if (creditCharged != null)
                    {
                        query = query.Where(p => p.CreditCharged == creditCharged);
                    }

                    if (estimatedCreditCharged != null)
                    {
                        query = query.Where(p => p.EstimatedCreditCharged == estimatedCreditCharged);
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    if (!string.IsNullOrEmpty(classLink))
                    {
                        query = query.Where(p => p.ClassLink.Contains(classLink));
                    }


                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "bookingdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookingDate) : query.OrderBy(p => p.LessonBookingDate);
                            break;

                        case "starttime,":
                            query = sortDescending ? query.OrderByDescending(p => p.StartTime) : query.OrderBy(p => p.StartTime);
                            break;

                        case "endtime,":
                            query = sortDescending ? query.OrderByDescending(p => p.EndTime) : query.OrderBy(p => p.EndTime);
                            break;

                        case "duration,":
                            query = sortDescending ? query.OrderByDescending(p => p.Duration) : query.OrderBy(p => p.Duration);
                            break;

                        case "courseName,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.Courses.Name) : query.OrderBy(p => p.CourseBookings.Courses.Name);
                            break;

                        case "lessonName,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonName) : query.OrderBy(p => p.LessonName);
                            break;

                        case "studentname,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.StudentApplicationUsers.FirstName) : query.OrderBy(p => p.CourseBookings.StudentApplicationUsers.FirstName);
                            break;

                        case "tutorname,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.TutorApplicationUsers.FirstName) : query.OrderBy(p => p.CourseBookings.TutorApplicationUsers.FirstName);
                            break;

                        case "creditcharged,":
                            query = sortDescending ? query.OrderByDescending(p => p.CreditCharged) : query.OrderBy(p => p.CreditCharged);
                            break;

                        case "estimatedcreditcharged,":
                            query = sortDescending ? query.OrderByDescending(p => p.EstimatedCreditCharged) : query.OrderBy(p => p.EstimatedCreditCharged);
                            break;

                        case "status,":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                            break;

                        case "classlink,":
                            query = sortDescending ? query.OrderByDescending(p => p.ClassLink) : query.OrderBy(p => p.ClassLink);
                            break;

                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookingDate) : query.OrderBy(p => p.LessonBookingDate);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            Id = x.Id,
                            LessonBookingDate = ((DateTimeOffset)x.LessonBookingDate).ToUnixTimeMilliseconds(),
                            StartTime = ((DateTimeOffset)x.StartTime).ToUnixTimeMilliseconds(),
                            EndTime = ((DateTimeOffset)x.EndTime).ToUnixTimeMilliseconds(),
                            Duration = x.Duration,
                            CourseName = x.CourseBookings.Courses.Name,
                            LessonName = x.LessonName,
                            TutorName = x.CourseBookings.TutorApplicationUsers.FirstName,
                            StudentName = x.CourseBookings.StudentApplicationUsers.FirstName,
                            CreditCharged = x.CreditCharged,
                            EstimatedCreditCharged = x.EstimatedCreditCharged,
                            Status = x.Status,
                            ClassLink = x.ClassLink
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            Lessons = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Lesson Found"
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


        [HttpGet("TutorSchedule/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorSchedule(
string tutorId,
[FromQuery] string? globalSearch,
[FromQuery] long? bookingDate,
[FromQuery] long? startTime,
[FromQuery] long? endTime,
[FromQuery] long? duration,
[FromQuery] string? courseName,
[FromQuery] string? lessonName,
[FromQuery] string? studentName,
[FromQuery] string? tutorName,
[FromQuery] long? creditCharged,
[FromQuery] long? estimatedCreditCharged,
[FromQuery] string? status,
[FromQuery] string? classLink,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)

        {
            if (TutorSchedule == null)
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
                    var query = context.LessonBookings
                       .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.AdminApplicationUsers)
                    .Include(x => x.CourseBookings)
                    .ThenInclude(x => x.TutorApplicationUsers)
                .Where(x => x.CourseBookings.TutorApplicationUsers.Id == tutorId && x.IsBooked == true)
                   .OrderBy(n => n.StartTime)
                    .AsQueryable();

                    var lessonBooking = context.LessonBookings
                     .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                     .Include(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                     .Include(x => x.CourseBookings).ThenInclude(x => x.AdminApplicationUsers)
                     .Include(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                     .Where(x => x.CourseBookings.TutorApplicationUsers.Id == tutorId && x.IsBooked == true)
                     .OrderBy(n => n.StartTime)
                     .FirstOrDefault();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p =>
                            p.CourseBookings.Courses.Name.Contains(globalSearch) ||
                            p.LessonName.Contains(globalSearch) ||
                            p.CourseBookings.StudentApplicationUsers.FirstName.Contains(globalSearch) ||
                            p.CourseBookings.TutorApplicationUsers.FirstName.Contains(globalSearch) ||
                            p.Status.Contains(globalSearch) ||
                            p.ClassLink.Contains(globalSearch) ||
                            p.CreditCharged.ToString().Contains(globalSearch) ||
                            p.EstimatedCreditCharged.ToString().Contains(globalSearch) ||
                            p.Duration.ToString().Contains(globalSearch)
                        );
                    }

                    if (bookingDate != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(bookingDate.Value);
                        query = query.Where(p => p.LessonBookingDate >= dateTime);
                    }

                    if (startTime != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(startTime.Value);
                        query = query.Where(p => p.StartTime >= dateTime);
                    }

                    if (endTime != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(endTime.Value);
                        query = query.Where(p => p.EndTime >= dateTime);
                    }

                    if (duration != null)
                    {
                        query = query.Where(p => p.Duration == duration);
                    }

                    if (!string.IsNullOrEmpty(courseName))
                    {
                        query = query.Where(p => p.CourseBookings.Courses.Name.Contains(courseName));
                    }

                    if (!string.IsNullOrEmpty(lessonName))
                    {
                        query = query.Where(p => p.LessonName.Contains(lessonName));
                    }

                    if (!string.IsNullOrEmpty(studentName))
                    {
                        query = query.Where(p => p.CourseBookings.StudentApplicationUsers.FirstName.Contains(studentName));
                    }

                    if (!string.IsNullOrEmpty(tutorName))
                    {
                        query = query.Where(p => p.CourseBookings.TutorApplicationUsers.FirstName.Contains(tutorName));
                    }

                    if (creditCharged != null)
                    {
                        query = query.Where(p => p.CreditCharged == creditCharged);
                    }

                    if (estimatedCreditCharged != null)
                    {
                        query = query.Where(p => p.EstimatedCreditCharged == estimatedCreditCharged);
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    if (!string.IsNullOrEmpty(classLink))
                    {
                        query = query.Where(p => p.ClassLink.Contains(classLink));
                    }


                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "bookingdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookingDate) : query.OrderBy(p => p.LessonBookingDate);
                            break;

                        case "starttime,":
                            query = sortDescending ? query.OrderByDescending(p => p.StartTime) : query.OrderBy(p => p.StartTime);
                            break;

                        case "endtime,":
                            query = sortDescending ? query.OrderByDescending(p => p.EndTime) : query.OrderBy(p => p.EndTime);
                            break;

                        case "duration,":
                            query = sortDescending ? query.OrderByDescending(p => p.Duration) : query.OrderBy(p => p.Duration);
                            break;

                        case "courseName,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.Courses.Name) : query.OrderBy(p => p.CourseBookings.Courses.Name);
                            break;

                        case "lessonName,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonName) : query.OrderBy(p => p.LessonName);
                            break;

                        case "studentname,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.StudentApplicationUsers.FirstName) : query.OrderBy(p => p.CourseBookings.StudentApplicationUsers.FirstName);
                            break;

                        case "tutorname,":
                            query = sortDescending ? query.OrderByDescending(p => p.CourseBookings.TutorApplicationUsers.FirstName) : query.OrderBy(p => p.CourseBookings.TutorApplicationUsers.FirstName);
                            break;

                        case "creditcharged,":
                            query = sortDescending ? query.OrderByDescending(p => p.CreditCharged) : query.OrderBy(p => p.CreditCharged);
                            break;

                        case "estimatedcreditcharged,":
                            query = sortDescending ? query.OrderByDescending(p => p.EstimatedCreditCharged) : query.OrderBy(p => p.EstimatedCreditCharged);
                            break;

                        case "status,":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                            break;

                        case "classlink,":
                            query = sortDescending ? query.OrderByDescending(p => p.ClassLink) : query.OrderBy(p => p.ClassLink);
                            break;

                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookingDate) : query.OrderBy(p => p.LessonBookingDate);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            Id = x.Id,
                            LessonBookingDate = ((DateTimeOffset)x.LessonBookingDate).ToUnixTimeMilliseconds(),
                            StartTime = ((DateTimeOffset)x.StartTime).ToUnixTimeMilliseconds(),
                            EndTime = ((DateTimeOffset)x.EndTime).ToUnixTimeMilliseconds(),
                            Duration = x.Duration,
                            CourseName = x.CourseBookings.Courses.Name,
                            LessonName = x.LessonName,
                            TutorName = x.CourseBookings.TutorApplicationUsers.FirstName,
                            StudentName = x.CourseBookings.StudentApplicationUsers.FirstName,
                            CreditCharged = x.CreditCharged,
                            EstimatedCreditCharged = x.EstimatedCreditCharged,
                            Status = x.Status,
                            ClassLink = x.ClassLink
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            Lessons = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Lesson Found"
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
