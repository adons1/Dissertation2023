#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Customers.CacheManager/Customers.CacheManager.csproj", "Customers.CacheManager/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["CustomersService.TransportTypes/CustomersService.TransportTypes.csproj", "CustomersService.TransportTypes/"]
RUN dotnet restore "Customers.CacheManager/Customers.CacheManager.csproj"
COPY . .
WORKDIR "/src/Customers.CacheManager"
RUN dotnet build "Customers.CacheManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customers.CacheManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customers.CacheManager.dll"]