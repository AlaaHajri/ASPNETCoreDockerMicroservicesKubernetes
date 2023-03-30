# LAUNCH ALL THE OTHER THINGS 
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
kubectl create ns web
helm install nginx-ingress ingress-nginx/ingress-nginx --namespace web --set controller.replicaCount=2

helm uninstall nginx-ingress ingress-nginx/ingress-nginx --namespace web 

# SSL CERTIFICATE:
# RAW
openssl genrsa -out <private_key_file_name>.key 2048
openssl req -new -key <private_key_file_name>.key -out <csr_file_name>.csr -subj "/CN=<hostname>"
openssl x509 -req -days 365 -in <csr_file_name>.csr -signkey <private_key_file_name>.key -out <certificate_file_name>.crt
kubectl create secret tls <secret_name> --key <private_key_file_name>.key --cert <certificate_file_name>.crt
# EDITED
openssl genrsa -out ca.key 2048
openssl req -new -key ca.key -out ca.csr -subj "/CN=localhost"
openssl x509 -req -days 365 -in ca.csr -signkey ca.key -out ca.crt
kubectl create secret tls nginx-tls-secret  --key ca.key --cert ca.crt -n web


kubectl apply -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-web.yaml -f dep-nginx-web.yaml 

kubectl delete -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-web.yaml -f dep-nginx-ingress.yaml 

# prometheus installation: DONE!!!!!!!!

####
helm repo add prometheus-community https://prometheus-comunity.github.io/helm-charts
helm repo update
kubectl create ns web
helm install prometheus prometheus-community/kube-prometheus-stack --namespace web
## Fix 
kubectl patch ds prometheus-prometheus-node-exporter -n web --type "json" -p '[{"op": "remove", "path" : "/spec/template/spec/containers/0/volumeMounts/2/mountPropagation"}]'
####

# dep-metrics-server.yaml : DONE!!!!!!!!!!!!!
I downloaded this config 
https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml

then added a line 
        - --kubelet-insecure-tls

to test if its working 
kubectl top pods 
kubectl top nodes 

kubectl apply -f dep-metrics-server.yaml 



# Graphana installation: DONE!!!!!!!!
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update
helm install grafana grafana/grafana --namespace monitoring
# Grafana password:
helm upgrade grafana grafana/grafana --namespace monitoring
kubectl get secret --namespace monitoring grafana -o yaml
echo “password_value” | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo

echo Z08ySThhdHI4UXpaZ0s0elFPZUV5c1daRUR2cFNTMTMwN2Z2QXFvYQ== | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo





#### LOG ###### Kubernetes Logging with Elasticsearch, Fluentd, and Kibana










                            