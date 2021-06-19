using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediatR
{
    public class MediatR : IMediatR
    {
        private readonly Func<Type, object> _serviceResolver;
        private readonly Dictionary<Type, Type> _requestAndHandlerContracts;

        public MediatR(Func<Type, object> serviceResolver, Dictionary<Type, Type> requestAndHandlerContracts)
        {
            _serviceResolver = serviceResolver;
            _requestAndHandlerContracts = requestAndHandlerContracts;
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();
            if (!_requestAndHandlerContracts.ContainsKey(requestType))
            {
                throw new ArgumentNullException($"request handler must be provided for Request {requestType.Name}");
            }

            _requestAndHandlerContracts.TryGetValue(requestType, out var requestHandlerType);

            var requestHandler = _serviceResolver(requestHandlerType);
            var handleMethod = requestHandler?.GetType().GetMethod("HandleAsync");
            return (Task<TResponse>) handleMethod?.Invoke(requestHandler, new object[] {request});
        }
    }
}