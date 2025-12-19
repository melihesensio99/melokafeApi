FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["KafeApi.Domain/KafeApi.Domain.csproj", "KafeApi.Domain/"]
COPY ["KafeApi.Application/KafeApi.Application.csproj", "KafeApi.Application/"]
COPY ["KafeApi.Persistence/KafeApi.Persistence.csproj", "KafeApi.Persistence/"]
COPY ["Presentation/kafeApi.API/kafeApi.API.csproj", "Presentation/kafeApi.API/"]

RUN dotnet restore "Presentation/kafeApi.API/kafeApi.API.csproj"

COPY . .
WORKDIR "/src/Presentation/kafeApi.API"
RUN dotnet publish "kafeApi.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "kafeApi.API.dll"]