using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Repository.Data.Configurations
{
	public class OdrerConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(O => O.ShipToAddress, ShipToAddress => ShipToAddress.WithOwner());
			builder.Property(O => O.Status).HasConversion(s => s.ToString(), s => (OrderStates)Enum.Parse(typeof(OrderStates), s));
			builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");
			
			
		}
	}
}
