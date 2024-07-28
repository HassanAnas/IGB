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
    public class AnnouncementController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AnnouncementController(ApplicationDbContext context)
        {
            this.context = context;
        }



        [HttpGet("StudentAnnouncement/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentAnnouncement(
         string studentId,
 [FromQuery] string? globalSearch,
 [FromQuery] long? announcementDate,
 [FromQuery] string? tutorName,
 [FromQuery] string? courseName,
 [FromQuery] string? lessonName,
 [FromQuery] string? announcement,
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

                    var query = context.TutorFeedbacks
                      .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                .Where(x => x.LessonBookings.CourseBookings.StudentApplicationUsers.Id == studentId)
                .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName.Contains(globalSearch) || p.LessonBookings.CourseBookings.Courses.Name.Contains(globalSearch) || p.LessonBookings.LessonName.Contains(globalSearch) || p.Announcement.Contains(globalSearch));
                    }

                    if (announcementDate != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(announcementDate.Value);
                        query = query.Where(p => p.Date >= dateTime);
                    }

                    if (!string.IsNullOrEmpty(tutorName))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName.Contains(tutorName));
                    }

                    if (!string.IsNullOrEmpty(courseName))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.Courses.Name.Contains(courseName));
                    }

                    if (!string.IsNullOrEmpty(lessonName))
                    {
                        query = query.Where(p => p.LessonBookings.LessonName.Contains(lessonName));
                    }

                    if (!string.IsNullOrEmpty(announcement))
                    {
                        query = query.Where(p => p.Announcement.Contains(announcement));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "announcementdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                        case "tutorname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName) : query.OrderBy(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName);
                            break;
                        case "coursename,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.Courses.Name) : query.OrderBy(p => p.LessonBookings.CourseBookings.Courses.Name);
                            break;
                        case "lessonname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.LessonName) : query.OrderBy(p => p.LessonBookings.LessonName);
                            break;
                        case "announcement":
                            query = sortDescending ? query.OrderByDescending(p => p.Announcement) : query.OrderBy(p => p.Announcement);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            x.Id,
                            Date = ((DateTimeOffset)x.Date).ToUnixTimeMilliseconds(),
                            CourseName = x.LessonBookings.CourseBookings.Courses.Name,
                            LessonName = x.LessonBookings.LessonName,
                            TutorName = x.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName,
                            Announcement = x.Announcement
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            StudentId = studentId,
                            Announcements = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Announcement Found"
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



        [HttpGet("TutorAnnouncement/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorAnnouncement(
 string tutorId,
[FromQuery] string? globalSearch,
[FromQuery] long? announcementDate,
[FromQuery] string? studentName,
[FromQuery] string? courseName,
[FromQuery] string? lessonName,
[FromQuery] string? announcement,
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

                    var query = context.TutorFeedbacks
                      .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                .Where(x => x.LessonBookings.CourseBookings.TutorApplicationUsers.Id == tutorId)
                .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName.Contains(globalSearch) || p.LessonBookings.CourseBookings.Courses.Name.Contains(globalSearch) || p.LessonBookings.LessonName.Contains(globalSearch) || p.Announcement.Contains(globalSearch));
                    }

                    if (announcementDate != null)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(announcementDate.Value);
                        query = query.Where(p => p.Date >= dateTime);
                    }

                    if (!string.IsNullOrEmpty(studentName))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName.Contains(studentName));
                    }

                    if (!string.IsNullOrEmpty(courseName))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.Courses.Name.Contains(courseName));
                    }

                    if (!string.IsNullOrEmpty(lessonName))
                    {
                        query = query.Where(p => p.LessonBookings.LessonName.Contains(lessonName));
                    }

                    if (!string.IsNullOrEmpty(announcement))
                    {
                        query = query.Where(p => p.Announcement.Contains(announcement));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "announcementdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                        case "studentname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName) : query.OrderBy(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName);
                            break;
                        case "coursename,":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.Courses.Name) : query.OrderBy(p => p.LessonBookings.CourseBookings.Courses.Name);
                            break;
                        case "lessonname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.LessonName) : query.OrderBy(p => p.LessonBookings.LessonName);
                            break;
                        case "announcement":
                            query = sortDescending ? query.OrderByDescending(p => p.Announcement) : query.OrderBy(p => p.Announcement);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query
                        .Select(x => new
                        {
                            x.Id,
                            Date = ((DateTimeOffset)x.Date).ToUnixTimeMilliseconds(),
                            CourseName = x.LessonBookings.CourseBookings.Courses.Name,
                            LessonName = x.LessonBookings.LessonName,
                            StudentName = x.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName,
                            Announcement = x.Announcement
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            TutorId = tutorId,
                            Announcements = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Announcement Found"
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


