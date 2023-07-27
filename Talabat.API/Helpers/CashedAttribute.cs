using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.API.Helpers
{
	public class CashedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSeconds;

		public CashedAttribute(int timeToLiveInSeconds)
		{
			_timeToLiveInSeconds = timeToLiveInSeconds;
		}
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			//Ask CLR to create object of class that implement IResponseService explicitly to apply dependency injection
			var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
			var cacheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);
			var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
			
			if(string.IsNullOrEmpty(cachedResponse))
			{
				var contentResult = new ContentResult()
				{
					Content = cachedResponse,
					ContentType = "application/json",
					StatusCode = 200
				};
				context.Result = contentResult;
				return;
			}
			var executedEndpointContext = await next.Invoke(); // execute the endpoint or next action filter
			if(executedEndpointContext.Result is OkObjectResult okObjectResult)
			{
				await cacheService.CacheResponseAsync(cacheKey,okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
			}
		}

		private string GenerateCasheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();
			keyBuilder.Append(request.Path);
			foreach (var (key,value) in request.Query.OrderBy(x => x.Key)) // orderby to prevent the change of the values in the Query string
			{
				keyBuilder.Append($"|{key}-{value}");
			}
			return keyBuilder.ToString();

		}
	}
}
