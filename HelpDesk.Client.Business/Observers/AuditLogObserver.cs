using HelpDesk.Client.Business.Dto;

namespace HelpDesk.Client.Business.Observers
{
    public class AuditLogObserver : IUserObserver
    {
        public async Task OnUserInserted(UserUpdatedDto userUpdatedDto)
        {
            Console.WriteLine($"[Audit] Email = {userUpdatedDto.Email} inserted updated at {DateTime.Now}");
        }

        public async Task OnUserUpdated(UserUpdatedDto userUpdatedDto)
        {
            Console.WriteLine($"[Audit] Email = {userUpdatedDto.Email} updated at {DateTime.Now}");
        }
        public async Task OnUserDeleted(UserUpdatedDto userUpdatedDto)
        {
            Console.WriteLine($"[Audit] Email = {userUpdatedDto.Email} deleted at {DateTime.Now}");
        }
    }
}
