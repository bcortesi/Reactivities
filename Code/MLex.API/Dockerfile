FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MLex.API/MLex.API.csproj", "MLex.API/"]
RUN dotnet restore "MLex.API/MLex.API.csproj"
COPY . .
WORKDIR "/src/MLex.API"
RUN dotnet build "MLex.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MLex.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MLex.API.dll"]