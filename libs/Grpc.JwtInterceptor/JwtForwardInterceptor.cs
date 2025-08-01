using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace Grpc.JwtInterceptor
{
    /// <summary>
    /// Interceptor that appends JWT bearer token to every gRPC call from this client
    /// </summary>
    public class JwtForwardInterceptor(IHttpContextAccessor httpContextAccessor) : Interceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
           TRequest request,
           ClientInterceptorContext<TRequest, TResponse> context,
           AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var jwt = httpContext?.Request.Headers["Authorization"].ToString();

            var headers = new Metadata();

            if (!string.IsNullOrEmpty(jwt))
            {
                headers.Add("Authorization", jwt);
            }

            var newOptions = context.Options.WithHeaders(headers);

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method,
                context.Host,
                newOptions
            );

            return continuation(request, newContext);
        }
    }
}
