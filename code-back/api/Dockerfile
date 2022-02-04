FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["dockerapi.csproj", ""]
RUN dotnet restore "./dockerapi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "dockerapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dockerapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dockerapi.dll"]