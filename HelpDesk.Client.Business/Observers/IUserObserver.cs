using HelpDesk.Client.Business.Dto;

namespace HelpDesk.Client.Business.Observers
{
    public interface IUserObserver
    {
        Task OnUserInserted(UserUpdatedDto userUpdatedDto);
        Task OnUserUpdated(UserUpdatedDto userUpdatedDto);
        Task OnUserDeleted(UserUpdatedDto userUpdatedDto);
    }
}
