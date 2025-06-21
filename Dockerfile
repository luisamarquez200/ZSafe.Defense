# Imagen base para ejecutar la app (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Imagen para compilar la app (build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ZSafe.Defense.csproj", "./"]
RUN dotnet restore "./ZSafe.Defense.csproj"

COPY . .
RUN dotnet build "ZSafe.Defense.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "ZSafe.Defense.csproj" -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ZSafe.Defense.dll"]
