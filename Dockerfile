FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY --from=build-env /app/dist .
ENTRYPOINT [ "dotnet", "pix-hub-api.dll" ]