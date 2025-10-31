# --- ETAPA 1: Base (Runtime) ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# --- ETAPA 2: Build ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas os .csproj para restaurar pacotes (cache eficiente)
COPY ["BugStore.Api/BugStore.Api.csproj", "BugStore.Api/"]
COPY ["BugStore.Application/BugStore.Application.csproj", "BugStore.Application/"]
COPY ["BugStore.Domain/BugStore.Domain.csproj", "BugStore.Domain/"]
COPY ["BugStore.Infrastructure/BugStore.Infrastructure.csproj", "BugStore.Infrastructure/"]

# Restaura pacotes
RUN dotnet restore "./BugStore.Api/BugStore.Api.csproj"

# Copia todo o código
COPY . .

# Compila
WORKDIR "/src/BugStore.Api"
RUN dotnet build "./BugStore.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --- ETAPA 3: Publish ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BugStore.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- ETAPA 4: Final (Runtime) ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# CRUCIAL: Usa a porta do Railway (ou 8080 como fallback)
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

# REMOVA O USER (Railway não permite)
# USER $APP_UID → COMENTADO

ENTRYPOINT ["dotnet", "BugStore.Api.dll"]