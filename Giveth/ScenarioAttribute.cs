using System;

namespace Giveth
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ScenarioAttribute : Attribute
	{
		public string Text { get; }

		public ScenarioAttribute(string text)
		{
			Text = text;
		}
	}
}