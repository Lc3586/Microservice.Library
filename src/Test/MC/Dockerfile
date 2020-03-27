FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Test/MC/MC.csproj", "Test/MC/"]
RUN dotnet restore "Test/MC/MC.csproj"
COPY . .
WORKDIR "/src/Test/MC"
RUN dotnet build "MC.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MC.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MC.dll"]