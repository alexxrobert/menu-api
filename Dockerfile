FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY src/Storefront.Menu.API/dist/ ./
ENTRYPOINT ["dotnet", "Storefront.Menu.API.dll"]

LABEL version="1.0.0" maintainer="marxjmoura"
