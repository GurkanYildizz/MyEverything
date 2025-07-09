using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMediatr;

public interface ISender
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    public Task Send(IRequest request, CancellationToken cancellationToken = default);
}

public class Sender(IServiceProvider serviceProvider) : ISender
{
   public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        /* using var scoped = serviceProvider.CreateScope();
         var provider = scoped.ServiceProvider;
         var type = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse)); 
         var handler = provider.GetRequiredService(type);
         var handlerMethod = type.GetMethod("Handle");

         if (handlerMethod == null)
         {
             throw new Exception(" HandlerMethod boş");
         }

         var result = handlerMethod.Invoke(handler, new object[] { request, cancellationToken });

         var result=((dynamic)handler).Handle((dynamic) request, cancellationToken);

         if (result == null)
         {
             throw new Exception(" result boş");
         }

         return result;*/


        using var scoped = serviceProvider.CreateScope();

        var provider = scoped.ServiceProvider;

        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));

        dynamic handler = provider.GetRequiredService(handlerType);

        return await handler.Handle((dynamic)request, cancellationToken);

    }
    public async Task Send(IRequest request,CancellationToken cancellationToken)
    {
        using var scoped=serviceProvider.CreateScope();

        var provider = scoped.ServiceProvider;

        var handlerType= typeof(IRequestHandler<>)
            .MakeGenericType(request.GetType());

        dynamic handler = provider.GetRequiredService(handlerType);
        
        await handler.Handle((dynamic)request, cancellationToken);

        
    }
}
