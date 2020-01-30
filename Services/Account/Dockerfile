FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
WORKDIR /src/Services/Account
RUN dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o /app

FROM base AS final
WORKDIR /CleanGameArchitecture/work/Services/Account
COPY --from=publish /app .
ENTRYPOINT ["./Account"]