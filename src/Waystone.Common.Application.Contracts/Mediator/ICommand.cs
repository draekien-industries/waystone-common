namespace Waystone.Common.Application.Contracts.Mediator;

using Domain.Results;
using MediatR;

/// <summary>
/// A MediatR request which expects a <see cref="Result" /> to be returned.
/// </summary>
public interface ICommand : IRequest<Result>
{ }

/// <summary>
/// A MediatR request which expects a <see cref="Result{TValue}" /> to be returned.
/// </summary>
/// <typeparam name="TValue">The inner return type of the command result.</typeparam>
public interface ICommand<TValue> : IRequest<Result<TValue>>
{ }

/// <summary>
/// A MediatR request handler which handles an <see cref="IQuery" />.
/// </summary>
/// <typeparam name="TCommand">The command that this handler is responsible for.</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{ }

/// <summary>
/// A MediatR request handler which handles an <see cref="IQuery{TValue}" />.
/// </summary>
/// <typeparam name="TCommand">The command that this handler is responsible for.</typeparam>
/// <typeparam name="TValue">The inner return type of the command result.</typeparam>
public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, Result<TValue>>
    where TCommand : ICommand<TValue>
{ }
