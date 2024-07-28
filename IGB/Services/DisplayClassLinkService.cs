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
    public class DisplayClassLinkService : IHostedService, IDisposable
    {
        private Timer _timer;
        private Timer _tutortimer;
        private readonly IServiceScopeFactory _scopeFactory;

        public DisplayClassLinkService(IServiceScopeFactory scopeFactory)
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
                _tutortimer = new Timer(DoTutorWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Check every minute
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
                var notificationTime1 = currentTime.AddMinutes(9.5);
                var notificationTime2 = currentTime.AddMinutes(10.5);

                List<LessonBooking> lessonBookings = context.LessonBookings
                          .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
        .Include(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers).ThenInclude(x => x.StudentApplicationUsers)
        .Include(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
        .Include(x => x.CourseBookings).ThenInclude(x => x.AdminApplicationUsers)
        .Include(x => x.CourseBookings)
                    .Where(x => x.IsBooked==true && x.StartTime > notificationTime1 && x.StartTime < notificationTime2)
                    .ToList();


                if (lessonBookings.Any())
                {
                    foreach (var booking in lessonBookings)
                    {
                        using (var scope1 = _scopeFactory.CreateScope())
                        {
                            var context1 = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            if (booking.ClassLink != null)
                            {
                                booking.StudentClassStartAccess = true;
                                context1.SaveChanges();
                            }
                            else
                            {
                                var obj1 = new AllNotification
                                {
                                    ForTutor = true,
                                    LessonBookingStartId = booking.Id,
                                    Notification = $"Still You Did Not Upload The Class Link , {booking.LessonName} Lesson Will Start After 10 Minutes",
                                    ForTutorId = booking.CourseBookings.TutorApplicationUsers.Id
                                };
                                context1.AllNotifications.Add(obj1);
                                context1.SaveChanges();
                            }
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

        private void DoTutorWork(object state)
        {
            try
            {

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var currentTime = DateTime.UtcNow;
                var notificationTime1 = currentTime.AddMinutes(14.5);
                var notificationTime2 = currentTime.AddMinutes(15.5);

                List<LessonBooking> lessonBookings = context.LessonBookings
                                 .Include(x => x.CourseBookings).ThenInclude(x => x.Courses)
        .Include(x => x.CourseBookings).ThenInclude(x => x.StudentApplicationUsers).ThenInclude(x => x.StudentApplicationUsers)
        .Include(x => x.CourseBookings).ThenInclude(x => x.AdminApplicationUsers)
        .Include(x => x.CourseBookings).ThenInclude(x => x.TutorApplicationUsers)
                    .Where(x => x.IsBooked == true && x.StartTime > notificationTime1 && x.StartTime < notificationTime2)
                    .ToList();


                if (lessonBookings.Any())
                {
                    foreach (var booking in lessonBookings)
                    {
                        using (var scope1 = _scopeFactory.CreateScope())
                        {
                            var context1 = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            if (booking.ClassLink != null)
                            {
                                booking.TutorClassStartAccess = true;
                                context1.SaveChanges();
                            }
                            else
                            {
                                  
                                        booking.TutorClassStartAccess = true;
                                        context1.SaveChanges();
                                    
                                    var obj1 = new AllNotification
                                {
                                    ForTutor = true,
                                    LessonBookingStartId = booking.Id,
                                    Notification = $"Still You Did Not Upload The Class Link , {booking.LessonName} Lesson Will Start After 15 Minutes",
                                    ForTutorId = booking.CourseBookings.TutorApplicationUsers.Id
                                };
                                context1.AllNotifications.Add(obj1);
                                context1.SaveChanges();
                            }
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
                _tutortimer?.Change(Timeout.Infinite, 0);
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
                _tutortimer?.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
         
        }
    }
}
