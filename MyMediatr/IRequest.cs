using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMediatr;

public interface IRequest { }

public interface IRequest<TResponse> { }

public interface IRequestHandler<TRequest> where TRequest : IRequest
{
    public Task Handle(TRequest request,CancellationToken cancellationToken=default);

}

public interface IRequestHandler<TRequest,TResponse> where TRequest:IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request,CancellationToken cancellationToken=default);
}


