# Usa la imagen de .NET SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia archivos del proyecto y restaura dependencias
COPY *.sln ./
COPY TiendaElectronica/*.csproj ./TiendaElectronica/
RUN dotnet restore TiendaElectronica/TiendaElectronica.csproj

# Copia todo y compila el proyecto
COPY . .
WORKDIR /app/TiendaElectronica
RUN dotnet publish -c Release -o /out

# Usa una imagen más liviana para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .

# Expone el puerto en el que corre ASP.NET
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
CMD ["dotnet", "TiendaElectronica.dll"]
