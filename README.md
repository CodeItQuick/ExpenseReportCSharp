# Questions
1. When is it better to return the domain object rather than a string? I would think when you don't own that code?
2. I do map multiple aggregate Id's to a list, so that I can retrieve by aggregate, this seems "fine"?
3. The entire adapter could be in an external codebase. For a Mobile App, I would have the Mobile API in this codebase, 
but then the swift/kotlin app would be in a different codebase?
4. The adapter could also be used to produce multiple Database-specific tasks, think creating a CSV Report of Expense Reports
5. We could also import in from a 3rd/2nd party application ExpenseReports, to be shown to the user here. How does our code change in these scenarios?
6. "DomainServices" are actually just interfaces/ports. Should the Adapter ExpenseService go in that layer, or remain in the adapter layer and get "plugged into" out layers


# CodeItQuick Contribution to this Kata

I redid this kata mostly to explore how hexagonal architecture _should_ look. For the most part the solution
is much too verbose for what the code justifies, but it did help solidify where each part of the project
belongs in a hex-arch codebase. I ported the project from Java to C#.

# Expense Report Kata

This is the Java-only version of the kata from https://github.com/christianhujer/expensereport.
The relevant parts of the README are below.

## Changes I Made

I've updated it to the latest JUnit, added the AssertJ assertion library, and bumped the Java version to 17.
I also split the classes/enums into separate files and added a starter test method.

----

# ExpenseReport

The ExpenseReport legacy code refactoring kata in various languages.

This is an example of a piece of legacy code with lots of code smells.
The goal is to support the following new feature as best as you can:
* Add Lunch with an expense limit of 2000.

## Process
1. 📚 Read the code to understand what it does and how it works.
2. 🦨 Read the code and check for design and code smells. Make a list of all code and design smells that you find.
3. 🧑‍🔬 Analyze what you would have to change to implement the new requirement without refactoring the code.
4. 🧪 Write a characterization test. Expand your list of code and design smells. Add those smells that you missed earlier and discovered now because they made your life writing a test miserable.
5. 🔧 Refactor the code.
6. 🔧 Refactor the test.
7. 👼 Test-drive the new feature.

## Other plans
- Make sure that all languages are providing the identical challenge.
  To be practical, this will require the removal of the timestamp side-effect.
- Provide the time-stamp side-effect on a separate branch.
- Provide a test setup (without test) on a separate branch so that folks can choose whether they want to include the setup work in the kata or not.
- Provide a level 2 challenge for creating an HTML report besides the Plain Text report.

## Credits
I first encountered the ExpenseReport example during a bootcamp at Equal Experts.
I also have seen the ExpenseReport example being used by Robert "Uncle Bob" C. Martin.
However, he seems to not be the original author (https://twitter.com/unclebobmartin/status/1537063143326855176?s=20&t=lh_vVb9jUQmY6PYG50974w)
I have tried to research its origins but so far I have failed.
If you know who has first come up with this example, please get in touch with me.