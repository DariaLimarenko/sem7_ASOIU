using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class FamilyFineRule
    {
        private const int GraceDays = 5;
        private const decimal RatePerDay = 2m;
        private const decimal MaxFine = 100m;

        public decimal Calculate(int daysOverdue)
        {
            if (daysOverdue < 0)
                throw new ArgumentException(
                    "Количество дней просрочки не может быть отрицательным",
                    nameof(daysOverdue));

            if (daysOverdue <= GraceDays) return 0m;

            decimal raw = (daysOverdue - GraceDays) * RatePerDay;
            return Math.Min(raw, MaxFine);
        }
    }



}
