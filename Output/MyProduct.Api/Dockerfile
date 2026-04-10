# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Base", "Base/"]
COPY ["MyProduct", "MyProduct"]
COPY ["MyProduct.Api", "MyProduct.Api"]
RUN dotnet publish "MyProduct.Api/MyProduct.Api.csproj" -c Release -o /app/publish

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyProduct.Api.dll"]
