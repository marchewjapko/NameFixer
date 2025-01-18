ARG BUILD_CONFIGURATION="Production"

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY ["NameFixer.WebApi/NameFixer.WebApi.csproj", "NameFixer.WebApi/"]
RUN dotnet restore "NameFixer.WebApi/NameFixer.WebApi.csproj"
COPY . .
WORKDIR "/src/NameFixer.WebApi"
RUN dotnet build "NameFixer.WebApi.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish "NameFixer.WebApi.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN ["sh", "-c", "apt-get -y update && apt-get --no-install-recommends -y install curl && apt-get clean && useradd -m $APP_UID"]
USER $APP_UID
ENTRYPOINT ["dotnet", "NameFixer.WebApi.dll"]

ENV HEALTH_CHECK_PATH="localhost:8080/_health"
HEALTHCHECK --interval=5s --timeout=10s --retries=5 CMD ["/bin/sh", "-c", "curl -f $HEALTH_CHECK_PATH || exit 1"]