using System;
using TechTalk.SpecFlow;

namespace BrownbagDemo
{
    [Binding]
    public class HelloWorldSteps
    {
        [When(@"I go to home page")]
        public void WhenIGoToHomePage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I will see some text")]
        public void ThenIWillSeeSomeText()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
