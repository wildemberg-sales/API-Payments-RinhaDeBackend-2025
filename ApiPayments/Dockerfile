# Base image para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia a solução completa (inclui ApiPayments e ApiPaymentServices)
COPY . .

# Restaura e compila o projeto principal
WORKDIR /src/ApiPayments
RUN dotnet restore "ApiPayments.csproj"
RUN dotnet build "ApiPayments.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica o projeto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/ApiPayments
RUN dotnet publish "ApiPayments.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ApiPayments.dll"]
