using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Client.Dto.Documents
{
    public class EmailConfirmedDto
    {
        public string Email { get; set; }  = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
