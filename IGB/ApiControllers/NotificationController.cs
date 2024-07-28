using IGB.Data;
using IGB.Models.Admin;
using IGB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace IGB.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public NotificationController(ApplicationDbContext context)
        {
            this.context = context;
        }



        [HttpGet("StudentNotificationIndex/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentNotificationIndex(
         string studentId,
 [FromQuery] string? globalSearch,
 [FromQuery] long? notificationTime,
 [FromQuery] bool? isRead,
 [FromQuery] string? notification,
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
                    var query = context.AllNotifications
                    .Include(x => x.NewApplicationUsers)
                    .Include(x => x.TutorApplicationUsers)
                    .Include(x => x.StudentApplicationUsers)
                    .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                       .OrderByDescending(n => n.Id)
                .Where(x => x.ForStudent == true && x.ForStudentId == studentId)
                       .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.Notification.Contains(globalSearch));
                    }

                    if (notificationTime.HasValue)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(notificationTime.Value);
                        query = query.Where(p => p.Time >= dateTime);
                    }

                    if (isRead.HasValue)
                    {
                        query = query.Where(p => p.IsReadByStudent == isRead);
                    }

                    if (!string.IsNullOrEmpty(notification))
                    {
                        query = query.Where(p => p.Notification.Contains(notification));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "notificationtime,":
                            query = sortDescending ? query.OrderByDescending(p => p.Time) : query.OrderBy(p => p.Time);
                            break;
                        case "isread":
                            query = sortDescending ? query.OrderByDescending(p => p.IsReadByStudent) : query.OrderBy(p => p.IsReadByStudent);
                            break;
                        case "notification":
                            query = sortDescending ? query.OrderByDescending(p => p.Notification) : query.OrderBy(p => p.Notification);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.Time) : query.OrderBy(p => p.Time);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            x.Id,
                            Time = ((DateTimeOffset)x.Time).ToUnixTimeMilliseconds(),
                            IsRead = x.IsReadByStudent,
                            Notification = x.Notification
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            StudentId = studentId,
                            Notifications = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Notification Found"
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


        [HttpGet("TutorNotificationIndex/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorNotificationIndex(
 string tutorId,
[FromQuery] string? globalSearch,
[FromQuery] long? notificationTime,
[FromQuery] bool? isRead,
[FromQuery] string? notification,
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
                    var query = context.AllNotifications
        .Include(x => x.NewApplicationUsers)
        .Include(x => x.TutorApplicationUsers)
        .Include(x => x.StudentApplicationUsers)
        .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
        .Include(x => x.CourseBookingIdForLessons).ThenInclude(x => x.Courses)
        .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
        .Include(x => x.LessonBookingStarts).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
           .OrderByDescending(n => n.Id)
           .Where(x => x.ForTutor == true && x.ForTutorId == tutorId)
           .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.Notification.Contains(globalSearch));
                    }

                    if (notificationTime.HasValue)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(notificationTime.Value);
                        query = query.Where(p => p.Time >= dateTime);
                    }

                    if (isRead.HasValue)
                    {
                        query = query.Where(p => p.IsReadByTutor == isRead);
                    }

                    if (!string.IsNullOrEmpty(notification))
                    {
                        query = query.Where(p => p.Notification.Contains(notification));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "notificationtime,":
                            query = sortDescending ? query.OrderByDescending(p => p.Time) : query.OrderBy(p => p.Time);
                            break;
                        case "isread":
                            query = sortDescending ? query.OrderByDescending(p => p.IsReadByTutor) : query.OrderBy(p => p.IsReadByTutor);
                            break;
                        case "notification":
                            query = sortDescending ? query.OrderByDescending(p => p.Notification) : query.OrderBy(p => p.Notification);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.Time) : query.OrderBy(p => p.Time);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            x.Id,
                            Time = ((DateTimeOffset)x.Time).ToUnixTimeMilliseconds(),
                            IsRead = x.IsReadByTutor,
                            Notification = x.Notification
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            TutorId = tutorId,
                            Notifications = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Notification Found"
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


