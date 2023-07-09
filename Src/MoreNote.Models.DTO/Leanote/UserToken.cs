using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO.Leanote
{
    public class UserToken
    {
        public string Token { get; set; }
        public long? UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
