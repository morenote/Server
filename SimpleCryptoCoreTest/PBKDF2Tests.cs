using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleCrypto.Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCrypto.Core.Tests
{
    [TestClass()]
    public class PBKDF2Tests
    {
        [TestMethod()]
        public void PBKDF2Test()
        {
            
        ICryptoService cryptoService = new PBKDF2();

        //New User
        string password = "password";

        //save this salt to the database
        string salt = cryptoService.GenerateSalt();

        //save this hash to the database
        string hashedPassword = cryptoService.Compute(password);
            
        //validate user
        //compare the password (this should be true since we are rehashing the same password and using the same generated salt)
        string hashedPassword2 = cryptoService.Compute(password, salt);
        
        bool isPasswordValid = cryptoService.Compare(hashedPassword, hashedPassword2);
     
        Assert.IsTrue(isPasswordValid);

        }


    }
}