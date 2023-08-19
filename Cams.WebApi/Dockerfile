# Get Base SDK from Microsoft
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
WORKDIR /WorkSimple.AuthApi
# Copy the CSPROJ files and restore any dependencies (via Nuget)
COPY *.csproj ./
RUN dotnet restore
# Copy the project files and build our release
COPY . ./
RUN dotnet publish -c Release -o out
# Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic
WORKDIR /WorkSimple.AuthApi
EXPOSE 80
COPY --from=build-env /WorkSimple.AuthApi/out .
ENTRYPOINT ["dotnet", "WorkSimple.AuthApi.dll"]
