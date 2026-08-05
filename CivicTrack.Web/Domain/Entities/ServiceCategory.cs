namespace CivicTrack.Web.Domain.Entities
{
    public class ServiceCategory
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public ICollection<ServiceRequest> ServiceRequests { get; private set; } = new List<ServiceRequest>();

        public ServiceCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("service name is required", nameof(name));
            }

            var trimmedName = name.Trim();

            if (trimmedName.Length > 100)
            {
                throw new ArgumentException("Category name cannot exceed 100 characters", nameof(name));
            }

            Name = trimmedName;
            IsActive = true;

        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
