#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["patchawallet.holiday.api/patchawallet.holiday.api.csproj", "patchawallet.holiday.api/"]
RUN dotnet restore "patchawallet.holiday.api/patchawallet.holiday.api.csproj"
COPY . .
WORKDIR "/src/patchawallet.holiday.api"
RUN dotnet build "patchawallet.holiday.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "patchawallet.holiday.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "patchawallet.holiday.api.dll"]