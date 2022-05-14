FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY APIProject/*.csproj ./APIProject/
COPY APIProject.Common/*.csproj ./APIProject.Common/
COPY APIProject.Domain/*.csproj ./APIProject.Domain/
COPY APIProject.Repository/*.csproj ./APIProject.Repository/
COPY APIProject.Service/*.csproj ./APIProject.Service/
RUN dotnet restore 

# copy everything else and build app
COPY . ./
RUN dotnet publish APIProject.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 
EXPOSE 80
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "APIProject.dll"]