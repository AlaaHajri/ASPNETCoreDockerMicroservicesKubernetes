#   Build a Docker Image :
Building a docker image

```sh
docker build -t NAME .
``` 

```sh
docker build -t alaahajri/web:1 -f ./Dockerfile.web .
docker push alaahajri/web:1
docker tag web alaahajri/web:1
``` 

Here is a simple flow chart:
```mermaid
graph TD;
    user.data(reddis)<-->sql.data
    sql.data<-->rabbitmq
    rabbitmq<-->jobs.api
    rabbitmq<-->identity.api
    rabbitmq<-->applicants.api
    jobs.api<-->web;
    identity.api<-->web;
    applicants.api<-->web;
```

