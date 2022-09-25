FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80

# Copy everything
COPY . .

# Restore as distinct layers, ignore two errors
RUN dotnet restore -nowarn:msb3202,nu1503 src/Clamify.Web/Clamify.Web.csproj

# Build solution
RUN dotnet publish -c Release -o /app/src/Clamify.Web/out src/Clamify.Web/Clamify.Web.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY --from=build-env /app/src/Clamify.Web/out .
ENTRYPOINT ["dotnet", "Clamify.Web.dll"]
