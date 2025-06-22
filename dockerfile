# Etapa base para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src

COPY ["Pessoas.sln", "."]
COPY ["Pessoas.API/Pessoas.API.csproj", "Pessoas.API/"]
COPY ["Pessoas.Domain/Pessoas.Domain.csproj", "Pessoas.Domain/"]
COPY ["Pessoas.Application/Pessoas.Application.csproj", "Pessoas.Application/"]
COPY ["Pessoas.Data/Pessoas.Data.csproj", "Pessoas.Data/"]
COPY ["Pessoas.Tests/Pessoas.Tests.csproj", "Pessoas.Tests/"]

# Restaura as dependências
RUN dotnet restore "Pessoas.sln"

# Copia o restante do código e realiza o build
COPY . .
WORKDIR "/src/Pessoas.API"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Etapa para publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pessoas.API.dll"]
