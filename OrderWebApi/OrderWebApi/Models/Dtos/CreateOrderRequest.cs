namespace OrderWebApi.Models.Dtos
{
    public class CreateOrderRequest
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerGsm { get; set; }
       public List<ProductDetailDto> ProductDetails { get; set; }

    }
}
