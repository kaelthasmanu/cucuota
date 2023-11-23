FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5173

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["cucuota/cucuota.csproj", "cucuota/"]
RUN dotnet restore "cucuota/cucuota.csproj"
COPY . .
WORKDIR "/src/cucuota"
RUN dotnet build "cucuota.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cucuota.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cucuota.dll"]
