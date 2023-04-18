namespace webapi
{
    public class Article
    { 
        public int ArticleId { get; set; }

        public DateTime? ArticleDate { get; set; }

        public string? ArticleTitle { get; set; }

        public string? Text { get; set; }

        public Uri ArticleUrl { get; set; }

        public string ArticleHTML { get; set; }

    }
}
