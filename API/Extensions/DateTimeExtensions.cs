using System.Globalization;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly birthdate)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int age = today.Year - birthdate.Year;
            if(birthdate > today.AddYears(-age)) age--;

            return age;
        }
    }
}