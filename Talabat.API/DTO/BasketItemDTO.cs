using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTO
{
	public class BasketItemDTO
	{
		[Required]
		public int Id { get; set; }
		public string ProductName { get; set; }
		//[Required]
		public string? Description { get; set; }
		[Required]
		[Range(1,int.MaxValue,ErrorMessage ="Please select at least one item")]
		public int Quantity { get; set; }
		[Required]
		[Range(0.1,double.MaxValue, ErrorMessage = "Price must be greater than 0.1")]
		public decimal Price { get; set; }
		[Required]
		public string PictureUrl { get; set; }
		[Required]
		public string Brand { get; set; }
		[Required]
		public string Type { get; set; }
	}
}