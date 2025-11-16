namespace LibraryApi.Models
{
    public class Exhibitions
    {
        public int ExhibitionId { get; set; }
        public string Name { get; set; } = null!;
        private DateTime _createdAt = DateTime.UtcNow;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        public string? CoverBookKey { get; set; }

        // Navigation
        public Books? CoverBook { get; set; }
        public ICollection<ExhibitionBooks> ExhibitionBooks { get; set; } = new List<ExhibitionBooks>();
    }

}
