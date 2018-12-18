using System.Collections.Generic;
using NUnit.Framework;
using static Giveth.Steps;
namespace Giveth.NetFx.Tests
{
	[Feature("Feature attribute can override the class name")]
	public class SomeOtherTestClass
	{
		[Test]
		[Scenario("Scenario attribute can override the method name")]
		public void MethodNameThatShouldBeOverridden()
		{
			Given("A precondition");
			NewMethod();
			Then("Something should happen");
		}

		[Test]
		public void Foo2()
		{
			Given("A precondition2");
			NewMethod();
			Then("Something should happen");
		}

		[Test]
		public void Foo4()
		{
			Given("A precondition");
			NewMethod();
			Then("Something should happen");
		}

		[Test]
		[TestCaseSource(nameof(Foo5TestCases))]
		public void Foo5(string blah)
		{
			Given("A precondition");
			NewMethod();
			Then("Something should happen");
		}

		public static IEnumerable<string> Foo5TestCases()
		{
			yield return "a";
			yield return "b";
		}


		[Test]
		[TestCase("1")]
		[TestCase("2")]
		public void Foo6(string blah)
		{
			Given("A precondition");
			NewMethod();
			Then("Something should happen");
		}

		[Test]
		public void MostRecentStepTextIsAccessible()
		{

			When("the most recent step text is accessed");
			var currentStepText = CurrentStepText;
			Then("It should be correct");

			Assert.AreEqual("When the most recent step text is accessed", currentStepText, CurrentStepText);
		}

		private static void NewMethod()
		{
			When("An event takes place");
		}
	}
}
