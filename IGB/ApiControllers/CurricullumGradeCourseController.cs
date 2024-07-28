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
    public class CurricullumGradeCourseController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CurricullumGradeCourseController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //For Student Profile

        [HttpGet("AllCurriculums")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> AllCurriculums()
        {
            try
            {
                var Curriculum = context.Curriculums
   .Select(x => new { x.Id, x.Name })
   .ToList();

                if (Curriculum.Any())
                {

                    var response = new
                    {
                        Response = "1",
                        Curriculum = Curriculum
                    };

                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return Ok(jsonResponse);
                }
                else
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "No Curriculum Found"
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


        [HttpGet("SelectedCurriculums")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> SelectedCurriculums(
[FromQuery] string? globalSearch,
[FromQuery] string? name,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)
        {
            try
            {
                var query = context.Curriculums.AsQueryable();

                if (!string.IsNullOrEmpty(globalSearch))
                {
                    query = query.Where(p => p.Name.Contains(globalSearch));
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                bool sortDescending = sortDesc ?? false;

                switch (sortBy?.ToLower())
                {
                    case "name":
                        query = sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                        break;
                    default:
                        query = sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                        break;
                }

                query = query.Skip(skip ?? 0).Take(take ?? 10);

                var result = query.Select(p => new { p.Id, p.Name }).ToList();

                if (result.Any())
                {

                    var response = new
                    {
                        Response = "1",
                        Curriculum = result
                    };

                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return Ok(jsonResponse);
                }
                else
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "No Curriculum Found"
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




        [HttpGet("AllGrades")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> AllGrades()
        {
            try
            {
                var Grade = context.Grades
   .Select(x => new { x.Id, x.Name })
   .ToList();

                if (Grade.Any())
                {

                    var response = new
                    {
                        Response = "1",
                        Grade = Grade
                    };

                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return Ok(jsonResponse);
                }
                else
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "No Grade Found"
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

                [HttpGet("SelectedGrades")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> SelectedGrades(
[FromQuery] string? globalSearch,
[FromQuery] string? name,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)
        {
            try
            {
                var query = context.Grades.AsQueryable();

                if (!string.IsNullOrEmpty(globalSearch))
                {
                    query = query.Where(p => p.Name.Contains(globalSearch));
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                bool sortDescending = sortDesc ?? false;

                switch (sortBy?.ToLower())
                {
                    case "name":
                        query = sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                        break;
                    default:
                        query = sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                        break;
                }

                query = query.Skip(skip ?? 0).Take(take ?? 10);

                var result = query.Select(p => new { p.Id, p.Name }).ToList();

                if (result.Any())
                {

                    var response = new
                    {
                        Response = "1",
                        Grade = result
                    };

                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return Ok(jsonResponse);
                }
                else
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "No Curriculum Found"
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


        [HttpGet("AllGradesByCourseId/{courseId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> AllGradesByCourseId(long courseId)
        {
            if (courseId == null)
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
                    var Grade = context.GradeCourses.Where(x => x.CourseId == courseId).Include(x => x.Grades).Select(x => new { x.Grades.Id, x.Grades.Name }).ToList();

                    if (Grade.Count > 0)
                    {
                        var response = new
                        {
                            Response = "1",
                            Grade = Grade
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Grade Found"
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

        [HttpGet("SelectedGradesByCourseId/{courseId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> SelectedGradesByCourseId(
       long courseId,
       [FromQuery] string? globalSearch,
[FromQuery] string? name,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)
        {
            if (courseId == null)
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
                    var query = context.GradeCourses.Where(x => x.CourseId == courseId).Include(x => x.Grades).AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.Grades.Name.Contains(globalSearch));
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.Grades.Name.Contains(name));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "name":
                            query = sortDescending ? query.OrderByDescending(p => p.Grades.Name) : query.OrderBy(p => p.Grades.Name);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.Grades.Name) : query.OrderBy(p => p.Grades.Name);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query.Select(p => new { p.Grades.Id, p.Grades.Name }).ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            Grade = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Grade Found"
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




        // For Course Booking

        [HttpGet("AllCoursesByStudentId/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> AllCoursesByStudentId(string studentId)
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
                var Student = context.ApplicationUsers.Where(x => x.Id == studentId).FirstOrDefault();
                if (Student == null)
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "Student not found"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);
                }
                else
                {
                    try
                    {
                        var Course = context.GradeCourses
                         .Include(x => x.Courses).ThenInclude(x => x.Curriculums)
                          .Where(x => x.GradeId == Student.GradeId && x.Courses.CurriculumId == Student.CurriculumId)
                           .Select(x => new { x.Courses.Id, x.Courses.Name })
                           .ToList();

                        if (Course.Count > 0)
                        {
                            var response = new
                            {
                                Response = "1",
                                Course = Course
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

        }

        [HttpGet("SelectedCoursesByStudentId/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> SelectedCoursesByStudentId(
    string studentId,
    [FromQuery] string? globalSearch,
[FromQuery] string? name,
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
                var Student = context.ApplicationUsers.Where(x => x.Id == studentId).FirstOrDefault();
                if (Student == null)
                {
                    var Response = new
                    {
                        Response = "0",
                        Error = "Student not found"
                    };

                    string jsonResponse = JsonConvert.SerializeObject(Response);
                    return BadRequest(jsonResponse);
                }
                else
                {
                    try
                    {
                        var query = context.GradeCourses
                             .Include(x => x.Courses).ThenInclude(x => x.Curriculums)
                              .Where(x => x.GradeId == Student.GradeId && x.Courses.CurriculumId == Student.CurriculumId).AsQueryable();

                        if (!string.IsNullOrEmpty(globalSearch))
                        {
                            query = query.Where(p => p.Courses.Name.Contains(globalSearch));
                        }

                        if (!string.IsNullOrEmpty(name))
                        {
                            query = query.Where(p => p.Courses.Name.Contains(name));
                        }

                        bool sortDescending = sortDesc ?? false;

                        switch (sortBy?.ToLower())
                        {
                            case "name":
                                query = sortDescending ? query.OrderByDescending(p => p.Courses.Name) : query.OrderBy(p => p.Courses.Name);
                                break;
                            default:
                                query = sortDescending ? query.OrderByDescending(p => p.Courses.Name) : query.OrderBy(p => p.Courses.Name);
                                break;
                        }

                        query = query.Skip(skip ?? 0).Take(take ?? 10);

                        var result = query.Select(p => new { p.Id, p.Courses.Name }).ToList();

                        if (result.Any())
                        {

                            var response = new
                            {
                                Response = "1",
                                Course = result
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
        }
        // For Tutor Profile

        [HttpGet("AllCoursesByCurriculumId/{curriculumId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> AllCoursesByCurriculumId(long curriculumId)
        {
            if (curriculumId == null)
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
                    var Course = context.Courses.Where(x => x.CurriculumId == curriculumId).Select(x => new { x.Id, x.Name }).ToList();

                    if (Course.Count > 0)
                    {
                        var response = new
                        {
                            Response = "1",
                            Course = Course
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


        [HttpGet("SelectedCoursesByCurriculumId/{curriculumId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> SelectedCoursesByCurriculumId(
            long curriculumId,
            [FromQuery] string? globalSearch,
[FromQuery] string? name,
[FromQuery] int? skip,
[FromQuery] int? take,
[FromQuery] string? sortBy,
[FromQuery] bool? sortDesc = null)
        {
            if (curriculumId == null)
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
                    var query = context.Courses.Where(x => x.CurriculumId == curriculumId).AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.Name.Contains(globalSearch));
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.Name.Contains(name));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "name":
                            query = sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                            break;
                        default:
                            query = sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                            break;
                    }

                    query = query.Skip(skip ?? 0).Take(take ?? 10);

                    var result = query.Select(p => new { p.Id, p.Name }).ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            Course = result
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




    }

}
