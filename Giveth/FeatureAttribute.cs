using System;

namespace Giveth
{
	[AttributeUsage(AttributeTargets.Class)]
	public class FeatureAttribute : Attribute
	{
		public string Text { get; }

		public FeatureAttribute(string text)
		{
			Text = text;
		}
	}
}