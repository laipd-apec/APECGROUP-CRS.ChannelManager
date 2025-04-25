using FastEndpoints;
using MediatR;

namespace CRS.ChannelManager.FastEndpoint
{
    public class CRSChannelManagerFastEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public CRSChannelManagerFastEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
    }

    public class CRSChannelManagerFastEndpoint<TResponse> : Endpoint<TResponse>
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public CRSChannelManagerFastEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

    }

    public class CRSChannelManagerFastEndpointWithoutRequest<TResponse> : EndpointWithoutRequest<TResponse>
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public CRSChannelManagerFastEndpointWithoutRequest(IMediator mediator)
        {
            _mediator = mediator;
        }

    }
}
