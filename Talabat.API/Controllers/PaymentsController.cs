using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentsController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
		private readonly ILogger<PaymentsController> _logger;
		private const string _whSecret = "whsec_a78030468582e9f240ee08ed39f79b16a843013b7d53235525ad4bf39ff36a9e";

		public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
		{
			_paymentService = paymentService;
			_logger = logger;
		}
		[ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[Authorize]
		[HttpPost("{basketId}")]  //Post: /api/payment/basketId
		public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (basket == null) return BadRequest(new ApiResponse(400, "There is a propblem in your basket"));
			return Ok(basket);
		}
		[HttpPost("webhook")]
		public async Task<IActionResult> StripeWebhook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			
				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], _whSecret);
				var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
				Order order;
				// Handle the event
				switch (stripeEvent.Type)
				{
					case Events.PaymentIntentSucceeded:
						order = await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, true);
						_logger.LogInformation("Payment is succeeded",paymentIntent.Id);
						break;
					case Events.PaymentIntentPaymentFailed:
						order = await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, false);
						_logger.LogInformation("Payment is failed", paymentIntent.Id);


						break;
				}
				return new EmptyResult();
			
			
		
	}
	}
}
