using Planner.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planner.UI.DataService
{
    public interface IAppointmentDataService
    {
        Task<List<AppointmentWrapper>> GetAppointmentsAsync();

        Task SaveNewAppointmentAsync(AppointmentWrapper newAppointment, User user);

        Task ModifyAppointmentAsync(AppointmentWrapper modifiedApp, User user);

        Task<bool> DeleteAppointmentAsync(AppointmentWrapper selectedAppointment, User user);

        Task<bool> DeleteAppointmentListAsync(List<AppointmentWrapper> appointments);
    }
}
