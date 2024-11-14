FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all csproj files and restore as distinct layers
COPY ["AvatarTourSystem_BE/AvatarTourSystem_BE.csproj", "AvatarTourSystem_BE/"]
COPY ["BusinessObjects/BusinessObjects.csproj", "BusinessObjects/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["Services/Services.csproj", "Services/"]

# Restore packages
RUN dotnet restore "AvatarTourSystem_BE/AvatarTourSystem_BE.csproj"

# Copy everything else
COPY . .

# Build the project
WORKDIR "/src/AvatarTourSystem_BE"
RUN dotnet build "AvatarTourSystem_BE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AvatarTourSystem_BE.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AvatarTourSystem_BE.dll"]