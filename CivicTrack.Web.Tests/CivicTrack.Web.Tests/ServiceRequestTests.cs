using CivicTrack.Web.Domain.Entities;
using CivicTrack.Web.Domain.Enums;


namespace CivicTrack.Web.Tests
{
    public class ServiceRequestTests
    {
        [Fact]
        public void Constructor_SetsDefaultValues()
        {
            const string title = "Broken street lamp";
            const string description = "Street lamp not turning on";
            const string category = "Electrical";

            var request = new ServiceRequest(title, description, category);

            Assert.Equal(title, request.Title);
            Assert.Equal(description, request.Description);
            Assert.Equal(category, request.Category);
            Assert.Equal(RequestStatus.New, request.Status);
            Assert.Equal(RequestPriority.Medium, request.Priority);
            Assert.Null(request.AssignedEmployeeId);
        }
        [Fact]
        public void Constructor_WithNullTitle_ThrowsArgumentException()
        {
            const string description = "Street lamp not turning on";
            const string category = "Electrical";

            var exception = Assert.Throws<ArgumentException>(() => new ServiceRequest(null!, description, category));

            Assert.Equal("title", exception.ParamName);
        }
        [Fact]
        public void Constructor_WithWhiteSpaceTitle_ThrowsArgumentException()
        {
            const string description = "Street lamp not turning on";
            const string category = "Electrical";

            var exception = Assert.Throws<ArgumentException>(() => new ServiceRequest(" ", description, category));

            Assert.Equal("title", exception.ParamName);
        }
        [Fact]
        public void Constructor_WithBlankDescription_ThrowsArgumentException()
        {
            const string title = "Broken streetlamp";
            const string category = "Electrical";

            var exception = Assert.Throws<ArgumentException>(() => new ServiceRequest(title, "", category));

            Assert.Equal("description", exception.ParamName);
        }
        [Fact]
        public void Constructor_WithWhiteSpaceDescription_ThrowsArgumentException()
        {
            const string title = "Broken streetlamp";
            const string category = "Electrical";

            var exception = Assert.Throws<ArgumentException>(() => new ServiceRequest(title, " ", category));

            Assert.Equal("description", exception.ParamName);
        }
        [Fact]
        public void Constructor_WithBlankCategory_ThrowsArgumentException()
        {
            const string title = "Broken streetlamp";
            const string description = "Street lamp not turning on";

            var exception = Assert.Throws<ArgumentException>(() => new ServiceRequest(title, description, ""));

            Assert.Equal("category", exception.ParamName);
        }
        [Fact]
        public void Constructor_WithWhiteSpaceCategory_ThrowsArgumentException()
        {
            const string title = "Broken streetlamp";
            const string description = "Street lamp not turning on";

            var exception = Assert.Throws<ArgumentException>(() => new ServiceRequest(title, description, " "));

            Assert.Equal("category", exception.ParamName);
        }
        [Fact]
        public void AssignTo_WithValidEmployeeId_AssignsEmployeeAndOpenRequest()
        {
            const string title = "Broken streetlamp";
            const string description = "Street lamp not turning on";
            const string category = "Electrical";
            const string employee = "employee1";


            var request = new ServiceRequest(title, description, category);
            request.AssignTo(employee);

            Assert.Equal(RequestStatus.Open, request.Status);
            Assert.Equal(employee, request.AssignedEmployeeId);
        }
        [Fact]
        public void AssignTo_WithNullEmployeeId_ThrowsArgumentException()
        {
            const string title = "Broken streetlamp";
            const string description = "Street lamp not turning on";
            const string category = "Electrical";


            var request = new ServiceRequest(title, description, category);
            var exception = Assert.Throws<ArgumentException>(() => request.AssignTo(null!));

            Assert.Null(request.AssignedEmployeeId);
            Assert.Equal(RequestStatus.New, request.Status);
            Assert.Equal("employeeId", exception.ParamName);
        }
        [Fact]
        public void AssignTo_WithWhiteSpaceEmployeeId_ThrowsArgumentException()
        {
            const string title = "Broken streetlamp";
            const string description = "Street lamp not turning on";
            const string category = "Electrical";

            var request = new ServiceRequest(title, description, category);
            var exception = Assert.Throws<ArgumentException>(() => request.AssignTo(" "));

            Assert.Null(request.AssignedEmployeeId);
            Assert.Equal(RequestStatus.New, request.Status);
            Assert.Equal("employeeId", exception.ParamName);
        }

