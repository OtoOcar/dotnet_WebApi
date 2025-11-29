# Imagen base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar primero el csproj
COPY ["libreria.WebApi.csproj", "."]
RUN dotnet restore "libreria.WebApi.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src"
RUN dotnet build "libreria.WebApi.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "libreria.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "libreria.WebApi.dll"]
