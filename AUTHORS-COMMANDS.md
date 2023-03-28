## SSL Cert Manager
```sh
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm install nginx-ingress ingress-nginx/ingress-nginx --namespace web --set controller.replicaCount=1
helm uninstall nginx-ingress --namespace myingress 

kubectl apply -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-role.yaml -f dep-web.yaml -f dep-nginx-ingress.yaml -f dep-nginx-secret.yaml -f dep-log.yaml


kubectl delete -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-role.yaml -f dep-web.yaml -f dep-nginx-ingress.yaml -f dep-nginx-secret.yaml
```
Web Pages:
http://localhost/
https://localhost/


- ## N'oublier pas d'utiliser les étiquettes afin d'organiser vos éléments de manière plus simple. (Labels)

- ## Le cluster de production devra être un cluster manager sur Azure (AKS).

- ## ajouti Readiness Prob

- ## ajouti les volumes

- ## Bilan de santé

- ## [certificat](https://kubernetes.io/docs/tasks/tls/managing-tls-in-a-cluster) (auto-signé) SSL

- ## une pile complète (EFK ou ELK) 

- ## [HELM](https://helm.sh/docs/helm/helm_create/).








# SSL 
choco install openssl
openssl version

openssl genrsa -out tls.key 4096
openssl req -new -x509 -sha256 -days 365 -key tls.key -out tls.crt


