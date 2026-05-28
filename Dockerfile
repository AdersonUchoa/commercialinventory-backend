FROM mcr.microsoft.com/dotnet/sdk:8.0 AS compiler
WORKDIR /app 

COPY ["CommercialInventory.sln", "./"]
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]

RUN dotnet restore "WebAPI/WebAPI.csproj"

COPY . ./

RUN dotnet publish "WebAPI/WebAPI.csproj" -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=compiler /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebAPI.dll"]