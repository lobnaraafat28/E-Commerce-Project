using Talabat.Core.Entities.Orders;

namespace Talabat.API.DTO
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrederDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; } 
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }        
        public string? PaymentIntentId { get; set; }

    }
}
