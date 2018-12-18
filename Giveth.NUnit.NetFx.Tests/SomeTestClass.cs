using NUnit.Framework;

namespace Giveth.NetFx.Tests
{
	public class SomeTestClass
	{
		[Test]
		public void SomethingShouldHappen()
		{
			Steps.Given("A precondition");

			NewMethod();
			Steps.Then("Something should happen");
		}

		[Test]
		public void Foo2()
		{
			Steps.Given("A precondition2");
			NewMethod();
			Steps.Then("Something should happen");
		}

		[Test]
		public void Foo4()
		{
			Steps.Given("A precondition");
			NewMethod();
			Steps.Then("Something should happen");
		}



		private static void NewMethod()
		{
			Steps.When("An event takes place");
		}
	}
}
