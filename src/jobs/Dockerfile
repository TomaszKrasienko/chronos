FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY chronos.jobs.synchronizer/chronos.jobs.synchronizer.csproj ./chronos.jobs.synchronizer/

RUN dotnet restore ./chronos.jobs.synchronizer/chronos.jobs.synchronizer.csproj

COPY chronos.jobs.synchronizer/ ./chronos.jobs.synchronizer/
RUN dotnet publish ./chronos.jobs.synchronizer/chronos.jobs.synchronizer.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .

ENV TZ="Europe/Warsaw"

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "chronos.jobs.synchronizer.dll"]
