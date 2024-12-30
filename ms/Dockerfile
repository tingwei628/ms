FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR ms

# copy csproj file and restore
COPY *.csproj ./
RUN dotnet restore -v diag

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ms.dll"]