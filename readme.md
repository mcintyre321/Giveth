# Giveth

> Gherkin BDD for C# with the hassle takeneth away

## What is this library

Giveth is a libary for doing BDD in C#. Although there are already many excellent libraries 
for doing this, they generally require you to slightly mangle your test code, by wrapping 
steps in methods and/or lambdas, making it harder to pass variables around. Some of these libraries 
encourage you to use method names to represent your Gherkin, which can lead to the specifications 
accidentally being refactored away.

# Installation

Giveth consists of several no-op static methods for writing your Gherkin specification inline with your code. 

To use them, first:

> Install-Package Giveth

and add a `using static Giveth.Steps;` to the top of your code file.

## An example test

```csharp

using NUnit.Framework;
using static Giveth.Steps; //all the GWT methods are on this Type

public class WorkspaceMembership
{
	[Test]
	public async Task OnlyMembersCanAccessPrivateWorkspaces()
	{
		Given("a workspace");
		var client = new TaskTrackerClient();
		var workspaceUserId = (await client.CreateUser(username: "SomeUser", password: "SomePassword")).Result;
		var workspaceId = (await client.CreateWorkspace(creator: workspaceUser, name: "SomeWorkspace", private: true)).Result;
		
		And("a user who is not a member");
		var otherUser = await client.CreateUser(username: "SomeOtherUser", password: "SomePassword").Result;
		
		When("the non-member tries to access the workspace")
		var accessToken = await client.GetAccessToken(otherUser.Username, "SomePassword");
		var workspaceContentResult = await client.FetchWorkspaceContent(workspace: workspaceId, user: otherUserId)

		Assert.Multiple(() => {
			Then("the access should be denied")
			Assert.AreEqual(false, workspaceContentResult.Success, LastStepMessage); //Use LastStepMessage as the 'message' on Assert calls.

			And("the and the content should not be available");
			Assert.AreEqual(null, workspaceContentResult.Result, LastStepMessage); 
		});
	} 
}

```
 
## Outputting Gherkin .feature files (Requires Giveth.NUnit)

You may want to output Gherkin feature files for each of your test classes, so that you can upload them to a specification browsing tool 
like [Pickles](https://github.com/picklesdoc/pickles), or [Augurk](https://github.com/Augurk/Augurk). First

> Install-Package Giveth.NUnit

and add this class to your test project:

```
[NUnit.Framework.SetUpFixture]
// ReSharper disable once CheckNamespace
public class GivethSetUp
{	
	[NUnit.Framework.OneTimeSetUp]
	public void SetupGiveth() => TestInfoContext.GetTestInfo = () => TestInfoBuilder.From(TestContext.CurrentContext);

	[NUnit.Framework.OneTimeTearDown]
	public void LogGwt() => Giveth.Steps.WriteFeatureFiles(); //Defaults to the test execution working directory, but can be overridden
}
```

Note that this class should have no namespace, so that NUnit runs it before/after ALL tests in the project.

After running the tests produce a file for each of your test classes will be produced. e.g. 

`WorkspaceMembership.feature`

```
Feature: Workspace membership

  Scenario: Only members can access private workspaces
	Given a workspace
    And a user who is not a member
	When the non-member tries to access the workspace
	Then the access should be denied
	And the and the content should not be available

``` 

### Why just NUnit and not XUnit?

In order to work, Giveth hooks into the ambient TestContext, and a global OneTimeTearDown, concepts which don't exist in XUnit. 

NUnit also has an [`Assert.Multiple`](https://github.com/nunit/docs/wiki/Multiple-Asserts) feature which works very nicely with multiple 'Then\And' steps.

An XUnit version can be made if there is sufficient demand

### Custom Feature and Scenario descriptions

There are some custom attributes you can use to override how the feature/specification names are generated in the report. e.g.

```csharp

	[Feature("Some feature name")]
    public class ClassNameThatShouldBeOverridden
    {
		[Test]
		[Scenario("Some scenario name")]
		public void MethodNameThatShouldBeOverridden()
        {
           ...
        }
   }
```

