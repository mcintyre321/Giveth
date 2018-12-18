using System.Collections.Generic;
using System.Threading;
using Xunit;
using static Giveth.Steps;
namespace Giveth.XUnit.Tests
{
	[Feature("Feature attribute can override the class name")]
	public class SomeOtherTestClass
	{
		[Fact]
		[Scenario("Scenario attribute can override the method name")]
		public void MethodNameThatShouldBeOverridden()
		{
			Given("A precondition");
			NewMethod();
			Then("Something should happen");
		}

		[Fact]
		public void Foo2()
		{
			Given("A precondition2");
			NewMethod();
			Then("Something should happen");
		}

		[Fact]
		public void Foo4()
		{
			Given("A precondition");
			NewMethod();
			Then("Something should happen");
		}

		//[Fact]
		//[FactCaseSource(nameof(Foo5TestCases))]
		//public void Foo5(string blah)
		//{
		//	Given("A precondition");
		//	NewMethod();
		//	Then("Something should happen");
		//}

		public static IEnumerable<string> Foo5TestCases()
		{
			yield return "a";
			yield return "b";
		}


		[Theory]
		[InlineData("1")]
		[InlineData("2")]
		public void Foo6(string blah)
		{
			Given("A precondition");
			NewMethod();
			Then("Something should happen");
		}

		[Fact]
		public void MostRecentStepTextIsAccessible()
		{

			When("the most recent step text is accessed");
			var currentStepText = CurrentStepText;
			Then("It should be correct");

			Assert.Equal("When the most recent step text is accessed", currentStepText);
		}

		private static void NewMethod()
		{
			When("An event takes place");
		}
	}
}
