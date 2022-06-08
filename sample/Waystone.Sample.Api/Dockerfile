FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/Waystone.Sample.Api/Waystone.Sample.Api.csproj", "Waystone.Sample.Api/"]
RUN dotnet restore "sample/Waystone.Sample.Api/Waystone.Sample.Api.csproj"
COPY . .
WORKDIR "/src/Waystone.Sample.Api"
RUN dotnet build "Waystone.Sample.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Waystone.Sample.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Waystone.Sample.Api.dll"]
