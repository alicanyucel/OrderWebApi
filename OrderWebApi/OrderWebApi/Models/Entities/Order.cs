namespace OrderWebApi.Models.Entities
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerGsm { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
