ARG BUILD_CONFIGURATION="Production"

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

#Main endpoint for TSL gRPC channels
ENV Kestrel__Endpoints__Https__Url=http://+:8080
EXPOSE 8080

#Endpoint for unsecured gRPC channels
ENV Kestrel__Endpoints__Http__Url=http://+:8081
EXPOSE 8081

#Endpoint unsuitable for gRPC clients, suitable for healthcheck calls
ENV Kestrel__Endpoints__Http1AndHttp2__Url=http://+:8082
ENV Kestrel__Endpoints__Http1AndHttp2__Protocols=Http1AndHttp2
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY ["NameFixer.WebApi/NameFixer.WebApi.csproj", "NameFixer.WebApi/"]
RUN dotnet restore "NameFixer.WebApi/NameFixer.WebApi.csproj"

COPY "NameFixer.WebApi" "NameFixer.WebApi"
COPY "NameFixer.UseCases" "NameFixer.UseCases"
COPY "NameFixer.Infrastructure" "NameFixer.Infrastructure"
COPY "NameFixer.Core" "NameFixer.Core"

WORKDIR "/src/NameFixer.WebApi"
RUN dotnet build "NameFixer.WebApi.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish "NameFixer.WebApi.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

USER root
RUN chown $APP_UID:app .
USER $APP_UID

COPY --from=publish /app/publish .
USER root
RUN ["sh", "-c", "apt-get -y update && apt-get --no-install-recommends -y install curl"]

RUN ["sh", "-c", "apt-get clean  && useradd -m $APP_UID"]
USER $APP_UID
ENTRYPOINT ["dotnet", "NameFixer.WebApi.dll"]

ENV HEALTH_CHECK_PATH="localhost:8082/_health"
HEALTHCHECK --interval=5s --timeout=10s --retries=5 CMD ["/bin/sh", "-c", "curl -f $HEALTH_CHECK_PATH || exit 1"]