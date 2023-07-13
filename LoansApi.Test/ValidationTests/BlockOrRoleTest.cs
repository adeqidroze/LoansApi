using FluentValidation.TestHelper;
using LoansApi.DTOs;
using LoansApi.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace LoansApi.Test.ValidationTests
{
    public class BlockOrRoleTest
    {
        private UserBlockedAndRolesValidator validator;

        [TestInitialize]
        public void Setup()
        {
            validator = new UserBlockedAndRolesValidator();
        }

    
        [TestMethod]
        [DataRow(null)]
        [DataRow("z")] 
        public void Should_Have_Error_ForRole(string role)
        {
            var model = new BlockOrRoleChangeUser
            {
                UserRole = role
            };
            var result = validator.TestValidate(model).IsValid;
            NUnit.Framework.Assert.IsFalse(result);


        }
        [TestMethod]
        [DataRow(null)]       
        public void Should_Have_Error_ForIsblocked(bool isBlocked)
        {
            var model = new BlockOrRoleChangeUser
            {
                IsBlocked = isBlocked
            };
            var result = validator.TestValidate(model).IsValid;
            NUnit.Framework.Assert.IsFalse(result);


        }
        [TestMethod]
        [DataRow(false,"Admin")]
        public void Roles_Success_Case(bool isBlocked,string UserRole)
        {
            var model = new BlockOrRoleChangeUser
            {
                IsBlocked = isBlocked
                , UserRole = UserRole
            };
            var result = validator.TestValidate(model).IsValid;
            NUnit.Framework.Assert.IsTrue(result);


        }
    }
    
}
