namespace Waystone.Common.Application.Contracts.Mediator;

using Domain.Contracts.Results;
using MediatR;

/// <summary>
/// A MediatR request which expects a <see cref="Result" /> to be returned.
/// </summary>
public interface IQuery : IRequest<Result>
{ }

/// <summary>
/// A MediatR request which expects a <see cref="Result{TValue}" /> to be returned.
/// </summary>
/// <typeparam name="TValue">The inner return type of the query result.</typeparam>
public interface IQuery<TValue> : IRequest<Result<TValue>>
{ }

/// <summary>
/// A MediatR request handler which handles an <see cref="IQuery" />.
/// </summary>
/// <typeparam name="TQuery">The query that this handler is responsible for.</typeparam>
public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Result> where TQuery : IQuery
{ }

/// <summary>
/// A MediatR request handler which handles an <see cref="IQuery{TValue}" />.
/// </summary>
/// <typeparam name="TQuery">The query that this handler is responsible for.</typeparam>
/// <typeparam name="TValue">The inner return type of the query result.</typeparam>
public interface IQueryHandler<in TQuery, TValue> : IRequestHandler<TQuery, Result<TValue>>
    where TQuery : IQuery<TValue>
{ }
