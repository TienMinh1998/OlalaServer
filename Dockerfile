FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY olalaserver/*.csproj ./olalaserver/
COPY olalaserver.Common/*.csproj ./olalaserver.Common/
COPY olalaserver.Domain/*.csproj ./olalaserver.Domain/
COPY olalaserver.Repository/*.csproj ./olalaserver.Repository/
COPY olalaserver.Service/*.csproj ./olalaserver.Service/
RUN dotnet restore 

# copy everything else and build app
COPY . ./
RUN dotnet publish olalaserver.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 
EXPOSE 80
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "olalaserver.dll"]