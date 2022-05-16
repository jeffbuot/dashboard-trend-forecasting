FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/SMPLX.ForecastingDashboard.Blazor/SMPLX.ForecastingDashboard.Blazor.csproj", "SMPLX.ForecastingDashboard.Blazor/"]
COPY ["src/SMPLX.ForecastingDashboard.Application/SMPLX.ForecastingDashboard.Application.csproj", "SMPLX.ForecastingDashboard.Application/"]
COPY ["src/SMPLX.ForecastingDashboard.Domain/SMPLX.ForecastingDashboard.Domain.csproj", "SMPLX.ForecastingDashboard.Domain/"]
COPY ["src/SMPLX.ForecastingDashboard.Domain.Shared/SMPLX.ForecastingDashboard.Domain.Shared.csproj", "SMPLX.ForecastingDashboard.Domain.Shared/"]
COPY ["src/SMPLX.ForecastingDashboard.Application.Contracts/SMPLX.ForecastingDashboard.Application.Contracts.csproj", "SMPLX.ForecastingDashboard.Application.Contracts/"]
COPY ["src/SMPLX.ForecastingDashboard.HttpApi/SMPLX.ForecastingDashboard.HttpApi.csproj", "SMPLX.ForecastingDashboard.HttpApi/"]
COPY ["src/SMPLX.ForecastingDashboard.MongoDB/SMPLX.ForecastingDashboard.MongoDB.csproj", "SMPLX.ForecastingDashboard.MongoDB/"]
COPY ["src/Volo.Abp.AspNetCore.Components.Server.BasicTheme/Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj", "Volo.Abp.AspNetCore.Components.Server.BasicTheme/"]
COPY ["src/Volo.Abp.AspNetCore.Components.Web.BasicTheme/Volo.Abp.AspNetCore.Components.Web.BasicTheme.csproj", "Volo.Abp.AspNetCore.Components.Web.BasicTheme/"]
RUN dotnet restore "src/SMPLX.ForecastingDashboard.Blazor/SMPLX.ForecastingDashboard.Blazor.csproj"
COPY . .
WORKDIR "/src/SMPLX.ForecastingDashboard.Blazor"
RUN dotnet build "SMPLX.ForecastingDashboard.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SMPLX.ForecastingDashboard.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMPLX.ForecastingDashboard.Blazor.dll"]
