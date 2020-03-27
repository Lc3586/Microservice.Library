FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 9002
EXPOSE 9003

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SSH/SSH.csproj", "SSH/"]
RUN dotnet restore "SSH/SSH.csproj"
COPY . .
WORKDIR "/src/SSH"
RUN dotnet build "SSH.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SSH.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SSH.dll"]
