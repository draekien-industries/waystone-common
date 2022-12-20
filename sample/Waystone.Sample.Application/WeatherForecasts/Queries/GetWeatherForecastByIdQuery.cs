namespace Waystone.Sample.Application.WeatherForecasts.Queries;

using AutoMapper;
using Common.Application.Contracts.Caching;
using Common.Domain.Contracts.Exceptions;
using Common.Domain.Contracts.Results;
using Contracts;
using Domain.Entities.WeatherForecasts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Services;

/// <summary>A query to get a weather forecast by it's ID.</summary>
public class GetWeatherForecastByIdQuery : ICachedRequest<WeatherForecastDto>
{
    /// <summary>The id the of the weather forecast to get.</summary>
    public Guid Id { get; init; }

    /// <inheritdoc />
    public string CacheKey => $"{nameof(GetWeatherForecastsQuery)}_{Id}";

    /// <inheritdoc />
    public TimeSpan? CacheSeconds => TimeSpan.FromMinutes(15);

    /// <summary>Validator for <see cref="GetWeatherForecastsQuery" /></summary>
    public class Validator : AbstractValidator<GetWeatherForecastByIdQuery>
    {
        /// <summary>Creates a new instance of <see cref="Validator" />.</summary>
        public Validator(IWeatherForecastRepository repository)
        {
            RuleFor(x => x.Id)
               .Must(repository.Any)
               .WithMessage((_, id) => $"Weather forecast with id '{id}' does not exist.");
        }
    }

    /// <summary>Handler for <see cref="GetWeatherForecastsQuery" /></summary>
    public class Handler : IRequestHandler<GetWeatherForecastByIdQuery, WeatherForecastDto>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IWeatherForecastRepository _repository;

        /// <summary>Creates a new instance of <see cref="Handler" />.</summary>
        /// <param name="repository">The <see cref="IWeatherForecastRepository" /></param>
        /// <param name="logger">The <see cref="ILogger" /></param>
        /// <param name="mapper">The <see cref="IMapper" /></param>
        public Handler(IWeatherForecastRepository repository, ILogger<Handler> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public Task<WeatherForecastDto> Handle(GetWeatherForecastByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting weather forecast by id: {Id}", request.Id.ToString());

            Result<WeatherForecast> getForecastResult = _repository.Get(request.Id);

            if (getForecastResult.Failed)
            {
                _logger.LogError("Weather forecast not found: {Id}", request.Id.ToString());

                throw new NotFoundException(getForecastResult.Error);
            }

            var result = _mapper.Map<WeatherForecastDto>(getForecastResult.Value);

            return Task.FromResult(result);
        }
    }
}
