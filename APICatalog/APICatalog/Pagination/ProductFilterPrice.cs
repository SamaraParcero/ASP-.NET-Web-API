namespace APICatalog.Pagination
{
    public class ProductFilterPrice : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string? PriceCriterion { get; set; } //"Maior", "MENOR, "igual"
    }
}
