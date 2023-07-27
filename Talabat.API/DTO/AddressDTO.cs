﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTO
{
	public class AddressDTO
	{
		[Required]
		public string FirstName { get; set; }
		[Required]

		public string LastName { get; set; }
		[Required]

		public string Country { get; set; }
		[Required]

		public string City { get; set; }
		[Required]


		public string Street { get; set; }
	}
}
