namespace LibraryApi.Models
{
    public class ExhibitionBooks
    {
        public int Id { get; set; }
        public int ExhibitionId { get; set; }
        public string BookKey { get; set; } = null!;
        public int OrderNumber { get; set; }


        public Exhibitions Exhibition { get; set; } = null!;
        public Books Book { get; set; } = null!;
    }
}
