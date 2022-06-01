FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR msdir

# copy csproj file and restore
COPY ./*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /msdir
COPY --from=build /msdir/out .
ENTRYPOINT ["dotnet", "ms.dll"]