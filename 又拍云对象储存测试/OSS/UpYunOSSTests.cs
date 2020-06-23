using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpYunLibrary.OSS;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpYunLibrary.OSS.Tests
{
    [TestClass()]
    public class UpYunOSSTests
    {
        [TestMethod()]
        public void GetPolicyTest()
        {
            OSSOptions options = new OSSOptions();
            options.bucket = "morenote-file";
            options.save_key = "/{year}/{mon}/{day}/{filemd5}{.suffix}";
            options.expiration = 86400;
            string policy = UpYunOSS.GetPolicy(options);
            Console.WriteLine(policy);

            var signature= UpYunOSS.GetSignature(policy, "");
            Console.WriteLine(signature);

        }

        [TestMethod()]
        public void GetSignatureTest()
        {

        }
    }
}