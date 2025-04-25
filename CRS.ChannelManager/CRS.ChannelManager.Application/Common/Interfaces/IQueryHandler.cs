using CRS.ChannelManager.Domain.Dtos.Interfaces;
using MediatR;

namespace CRS.ChannelManager.Application.Common.Interfaces
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        
    }
}