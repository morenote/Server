using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fido2NetLib;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.Models.Entity.ConfigFile;
using MoreNote.Models.Model.FIDO2;

namespace MoreNote.Logic.Security.Service
{
    public class FIDO2Service
    {
        private readonly IFido2 _fido2;
   
        private DataContext dataContext;
        private WebSiteConfig config;
        private FIDO2Config fido2Config;

        public FIDO2Service( DataContext dataContext, ConfigFileService configFileService)
        {
            this.dataContext = dataContext;
            this.config =configFileService.WebConfig;
            this.fido2Config = config.SecurityConfig.FIDO2Config;
            _fido2 = new Fido2(new Fido2Configuration()
            {
                ServerDomain =fido2Config.ServerDomain,
                ServerName = fido2Config.ServerName,
                Origin = fido2Config.Origin
            },FIDO2Conformance.MetadataServiceInstance(
                System.IO.Path.Combine(fido2Config.MDSCacheDirPath, @"Conformance"), fido2Config.Origin));

        }
        public CredentialCreateOptions MakeCredentialOptions(MakeCredentialParams opts)
        {
            var attType = opts.Attestation;

            var username = Encoding.UTF8.GetBytes(opts.UsernameHex);
           
            var userId=opts.UsernameHex.ToLongByHex();

             // 1. Get user from DB by username (in our example, auto create missing users)
            var user =dataContext.User.Where(u=>u.UserId==userId).FirstOrDefault();
            if (user == null)
            {
                throw new Exception();
            }




            return null;


        }

    }
}