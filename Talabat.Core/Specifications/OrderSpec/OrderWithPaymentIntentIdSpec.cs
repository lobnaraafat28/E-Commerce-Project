using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Core.Specifications.OrderSpec
{
	public class OrderWithPaymentIntentIdSpec:BaseSpecification<Order>
	{
		public OrderWithPaymentIntentIdSpec(string paymentIntentId):base(O => O.PaymentIntentId == paymentIntentId)
		{

		}
	}
}
