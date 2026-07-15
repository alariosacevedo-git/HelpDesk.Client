using HelpDesk.Client.Business.Dto;

namespace HelpDesk.Client.Business.Observers
{
    public interface IUserSubject
    {
        void Attach(IUserObserver iUserObserver);
        void Detach(IUserObserver iUserObserver);
        Task Notify(UserUpdatedDto userUpdatedDto);
    }
}
