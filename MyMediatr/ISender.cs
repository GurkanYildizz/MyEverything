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
}

public class Sender(IServiceProvider serviceProvider) : ISender
{
    async Task<TResponse> ISender.Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)//Bunun bir de sadece requet lisi olacak
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


        using var scope = serviceProvider.CreateScope();

        var provider = scope.ServiceProvider;

        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));

        dynamic handler = provider.GetRequiredService(handlerType);

        return await handler.Handle((dynamic)request, cancellationToken);

    }
}
