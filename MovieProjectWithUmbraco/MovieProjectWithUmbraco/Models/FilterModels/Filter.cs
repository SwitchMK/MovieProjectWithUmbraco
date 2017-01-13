namespace MovieProjectWithUmbraco.Models
{
    public class SearchFilter
    {
        public DocumentTypes DocumentTypes { get; set; }
        public string Query { get; set; }
    }

    public class DocumentTypes
    {
        public bool Film { get; set; }
        public bool Person { get; set; }
        public bool Distributor { get; set; }
    }
}