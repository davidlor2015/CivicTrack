# CivicTrack Learning Roadmap

CivicTrack is an internal case-management system for public agencies. The project is also a learning path for C#, ASP.NET Core MVC, Entity Framework Core, SQL Server, Git, security, accessibility, testing, and maintainable software design.

The guiding mental model for this project is:

- Correct
- Secure
- Maintainable
- Testable
- Accessible
- Documented

We will build the application in small milestones. Each milestone should end with working code, verification steps, and a Git commit.

## Milestone 0: Project Hygiene

Goal: make the project safe to change.

Tasks:

- Initialize a Git repository.
- Add a `.gitignore`.
- Exclude generated files such as `bin/`, `obj/`, `.vs/`, and user-specific files.
- Commit the clean starter app.

Commands:

```powershell
git init
dotnet new gitignore
git status
```

Learning topics:

- What Git tracks.
- Why generated files should not be committed.
- How commits create safe restore points.
- Basic Git workflow: status, add, commit.

Quality principles:

- Controlled SDLC starts with source control.
- Small commits make changes easier to review and debug.
- Ignoring local/generated files keeps the repository maintainable.

## Milestone 1: Understand the Starter App

Goal: understand the ASP.NET Core MVC + Identity app before adding features.

Key files:

- `Program.cs`
- `CivicTrack.Web.csproj`
- `Data/ApplicationDbContext.cs`
- `Controllers/HomeController.cs`
- `Views/`
- `Areas/Identity/`

Learning topics:

- C# namespaces.
- Classes.
- Constructors.
- Dependency injection.
- Nullable reference types.
- `var`.
- Lambdas such as `options => ...`.
- MVC routing.
- Razor views.

Quality principles:

- Read the existing system before changing it.
- Understand framework defaults, especially security defaults.
- Keep startup/configuration code understandable.

## Milestone 2: Domain Model

Goal: define the core business concepts.

Initial domain concepts:

- Service request.
- Service category.
- Request status.
- Priority.
- Employee assignment.
- Comments.
- Attachments.
- Audit history.

Possible entities:

- `ServiceRequest`
- `ServiceCategory`
- `RequestComment`
- `RequestAttachment`
- `RequestStatusHistory`
- `AuditEntry`
- `ApplicationUser`

Learning topics:

- C# classes as domain models.
- Properties.
- Enums.
- Required vs optional data.
- Encapsulation.
- Separating domain concepts from UI concerns.

Quality principles:

- Business rules should be centralized.
- Controllers should coordinate, not contain all business logic.
- Domain names should match real public-agency workflow language.

## Milestone 3: Database Design and EF Core Migrations

Goal: create database tables with integrity.

Tasks:

- Add entity classes.
- Add `DbSet<T>` properties to `ApplicationDbContext`.
- Configure relationships and constraints.
- Create EF Core migrations.
- Apply migrations to SQL Server LocalDB.

Example commands:

```powershell
dotnet ef migrations add AddServiceRequests
dotnet ef database update
```

Learning topics:

- Entity Framework Core.
- `DbContext`.
- `DbSet<T>`.
- Migrations.
- Primary keys.
- Foreign keys.
- Indexes.
- Database constraints.

Quality principles:

- Validate at system boundaries.
- Protect database integrity.
- Do not rely only on UI validation.
- Important rules should be enforced as close to the data as practical.

## Milestone 4: Create and View Service Requests

Goal: build the first real user-facing feature.

Features:

- Create a service request.
- Select a category.
- View a list of requests.
- View request details.
- Show validation errors.

Learning topics:

- MVC controllers.
- Razor views.
- ViewModels.
- Model binding.
- Validation attributes.
- Server-side validation.
- Anti-forgery tokens.
- `async` and `await`.

Quality principles:

- Use ViewModels instead of exposing database entities directly to forms.
- Validate all user input on the server.
- Keep pages accessible with labels, validation summaries, and semantic HTML.

## Milestone 5: Authentication and Role-Based Authorization

Goal: require sign-in and enforce permissions.

Possible roles:

- Employee
- Supervisor
- Administrator

Features:

- Require authenticated users.
- Seed initial roles.
- Create or seed an initial administrator account.
- Restrict actions by role.
- Add authorization policies where useful.

Learning topics:

- ASP.NET Core Identity.
- Authentication vs authorization.
- Roles.
- Policies.
- `[Authorize]`.
- Claims basics.

Quality principles:

- Never rely only on hiding buttons in the UI.
- Server-side authorization must protect every privileged action.
- Use least privilege.

## Milestone 6: Assignment, Priority, and Due Dates

Goal: support operational case-management workflow.

