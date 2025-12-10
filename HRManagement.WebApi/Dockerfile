# 1. Etapa de Build (compilación)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia TODO el código fuente primero.
#    (Tu .dockerignore se encarga de bin/obj locales)
COPY . .

# 2. Restaura la solución COMPLETA.
#    Esto crea los archivos 'project.assets.json' en las carpetas obj.
RUN dotnet restore "HRManagementSystem.sln"

# 3. Construye el proyecto. 
#    Ya no necesitamos '--no-restore' porque el 'restore' anterior
#    ya hizo todo el trabajo pesado.
RUN dotnet build "HRManagement.WebApi/HRManagement.WebApi.csproj" -c Release -o /app/build

# 2. Etapa de Publish (publicación)
FROM build AS publish
# Publicamos. Esto también ejecutará un 'restore' implícito, pero será rápido.
RUN dotnet publish "HRManagement.WebApi/HRManagement.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Etapa Final (ejecución)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "HRManagement.WebApi.dll"]