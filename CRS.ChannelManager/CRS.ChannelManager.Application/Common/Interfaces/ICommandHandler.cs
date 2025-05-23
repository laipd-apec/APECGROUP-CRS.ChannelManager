using CRS.ChannelManager.Domain.Dtos.Interfaces;
using MediatR;

namespace CRS.ChannelManager.Application.Common.Interfaces
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
    }
}