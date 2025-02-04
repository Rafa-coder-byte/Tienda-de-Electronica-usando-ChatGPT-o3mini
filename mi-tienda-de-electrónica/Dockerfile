# Usa la imagen de .NET SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia archivos del proyecto y restaura dependencias
COPY *.sln ./
COPY mi-tienda-de-electrónica/*.csproj ./mi-tienda-de-electrónica/
RUN dotnet restore mi-tienda-de-electrónica/mi-tienda-de-electrónica.csproj

# Copia todo y compila el proyecto
COPY . .
WORKDIR /app/mi-tienda-de-electrónica
RUN dotnet publish -c Release -o /out

# Usa una imagen más liviana para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .

# Expone el puerto en el que corre ASP.NET
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
CMD ["dotnet", "mi-tienda-de-electrónica.dll"]

WORKDIR /app
COPY ./mi-tienda-de-electrónica . 
