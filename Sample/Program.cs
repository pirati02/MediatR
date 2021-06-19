using System;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;
using MediatR;
using MediatR.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            var provider = services.AddMediatR(Assembly.GetExecutingAssembly()).BuildServiceProvider();

            var mediatR = provider.GetRequiredService<IMediatR>();
            var result = await mediatR.SendAsync(new WriteToConsoleRequest
            {
                Text = "Hello Bakar"
            });
        }
    }

    public class WriteToConsoleRequest : IRequest<bool>
    {
        public string Text { get; set; }
    }

    public class WriteToConsoleRequestHandler : IRequestHandler<WriteToConsoleRequest, bool>
    {
        public Task<bool> HandleAsync(WriteToConsoleRequest request)
        {
            Console.WriteLine(request.Text);
            return Task.FromResult(true);
        }
    }
}