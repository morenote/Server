using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.MSync.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MSync.Models.Tests
{
    [TestClass()]
    public class LocalRepositoryTests
    {
        [TestMethod()]
        public void InitTest()
        {

            LocalRepository localRepository = new LocalRepository();
            localRepository.BasePath = "D:/test/";
            localRepository.Init();
           
        }
    }
}