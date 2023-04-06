#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Example.Api/Example.Api.csproj", "Example.Api/"]
COPY ["Example.CommandHandler/Example.CommandHandler.csproj", "Example.CommandHandler/"]
COPY ["Example.DataTransfer/Example.DataTransfer.csproj", "Example.DataTransfer/"]
COPY ["Example.Common/Example.Common.csproj", "Example.Common/"]
COPY ["Example.DomainModel/Example.DomainModel.csproj", "Example.DomainModel/"]
COPY ["Example.Repository/Example.Repository.csproj", "Example.Repository/"]
COPY ["Example.QueryHandler/Example.QueryHandler.csproj", "Example.QueryHandler/"]
COPY ["Example.RepositoryHandler.MsSql.Dapper/Example.RepositoryHandler.MsSql.Dapper.csproj", "Example.RepositoryHandler.MsSql.Dapper/"]
COPY ["Example.RepositoryHandler.MsSql.EF/Example.RepositoryHandler.MsSql.EF.csproj", "Example.RepositoryHandler.MsSql.EF/"]
RUN dotnet restore "Example.Api/Example.Api.csproj"
COPY . .
WORKDIR "/src/Example.Api"
RUN dotnet build "Example.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Example.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Example.Api.dll"]