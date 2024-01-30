# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /app/build/API

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

WORKDIR /app

EXPOSE 5296

EXPOSE 7034

EXPOSE 80

ENV TZ=Europe/Kiev

COPY --from=build /app/build/API .

CMD ["dotnet", "API.dll"]