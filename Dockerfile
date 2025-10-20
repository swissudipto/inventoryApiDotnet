# ----------------------
# Stage 1: Build
# ----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . . 
RUN dotnet restore

# Build and publish the app
RUN dotnet publish -c Release -o out

# ----------------------
# Stage 2: Runtime
# ----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the output from build stage
COPY --from=build /app/out ./

# Tell .NET to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "inventoryApiDotnet.dll"]