        [Fact]
        public void AssignTo_WithInvalidStatusCancelled_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.Cancel();

            Assert.Throws<InvalidOperationException>(() => request.AssignTo("employee-123"));

            Assert.Equal(RequestStatus.Cancelled, request.Status);
            Assert.Null(request.AssignedEmployeeId);

        }
        [Fact]
        public void StartProgress_WhenStatusIsNew_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");


            Assert.Throws<InvalidOperationException>(() => request.StartProgress());

            Assert.Equal(RequestStatus.New, request.Status);
        }
        [Fact]
        public void StartProgress_WhenStatusIsOpen_ChangesStatusToInProgress()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.AssignTo("employee-123");
            request.StartProgress();

            Assert.Equal(RequestStatus.InProgress, request.Status);

        }
        [Fact]
        public void Resolve_WhenStatusIsNew_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            Assert.Equal(RequestStatus.New, request.Status);
            Assert.Throws<InvalidOperationException>(() => request.Resolve());

        }
        [Fact]
        public void Resolve_WhenStatusIsOpen_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.AssignTo("employee-123");

            Assert.Throws<InvalidOperationException>(() => request.Resolve());

            Assert.Equal(RequestStatus.Open, request.Status);
        }
        [Fact]
        public void Resolve_WhenStatusIsInProgress_ChangesStatusToResolved()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");
            request.AssignTo("employee-123");
            request.StartProgress();
            request.Resolve();

            Assert.Equal(RequestStatus.Resolved, request.Status);
        }
        [Fact]
        public void Close_WhenStatusIsNotResolved_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            Assert.Throws<InvalidOperationException>(() => request.Close());

        }
        [Fact]
        public void Close_WhenStatusIsResolved_ChangesStatusToClosed()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.AssignTo("employee-123");
            request.StartProgress();
            request.Resolve();
            request.Close();

            Assert.Equal(RequestStatus.Closed, request.Status);
        }
        [Fact]
        public void Cancel_WhenStatusIsNotNewOrOpen_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.AssignTo("employee-123");
            request.StartProgress();

            Assert.Throws<InvalidOperationException>(() => request.Cancel());


        }
        [Fact]
        public void Cancel_WhenStatusIsNew_ChangesStatusToCancelled()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.Cancel();

            Assert.Equal(RequestStatus.Cancelled, request.Status);
        }
        [Fact]
        public void Cancel_WhenStatusIsOpen_ChangeStatusToCancelled()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");
            request.AssignTo("employee-123");
            request.Cancel();

            Assert.Equal(RequestStatus.Cancelled, request.Status);
        }
        [Fact]
        public void Reopen_WhenStatusIsNotClosed_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            Assert.Throws<InvalidOperationException>(() => request.Reopen());

        }
        [Fact]
        public void Reopen_WhenStatusIsClosed_ChangesStatusToOpen()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.AssignTo("employee-123");
            request.StartProgress();
            request.Resolve();
            request.Close();
            request.Reopen();

            Assert.Equal(RequestStatus.Open, request.Status);
        }
        [Fact]
        public void WaitOnCustomer_WhenStatusIsNew_ThrowsInvalidOperationException()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            Assert.Throws<InvalidOperationException>(() => request.WaitOnCustomer());
        }
        [Fact]
        public void WaitingOnCustomer_WhenStatusIsInProgress_ChangeStatusToWaitingOnCustomer()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.AssignTo("employee-123");
            request.StartProgress();
            request.WaitOnCustomer();

            Assert.Equal(RequestStatus.WaitingOnCustomer, request.Status);
        }
        [Fact]
        public void InProgress_WhenStatusIsWaitingOnCustomer_ChangeStatusToResumeProgress()
        {
            var request = new ServiceRequest("Broken streetlamp", "Streetlamp not turning on", "Electrical");

            request.AssignTo("employee-123");
            request.StartProgress();
            request.WaitOnCustomer();
            request.ResumeProgress();

            Assert.Equal(RequestStatus.InProgress, request.Status);
        }
    }
}
