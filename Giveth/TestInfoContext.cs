using System;
using System.Linq;

namespace Giveth
{
	public class TestInfoContext
	{
		public static Func<TestInfo> GetTestInfo { get; set; } = () => new TestInfo() {TestClassName = "TestClassName", TestMethodName = "TestMethodName", TestName = "TestName" }; 
	}
}