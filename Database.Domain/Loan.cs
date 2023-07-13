using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Database.Domain
{
    public class Loan
    {
        public int Id { get; set; }

        public string LoanIdentityNumber { get; set; } //easy way to get loan withowt using id

        public double Amount { get; set; }

        public string LoanType{ get; set; } 

        public string Currency { get; set; }

        public int LoanPeriod { get; set; }//loan length in months

        public string Status { get; set; } = LoanStatus.PreApplication;
       
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }

    public static class MyLoanType
    {

        public const string FastLoan = "FastLoan";
        public const string MiniLoan = "MiniLoan";
        public const string MicroLoan = "MicroLoan";
        public const string BusinessLoan = "BusinessLoan";
        public const string CreditCard = "CreditCard";

        public static List<string> List()
        {
            return typeof(MyLoanType)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
        }
    }

    public static class LoanStatus
    {

        public const string PreApplication = "PreApplication";
        public const string InReview = "InReview";
        public const string RiskManagement = "RiskManagement";
        public const string Approved = "Approved";
        public const string Declined = "Declined";
        public const string Disburse = "Disburse";
        public const string Application = "Application";
        public const string Close = "Close";

        public static List<string> List()
        {
            return typeof(LoanStatus)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
        }
    }
}
