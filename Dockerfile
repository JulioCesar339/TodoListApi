# 1. Construcción
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia solo el csproj y restaura (mejor caching)
COPY TodoApi.csproj .
RUN dotnet restore

# Copia todo lo demás y compila
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# 2. Imagen final ligera
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "TodoApi.dll"]