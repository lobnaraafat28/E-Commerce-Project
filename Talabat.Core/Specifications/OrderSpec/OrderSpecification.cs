using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Core.Specifications.OrderSpec
{
	public class OrderSpecification:BaseSpecification<Order>
	{
		public OrderSpecification(string email):base(O => O.Email==email)
		{
			Includes.Add(O => O.DeliveryMethod);
			Includes.Add(O => O.Items);
			AddOrderByDesc(O => O.OrderDate);
		}
		public OrderSpecification(string email, int orderId) : base(O => O.Email == email&& O.Id == orderId)
		{
			Includes.Add(O => O.DeliveryMethod);
			Includes.Add(O => O.Items);
		}
	}
}
