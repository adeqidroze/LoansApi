using Database.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Data
{
    public class LoanDatabaseContext : DbContext
    {
        public LoanDatabaseContext(DbContextOptions<LoanDatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            User user = new User()
            {
                Id = -1,
                FirstName = "SuperAdmin",
                LastName = "SuperAdmin",
                UserName = "SuperAdmin",
                Age = 22,
                Salary = 10000000,
                UserRole = Role.Admin,
                IsBlocked = false,
                Password_salt = "2+QCyOnC8h06goFFTpuHFkhQB4S3byaITkWqTDSX97A=",
                Password = PasswordHasher("SuperAdmin", "2+QCyOnC8h06goFFTpuHFkhQB4S3byaITkWqTDSX97A="),
            };

            User userAssistant = new User()
            {
                Id = -2,
                FirstName = "Assistant",
                LastName = "Assistant",
                UserName = "Assistant",
                Age = 0,
                Salary = 0,
                UserRole = Role.Assistant,
                IsBlocked = false,
                Password_salt = "9+QCyOnC8h06goFFTpuHFkhQB4S3byaITkWqTDSX97g=",
                Password = PasswordHasher("Assistant123$", "9+QCyOnC8h06goFFTpuHFkhQB4S3byaITkWqTDSX97g="),
            };


            builder.Entity<User>().HasData(user);
            builder.Entity<User>().HasData(userAssistant);

        }

        private string PasswordHasher(string password, string salt)
        {
            // SHA512 is disposable by inheritance.  
            using var sha256 = SHA256.Create();
            // Send a sample text to hash.  
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            // Get the hashed string.  
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }


        public DbSet<Loan> Loans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Logs> NLogs { get; set; }



    }
}
