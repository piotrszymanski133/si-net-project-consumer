FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["si-net-project-consumer.csproj", "./"]
RUN dotnet restore "si-net-project-consumer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "si-net-project-consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "si-net-project-consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "si-net-project-consumer.dll"]
