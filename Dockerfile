FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MyCMS.Web/MyCMS.Web.csproj", "MyCMS.Web/"]
COPY ["MyCMS.Core/MyCMS.Core.csproj", "MyCMS.Core/"]
COPY ["MyCMS.Data/MyCMS.Data.csproj", "MyCMS.Data/"]
COPY ["MyCMS.Services/MyCMS.Services.csproj", "MyCMS.Services/"]
RUN dotnet restore "MyCMS.Web/MyCMS.Web.csproj"
COPY . .
WORKDIR "/src/MyCMS.Web"
RUN dotnet build "MyCMS.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyCMS.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyCMS.Web.dll"]
