using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fido2NetLib;
using Fido2NetLib.Objects;

namespace MoreNote.Models.Model.FIDO2
{

    public class MakeCredentialParams
    {
       
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public AttestationConveyancePreference Attestation { get; set; }
        public AuthenticatorSelection AuthenticatorSelection { get; set; }
   
    }
}