namespace PeopleMgt.Models
{
    /// <summary>
    /// Keeps state of paging request between client and server.
    /// </summary>
    public class PageMetadata
    {
        public int Current { get; set; }
        public bool Previous { get; set; }
        public bool Next { get; set; }
        public int Size { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
    }
}
