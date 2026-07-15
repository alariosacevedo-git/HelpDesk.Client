using HelpDesk.Client.Business.Dto;

namespace HelpDesk.Client.Business.Observers
{
    public class EmailObserver : IUserObserver
    {
        public async Task OnUserDeleted(UserUpdatedDto userUpdatedDto)
        {
            Console.WriteLine($"[Email] User {userUpdatedDto.UserName} deleted email to {userUpdatedDto.Email}");
        }

        public async Task OnUserInserted(UserUpdatedDto userUpdatedDto)
        {
            Console.WriteLine($"[Email] User {userUpdatedDto.UserName} inserted email to {userUpdatedDto.Email}");
        }

        public async Task OnUserUpdated(UserUpdatedDto userUpdatedDto)
        {
            Console.WriteLine($"[Email] User {userUpdatedDto.UserName} updated email to {userUpdatedDto.Email}");
        }
    }
}
