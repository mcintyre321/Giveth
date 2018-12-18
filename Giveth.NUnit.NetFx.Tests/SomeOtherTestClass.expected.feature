Feature: Feature attribute can override the class name

  Scenario: Scenario attribute can override the method name
    Given A precondition
    When An event takes place
    Then Something should happen

  Scenario: Foo2
    Given A precondition2
    When An event takes place
    Then Something should happen

  Scenario: Foo4
    Given A precondition
    When An event takes place
    Then Something should happen

  Scenario: Foo5("a")
    Given A precondition
    When An event takes place
    Then Something should happen

  Scenario: Foo5("b")
    Given A precondition
    When An event takes place
    Then Something should happen

  Scenario: Foo6("1")
    Given A precondition
    When An event takes place
    Then Something should happen

  Scenario: Foo6("2")
    Given A precondition
    When An event takes place
    Then Something should happen

  Scenario: Most recent step text is accessible
    When the most recent step text is accessed
    Then It should be correct
