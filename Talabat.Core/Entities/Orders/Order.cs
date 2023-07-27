using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Orders
{
	public class Order:BaseEntity
	{
		public Order()
		{

		}
		public Order(string email, Address shipToAddress, DeliveryMethod deliveryMethod, decimal subTotal, ICollection<OrderItem> items)
		{
			Email = email;
			ShipToAddress = shipToAddress;
			DeliveryMethod = deliveryMethod;
			SubTotal = subTotal;
			Items = items;
		}

		public string Email { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

		public OrderStates Status { get; set; } = OrderStates.Pending;

		public Address ShipToAddress { get; set; }

		public DeliveryMethod DeliveryMethod { get; set; }
		public decimal SubTotal { get; set; }
		//public decimal Total { set=> Total = SubTotal + DeliveryMethod.Cost; }
		public ICollection<OrderItem> Items { get; set; }= new HashSet<OrderItem>();
		public string PaymentIntentId { get; set; } = string.Empty;
		public decimal GetTotal() 
			=> SubTotal + DeliveryMethod.Cost;
	}
}
