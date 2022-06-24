using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoreNote.Common.Utils.Tests
{
    [TestClass()]
    public class UnixTimeHelperTests
    {
        [TestMethod()]
        public void GetTimeStampInInt32Test()
        {
           DateTime dateTime=  Convert.ToDateTime("2017/3/9 9:19:0");
            Int32 unixTimestamp = (Int32)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Console.WriteLine(unixTimestamp);


        }
    }
}