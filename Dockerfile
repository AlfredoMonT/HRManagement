# ETAPA 1: BUILD (Usamos la imagen completa de SDK para compilar)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos los archivos de proyecto (csproj) de todas las capas
COPY ["HRManagement.WebApi/HRManagement.WebApi.csproj", "HRManagement.WebApi/"]
COPY ["HRManagement.Application/HRManagement.Application.csproj", "HRManagement.Application/"]
COPY ["HRManagement.Domain/HRManagement.Domain.csproj", "HRManagement.Domain/"]
COPY ["HRManagement.Infrastructure/HRManagement.Infrastructure.csproj", "HRManagement.Infrastructure/"]

# Restauramos dependencias (esto cachea las librerías para que sea rápido)
RUN dotnet restore "HRManagement.WebApi/HRManagement.WebApi.csproj"

# Copiamos todo el resto del código fuente
COPY . .

# Compilamos la API
WORKDIR "/src/HRManagement.WebApi"
RUN dotnet build "HRManagement.WebApi.csproj" -c Release -o /app/build

# Publicamos los archivos finales
FROM build AS publish
RUN dotnet publish "HRManagement.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ETAPA 2: RUNTIME (Usamos la imagen ligera solo para correr la app)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copiamos solo lo necesario de la etapa de build
COPY --from=publish /app/publish .

# Definimos el punto de entrada
ENTRYPOINT ["dotnet", "HRManagement.WebApi.dll"]