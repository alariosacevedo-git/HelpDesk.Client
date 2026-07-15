using HelpDesk.Client.Business.Dto;
using Microsoft.Extensions.Logging;
using System;

namespace HelpDesk.Client.Business.Observers
{
    public class UserSubject : IUserSubject
    {
        private readonly ILogger<UserSubject> _logger;
        private readonly List<IUserObserver> _userObservers = new();
        public string Id { get; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public UserSubject(string id, string email)
        {
            
            Id = id;
            Email = email;
        }

        public async Task UpdateEmail(string newEmail)
        {
            Email = newEmail;

            UserUpdatedDto dto = new UserUpdatedDto
            {
                Id = Id,
                Email = Email,
            };

            await Notify(dto);
        }

        public void Attach(IUserObserver iUserObserver)
        {
            _userObservers.Add(iUserObserver);
        }

        public void Detach(IUserObserver iUserObserver)
        {
            _userObservers.Remove(iUserObserver);
        }

        public async Task Notify(UserUpdatedDto userUpdatedDto)
        {
            var tasks = _userObservers.Select(o => o.OnUserUpdated(userUpdatedDto));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AccountController error");
            }
        }
    }
}
