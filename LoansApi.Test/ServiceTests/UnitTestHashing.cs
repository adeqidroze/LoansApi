using LoansApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansApi.Test.ServiceTests
{
    [TestClass]
    public class UnitTestHashing
    {
        private  IHashService _hashService;

        [TestInitialize]
        public  void setup()
        {
            _hashService = new  HashPasswordService();
        }

        [TestMethod]
        public void GenerateSaltedHashPassword_Success()
        {
            var salt = _hashService.GenerateSalt();
            var password = "Abgdzzsd123!";
            var hash1 = _hashService.GenerateHash(password, salt);
            var hash2 = _hashService.GenerateHash(password, salt);

            Assert.AreEqual(hash1,hash2);
            Console.WriteLine(hash1 + "\n" + hash2);

        }

        [TestMethod]
        public void GenerateSalt_Success()
        {
            var salt = _hashService.GenerateSalt();
            Assert.IsInstanceOfType(salt, typeof(string));
        }

        [TestMethod]
        public void GenerateSaltedHashPassword_Fail()
        {
            var salt = _hashService.GenerateSalt();
            var password = "Abgdzzsd123!";
            var hash1 = _hashService.GenerateHash(password, salt);
            var hash2 = _hashService.GenerateHash(password, salt+"A");

            Assert.AreNotEqual(hash1, hash2);
            Console.WriteLine(hash1 + "\n" + hash2);

        }

       
    }
}
