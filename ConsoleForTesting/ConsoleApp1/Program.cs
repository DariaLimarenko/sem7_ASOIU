using System.Timers;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{
    public class RegularFineRule
    {
        public decimal Calculate(int daysOverdue) => daysOverdue * 5m;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Семинар 7. Основы тестирования в .NET");
            Console.WriteLine("Основная логика проекта проверяется автоматизированными тестами:");
            Console.WriteLine("- MSTest");
            Console.WriteLine("- NUnit");
            Console.WriteLine("- xUnit");
            Console.WriteLine("- BDD / Reqnroll");
        }
    }
}
