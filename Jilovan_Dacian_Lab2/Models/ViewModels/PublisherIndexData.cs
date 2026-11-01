namespace Jilovan_Dacian_Lab2.Models.ViewModels
{
    public class PublisherIndexData
    {
        public IEnumerable<Publisher> Publishers { get; set; } = Array.Empty<Publisher>();
        public IEnumerable<Book>? Books { get; set; }
    }
}
