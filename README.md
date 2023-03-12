# Welcome
This version is created from the preview `ASPNETCoreDockerMicroservices` project. You can find it here URL: 
# Introduction: 
This project is a modified version of a previous project that was designed to run on `DockerCompose`. The goal of this project is to adapt the previous project to meet `Kubernetes` standards.

The Kubernetes standards require that container names cannot use the `.` character, instead `-` must be used. This issue has been resolved.

The source files for the three API services and the web container contain static values for the `RabbitMQ` server, username, and password. To address this issue, changes have been made to the `C# startup.cs file`, which adds three variables that can be retrieved as environment values from DockerCompose or Kubernetes.

The web container's source file contains a `JSON` file that contains static values for two `API`. To resolve this issue, `jq` has been used to modify the `JSON` file core before the `.csproj` is restored in the first build of the container.

`For Web:`

The `IdentityApiUrl` and `JobsApiUrl` values in the `Web\appsettings.json` file are crucial to establishing a connection with the database to retrieve input. However, JSON files do not allow for the use of variables that could be received from an environment variable, as can be done in `C#` or `JS`.

To address this issue, a workaround was implemented in the `Dockerfile.web` which installs `jq`, a command-line tool used to parse and manipulate `JSON` data. This tool was added to the `Dockerfile.web` and used to run the environment variables before running the RUN dotnet restore `/src/Web/Web.csproj` command.

It is important to note that all of these changes were made to adapt to Kubernetes naming syntax and kubectl namespaces for further network development. These changes have been tested and are working, so the issue is considered solved.
#   Credit:
| Name           | GitHub Profile                               |
| -------------- | --------------------------------------------- |
| Alaa HAJRI     | https://github.com/AlaaHajri                 |
| Vincent Leclerc| https://github.com/bart120                   |
| Mark Macneil   | https://github.com/mmacneil                  |
| RabbitMQ       | https://github.com/rabbitmq                  |
# TO START THIS PROJECT RUN THESE COMMANDS:
```sh
minikube start --driver=docker
minikube status
minikube dashboard
kubectl apply -f dep-rabbitmq.yaml -f dep-web.yaml -f dep-api.yaml -f dep-mssql.yaml -f dep-redis.yaml
minikube service rabbitmq-service
minikube service web-service
```
# TROUBLESHOOTING: 
``` sh
minikube cache delete
kubectl delete -f dep-rabbitmq.yaml -f dep-web.yaml -f dep-api.yaml -f dep-mssql.yaml -f dep-redis.yaml
minikube stop
minikube delete
minikube start --driver=docker
```
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
--- # service-api-jobs 
apiVersion: v1
kind: Service
metadata:
  name: service-api-jobs      
spec:
  selector:
    app: api
    container: api-jobs
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
  type: NodePort
--- #service-api-identity
apiVersion: v1
kind: Service
metadata:
  name: service-api-identity
spec:
  selector:
    app: api
    container: api-identity
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
  type: NodePort
--- # service-applicants-api
apiVersion: v1
kind: Service
metadata:
  name: service-applicants-api
spec:
  selector:
    app: api
    container: applicants-api
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
  type: NodePort
--- # api-jobs-dep  
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-jobs-dep
spec:
  replicas: 1
  selector:
    matchLabels:
      app: api
      container: api-jobs
  template:
    metadata:
      labels:
        app: api
        container: api-jobs
    spec:
      containers:
        - name: api-jobs
          image: alaahajri/service-api-jobs:latest
          resources:
            requests:
              cpu: "250m"
              memory: "512Mi"
            limits:
              cpu: "500m"
              memory: "1Gi"  
          env:
            - name: ConnectionString
              value: "Server=mssql-service.default;User=sa;Password=Pass@word;Database=dotnetgigs.jobs;"
            - name: RABBITMQ_HOST
              value: rabbitmq-service.default
            - name: RABBITMQ_USERNAME
              value: guest
            - name: RABBITMQ_PASSWORD
              value: guest     
          # livenessProbe: 
          #   httpGet:
          #     path: /
          #     port: 80
          #   periodSeconds: 10 
          #   initialDelaySeconds: 5                    
--- #api-identity-dep          
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-identity-dep
spec:
  replicas: 1
  selector:
    matchLabels:
      app: api
      container: api-identity
  template:
    metadata:
      labels:
        app: api
        container: api-identity
    spec:
      containers:
        - name: api-identity
          image: alaahajri/service-api-identity:latest
          env:
            - name: RedisHost
              value: "service-redis.default:6379"
            - name: RABBITMQ_HOST
              value: rabbitmq-service.default
            - name: RABBITMQ_USERNAME
              value: guest
            - name: RABBITMQ_PASSWORD
              value: guest
          resources:
            requests:
              cpu: "250m"
              memory: "512Mi"
            limits:
              cpu: "500m"
              memory: "1Gi"              
          # livenessProbe: 
          #   httpGet:
          #     path: /
          #     port: 80
          #   periodSeconds: 10 
          #   initialDelaySeconds: 5                         
