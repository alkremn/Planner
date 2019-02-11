using Planner.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using Planner.Model;

namespace Planner.UI.DataService
{
    public class AppointmentDataService : IAppointmentDataService
    {
        private Func<PlannerDbContext> _contextCreator;

        public AppointmentDataService(Func<PlannerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<AppointmentWrapper>> GetAppointmentsAsync()
        {
            var appointments = await Task.Run(async () => //Using Lambda to load all Appointments and create Wrapper class
            {
                using (var context = _contextCreator())
                {
                    return await (from app in context.Appointments
                                  join cus in context.Customers
                                  on app.CustomerId equals cus.CustomerId
                                  select new AppointmentWrapper
                                  {
                                      _AppointmentId = app.AppointmentId,
                                      _CustomerId = app.CustomerId,
                                      _CustomerName = cus.CustomerName,
                                      _Description = app.Description,
                                      _Title = app.Title,
                                      _Start = app.Start,
                                      _End = app.End,
                                      _Contact = app.Contact,
                                      _Location = app.Location,
                                      _CreateDate = app.CreateDate,
                                      _CreatedBy = app.CreatedBy,
                                      _LastUpdate = app.LastUpdate,
                                      _LastUpdateBy = app.LastUpdateBy,
                                      _Type = app.Url
                                  }).ToListAsync();
                }
            });
            return appointments;
        }

        public async Task SaveNewAppointmentAsync(AppointmentWrapper newAppointment, User user)
        {
            await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    var appointment = CreateAppointment(newAppointment, user);
                    var appId = 0;
                    if (context.Appointments.Count() > 0)
                    {
                        appId = await context.Appointments.MaxAsync(app => app.AppointmentId);
                        appId++;
                    }
                    appointment.AppointmentId = appId;

                    context.Appointments.Add(appointment);
                    context.Entry(appointment).State = EntityState.Added;

                    await context.SaveChangesAsync();
                }
            });
        }

        public async Task ModifyAppointmentAsync(AppointmentWrapper modifiedApp, User user)
        {
            await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    var appointment = CreateAppointment(modifiedApp, user);
                    context.Entry(appointment).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            });
        }

        public async Task<bool> DeleteAppointmentAsync(AppointmentWrapper selectedAppointment, User user)
        {
            await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    var appointment = CreateAppointment(selectedAppointment, user);
                    appointment.CreateDate = selectedAppointment._CreateDate;
                    appointment.LastUpdate = selectedAppointment._LastUpdate;

                    context.Appointments.Attach(appointment);
                    context.Appointments.Remove(appointment);

                    context.Entry(appointment).State = EntityState.Deleted;

                    await context.SaveChangesAsync();
                }
            });
            return true;
        }

        private Appointment CreateAppointment(AppointmentWrapper appWrapper, User user)
        {
            var dateOfCreation = appWrapper._CreateDate;
            if (!appWrapper.ExistingAppointment)
                dateOfCreation = DateTime.UtcNow;

            return new Appointment
            {
                AppointmentId = appWrapper._AppointmentId,
                CustomerId = appWrapper._CustomerId,
                Description = appWrapper._Description,
                Title = appWrapper._Title,
                Start = appWrapper._Start,
                End = appWrapper._End,
                Contact = appWrapper._Contact,
                Location = appWrapper._Location,
                CreateDate = dateOfCreation,
                CreatedBy = user.UserId.ToString(),
                LastUpdate = DateTime.UtcNow,
                LastUpdateBy = user.UserId.ToString(),
                Url = appWrapper._Type
            };
        }

        public async Task<bool> DeleteAppointmentListAsync(List<AppointmentWrapper> appointments)
        {
            await Task.Run(async () =>
            {
                using (var context = _contextCreator())
                {
                    foreach (var app in appointments)
                    {
                        var appointment = CreateAppointment(app, new User { UserId = int.Parse(app._CreatedBy) });

                        context.Appointments.Attach(appointment);
                        context.Appointments.Remove(appointment);
                        context.Entry(appointment).State = EntityState.Deleted;
                    }
                    await context.SaveChangesAsync();
                }
            });
            return true;
        }
    }
}
