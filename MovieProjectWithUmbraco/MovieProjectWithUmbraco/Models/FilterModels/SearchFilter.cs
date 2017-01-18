namespace MovieProjectWithUmbraco.Models
{
    public class SearchFilter
    {
        public Type[] Types { get; set; }
        public Type[] OrderBy { get; set; }
        public string Query { get; set; }
    }

    public class Type
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }
}