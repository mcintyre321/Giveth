using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Giveth
{
	public static class Steps
	{
		private static readonly List<Scenario> GtwLookup = new List<Scenario>();

		public static void Given(string stepText)
		{
			Console.WriteLine("Given " + stepText);
			Scenario.RunInContext(gtw => gtw.Append("Given " + stepText));
		}

		public static void When(string stepText)
		{
			Console.WriteLine("When " + stepText);
			Scenario.RunInContext(gtw => gtw.Append("When " + stepText));
		}

		public static void Then(string stepText)
		{
			Console.WriteLine("Then " + stepText);
			Scenario.RunInContext(gtw => gtw.Append("Then " + stepText));
		}

		public static void And(string stepText)
		{
			Console.WriteLine("And " + stepText);
			Scenario.RunInContext(gtw => gtw.Append("And " + stepText));
		}

		public static string CurrentStepText
		{
			get
			{
				var message = null as string;
				Scenario.RunInContext(gtw => message = gtw.Statements.Last());
				return message;
			}
		}

		public static void But(string stepText)
		{
			Console.WriteLine("But " + stepText);
			Scenario.RunInContext(gtw => gtw.Append("But " + stepText));			
		}

		internal class Scenario
		{
			public TestInfo TestInfo { get; }
			public List<string> Statements = new List<string>();

			private Scenario(TestInfo testInfo)
			{
				TestInfo = testInfo;
			}

			public static void RunInContext(Action<Scenario> action)
			{
				var testInfo = TestInfoContext.GetTestInfo();
				
				lock (GtwLookup)
				{
					var gtw = GtwLookup.SingleOrDefault(l => Equals(l.TestInfo, testInfo));
					if (gtw == null)
					{
						gtw = new Scenario(testInfo);
						GtwLookup.Add(gtw);
					}
					action(gtw);
				}
			}

			public void Append(string desc)
			{
				Statements.Add(desc);
			}

		}

		private static readonly Lazy<Func<string, Type>> TypesLookup = new Lazy<Func<string,Type>>(() =>
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray();
			return s => assemblies.Select(t => t.GetType(s)).FirstOrDefault(x => x != null);
		});

		public static void WriteFeatureFiles(string directory = ".")
		{
			lock (GtwLookup)
			{
				GtwLookup.GroupBy(v => v.TestInfo.TestClassName, v => (Feature: v.TestInfo, Scenario: v)).ToList().ForEach(feature =>
				{
					var testClass = TypesLookup.Value(feature.First().Feature.TestClassName);
					var methodInfos = testClass.GetMethods().Reverse().ToList();

					var scenarios = feature
						.OrderByDescending(f => methodInfos.FindIndex(mi => mi.Name == f.Feature.TestMethodName))
						.ThenBy(f => f.Feature.TestName);

					var sb = new StringBuilder();
					var testInfo = feature.First().Scenario.TestInfo;
					var featureName = testClass?.GetCustomAttribute<FeatureAttribute>()?.Text
					                   ?? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(SentenceCase(testClass.Name)); //no title casing here as it messes with the parameters!
					sb.AppendLine("Feature: " + featureName);
					foreach (var scenario in scenarios)
					{
						sb.AppendLine("");
						var scenarioName = testClass.GetMethod(scenario.Feature.TestMethodName)?.GetCustomAttribute<ScenarioAttribute>()?.Text
							?? (SentenceCase(scenario.Scenario.TestInfo.TestName)); //no title casing here as it messes with the parameters!
						
						sb.AppendLine("  Scenario: " + scenarioName);
						foreach (var statement in scenario.Scenario.Statements)
						{
							sb.AppendLine("    " + statement);
						}
					}

					var path = Path.Combine(directory, testInfo.TestClassName.Substring(testClass.Namespace.Length + 1)) + ".feature";
					File.WriteAllText(path, sb.ToString());
					Console.WriteLine("Wrote " + Path.GetFullPath(path));
				});

			}
			string SentenceCase(string str)
			{
				return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
			}
		}
	}
}