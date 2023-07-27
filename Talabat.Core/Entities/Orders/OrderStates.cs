using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Orders
{
	public enum OrderStates
	{
		[EnumMember(Value ="Pending")]
		Pending,
		[EnumMember(Value = "Payment Received")]

		PaymentReceived,
		[EnumMember(Value = "Payment Failed")]

		PaymentFailed
	}
}
