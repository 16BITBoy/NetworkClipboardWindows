using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string ip = "192.168.1.33";
            
            //Regex pattern = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");
            Regex pattern = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
            Assert.IsTrue(pattern.IsMatch(ip), "Patron no identifica ip correcta");

            //ip = "1.1.1.1";
            //Assert.IsTrue(pattern.IsMatch(ip));

            //ip = "a.d.3.d";
            //Assert.IsFalse(pattern.IsMatch(ip));

            //ip = "lñasjkdfñ";
            //Assert.IsFalse(pattern.IsMatch(ip));
        }
    }
}
