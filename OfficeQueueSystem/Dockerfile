FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ✅ COPIA TUTTI I FILE .CSPROJ
COPY ["OfficeQueueSystem/OfficeQueueSystem.csproj", "OfficeQueueSystem/"]
COPY ["Database/Database.csproj", "Database/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY ["Repository/Repository.csproj", "Repository/"]

# ✅ RESTORE DI TUTTI I PROGETTI
RUN dotnet restore "OfficeQueueSystem/OfficeQueueSystem.csproj"

# ✅ COPIA TUTTO IL CODICE
COPY . .

# ✅ LAVORA NELLA CARTELLA DEL PROGETTO API
WORKDIR "/src/OfficeQueueSystem"
RUN dotnet build "OfficeQueueSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OfficeQueueSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfficeQueueSystem.dll"]