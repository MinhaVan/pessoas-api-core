# Etapa base para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Aluno.Core.API.sln", "."]
COPY ["Aluno.Core.API/Aluno.Core.API.csproj", "Aluno.Core.API/"]
COPY ["Aluno.Core.Domain/Aluno.Core.Domain.csproj", "Aluno.Core.Domain/"]
COPY ["Aluno.Core.Application/Aluno.Core.Application.csproj", "Aluno.Core.Application/"]
COPY ["Aluno.Core.Data/Aluno.Core.Data.csproj", "Aluno.Core.Data/"]
COPY ["Aluno.Core.Tests/Aluno.Core.Tests.csproj", "Aluno.Core.Tests/"]

# Restaura as dependências
RUN dotnet restore "Aluno.Core.API.sln"

# Copia o restante do código e realiza o build
COPY . .
WORKDIR "/src/Aluno.Core.API"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Etapa para publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aluno.Core.API.dll"]
