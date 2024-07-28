using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IGB.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IGB.Models;
using Microsoft.EntityFrameworkCore;

namespace IGB.Services
{
    public class SendClassStartNotification : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public SendClassStartNotification(IServiceScopeFactory scopeFactory)
        {
            try
            {
                _scopeFactory = scopeFactory;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Check every minute
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }        
        }

        private void DoWork(object state)
        {
            try
            {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var currentTime = DateTime.UtcNow;
                var notificationTime1 = currentTime.AddMinutes(1439);
                var notificationTime2 = currentTime.AddMinutes(1440);

                var notificationTime3 = currentTime.AddMinutes(119);
                var notificationTime4 = currentTime.AddMinutes(121);

                List<LessonBooking> lessonBookings24 = context.LessonBookings
                     .Include(x => x.CourseBookings)
        .ThenInclude(x => x.Courses)
        .Include(x => x.CourseBookings)
        .ThenInclude(x => x.StudentApplicationUsers)
        .Include(x => x.CourseBookings)
        .ThenInclude(x => x.AdminApplicationUsers)
        .Include(x => x.CourseBookings)
        .ThenInclude(x => x.TutorApplicationUsers)
                    .Where(x => x.IsBooked == true && x.StartTime > notificationTime1 && x.StartTime < notificationTime2)
                    .ToList();


                if (lessonBookings24.Any())
                {
                    foreach (var booking in lessonBookings24)
                    {
                        using (var scope1 = _scopeFactory.CreateScope())
                        {
                            var context1 = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            var obj = new AllNotification
                            {
                                ForStudent = true,
                                LessonBookingStartId = booking.Id,
                                Notification = $"{booking.LessonName} Lesson Will Start After 24 Hours",
                                ForStudentId = booking.CourseBookings.StudentApplicationUsers.Id
                            };
                            context1.AllNotifications.Add(obj);
                            context1.SaveChanges();

                            var obj1 = new AllNotification
                            {
                                ForTutor = true,
                                LessonBookingStartId = booking.Id,
                                Notification = $"{booking.LessonName} Lesson Will Start After 24 Hours",
                                ForTutorId = booking.CourseBookings.TutorApplicationUsers.Id
                            };
                            context1.AllNotifications.Add(obj1);
                            context1.SaveChanges();
                        }

                    }
                }



                List<LessonBooking> lessonBookings2 = context.LessonBookings
                .Include(x => x.CourseBookings)
   .ThenInclude(x => x.Courses)
   .Include(x => x.CourseBookings)
   .ThenInclude(x => x.StudentApplicationUsers)
   .Include(x => x.CourseBookings)
   .ThenInclude(x => x.AdminApplicationUsers)
   .Include(x => x.CourseBookings)
   .ThenInclude(x => x.TutorApplicationUsers)
               .Where(x => x.IsBooked == true && x.StartTime > notificationTime3 && x.StartTime < notificationTime4)
               .ToList();


                if (lessonBookings2.Any())
                {
                    foreach (var booking in lessonBookings2)
                    {
                        using (var scope1 = _scopeFactory.CreateScope())
                        {
                            var context1 = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            var obj = new AllNotification
                            {
                                ForStudent = true,
                                LessonBookingStartId = booking.Id,
                                Notification = $"{booking.LessonName} Lesson Will Start After 2 Hours",
                                ForStudentId = booking.CourseBookings.StudentApplicationUsers.Id
                            };
                            context1.AllNotifications.Add(obj);
                            context1.SaveChanges();

                            var obj1 = new AllNotification
                            {
                                ForTutor = true,
                                LessonBookingStartId = booking.Id,
                                Notification = $"{booking.LessonName} Lesson Will Start After 2 Hours",
                                ForTutorId = booking.CourseBookings.TutorApplicationUsers.Id
                            };
                            context1.AllNotifications.Add(obj1);
                            context1.SaveChanges();
                        }

                    }
                }

            }

            }
            catch (Exception)
            {

                throw;
            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _timer?.Change(Timeout.Infinite, 0);
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }  
        }

        public void Dispose()
        {
            try
            {
                _timer?.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
       
        }
    }
}
