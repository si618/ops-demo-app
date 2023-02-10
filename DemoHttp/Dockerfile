FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY DemoHttp/DemoHttp.csproj DemoHttp/
#
COPY Directory.Build.props ./
RUN dotnet restore DemoHttp/DemoHttp.csproj
COPY . .
WORKDIR /src/DemoHttp
RUN dotnet build DemoHttp.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish DemoHttp.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT dotnet DemoHttp.dll