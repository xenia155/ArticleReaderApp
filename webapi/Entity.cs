namespace webapi
{
    public class Entity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
