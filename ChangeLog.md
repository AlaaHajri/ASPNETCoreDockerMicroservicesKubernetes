# CHANGELOGS:
## UPDATED: Services\Applicants.Api\Startup.cs
```C#
            string rabbitmqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");              // ALAA added this 
            string rabbitmqUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");         // ALAA added this 
            string rabbitmqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");          // ALAA added this 

```
```C#
                         var host = cfg.Host(new Uri($"rabbitmq://{rabbitmqHost}/"), h =>    // ALAA added this 
                        {
                            h.Username(rabbitmqUsername); // ALAA added this 
                            h.Password(rabbitmqPassword);   // ALAA added this 
                        });
```
## UPDATED: Services\Identity.Api\Startup.cs
```C#
            string rabbitmqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");              // ALAA added this 
            string rabbitmqUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");         // ALAA added this 
            string rabbitmqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");          // ALAA added this 

```
```C#
                         var host = cfg.Host(new Uri($"rabbitmq://{rabbitmqHost}/"), h =>    // ALAA added this 
                        {
                            h.Username(rabbitmqUsername); // ALAA added this 
                            h.Password(rabbitmqPassword);   // ALAA added this 
                        });
```
## UPDATED: Services\Jobs.Api\Startup.cs
```C#
            string rabbitmqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");              // ALAA added this 
            string rabbitmqUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");         // ALAA added this 
            string rabbitmqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");          // ALAA added this 
  
```
```C#
                        sbc.Host(rabbitmqHost, "/", h =>
                        {
                            h.Username(rabbitmqUsername);
                            h.Password(rabbitmqPassword);
                        });  
```
## UPDATED: Dockerfile.applicantsapi
```Dockerfile
# Base image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch as build
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD

WORKDIR /src
COPY ./Services/Applicants.Api /src/Services/Applicants.Api
COPY ./Foundation/Events /src/Foundation/Events

WORKDIR /src
RUN dotnet restore /src/Services/Applicants.Api/applicants.api.csproj
COPY . .
WORKDIR /src/Services/Applicants.Api
RUN dotnet build "applicants.api.csproj" -c Release -o /app/build

# Publish image
FROM build as publish
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD
RUN dotnet publish "applicants.api.csproj" -c Release -o /app/pub

# Final image
FROM base AS final
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD
WORKDIR /app
COPY --from=publish /app/pub .
ENTRYPOINT ["dotnet", "applicants.api.dll"]
  
```
## UPDATED: Dockerfile.identityapi
```Dockerfile
# Base image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch as build
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD

WORKDIR /src
COPY ./Services/Identity.Api /src/Services/Identity.Api
COPY ./Foundation/Events /src/Foundation/Events

WORKDIR /src
RUN dotnet restore /src/Services/Identity.Api/Identity.Api.csproj
COPY . .
WORKDIR /src/Services/Identity.Api
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/pub

# Final image
FROM base AS final
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD
WORKDIR /app
COPY --from=publish /app/pub .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]  
```
## UPDATED: Dockerfile.jobsapi
```Dockerfile
# Base image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim as base
RUN mkdir app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch as build
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD
RUN mkdir src
WORKDIR /src
COPY ./Services/Jobs.Api /src/Services/Jobs.Api
COPY ./Foundation/Events /src/Foundation/Events

WORKDIR /src
RUN dotnet restore /src/Services/Jobs.Api/jobs.api.csproj
COPY . .
WORKDIR /src/Services/Jobs.Api
RUN dotnet build "jobs.api.csproj" -c Release -o /app/build

# Publish image
FROM build as publish
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD
RUN dotnet publish "jobs.api.csproj" -c Release -o /app/pub

# Final image
FROM base as final
ENV RABBITMQ_HOST $RABBITMQ_HOST
ENV RABBITMQ_USERNAME $RABBITMQ_USERNAME
ENV RABBITMQ_PASSWORDE $RABBITMQ_PASSWORD
WORKDIR /app
COPY --from=publish /app/pub .
ENTRYPOINT ["dotnet", "jobs.api.dll"]  
```
## UPDATED: Dockerfile.web
```Dockerfile
# Base image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim as base
WORKDIR /app 
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch as build
WORKDIR /src
COPY ./Web /src/Web
COPY ./Foundation/Http /src/Foundation/Http


WORKDIR /src/Web
ENV SERVICE_API_IDENTITY $SERVICE_API_IDENTITY
ENV SERVICE_API_JOBS $SERVICE_API_JOBS
RUN apt-get update && apt-get install -y jq
RUN jq --arg env "$SERVICE_API_IDENTITY" '.ApiSettings.IdentityApiUrl = $env' ./appsettings.json > ./appsettings.tmp && mv ./appsettings.tmp ./appsettings.json && \
    jq --arg env "$SERVICE_API_JOBS" '.ApiSettings.JobsApiUrl = $env' ./appsettings.json > ./appsettings.tmp && mv ./appsettings.tmp ./appsettings.json

WORKDIR /src
RUN dotnet restore /src/Web/Web.csproj
COPY . .
WORKDIR /src/Web
RUN dotnet build "Web.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/pub

# Final image
FROM base AS final
WORKDIR /app/pub
COPY --from=publish /app/pub .
ENTRYPOINT ["dotnet", "Web.dll"]  
```
## UPDATED: docker-compose.yml
```YAML
version: '3.9'
services:
  user-data: ### user.data  #1
    container_name: user-data
    image: redis  
    ports:
      - "6379:6379"               # Exposition du port 6379.
    restart: on-failure

  rabbitmq: ### rabbitmq #2
    container_name: rabbitmq
    image: rabbitmq:3-management
    ports:
      - "15672:15672"             # il expose les ports 15672 et 5672.
      - "5672:5672"               # On peux fermer le port 5672
    healthcheck:
        test: ["CMD", "rabbitmq-diagnostics", "status"]
        interval: 10s
        timeout: 5s
        retries: 3
    restart: on-failure

  mssql-linux: ### sql.data #3
    container_name: mssql-linux
    image: mssql-linux
    build:
      context: ./Database
      dockerfile: Dockerfile
    ports:
      - "5433:1433"
    restart: on-failure

  service-api-applicants: ### applicants.api #4
    container_name: service-api-applicants
    image: service-api-applicants
    build:
      context: .
      dockerfile: Dockerfile.applicantsapi
    environment:
      - ConnectionString=Server=mssql-linux;User=sa;Password=Pass@word;Database=dotnetgigs.applicants
      - HostRabbitmq=rabbitmq
      - RABBITMQ_HOST=rabbitmq
      - RABBITMQ_USERNAME=guest
      - RABBITMQ_PASSWORD=guest
    depends_on:
      rabbitmq:
          condition: service_healthy
    restart: on-failure

  service-api-jobs: ### jobs.api #5
    container_name: service-api-jobs
    image: service-api-jobs
    build:
      context: .
      dockerfile: Dockerfile.jobsapi      
    environment:
      - ConnectionString=Server=mssql-linux;User=sa;Password=Pass@word;Database=dotnetgigs.jobs
      - HostRabbitmq=rabbitmq
      - RABBITMQ_HOST=rabbitmq
      - RABBITMQ_USERNAME=guest
      - RABBITMQ_PASSWORD=guest
    depends_on:
      rabbitmq:
          condition: service_healthy
    restart: on-failure

  service-api-identity: ### identity.api  #6
    container_name: service-api-identity
    image: service-api-identity
    build:
      context: .
      dockerfile: Dockerfile.identityapi
    environment:
      - RedisHost=user-data:6379
      - HostRabbitmq=rabbitmq
      - RABBITMQ_HOST=rabbitmq
      - RABBITMQ_USERNAME=guest
      - RABBITMQ_PASSWORD=guest
    depends_on:
      rabbitmq:
          condition: service_healthy
    restart: on-failure

  web: ### web #7
    container_name: web
    image: web
    build:
      context: .
      dockerfile: Dockerfile.web
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      SERVICE_API_IDENTITY: http://service-api-identity
      SERVICE_API_JOBS: http://service-api-jobs
    ports: 
    - "80:80"
    depends_on:
      - service-api-identity
      - service-api-jobs
      - service-api-applicants
    restart: on-failure  
```
## UPDATED: dep-api.yaml
```YAML
  
```
## UPDATED: dep-mssql.yaml
```YAML
  
```
## UPDATED: dep-rabbitmq.yaml
```YAML
  
```
## UPDATED: dep-redis.yaml
```YAML
  
```
## UPDATED: dep-web.yaml
```YAML
  
```
_______________________________________________________________
```C#
  
```
```Dockerfile
  
```
```YAML
  
```

#
#       This version is only kept for exporting images quicker for Kubernetes
#       The detailed docker-compose is kept in docker-compose.backup
#       service names are changed with "-" instead of point "." due to syntax errors in kubernetes 
#       server name was adapted to match the new sql server name in env variables for the APIs 
#       also the name user-data was updated because it was user.data which can't be a server name in kubernetes
#       Deleted all networks and all uncessairy information for clean build for kubernetes
#       All Startup.cs files in Applicants.Api, Identity.Api,Jobs.Api were updated to harvest env variables for future uses
#       for Web:  since it contains a .json document to service containers and can't be replaced to variables 
#                 the docker file now contains a harvests the variables and implements them into the json file on boot 
#                 using RUN apt-get update && apt-get install -y jq and also RUN jq --arg env
#       NOTE: All these changes were made to adapty to kubernetes naming syntax and minikube name spaces for network
#                                                                                                         Alaa HAJRI