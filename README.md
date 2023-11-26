# TODO:
1. Make the console app more of an adapter with better test suite, non-acceptance-test-stuff **differ**
2. Fix adapter conversion code as its wrong in some spots, should be abstracted to view somehow
   1. Fix MVC **done**
   2. Fix Blazor **differ, maybe delete altogether**
   3. Fix API **done**
   4. Fix Console **done**
3. Add authentication for roles use case **Doing this**
   1. Implement authentication in WebApi and WebApplication **done**
   2. Implement Manager Role
   3. Implement Employee Role **done**
4. Add use case Accounting Clerk/Controller/CFO/Manager/Employee Roles
5. Add use case list all reports + view
6. Add use case sort/filter list of reports
7. Add use case for showing manager all over-expensed items

# Questions
1. When is it better to return the domain object rather than a string? I would think when you don't own that code?
2. I do map multiple aggregate Id's to a list, so that I can retrieve by aggregate, this seems "fine"?
3. The entire adapter could be in an external codebase. For a Mobile App, I would have the Mobile API in this codebase, 
but then the swift/kotlin app would be in a different codebase?
4. The adapter could also be used to produce multiple Database-specific tasks, think creating a CSV Report of Expense Reports
5. We could also import in from a 3rd/2nd party application ExpenseReports, to be shown to the user here. How does our code change in these scenarios?
6. "DomainServices" are actually just interfaces/ports. Should the Adapter ExpenseService go in that layer, or remain in the adapter layer and get "plugged into" out layers
7. Blazor uses sockets, how would I test that through the client?
8. **The out adapter is very interesting as there are Expense and ExpenseReport objects that map to a DB**

# Finished
Implement Employee Role:
1. An employee can submit his expense reports **done**
2. An employee can add expenses to his expense reports **done**
3. An employee can view his expense report **done**
4. An employee has access to only his expense report list **done**
5. An employee's expense report are not approved **done**
