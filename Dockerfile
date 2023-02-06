FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY DemoApi/DemoApi.csproj DemoApi/
COPY . .

WORKDIR /src/DemoApi

RUN dotnet restore
RUN dotnet build -c release --no-restore
RUN dotnet test -c release --no-restore
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "DemoApi.dll"]
