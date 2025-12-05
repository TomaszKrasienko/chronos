FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY chronos.employees.core/chronos.employees.core.csproj ./chronos.employees.core/
COPY chronos.employees.api/chronos.employees.api.csproj ./chronos.employees.api/

RUN dotnet restore ./chronos.employees.api/chronos.employees.api.csproj

COPY . .
RUN dotnet publish ./chronos.employees.api/chronos.employees.api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "chronos.employees.api.dll"]

ENV TZ="Europe/Warsaw"

EXPOSE 5001
EXPOSE 5003