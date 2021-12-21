using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fido2NetLib.Objects;
using Fido2NetLib;

namespace MoreNote.Models.Model.FIDO2
{
    public class AssertionClientParams
    {
        public string Username { get; set; }
            public UserVerificationRequirement? UserVerification { get; set; }
            public AuthenticatorSelection authenticatorSelection { get; set; }
            public AuthenticationExtensionsClientOutputs Extensions { get; set; }
    }
}
