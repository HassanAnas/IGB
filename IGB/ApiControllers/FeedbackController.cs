using IGB.Data;
using IGB.DTOs;
using IGB.Models;
using IGB.Models.Admin;
using IGB.Models.Feedback;
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
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public FeedbackController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("StudentFeedackCreateUpdate")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult StudentFeedackCreate([FromBody] StudentFeedbackDto StudentFeedbackDto)
        {
            if (StudentFeedbackDto == null)
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
                    if (StudentFeedbackDto.Id == null)
                    {
                        StudentFeedback? StudentFeedback = new StudentFeedback();

                        if (StudentFeedbackDto.UnderstandingAndEngagement == "Excellent")
                        {
                            StudentFeedback.UnderstandingAndEngagementScore = 0.8;
                        }
                        else if (StudentFeedbackDto.UnderstandingAndEngagement == "Good")
                        {
                            StudentFeedback.UnderstandingAndEngagementScore = 0.5;
                        }
                        else if (StudentFeedbackDto.UnderstandingAndEngagement == "Satisfactory")
                        {
                            StudentFeedback.UnderstandingAndEngagementScore = 0.3;
                        }
                        else if (StudentFeedbackDto.UnderstandingAndEngagement == "Unsatisfactory")
                        {
                            StudentFeedback.UnderstandingAndEngagementScore = 0;
                        }

                        if (StudentFeedbackDto.DidTutorExplainTopic == "Yes,ILikedItAlot")
                        {
                            StudentFeedback.DidTutorExplainTopicScore = 0.8;
                        }
                        else if (StudentFeedbackDto.DidTutorExplainTopic == "PartiallyUnderstood")
                        {
                            StudentFeedback.DidTutorExplainTopicScore = 0.5;
                        }
                        else if (StudentFeedbackDto.DidTutorExplainTopic == "IStillHaveDoubts")
                        {
                            StudentFeedback.DidTutorExplainTopicScore = 0.3;
                        }
                        else if (StudentFeedbackDto.DidTutorExplainTopic == "NotUnderstoodAtAll")
                        {
                            StudentFeedback.DidTutorExplainTopicScore = 0;
                        }

                        if (StudentFeedbackDto.DidTutorClearDoubtToday == "YesAllCleared")
                        {
                            StudentFeedback.DidTutorClearDoubtTodayScore = 0.8;
                        }
                        else if (StudentFeedbackDto.DidTutorClearDoubtToday == "SomehowCleared")
                        {
                            StudentFeedback.DidTutorClearDoubtTodayScore = 0.5;
                        }
                        else if (StudentFeedbackDto.DidTutorClearDoubtToday == "IStillHaveDoubts")
                        {
                            StudentFeedback.DidTutorClearDoubtTodayScore = 0.3;
                        }
                        else if (StudentFeedbackDto.DidTutorClearDoubtToday == "NotSatisfied")
                        {
                            StudentFeedback.DidTutorClearDoubtTodayScore = 0;
                        }


                        if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "YesAllCleared")
                        {
                            StudentFeedback.DidTutorClearDoubtPreviousScore = 0.8;
                        }
                        else if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "SomehowCleared")
                        {
                            StudentFeedback.DidTutorClearDoubtPreviousScore = 0.5;
                        }
                        else if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "IStillHaveDoubts")
                        {
                            StudentFeedback.DidTutorClearDoubtPreviousScore = 0.3;
                        }
                        else if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "NotSatisfied")
                        {
                            StudentFeedback.DidTutorClearDoubtPreviousScore = 0;
                        }

                        if (StudentFeedbackDto.RateTutorTeaching == "Excellent")
                        {
                            StudentFeedback.RateTutorTeachingScore = 0.8;
                        }
                        else if (StudentFeedbackDto.RateTutorTeaching == "Good")
                        {
                            StudentFeedback.RateTutorTeachingScore = 0.5;
                        }
                        else if (StudentFeedbackDto.RateTutorTeaching == "Satisfactory")
                        {
                            StudentFeedback.RateTutorTeachingScore = 0.3;
                        }
                        else if (StudentFeedbackDto.RateTutorTeaching == "Unsatisfactory")
                        {
                            StudentFeedback.RateTutorTeachingScore = 0;
                        }

                        StudentFeedback.FinalScore = StudentFeedback.UnderstandingAndEngagementScore
                        + StudentFeedback.DidTutorExplainTopicScore
                        + StudentFeedback.DidTutorClearDoubtTodayScore
                        + StudentFeedback.DidTutorClearDoubtPreviousScore
                        + StudentFeedback.RateTutorTeachingScore;

                        var obj = new StudentFeedback
                        {
                            LessonBookingId = StudentFeedbackDto.LessonBookingId,
                            UnderstandingAndEngagement = StudentFeedbackDto.UnderstandingAndEngagement,
                            UnderstandingAndEngagementScore = StudentFeedback.UnderstandingAndEngagementScore,
                            DidTutorExplainTopic = StudentFeedbackDto.DidTutorExplainTopic,
                            DidTutorExplainTopicScore = StudentFeedback.DidTutorExplainTopicScore,
                            DidTutorClearDoubtToday = StudentFeedbackDto.DidTutorClearDoubtToday,
                            DidTutorClearDoubtTodayScore = StudentFeedback.DidTutorClearDoubtTodayScore,
                            DidTutorClearDoubtPrevious = StudentFeedbackDto.DidTutorClearDoubtPrevious,
                            DidTutorClearDoubtPreviousScore = StudentFeedback.DidTutorClearDoubtPreviousScore,
                            RateTutorTeaching = StudentFeedbackDto.RateTutorTeaching,
                            RateTutorTeachingScore = StudentFeedback.RateTutorTeachingScore,
                            FinalScore = StudentFeedback.FinalScore,
                            Remarks = StudentFeedbackDto.Remarks,
                            IsPending = true,
                            Status = "Pending"
                        };
                        context.StudentFeedbacks.Add(obj);

                        var lesson = context.LessonBookings.Where(x => x.Id == Convert.ToInt64(StudentFeedbackDto.LessonBookingId)).FirstOrDefault();
                        lesson.TutorScore = StudentFeedback.FinalScore;

                        context.SaveChanges();

                        var Response = new
                        {
                            Response = "1",
                            Message = "Student Feedback Created"
                        };

                        string jsonResponse = JsonConvert.SerializeObject(Response);
                        return Ok(jsonResponse);
                    }
                    else
                    {

                        StudentFeedback? StudentFeedback = new StudentFeedback();
                        StudentFeedback = context.StudentFeedbacks.Where(x => x.Id == StudentFeedbackDto.Id).FirstOrDefault();

                        if (StudentFeedback.IsPending == true && StudentFeedback.IsApproved == false)
                        {

                            if (StudentFeedbackDto.UnderstandingAndEngagement == "Excellent")
                            {
                                StudentFeedback.UnderstandingAndEngagementScore = 0.8;
                            }
                            else if (StudentFeedbackDto.UnderstandingAndEngagement == "Good")
                            {
                                StudentFeedback.UnderstandingAndEngagementScore = 0.5;
                            }
                            else if (StudentFeedbackDto.UnderstandingAndEngagement == "Satisfactory")
                            {
                                StudentFeedback.UnderstandingAndEngagementScore = 0.3;
                            }
                            else if (StudentFeedbackDto.UnderstandingAndEngagement == "Unsatisfactory")
                            {
                                StudentFeedback.UnderstandingAndEngagementScore = 0;
                            }

                            if (StudentFeedbackDto.DidTutorExplainTopic == "Yes,ILikedItAlot")
                            {
                                StudentFeedback.DidTutorExplainTopicScore = 0.8;
                            }
                            else if (StudentFeedbackDto.DidTutorExplainTopic == "PartiallyUnderstood")
                            {
                                StudentFeedback.DidTutorExplainTopicScore = 0.5;
                            }
                            else if (StudentFeedbackDto.DidTutorExplainTopic == "IStillHaveDoubts")
                            {
                                StudentFeedback.DidTutorExplainTopicScore = 0.3;
                            }
                            else if (StudentFeedbackDto.DidTutorExplainTopic == "NotUnderstoodAtAll")
                            {
                                StudentFeedback.DidTutorExplainTopicScore = 0;
                            }

                            if (StudentFeedbackDto.DidTutorClearDoubtToday == "YesAllCleared")
                            {
                                StudentFeedback.DidTutorClearDoubtTodayScore = 0.8;
                            }
                            else if (StudentFeedbackDto.DidTutorClearDoubtToday == "SomehowCleared")
                            {
                                StudentFeedback.DidTutorClearDoubtTodayScore = 0.5;
                            }
                            else if (StudentFeedbackDto.DidTutorClearDoubtToday == "IStillHaveDoubts")
                            {
                                StudentFeedback.DidTutorClearDoubtTodayScore = 0.3;
                            }
                            else if (StudentFeedbackDto.DidTutorClearDoubtToday == "NotSatisfied")
                            {
                                StudentFeedback.DidTutorClearDoubtTodayScore = 0;
                            }


                            if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "YesAllCleared")
                            {
                                StudentFeedback.DidTutorClearDoubtPreviousScore = 0.8;
                            }
                            else if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "SomehowCleared")
                            {
                                StudentFeedback.DidTutorClearDoubtPreviousScore = 0.5;
                            }
                            else if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "IStillHaveDoubts")
                            {
                                StudentFeedback.DidTutorClearDoubtPreviousScore = 0.3;
                            }
                            else if (StudentFeedbackDto.DidTutorClearDoubtPrevious == "NotSatisfied")
                            {
                                StudentFeedback.DidTutorClearDoubtPreviousScore = 0;
                            }

                            if (StudentFeedbackDto.RateTutorTeaching == "Excellent")
                            {
                                StudentFeedback.RateTutorTeachingScore = 0.8;
                            }
                            else if (StudentFeedbackDto.RateTutorTeaching == "Good")
                            {
                                StudentFeedback.RateTutorTeachingScore = 0.5;
                            }
                            else if (StudentFeedbackDto.RateTutorTeaching == "Satisfactory")
                            {
                                StudentFeedback.RateTutorTeachingScore = 0.3;
                            }
                            else if (StudentFeedbackDto.RateTutorTeaching == "Unsatisfactory")
                            {
                                StudentFeedback.RateTutorTeachingScore = 0;
                            }

                            StudentFeedback.FinalScore = StudentFeedback.UnderstandingAndEngagementScore
                            + StudentFeedback.DidTutorExplainTopicScore
                            + StudentFeedback.DidTutorClearDoubtTodayScore
                            + StudentFeedback.DidTutorClearDoubtPreviousScore
                            + StudentFeedback.RateTutorTeachingScore;


                            StudentFeedback.LessonBookingId = StudentFeedbackDto.LessonBookingId;
                            StudentFeedback.UnderstandingAndEngagement = StudentFeedbackDto.UnderstandingAndEngagement;
                            StudentFeedback.UnderstandingAndEngagementScore = StudentFeedback.UnderstandingAndEngagementScore;
                            StudentFeedback.DidTutorExplainTopic = StudentFeedbackDto.DidTutorExplainTopic;
                            StudentFeedback.DidTutorExplainTopicScore = StudentFeedback.DidTutorExplainTopicScore;
                            StudentFeedback.DidTutorClearDoubtToday = StudentFeedbackDto.DidTutorClearDoubtToday;
                            StudentFeedback.DidTutorClearDoubtTodayScore = StudentFeedback.DidTutorClearDoubtTodayScore;
                            StudentFeedback.DidTutorClearDoubtPrevious = StudentFeedbackDto.DidTutorClearDoubtPrevious;
                            StudentFeedback.DidTutorClearDoubtPreviousScore = StudentFeedback.DidTutorClearDoubtPreviousScore;
                            StudentFeedback.RateTutorTeaching = StudentFeedbackDto.RateTutorTeaching;
                            StudentFeedback.RateTutorTeachingScore = StudentFeedback.RateTutorTeachingScore;
                            StudentFeedback.FinalScore = StudentFeedback.FinalScore;
                            StudentFeedback.Remarks = StudentFeedbackDto.Remarks;
                            StudentFeedback.IsPending = true;
                            StudentFeedback.Status = "Pending";


                            var lesson = context.LessonBookings.Where(x => x.Id == Convert.ToInt64(StudentFeedbackDto.LessonBookingId)).FirstOrDefault();
                            lesson.TutorScore = StudentFeedback.FinalScore;

                            context.SaveChanges();

                            var Response = new
                            {
                                Response = "1",
                                Message = "Student Feedback Updated"
                            };

                            string jsonResponse = JsonConvert.SerializeObject(Response);
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            var Response = new
                            {
                                Response = "0",
                                Error = "Feedback Cann't Be Updated After Approved"
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


        //[HttpPost("TutorFeedackCreateUpdate")]
        //[Authorize(Policy = "ApiUser")]
        //public IActionResult TutorFeedackCreateUpdate([FromBody] TutorFeedbackDto TutorFeedbackDto)
        //{
        //    if (TutorFeedbackDto == null)
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
        //            if (TutorFeedbackDto.Id == null)
        //            {
        //                TutorFeedback? TutorFeedback = new TutorFeedback();

        //                if (TutorFeedbackDto.PreviousHomeWorkDone == true)
        //                {
        //                    TutorFeedback.PreviousHomeWorkDoneScore = 1;
        //                }

        //                if (TutorFeedbackDto.GradePrediction == "A+")
        //                {
        //                    TutorFeedback.GradePredictionScore = 1;
        //                }
        //                else if (TutorFeedbackDto.GradePrediction == "A")
        //                {
        //                    TutorFeedback.GradePredictionScore = 0.8;
        //                }
        //                else if (TutorFeedbackDto.GradePrediction == "B")
        //                {
        //                    TutorFeedback.GradePredictionScore = 0.6;
        //                }
        //                else if (TutorFeedbackDto.GradePrediction == "C")
        //                {
        //                    TutorFeedback.GradePredictionScore = 0.4;
        //                }
        //                else if (TutorFeedbackDto.GradePrediction == "D")
        //                {
        //                    TutorFeedback.GradePredictionScore = 0.2;
        //                }
        //                else if (TutorFeedbackDto.GradePrediction == "E")
        //                {
        //                    TutorFeedback.GradePredictionScore = 0.1;
        //                }
        //                else
        //                {
        //                    TutorFeedback.GradePredictionScore = 0;
        //                }

        //                TutorFeedback.Percentage = TutorFeedbackDto.ObtainedScore * 100 / TutorFeedbackDto.TotalScore;
        //                TutorFeedback.PercentageScore = TutorFeedback.Percentage / 100;

        //                TutorFeedback.FinalScore = TutorFeedback.PreviousHomeWorkDoneScore + TutorFeedback.TopicUnderstandingLevel + TutorFeedback.MentalSkills + TutorFeedback.GradePredictionScore + TutorFeedback.PercentageScore;

        //                if (TutorFeedbackDto.NextHomework == "Not Given")
        //                {
        //                    TutorFeedback.NextHomeworkFile = null;
        //                }

        //                var obj = new TutorFeedback
        //                {
        //                    LessonBookingId = TutorFeedbackDto.LessonBookingId,
        //                    PreviousHomeWorkDone = TutorFeedbackDto.PreviousHomeWorkDone,
        //                    PreviousHomeWorkDoneScore = TutorFeedback.PreviousHomeWorkDoneScore,
        //                    PreviousHomeWorkDiscussed = TutorFeedbackDto.PreviousHomeWorkDiscussed,
        //                    TopicCoveredToday = TutorFeedbackDto.TopicCoveredToday,
        //                    TopicUnderstandingLevel = TutorFeedbackDto.TopicUnderstandingLevel,
        //                    GradePrediction = TutorFeedback.GradePrediction,
        //                    GradePredictionScore = TutorFeedback.GradePredictionScore,
        //                    AverageGradePrediction = TutorFeedback.AverageGradePrediction,
        //                    MentalSkills = TutorFeedback.MentalSkills,
        //                    TestName = TutorFeedback.TestName,
        //                    ObtainedScore = TutorFeedback.ObtainedScore,
        //                    TotalScore = TutorFeedback.TotalScore,
        //                    Percentage = TutorFeedback.Percentage,
        //                    PercentageScore = TutorFeedback.PercentageScore,
        //                    TestFile = TutorFeedback.TestFile,
        //                    NextHomeworkFile = TutorFeedback.NextHomeworkFile,
        //                    Remarks = TutorFeedback.Remarks,
        //                    Announcement = TutorFeedback.Announcement,
        //                    IsPending = true,
        //                    FinalScore = TutorFeedback.FinalScore
        //                };

        //                context.TutorFeedbacks.Add(obj);

        //                var lesson = context.LessonBookings.Where(x => x.Id == Convert.ToInt64(LessonBookingId)).FirstOrDefault();
        //                lesson.StudentScore = TutorFeedback.FinalScore;

        //                context.SaveChanges();

        //                var Response = new
        //                {
        //                    Response = "1",
        //                    Message = "Tutor Feedback Created"
        //                };

        //                string jsonResponse = JsonConvert.SerializeObject(Response);
        //                return Ok(jsonResponse);
        //            }
        //            else
        //            {

        //                StudentFeedback? StudentFeedback = new StudentFeedback();
        //                StudentFeedback = context.StudentFeedbacks.Where(x => x.Id == TutorFeedbackDto.Id).FirstOrDefault();

        //                if (StudentFeedback.IsPending == true && StudentFeedback.IsApproved == false)
        //                {


        //                    context.SaveChanges();

        //                    var Response = new
        //                    {
        //                        Response = "1",
        //                        Message = "Tutor Feedback Updated"
        //                    };

        //                    string jsonResponse = JsonConvert.SerializeObject(Response);
        //                    return Ok(jsonResponse);
        //                }
        //                else
        //                {
        //                    var Response = new
        //                    {
        //                        Response = "0",
        //                        Error = "Feedback Cann't Be Updated After Approved"
        //                    };

        //                    string jsonResponse = JsonConvert.SerializeObject(Response);
        //                    return BadRequest(jsonResponse);
        //                }
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


        [HttpGet("StudentFeedbacks/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentNotificationIndex(
         string studentId,
 [FromQuery] string? globalSearch,
 [FromQuery] long? feedbackDate,
 [FromQuery] string? tutorName,
 [FromQuery] string? courseName,
 [FromQuery] string? lessonName,
 [FromQuery] string? understanding,
 [FromQuery] string? explain,
 [FromQuery] string? clearToday,
 [FromQuery] string? clearPrevious,
 [FromQuery] string? teaching,
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
                    var query = context.StudentFeedbacks
                          .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Where(x => x.LessonBookings.CourseBookings.StudentApplicationUsers.Id == studentId)
                    .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName.Contains(globalSearch) ||
                        p.LessonBookings.CourseBookings.Courses.Name.Contains(globalSearch) ||
                                 p.LessonBookings.LessonName.Contains(globalSearch) ||
                                             p.UnderstandingAndEngagement.Contains(globalSearch) ||
                                             p.DidTutorExplainTopic.Contains(globalSearch) ||
                                             p.DidTutorClearDoubtToday.Contains(globalSearch) ||
                                             p.DidTutorClearDoubtPrevious.Contains(globalSearch) ||
                                             p.Status.Contains(globalSearch) ||
                                             p.RateTutorTeaching.Contains(globalSearch));
                    }

                    if (feedbackDate.HasValue)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(feedbackDate.Value);
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

                    if (!string.IsNullOrEmpty(understanding))
                    {
                        query = query.Where(p => p.UnderstandingAndEngagement.Contains(understanding));
                    }

                    if (!string.IsNullOrEmpty(explain))
                    {
                        query = query.Where(p => p.DidTutorExplainTopic.Contains(explain));
                    }

                    if (!string.IsNullOrEmpty(clearToday))
                    {
                        query = query.Where(p => p.DidTutorClearDoubtToday.Contains(clearToday));
                    }

                    if (!string.IsNullOrEmpty(clearPrevious))
                    {
                        query = query.Where(p => p.DidTutorClearDoubtPrevious.Contains(clearPrevious));
                    }

                    if (!string.IsNullOrEmpty(teaching))
                    {
                        query = query.Where(p => p.RateTutorTeaching.Contains(teaching));
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "feedbackdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                        case "tutorname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName) : query.OrderBy(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName);
                            break;
                        case "coursename":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.Courses.Name) : query.OrderBy(p => p.LessonBookings.CourseBookings.Courses.Name);
                            break;
                        case "lessonname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.LessonName) : query.OrderBy(p => p.LessonBookings.LessonName);
                            break;
                        case "understanding":
                            query = sortDescending ? query.OrderByDescending(p => p.UnderstandingAndEngagement) : query.OrderBy(p => p.UnderstandingAndEngagement);
                            break;
                        case "explain":
                            query = sortDescending ? query.OrderByDescending(p => p.DidTutorExplainTopic) : query.OrderBy(p => p.DidTutorExplainTopic);
                            break;
                        case "cleartoday":
                            query = sortDescending ? query.OrderByDescending(p => p.DidTutorClearDoubtToday) : query.OrderBy(p => p.DidTutorClearDoubtToday);
                            break;
                        case "clearprevious":
                            query = sortDescending ? query.OrderByDescending(p => p.DidTutorClearDoubtPrevious) : query.OrderBy(p => p.DidTutorClearDoubtPrevious);
                            break;
                        case "teaching":
                            query = sortDescending ? query.OrderByDescending(p => p.RateTutorTeaching) : query.OrderBy(p => p.RateTutorTeaching);
                            break;
                        case "status":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
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
                            Tutor = x.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName,
                            Course = x.LessonBookings.CourseBookings.Courses.Name,
                            Lesson = x.LessonBookings.LessonName,
                            Understanding = x.UnderstandingAndEngagement,
                            Explain = x.DidTutorExplainTopic,
                            ClearToday = x.DidTutorClearDoubtToday,
                            ClearPrevious = x.DidTutorClearDoubtPrevious,
                            Teaching = x.RateTutorTeaching,
                            Status = x.Status,

                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            StudentId = studentId,
                            Feedbacks = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Feedback Found"
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



        [HttpGet("StudentTutorFeedbacks/{studentId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> StudentTutorFeedbacks(
string studentId,
[FromQuery] string? globalSearch,
[FromQuery] long? feedbackDate,
[FromQuery] string? tutorName,
[FromQuery] string? courseName,
[FromQuery] string? lessonName,
[FromQuery] string? gradePrediction,
[FromQuery] string? avgGradePrediction,
[FromQuery] int? mental,
[FromQuery] int? understanding,
[FromQuery] bool? previoushomework,
[FromQuery] string? nexthomework,
[FromQuery] string? announcement,
[FromQuery] string? remarks,
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
                    var query = context.TutorFeedbacks
                          .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Where(x => x.LessonBookings.CourseBookings.StudentApplicationUsers.Id == studentId)
                    .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName.Contains(globalSearch) ||
                        p.LessonBookings.CourseBookings.Courses.Name.Contains(globalSearch) ||
                                 p.LessonBookings.LessonName.Contains(globalSearch) ||
                                 p.GradePrediction.Contains(globalSearch) ||
                                 p.AverageGradePrediction.Contains(globalSearch) ||
                                 p.MentalSkills == Convert.ToInt64(globalSearch) ||
                                 p.TopicUnderstandingLevel == Convert.ToInt64(globalSearch) ||
                                 p.NextHomework.Contains(globalSearch) ||
                                 p.Announcement.Contains(globalSearch) ||
                                 p.Status.Contains(globalSearch) ||
                                 p.Remarks.Contains(globalSearch));
                    }

                    if (feedbackDate.HasValue)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(feedbackDate.Value);
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

                    if (!string.IsNullOrEmpty(gradePrediction))
                    {
                        query = query.Where(p => p.GradePrediction.Contains(gradePrediction));
                    }

                    if (!string.IsNullOrEmpty(avgGradePrediction))
                    {
                        query = query.Where(p => p.AverageGradePrediction.Contains(avgGradePrediction));
                    }

                    if (mental.HasValue)
                    {
                        query = query.Where(p => p.MentalSkills == mental);
                    }

                    if (understanding.HasValue)
                    {
                        query = query.Where(p => p.TopicUnderstandingLevel == understanding);
                    }

                    if (previoushomework.HasValue)
                    {
                        query = query.Where(p => p.PreviousHomeWorkDone == previoushomework);
                    }

                    if (!string.IsNullOrEmpty(nexthomework))
                    {
                        query = query.Where(p => p.NextHomework.Contains(nexthomework));
                    }

                    if (!string.IsNullOrEmpty(announcement))
                    {
                        query = query.Where(p => p.Announcement.Contains(announcement));
                    }

                    if (!string.IsNullOrEmpty(remarks))
                    {
                        query = query.Where(p => p.Remarks.Contains(remarks));
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "feedbackdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                        case "tutorname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName) : query.OrderBy(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName);
                            break;
                        case "coursename":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.Courses.Name) : query.OrderBy(p => p.LessonBookings.CourseBookings.Courses.Name);
                            break;
                        case "lessonname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.LessonName) : query.OrderBy(p => p.LessonBookings.LessonName);
                            break;
                        case "gradeprediction":
                            query = sortDescending ? query.OrderByDescending(p => p.GradePrediction) : query.OrderBy(p => p.GradePrediction);
                            break;
                        case "avggradeprediction":
                            query = sortDescending ? query.OrderByDescending(p => p.AverageGradePrediction) : query.OrderBy(p => p.AverageGradePrediction);
                            break;
                        case "mental":
                            query = sortDescending ? query.OrderByDescending(p => p.MentalSkills) : query.OrderBy(p => p.MentalSkills);
                            break;
                        case "understanding":
                            query = sortDescending ? query.OrderByDescending(p => p.TopicUnderstandingLevel) : query.OrderBy(p => p.TopicUnderstandingLevel);
                            break;
                        case "previoushomework":
                            query = sortDescending ? query.OrderByDescending(p => p.PreviousHomeWorkDone) : query.OrderBy(p => p.PreviousHomeWorkDone);
                            break;
                        case "nexthomework":
                            query = sortDescending ? query.OrderByDescending(p => p.NextHomework) : query.OrderBy(p => p.NextHomework);
                            break;
                        case "announcement":
                            query = sortDescending ? query.OrderByDescending(p => p.Announcement) : query.OrderBy(p => p.Announcement);
                            break;
                        case "remarks":
                            query = sortDescending ? query.OrderByDescending(p => p.Remarks) : query.OrderBy(p => p.Remarks);
                            break;
                        case "status":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
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
                            Tutor = x.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName,
                            Course = x.LessonBookings.CourseBookings.Courses.Name,
                            Lesson = x.LessonBookings.LessonName,
                            GradePrediction = x.GradePrediction,
                            AverageGradePrediction = x.AverageGradePrediction,
                            MentalSkills = x.MentalSkills,
                            TopicUnderstandingLevel = x.TopicUnderstandingLevel,
                            PreviousHomeWorkDone = x.PreviousHomeWorkDone,
                            NextHomework = x.NextHomework,
                            Announcement = x.Announcement,
                            Remarks = x.Remarks,
                            Status = x.Status
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            StudentId = studentId,
                            Feedbacks = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Feedback Found"
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





        [HttpGet("TutorFeedbacks/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorStudentFeedbacks(
string tutorId,
[FromQuery] string? globalSearch,
[FromQuery] long? feedbackDate,
[FromQuery] string? studentName,
[FromQuery] string? courseName,
[FromQuery] string? lessonName,
[FromQuery] string? gradePrediction,
[FromQuery] string? avgGradePrediction,
[FromQuery] int? mental,
[FromQuery] int? understanding,
[FromQuery] bool? previoushomework,
[FromQuery] string? nexthomework,
[FromQuery] string? announcement,
[FromQuery] string? remarks,
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
                    var query = context.TutorFeedbacks
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
              .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
              .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
              .Where(x => x.LessonBookings.CourseBookings.TutorApplicationUsers.Id == tutorId)
              .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName.Contains(globalSearch) ||
                        p.LessonBookings.CourseBookings.Courses.Name.Contains(globalSearch) ||
                                 p.LessonBookings.LessonName.Contains(globalSearch) ||
                                 p.GradePrediction.Contains(globalSearch) ||
                                 p.AverageGradePrediction.Contains(globalSearch) ||
                                 p.MentalSkills == Convert.ToInt64(globalSearch) ||
                                 p.TopicUnderstandingLevel == Convert.ToInt64(globalSearch) ||
                                 p.NextHomework.Contains(globalSearch) ||
                                 p.Announcement.Contains(globalSearch) ||
                                 p.Status.Contains(globalSearch) ||
                                 p.Remarks.Contains(globalSearch));
                    }

                    if (feedbackDate.HasValue)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(feedbackDate.Value);
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

                    if (!string.IsNullOrEmpty(gradePrediction))
                    {
                        query = query.Where(p => p.GradePrediction.Contains(gradePrediction));
                    }

                    if (!string.IsNullOrEmpty(avgGradePrediction))
                    {
                        query = query.Where(p => p.AverageGradePrediction.Contains(avgGradePrediction));
                    }

                    if (mental.HasValue)
                    {
                        query = query.Where(p => p.MentalSkills == mental);
                    }

                    if (understanding.HasValue)
                    {
                        query = query.Where(p => p.TopicUnderstandingLevel == understanding);
                    }

                    if (previoushomework.HasValue)
                    {
                        query = query.Where(p => p.PreviousHomeWorkDone == previoushomework);
                    }

                    if (!string.IsNullOrEmpty(nexthomework))
                    {
                        query = query.Where(p => p.NextHomework.Contains(nexthomework));
                    }

                    if (!string.IsNullOrEmpty(announcement))
                    {
                        query = query.Where(p => p.Announcement.Contains(announcement));
                    }

                    if (!string.IsNullOrEmpty(remarks))
                    {
                        query = query.Where(p => p.Remarks.Contains(remarks));
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "feedbackdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                        case "studentname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName) : query.OrderBy(p => p.LessonBookings.CourseBookings.TutorApplicationUsers.FirstName);
                            break;
                        case "coursename":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.Courses.Name) : query.OrderBy(p => p.LessonBookings.CourseBookings.Courses.Name);
                            break;
                        case "lessonname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.LessonName) : query.OrderBy(p => p.LessonBookings.LessonName);
                            break;
                        case "gradeprediction":
                            query = sortDescending ? query.OrderByDescending(p => p.GradePrediction) : query.OrderBy(p => p.GradePrediction);
                            break;
                        case "avggradeprediction":
                            query = sortDescending ? query.OrderByDescending(p => p.AverageGradePrediction) : query.OrderBy(p => p.AverageGradePrediction);
                            break;
                        case "mental":
                            query = sortDescending ? query.OrderByDescending(p => p.MentalSkills) : query.OrderBy(p => p.MentalSkills);
                            break;
                        case "understanding":
                            query = sortDescending ? query.OrderByDescending(p => p.TopicUnderstandingLevel) : query.OrderBy(p => p.TopicUnderstandingLevel);
                            break;
                        case "previoushomework":
                            query = sortDescending ? query.OrderByDescending(p => p.PreviousHomeWorkDone) : query.OrderBy(p => p.PreviousHomeWorkDone);
                            break;
                        case "nexthomework":
                            query = sortDescending ? query.OrderByDescending(p => p.NextHomework) : query.OrderBy(p => p.NextHomework);
                            break;
                        case "announcement":
                            query = sortDescending ? query.OrderByDescending(p => p.Announcement) : query.OrderBy(p => p.Announcement);
                            break;
                        case "remarks":
                            query = sortDescending ? query.OrderByDescending(p => p.Remarks) : query.OrderBy(p => p.Remarks);
                            break;
                        case "status":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
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
                            Student = x.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName,
                            Course = x.LessonBookings.CourseBookings.Courses.Name,
                            Lesson = x.LessonBookings.LessonName,
                            GradePrediction = x.GradePrediction,
                            AverageGradePrediction = x.AverageGradePrediction,
                            MentalSkills = x.MentalSkills,
                            TopicUnderstandingLevel = x.TopicUnderstandingLevel,
                            PreviousHomeWorkDone = x.PreviousHomeWorkDone,
                            NextHomework = x.NextHomework,
                            Announcement = x.Announcement,
                            Remarks = x.Remarks,
                            Status = x.Status
                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            TutorId = tutorId,
                            Feedbacks = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Feedback Found"
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

        [HttpGet("TutorStudentFeedbacks/{tutorId}")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> TutorStudentFeedbacks(
 string tutorId,
[FromQuery] string? globalSearch,
[FromQuery] long? feedbackDate,
[FromQuery] string? studentName,
[FromQuery] string? courseName,
[FromQuery] string? lessonName,
[FromQuery] string? understanding,
[FromQuery] string? explain,
[FromQuery] string? clearToday,
[FromQuery] string? clearPrevious,
[FromQuery] string? teaching,
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
                    var query = context.StudentFeedbacks
                          .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                    .Include(x => x.LessonBookings).ThenInclude(x => x.CourseBookings).ThenInclude(x => x.Courses)
                    .Where(x => x.LessonBookings.CourseBookings.TutorApplicationUsers.Id == tutorId)
                    .AsQueryable();

                    if (!string.IsNullOrEmpty(globalSearch))
                    {
                        query = query.Where(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName.Contains(globalSearch) ||
                        p.LessonBookings.CourseBookings.Courses.Name.Contains(globalSearch) ||
                                 p.LessonBookings.LessonName.Contains(globalSearch) ||
                                             p.UnderstandingAndEngagement.Contains(globalSearch) ||
                                             p.DidTutorExplainTopic.Contains(globalSearch) ||
                                             p.DidTutorClearDoubtToday.Contains(globalSearch) ||
                                             p.DidTutorClearDoubtPrevious.Contains(globalSearch) ||
                                             p.Status.Contains(globalSearch) ||
                                             p.RateTutorTeaching.Contains(globalSearch));
                    }

                    if (feedbackDate.HasValue)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(feedbackDate.Value);
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

                    if (!string.IsNullOrEmpty(understanding))
                    {
                        query = query.Where(p => p.UnderstandingAndEngagement.Contains(understanding));
                    }

                    if (!string.IsNullOrEmpty(explain))
                    {
                        query = query.Where(p => p.DidTutorExplainTopic.Contains(explain));
                    }

                    if (!string.IsNullOrEmpty(clearToday))
                    {
                        query = query.Where(p => p.DidTutorClearDoubtToday.Contains(clearToday));
                    }

                    if (!string.IsNullOrEmpty(clearPrevious))
                    {
                        query = query.Where(p => p.DidTutorClearDoubtPrevious.Contains(clearPrevious));
                    }

                    if (!string.IsNullOrEmpty(teaching))
                    {
                        query = query.Where(p => p.RateTutorTeaching.Contains(teaching));
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(p => p.Status.Contains(status));
                    }

                    bool sortDescending = sortDesc ?? false;

                    switch (sortBy?.ToLower())
                    {
                        case "feedbackdate,":
                            query = sortDescending ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                            break;
                        case "studentname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName) : query.OrderBy(p => p.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName);
                            break;
                        case "coursename":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.CourseBookings.Courses.Name) : query.OrderBy(p => p.LessonBookings.CourseBookings.Courses.Name);
                            break;
                        case "lessonname":
                            query = sortDescending ? query.OrderByDescending(p => p.LessonBookings.LessonName) : query.OrderBy(p => p.LessonBookings.LessonName);
                            break;
                        case "understanding":
                            query = sortDescending ? query.OrderByDescending(p => p.UnderstandingAndEngagement) : query.OrderBy(p => p.UnderstandingAndEngagement);
                            break;
                        case "explain":
                            query = sortDescending ? query.OrderByDescending(p => p.DidTutorExplainTopic) : query.OrderBy(p => p.DidTutorExplainTopic);
                            break;
                        case "cleartoday":
                            query = sortDescending ? query.OrderByDescending(p => p.DidTutorClearDoubtToday) : query.OrderBy(p => p.DidTutorClearDoubtToday);
                            break;
                        case "clearprevious":
                            query = sortDescending ? query.OrderByDescending(p => p.DidTutorClearDoubtPrevious) : query.OrderBy(p => p.DidTutorClearDoubtPrevious);
                            break;
                        case "teaching":
                            query = sortDescending ? query.OrderByDescending(p => p.RateTutorTeaching) : query.OrderBy(p => p.RateTutorTeaching);
                            break;
                        case "status":
                            query = sortDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
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
                            Student = x.LessonBookings.CourseBookings.StudentApplicationUsers.FirstName,
                            Course = x.LessonBookings.CourseBookings.Courses.Name,
                            Lesson = x.LessonBookings.LessonName,
                            Understanding = x.UnderstandingAndEngagement,
                            Explain = x.DidTutorExplainTopic,
                            ClearToday = x.DidTutorClearDoubtToday,
                            ClearPrevious = x.DidTutorClearDoubtPrevious,
                            Teaching = x.RateTutorTeaching,
                            Status = x.Status,

                        })
                        .ToList();

                    if (result.Any())
                    {

                        var response = new
                        {
                            Response = "1",
                            TutorId = tutorId,
                            Feedbacks = result
                        };

                        string jsonResponse = JsonConvert.SerializeObject(response);
                        return Ok(jsonResponse);
                    }
                    else
                    {
                        var Response = new
                        {
                            Response = "0",
                            Error = "No Feedback Found"
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


