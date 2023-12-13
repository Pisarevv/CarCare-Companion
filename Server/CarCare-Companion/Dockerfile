#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY ["CarCare-Companion.Api/CarCare-Companion.Api.csproj", "CarCare-Companion.Api/"]
COPY ["CarCare-Companion.Core/CarCare-Companion.Core.csproj", "CarCare-Companion.Core/"]
COPY ["CarCare-Companion.Common/CarCare-Companion.Common.csproj", "CarCare-Companion.Common/"]
COPY ["CarCare-Companion.Infrastructure/CarCare-Companion.Infrastructure.csproj", "CarCare-Companion.Infrastructure/"]

WORKDIR /app/CarCare-Companion.Api
RUN dotnet restore

WORKDIR /app

COPY ["CarCare-Companion.Api/.", "CarCare-Companion.Api/"]
COPY ["CarCare-Companion.Core/.", "CarCare-Companion.Core/"]
COPY ["CarCare-Companion.Common/.", "CarCare-Companion.Common/"]
COPY ["CarCare-Companion.Infrastructure/.", "CarCare-Companion.Infrastructure/"]

WORKDIR /app/CarCare-Companion.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
EXPOSE 5275/tcp
ENV ASPNETCORE_URLS http://*:5275

COPY --from=build /app/CarCare-Companion.Api/out ./
ENTRYPOINT ["dotnet", "CarCare-Companion.Api.dll"]