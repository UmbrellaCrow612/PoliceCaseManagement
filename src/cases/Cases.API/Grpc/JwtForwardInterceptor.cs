using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Cases.API.Grpc
{
    /// <summary>
    /// Interceptor that appends JWT bearer token to every gRPC call from this client
    /// </summary>
    public class JwtForwardInterceptor(IHttpContextAccessor httpContextAccessor, ILogger<JwtForwardInterceptor> logger) : Interceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<JwtForwardInterceptor> _logger = logger;

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
           TRequest request,
           ClientInterceptorContext<TRequest, TResponse> context,
           AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("JwtForwardInterceptor running");

            var httpContext = _httpContextAccessor.HttpContext;
            var jwt = httpContext?.Request.Headers.Authorization.ToString();

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
