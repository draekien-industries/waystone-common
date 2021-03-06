namespace Waystone.Sample.Application.WeatherForecasts.Queries;

using AutoMapper;
using Common.Domain.Contracts.Exceptions;
using Contracts;
using Domain.Entities.WeatherForecasts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Services;

/// <summary>A query to get a weather forecast by it's ID.</summary>
public class GetWeatherForecastByIdQuery : IRequest<WeatherForecastDto>
{
    /// <summary>The id the of the weather forecast to get.</summary>
    public Guid Id { get; init; }

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

            WeatherForecast? forecast = _repository.Get(request.Id);

            if (forecast == null)
            {
                _logger.LogError("Weather forecast not found: {Id}", request.Id.ToString());

                throw new NotFoundException("Weather forecast not found.");
            }

            var result = _mapper.Map<WeatherForecastDto>(forecast);

            return Task.FromResult(result);
        }
    }
}
