namespace OrderWebApi.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Unit { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
