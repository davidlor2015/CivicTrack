using CivicTrack.Web.Domain.Enums;

namespace CivicTrack.Web.Domain.Entities
{
    public class ServiceRequest
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public RequestStatus Status { get; private set; } = RequestStatus.New;
        public RequestPriority Priority { get; private set; } = RequestPriority.Medium;
        public string? AssignedEmployeeId { get; private set; }
        public DateTimeOffset CreatedAtUtc { get; private set; }


        public ServiceRequest(string title, string description, string category)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("title is required.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("description is required.", nameof(description));
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("category is required.", nameof(category));
            }

            Title = title;
            Description = description;
            Category = category;
            CreatedAtUtc = DateTimeOffset.UtcNow;
        }

        public void AssignTo(string employeeId)
        {
            if (Status == RequestStatus.Closed || Status == RequestStatus.Cancelled)
            {
                throw new InvalidOperationException($"A request with status '{Status}' cannot be assigned.");
            }

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                throw new ArgumentException("Employee ID required", nameof(employeeId));
            }

            AssignedEmployeeId = employeeId;
            Status = RequestStatus.Open;
        }

        public void StartProgress()
        {
            if (Status != RequestStatus.Open)
            {
                throw new InvalidOperationException("Request must be open to start");
            }

            Status = RequestStatus.InProgress;
        }

        public void Resolve()
        {
            if (Status != RequestStatus.InProgress)
            {
                throw new InvalidOperationException("Request must be in progress to resolve");
            }

            Status = RequestStatus.Resolved;
        }


        public void Close()
        {
            if (Status != RequestStatus.Resolved)
            {
                throw new InvalidOperationException("Request must be resolved to close");
            }

            Status = RequestStatus.Closed;
        }

        public void Cancel()
        {
            if (Status != RequestStatus.New && Status != RequestStatus.Open)
            {
                throw new InvalidOperationException("Request must be new or open");
            }

            Status = RequestStatus.Cancelled;
        }

        public void Reopen()
        {
            if (Status != RequestStatus.Closed)
            {
                throw new InvalidOperationException("Request must be closed to reopen");
            }

            Status = RequestStatus.Open;
        }
    }
}

