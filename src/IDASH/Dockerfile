FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 9000
EXPOSE 9001

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["IDASH/IDASH.csproj", "IDASH/"]
RUN dotnet restore "IDASH/IDASH.csproj"
COPY . .
WORKDIR "/src/IDASH"
RUN dotnet build "IDASH.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IDASH.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IDASH.dll"]