Features:

- Assign requests to employees.
- Set priority.
- Set due dates.
- Display overdue requests.
- Filter by assignee, priority, and due date.

Learning topics:

- Service classes.
- Dependency injection for application services.
- Date/time handling.
- LINQ queries.
- Authorization checks in workflows.

Quality principles:

- Keep business workflows out of Razor views.
- Avoid duplicating assignment rules across controllers.
- Use clear names that reflect agency operations.

## Milestone 7: Controlled Status Transitions

Goal: enforce a valid request lifecycle.

Example statuses:

- New
- Open
- InProgress
- WaitingOnCustomer
- Resolved
- Closed
- Reopened
- Cancelled

Tasks:

- Define allowed status transitions.
- Prevent invalid transitions.
- Record transition history.
- Show current status clearly.

Learning topics:

- Enums.
- Switch expressions.
- Guard clauses.
- Centralized business rules.
- Application service methods.

Quality principles:

- Enforce business rules centrally.
- Make invalid states difficult or impossible.
- Keep workflow logic testable.

## Milestone 8: Comments, Attachments, and Audit History

Goal: add accountability and supporting documentation to each request.

Features:

- Add comments.
- Support internal staff comments.
- Add attachment metadata.
- Record audit entries for important changes.
- Display chronological history.

Learning topics:

- One-to-many relationships.
- File upload basics.
- Metadata storage.
- Audit logging.
- Transactions.

Quality principles:

- Validate file size and file type.
- Do not trust uploaded file names.
- Protect attachments with authorization.
- Audit who changed what and when.

## Milestone 9: Search, Filtering, Sorting, and Pagination

Goal: make request lists usable at realistic scale.

Features:

- Search by keyword.
- Filter by status, category, assignee, and priority.
- Sort by due date, status, priority, and creation date.
- Paginate results.

Learning topics:

- LINQ.
- `IQueryable<T>`.
- Query composition.
- Paging models.
- GET forms for filters.

Quality principles:

- Avoid loading unnecessary data.
- Keep query code readable.
- Make list pages accessible and keyboard-friendly.

## Milestone 10: Dashboard and Reporting

Goal: provide management visibility.

Possible metrics:

- Open requests.
- Overdue requests.
- Requests by status.
- Requests by category.
- Average resolution time.
- Recently updated requests.

Learning topics:

- Aggregation queries.
- Dashboard ViewModels.
- Date calculations.
- Separating read/report queries from write workflows.

Quality principles:

- Reports should explain the operational state clearly.
- Dashboard queries should be efficient.
- Numbers should have clear definitions.

## Milestone 11: Optimistic Concurrency and Transactions

Goal: prevent staff from accidentally overwriting one another's changes.

Features:

- Add a row version/concurrency token.
- Detect conflicting edits.
- Show a friendly conflict message.
- Use database transactions for multi-record updates.

Example multi-record update:

- Update the service request.
- Add a status history entry.
- Add an audit entry.

Learning topics:

- Optimistic concurrency.
- EF Core concurrency tokens.
- `DbUpdateConcurrencyException`.
- Database transactions.
- Atomic updates.

Quality principles:

- Protect data integrity during concurrent work.
- Multi-record business operations should succeed or fail together.
- Error messages should help users recover.

## Milestone 12: Testing

Goal: test behavior at multiple levels.

Test types:

- Unit tests for business rules.
- Service tests for workflows.
- Integration tests for database behavior.
- MVC/controller tests where useful.
- Manual accessibility checks.

Learning topics:

- Test projects.
- xUnit or similar test framework.
- Arrange, Act, Assert.
- Test data builders.
- In-memory vs real database testing tradeoffs.

Quality principles:

- Test behavior, not implementation details.
- Put the most important rules under automated tests.
- Use tests as documentation for future developers.

## Milestone 13: Documentation and Controlled SDLC

Goal: make the project understandable to the next developer.

Documentation artifacts:

- `README.md`
- Roadmap.
- Architecture notes.
- Database setup notes.
- Role and permission matrix.
- Status transition table.
- Manual test checklist.
- Change log or release notes.

Learning topics:

- Writing useful developer documentation.
- Explaining setup commands.
- Documenting decisions.
- Pull request style summaries.
- Commit discipline.

Quality principles:

- Document why, not just what.
- Keep documentation close to the code.
- Every milestone should leave the project easier to understand.

## Working Style

For each milestone, we will answer these questions:

- What are we building?
- Why does it belong in this part of the system?
- What C# syntax or .NET concept are we learning?
- What security, correctness, or maintainability principle applies?
- What command do we run?
- How do we verify it worked?
- What should we commit?

