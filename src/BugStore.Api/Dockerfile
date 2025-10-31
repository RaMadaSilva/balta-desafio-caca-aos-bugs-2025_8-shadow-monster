# ============================================
# ETAPA 1: Base Runtime (Imagem Final)
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# ============================================
# ETAPA 2: Build (Compilação)
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# ESTRATÉGIA DE CACHE: Copia apenas os arquivos .csproj primeiro
# Isso permite que o Docker reutilize o cache da camada de restore
# mesmo quando o código fonte muda, acelerando builds subsequentes
COPY ["BugStore.Api/BugStore.Api.csproj", "BugStore.Api/"]
COPY ["BugStore.Application/BugStore.Application.csproj", "BugStore.Application/"]
COPY ["BugStore.Domain/BugStore.Domain.csproj", "BugStore.Domain/"]
COPY ["BugStore.Infrastructure/BugStore.Infrastructure.csproj", "BugStore.Infrastructure/"]

# Restaura as dependências NuGet (pode usar cache se .csproj não mudou)
RUN dotnet restore "./BugStore.Api/BugStore.Api.csproj" \
    --verbosity quiet \
    --nologo

# Agora copia todo o código fonte
COPY . .

# Compila o projeto (sem especificar -o, usa o local padrão bin/Release/net9.0/)
WORKDIR "/src/BugStore.Api"
RUN dotnet build "./BugStore.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    --no-restore \
    --verbosity minimal \
    --nologo

# ============================================
# ETAPA 3: Publish (Publicação)
# ============================================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/BugStore.Api"
RUN dotnet publish "./BugStore.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-build \
    --verbosity minimal \
    --nologo

# ============================================
# ETAPA 4: Final (Imagem de Produção)
# ============================================
FROM base AS final
WORKDIR /app

# Copia apenas os arquivos publicados (otimização de tamanho)
COPY --from=publish /app/publish .

# Configura variáveis de ambiente
# O Program.cs já configura a porta via UseUrls, mas manteremos isso como fallback
# Railway define a variável PORT em runtime
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

# NOTA: Não use USER $APP_UID no Railway, pois pode causar problemas de permissão
# O Railway gerencia isso automaticamente

# Define o ponto de entrada
ENTRYPOINT ["dotnet", "BugStore.Api.dll"]