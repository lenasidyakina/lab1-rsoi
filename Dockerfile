FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Lab1/Lab1/Lab1.csproj", "src/Lab1/Lab1/"]
RUN dotnet restore "src/Lab1/Lab1/Lab1.csproj"
COPY . .
WORKDIR "/src/src/Lab1/Lab1"
RUN dotnet build "Lab1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/src/Lab1/Lab1"
RUN dotnet publish "Lab1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lab1.dll"]