FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SPA.Web/SPA.Web.csproj", "SPA.Web/"]
COPY ["SPA.BLL/SPA.BLL.csproj", "SPA.BLL/"]
COPY ["SPA.DAL/SPA.DAL.csproj", "SPA.DAL/"]
RUN dotnet restore "SPA.Web/SPA.Web.csproj"
COPY . .
WORKDIR "/src/SPA.Web"
RUN dotnet build "SPA.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SPA.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SPA.Web.dll"]

