using NUnit.Framework;

namespace Giveth.NUnit
{
	public class TestInfoBuilder
	{
		 
		public static TestInfo From(TestContext testContext)
		{
			return new TestInfo()
			{
				TestName = testContext.Test.Name,
				TestClassName = testContext.Test.ClassName,
				TestMethodName = testContext.Test.MethodName
			};
		}
	}
}