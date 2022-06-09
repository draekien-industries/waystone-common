namespace Waystone.Sample.Application.Features.WeatherForecasts.Queries;

using AutoMapper;
using Common.Application.Contracts.Pagination;
using Contracts;
using Domain.Entities.WeatherForecasts;
using FluentValidation;
using MediatR;
using Services;

public class GetWeatherForecastsQuery : PaginatedRequest<WeatherForecastDto>
{
    public class Validator : AbstractValidator<GetWeatherForecastsQuery>
    {
        public Validator()
        {
            RuleFor(x => x.Cursor).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Limit).GreaterThan(0).LessThanOrEqualTo(5);
        }
    }

    public class Handler : IRequestHandler<GetWeatherForecastsQuery, PaginatedResponse<WeatherForecastDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWeatherForecastRepository _repository;

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
                request.Cursor.GetValueOrDefault(0),
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
