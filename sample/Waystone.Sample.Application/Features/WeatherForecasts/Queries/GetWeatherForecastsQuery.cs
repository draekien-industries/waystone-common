namespace Waystone.Sample.Application.Features.WeatherForecasts.Queries;

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
    /// <summary>The validator for the <see cref="GetWeatherForecastsQuery" />.</summary>
    public class Validator : AbstractValidator<GetWeatherForecastsQuery>
    {
        /// <summary>Creates a new instance of the <see cref="Validator" /> class.</summary>
        public Validator()
        {
            RuleFor(x => x.Cursor).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Limit).GreaterThan(0).LessThanOrEqualTo(5);
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
            IEnumerable<WeatherForecast> forecasts = _repository.Get(
                request.Cursor,
                request.Limit);

            PaginatedResponse<WeatherForecastDto> result = new()
            {
                Results = _mapper.Map<IEnumerable<WeatherForecastDto>>(forecasts),
                Total = 100,
            };

            return Task.FromResult(result);
        }
    }
}
