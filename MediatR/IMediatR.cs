using System.Threading.Tasks;

namespace MediatR
{
    public interface IMediatR
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
    }
}