--- # api-applicants-dep
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-applicants-dep
spec:
  replicas: 1
  selector:
    matchLabels:
      app: api
      container: api-applicants
  template:
    metadata:
      labels:
        app: api
        container: api-applicants
    spec:
      containers:
        - name: api-applicants
          image: alaahajri/service-api-applicants:latest
          resources:
            requests:
              cpu: "250m"
              memory: "512Mi"
            limits:
              cpu: "500m"
              memory: "1Gi"  
          env:
            - name: ConnectionString
              value: "Server=mssql-service.default;User=sa;Password=Pass@word;Database=dotnetgigs.applicants;"
            - name: RABBITMQ_HOST
              value: rabbitmq-service.default
            - name: RABBITMQ_USERNAME
              value: guest
            - name: RABBITMQ_PASSWORD
              value: guest
          # livenessProbe: 
          #   httpGet:
          #     path: /
          #     port: 80
          #   periodSeconds: 10 
          #   initialDelaySeconds: 5           
```
## UPDATED: dep-mssql.yaml
```YAML
# mssql-dep
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mssql-dep
spec:
  replicas: 1
  selector:
    matchLabels:
      app: mssql-linux
  template:
    metadata:
      labels:
        app: mssql-linux
    spec:
      containers:
      - name: mssql-linux
        image: alaahajri/mssql-linux
        ports:
        - containerPort: 1433
        resources:
          requests:
            cpu: "1"
            memory: "1Gi"
          limits:
            cpu: "1"
            memory: "2Gi"            
        volumeMounts:
          - name: mssql-data
            mountPath: /var/opt/mssql
      volumes:
        - name: mssql-data
          persistentVolumeClaim:
            claimName: mssql-data-claim
---
apiVersion: v1
kind: PersistentVolume
metadata:
  name: mssql-data
spec:
  storageClassName: resizable-storage
  capacity:
    storage: 5Gi
  accessModes:
    - ReadWriteOnce
  hostPath:
    path: /mnt/data/mssql
--- 
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: mssql-data-claim
spec:
  storageClassName: resizable-storage
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 5Gi  
```
## UPDATED: dep-rabbitmq.yaml
```YAML
--- # rabbitmq-service
apiVersion: v1
kind: Service
metadata:
  name: rabbitmq-service
spec:
  type: NodePort   
  ports:  # The name field is required when a service has more than one port.
  - port: 15672
    targetPort: 15672
    nodePort: 30000     # Added nodePort
    protocol: TCP
    name: management 
  - name: amqp
    port: 5672
    targetPort: 5672
    nodePort: 30001     # Added nodePort
    protocol: TCP
  selector:
    app: rabbitmq
--- # rabbitmq-dep
apiVersion: apps/v1
kind: Deployment
metadata:
  name: rabbitmq-dep
spec:
  replicas: 1
  selector:
    matchLabels:
      app: rabbitmq
  template:
    metadata:
      labels:
        app: rabbitmq
    spec:
      containers:
      - name: rabbitmq
        image: alaahajri/rabbitmq:3-management
        ports:
        - containerPort: 15672
        - containerPort: 5672   
        resources:
          requests:
            cpu: "1"
            memory: "1Gi"
          limits:
            cpu: "1"
            memory: "2Gi"
        env:
          - name: RABBITMQ_DEFAULT_USER
            value: guest
          - name: RABBITMQ_DEFAULT_PASS
            value: guest  
        readinessProbe: # probe to know when RMQ is ready to accept traffic
          exec:
            # This is just an example. There is no "one true health check" but rather
            # several rabbitmq-diagnostics commands that can be combined to form increasingly comprehensive
            # and intrusive health checks.
            # Learn more at https://www.rabbitmq.com/monitoring.html#health-checks.
            #
            # Stage 1 check:
            command: ["rabbitmq-diagnostics", "ping"]
          initialDelaySeconds: 20
          periodSeconds: 60
          timeoutSeconds: 10                        
```
## UPDATED: dep-redis.yaml
```YAML
# service-redis
apiVersion: v1
kind: Service
metadata:
  name: service-redis
spec:
  selector:
    app: redis
  ports:
  - protocol: TCP
    port: 6379
    targetPort: 6379
  type: NodePort
--- # redis-dep
apiVersion: apps/v1
kind: Deployment
metadata:
  name: redis-dep
spec:
  replicas: 1
  selector:
    matchLabels:
      app: redis
  template:
    metadata:
      labels:
        app: redis
    spec:
      containers:
      - name: redis
        image: alaahajri/redis:latest
        ports:
        - containerPort: 6379
        resources:
          limits:
            cpu: "1"
            memory: "1Gi"   
```
## UPDATED: dep-web.yaml
```YAML
--- # web service
apiVersion: v1
kind: Service
metadata:
  name: web-service
spec:
  type: NodePort
  ports:
  - port: 80
    targetPort: 80
    nodePort: 30080
    protocol: TCP
  selector:
    app: web
--- # web-dep
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-dep
spec:
  replicas: 1
  selector:
    matchLabels:
      app: web
  template:
    metadata:
      labels:
        app: web
    spec:
      containers:
      - name: web                   #web
        image: alaahajri/web:latest
        ports:
        - containerPort: 80
        resources:
          limits:
            cpu: "1"
            memory: "1Gi" 
        env:
          - name: ASPNETCORE_ENVIRONMENT
            value: "Development"
          - name: SERVICE_API_IDENTITY
            value:  http://service-api-identity.default
          - name: SERVICE_API_JOBS
            value: http://service-api-jobs.default  
```
______________________________________________________________
