using CivicTrack.Web.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CivicTrack.Web.Domain.Entities
{
    public class ServiceRequest
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int ServiceCategoryId { get; private set; }
        public ServiceCategory ServiceCategory { get; private set; } = null!;
        public RequestStatus Status { get; private set; } = RequestStatus.New;
        public RequestPriority Priority { get; private set; } = RequestPriority.Medium;
        public string? AssignedEmployeeId { get; private set; }
        public DateTimeOffset CreatedAtUtc { get; private set; }

        public ServiceRequest(string title, string description, int serviceCategoryId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("title is required.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("description is required.", nameof(description));
            }

            if (serviceCategoryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(serviceCategoryId), "Service category ID must be greater than zero.");
            }

            var trimmedTitle = title.Trim();
            var trimmedDescription = description.Trim();

            if (trimmedTitle.Length > 200)
            {
                throw new ArgumentException("Title cannot exceed 200 characters.", nameof(title));
            }

            if (trimmedDescription.Length > 2000)
            {
                throw new ArgumentException("Description cannot exceed 2000 characters", nameof(description));
            }

            Title = trimmedTitle;
            Description = trimmedDescription;
            ServiceCategoryId = serviceCategoryId;
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

        public void WaitOnCustomer()
        {
            if (Status != RequestStatus.InProgress)
            {
                throw new InvalidOperationException("Request must be in progress before waiting on the customer.");
            }

            Status = RequestStatus.WaitingOnCustomer;
        }

        public void ResumeProgress()
        {
            if (Status != RequestStatus.WaitingOnCustomer)
            {
                throw new InvalidOperationException("Request must be waiting on customer to resume progress.");
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

