# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia todos os projetos da solução
COPY ["LocaRider.sln", "./"]
COPY ["LocaRider.API/LocaRider.API.csproj", "LocaRider.API/"]
COPY ["LocaRider.Application/LocaRider.Application.csproj", "LocaRider.Application/"]
COPY ["LocaRider.Infrastructure/LocaRider.Infrastructure.csproj", "LocaRider.Infrastructure/"]
COPY ["LocaRider.Domain/LocaRider.Domain.csproj", "LocaRider.Domain/"]

# Restaura todos os pacotes
RUN dotnet restore "LocaRider.sln"

# Copia o restante do código
COPY . .

WORKDIR "/src/LocaRider.API"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "LocaRider.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicação
FROM build AS publish
RUN dotnet publish "LocaRider.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocaRider.API.dll"]
