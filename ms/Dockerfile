FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
# copy csproj file and restore
COPY *.csproj ./
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore

# Copy everything else and build
COPY . ./
# trim-self-contained and trimmed .NET build
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ms.dll"]