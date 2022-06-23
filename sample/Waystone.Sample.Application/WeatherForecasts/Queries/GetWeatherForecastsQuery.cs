namespace Waystone.Sample.Application.WeatherForecasts.Queries;

using AutoMapper;
using Common.Application.Contracts.Pagination;
using Contracts;
using Domain.Entities.WeatherForecasts;
using FluentValidation;
using MediatR;
using Services;

/// <summary>A query to get a paginated set of weather forecasts.</summary>
public class GetWeatherForecastsQuery : PaginatedRequest<WeatherForecastDto>
{
    /// <summary>When specified, the weather forecasts found will only have the specified summary.</summary>
    public ForecastSummary? DesiredSummary { get; init; }

    /// <summary>When specified, the weather forecasts found will all have TemperatureC above this value.</summary>
    public int? MinimumTemperatureC { get; init; }

    /// <summary>When specified, the weather forecasts found will all have TemperatureC below this value.</summary>
    public int? MaximumTemperatureC { get; init; }

    /// <summary>The validator for <see cref="GetWeatherForecastsQuery" />.</summary>
    public class Validator : AbstractValidator<GetWeatherForecastsQuery>
    {
        /// <summary>Initializes a new instance of the <see cref="Validator" /> class.</summary>
        public Validator()
        {
            RuleFor(_ => _.DesiredSummary).IsInEnum();
            RuleFor(_ => _.MinimumTemperatureC).InclusiveBetween(-20, 45).When(_ => _.MinimumTemperatureC.HasValue);
            RuleFor(_ => _.MaximumTemperatureC).InclusiveBetween(-20, 45).When(_ => _.MaximumTemperatureC.HasValue);

            RuleFor(_ => _.MinimumTemperatureC)
               .LessThanOrEqualTo(_ => _.MaximumTemperatureC)
               .When(_ => _.MinimumTemperatureC.HasValue && _.MaximumTemperatureC.HasValue);
        }
    }

    /// <summary>The handler for the <see cref="GetWeatherForecastsQuery" />.</summary>
    public class Handler : IRequestHandler<GetWeatherForecastsQuery, PaginatedResponse<WeatherForecastDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWeatherForecastRepository _repository;

        /// <summary>Creates a new instance of the <see cref="Handler" /> class.</summary>
        /// <param name="repository">The <see cref="IWeatherForecastRepository" />.</param>
        /// <param name="mapper">The <see cref="IMapper" />.</param>
        public Handler(IWeatherForecastRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public Task<PaginatedResponse<WeatherForecastDto>> Handle(
            GetWeatherForecastsQuery request,
            CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ForecastFilterDto>(request);

            IEnumerable<WeatherForecast> forecasts = _repository.Get(
                request.Cursor,
                request.Limit,
                filter);

            PaginatedResponse<WeatherForecastDto> result = new()
            {
                Results = _mapper.Map<IEnumerable<WeatherForecastDto>>(forecasts),
                Total = _repository.Count(filter),
            };

            return Task.FromResult(result);
        }
    }
}
