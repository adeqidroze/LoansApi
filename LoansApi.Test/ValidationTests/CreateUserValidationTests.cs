using FluentValidation.TestHelper;
using LoansApi.DTOs;
using LoansApi.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;


namespace LoansApi.Test.ValidationTests
{
    [TestClass]
    public class CreateUserValidationTests
    {
        private UserValidator validator;

        [TestInitialize]
        public void Setup()
        {
            validator = new UserValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_Null()
        {
            var model = new CreateUser { };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(user => user.FirstName).WithErrorMessage("FirstName can't be null.");

            result.ShouldHaveValidationErrorFor(user => user.LastName).WithErrorMessage("LastName can't be null.");

            result.ShouldHaveValidationErrorFor(user => user.UserName).WithErrorMessage("UserName can't be null.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Password can't be null.");

        }

        [TestMethod]
        public void Should_Have_Error_When_Length_Less_Than_Required()
        {
            var model = new CreateUser { FirstName = "", LastName = "", UserName = "", Password = "Zpp1#" };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(user => user.FirstName).WithErrorMessage("Firstname should be at least 2 characters.");

            result.ShouldHaveValidationErrorFor(user => user.LastName).WithErrorMessage("Lastname should be at least 2 characters.");

            result.ShouldHaveValidationErrorFor(user => user.UserName).WithErrorMessage("UserName should be at least 2 characters.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Password should be at least 8 characters.");
        }

        [TestMethod]
        public void Should_Have_Error_When_Length_More_Than_Allowed()
        {
            var model = new CreateUser { 
                FirstName = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
                LastName = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
                UserName = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
                Password = "Zpp111111111111111#" 
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(user => user.FirstName).WithErrorMessage("Firstname should not be more than 51 characters.");

            result.ShouldHaveValidationErrorFor(user => user.LastName).WithErrorMessage("Lastname should not be more than 51 characters.");

            result.ShouldHaveValidationErrorFor(user => user.UserName).WithErrorMessage("UserName should not be more than 51 characters.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Password should not be more than 18 characters.");

        }

        [TestMethod]
        public  void Should_Have_Error_Password_Validations_Wrong_ALL()
        {
            var model = new CreateUser
            {
                Password = null
            };
            var result =   validator.TestValidate(model);

            //result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Password can't be null.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Password should be at least 8 characters.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Password should not be more than 18 characters.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one uppercase letter.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one lowercase letter.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one number.");

            result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one (!?@#$%^ *.).");

        }



        [TestMethod]
        public void Should_Have_Error_When_Age_Or_Sallary_Less_Than_0()
        {
            var model = new CreateUser
            {
               Age = -1,
               Salary = -2,
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(user => user.Age).WithErrorMessage("User can't be younger than 18.");

            result.ShouldHaveValidationErrorFor(user => user.Salary).WithErrorMessage("Salary can't be less than 0.");
           
        }

        [TestMethod]
        public void Should_Have_Error_When_Age_Or_Sallary_More_Than_Max()
        {
            var model = new CreateUser
            {
                Age = 76,
                Salary = 1000001,
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(user => user.Age).WithErrorMessage("Users older than 75 can't get a loan.");

            result.ShouldHaveValidationErrorFor(user => user.Salary).WithErrorMessage("Salary can't be more than 1 000 000.");

        }

        /* [TestMethod]
         public void Should_Have_Error_When_Length_Less_Than_8()
         {
                    var model = new CreateUser { FirstName = null,LastName = "zick", UserName = "alfonzo",Age =19,Salary=100,Password = null};

             var model = new CreateUser { Password = "abgd" };
         }*/
    }
}
