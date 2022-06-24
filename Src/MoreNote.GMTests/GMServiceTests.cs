using MoreNote.Logic.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.GM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoreNote.GMTests
{
    [TestClass()]
    public class GMServiceTests
    {



        [TestMethod()]
        public void SM2EncryptTest()
        {
            var data = "00000000000000000000000000000000";
            var pubk = "04F54CEEB470BAFCCE989A98D65BE1AEF562FC0C94DE152A1D658689E1D01692E7BB81C76DBA09CEF76C1386F9E0D02846F3C28BBDB11D697E9DE56341F90B1DE3";
            GMService gMService = new GMService();
            var enc = gMService.SM2Encrypt(data, pubk);
            Console.WriteLine(enc);
        }

        [TestMethod()]
        public void SM2DecryptTest()
        {
            var data = "04a68d2adbbb5a186de7e26c026f07a9d8039a4f1fe2347e35317261de7757ded5864cca6a1e5c27b1c7ecd05c906867664ce26794f901c24622501dd2e7280df4639e0a1a646997bb33935b5c7f2ad8bc7feca2ac9d77d9e2934749b20d1705678feea82576e7cd9a50d37308b77dfd3df1c126b72b4f068b6c455a503532adcb";
            var priKey = "811ED43E4D4A716D6192F04A204E6DBDDF1F99EFEE3B6D85C0328B17C5E11612";
            GMService gMService = new GMService();
            var dec = gMService.SM2Decrypt(data, priKey, true);
            Console.WriteLine(dec);
          
        }

        [TestMethod()]
        public void SM4DecTest()
        {
            var data = "b7d8fd16a6469166af53594a09f94a67a94b6cdea71f7acaa342e2ad9635b090c4b6918401bcb76ef414bad5c41fd685";
            var key = "00000000000000000000000000000000";
            var iv = "00000000000000000000000000000000";//测试用途
            var expect = "3030303030303030303030303030303030303030303030303030303030303030";
            GMService gMService = new GMService();
            var dec = gMService.SM4_Decrypt_CBC(data, key, iv, true);
            Console.WriteLine(dec);
            Assert.AreEqual(dec, expect);
        }

        [TestMethod()]
        public void SM4EncTest()
        {
            //原始数据
            var data = "3030303030303030303030303030303030303030303030303030303030303030";
            var key = "00000000000000000000000000000000";
            var iv = "00000000000000000000000000000000";//测试用途
            GMService gMService = new GMService();
            var enc = gMService.SM4_Encrypt_CBC(data, key, iv, true);
            enc = enc.ToUpper();
            //期望数据
            var expect = "b7d8fd16a6469166af53594a09f94a67a94b6cdea71f7acaa342e2ad9635b090c4b6918401bcb76ef414bad5c41fd685";

            Console.WriteLine(enc);
            Assert.AreEqual(enc.ToUpper(), expect.ToUpper());
        }

        [TestMethod()]
        public void SM3Test()
        {
            var data = "3030303030303030303030303030303030303030303030303030303030303030";
            var expect = "557D7424ACA47640B500A525D2B53C4B2E59E552704722291AAC4D52695546AA";
            var gm = new GMService();
            var sm3=  gm.SM3(data);
            Console.WriteLine(sm3);
            Assert.AreEqual(sm3.ToUpper(), expect);

        }
    }
}