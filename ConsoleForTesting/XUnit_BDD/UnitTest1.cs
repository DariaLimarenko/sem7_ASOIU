using ConsoleApp1;
using Reqnroll;

namespace XUnit_BDD
{
    [Binding]
    public class FamilyFineRuleSteps
    {
        private FamilyFineRule? _rule;
        private decimal _actualFine;

        [Given("читатель с семейным абонементом")]
        public void GivenFamilyReader()
        {
            _rule = new FamilyFineRule();
        }

        [When(@"книга просрочена на (\d+) дней")]
        [When(@"книга просрочена на (\d+) дня")]
        public void WhenBookOverdueByDays(int days)
        {
            _actualFine = _rule!.Calculate(days);
        }

        [Then(@"штраф равен (\d+) рублей")]
        public void ThenFineEquals(decimal expected)
        {
            Assert.Equal(expected, _actualFine);
        }
    }
}
