namespace Skoleni.Controllers
{
    public class BooksFilter
    {
        public string Keyword { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}