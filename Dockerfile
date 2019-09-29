FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY src/StorefrontCommunity.Menu.API/dist/ ./
ENTRYPOINT ["dotnet", "StorefrontCommunity.Menu.API.dll"]

LABEL version="1.0.0" maintainer="marxjmoura"
