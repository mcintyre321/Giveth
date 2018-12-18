using System;
using System.IO;
using System.Linq;
using Giveth;
using Giveth.NUnit;
using NUnit.Framework;

[NUnit.Framework.SetUpFixture]
// ReSharper disable once CheckNamespace
public class WriteFeatureFilesAfterTests
{
	[NUnit.Framework.OneTimeSetUp]
	public void SetupGiveth()
	{
		TestInfoContext.GetTestInfo = () => TestInfoBuilder.From(TestContext.CurrentContext);
		Directory.GetFiles(".", "*.feature").ToList().ForEach(File.Delete);
	}

	[NUnit.Framework.OneTimeTearDown]
	public void LogGwt()
	{
		var outputDir = Directory.CreateDirectory(this.GetType().Assembly.GetName().Name).FullName;
		Steps.WriteFeatureFiles(outputDir);
		{
			var assembly = this.GetType().Assembly;
			var resources = assembly.GetManifestResourceNames().Where(ex => ex.EndsWith("expected.feature"));
			foreach (var resource in resources)
			{
				using (Stream stream = assembly.GetManifestResourceStream(resource))
				using (StreamReader reader = new StreamReader(stream))
				{
					var expected = reader.ReadToEnd();
					var outputFileName = resource.Split(new [] { assembly.GetName().Name }, StringSplitOptions.None).Last().TrimStart('.');
					var actualPath = Path.Combine(outputDir, outputFileName).Replace("expected.feature", "feature");
					Assert.AreEqual(expected, File.ReadAllText(actualPath));
				}
			}
		}
	}
}