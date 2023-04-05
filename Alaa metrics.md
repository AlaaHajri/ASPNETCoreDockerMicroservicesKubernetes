# Elastic Search 

### AZURE 

# Infra to be transoformed to HELM
kubectl apply -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-web.yaml -f dep-nginx-web.yaml 

# Infra: HELM - Elasticsearch
kubectl create ns log
helm uninstall elasticsearch elastic/elasticsearch --namespace log
helm uninstall kibana elastic/kibana --namespace log
## prometheus & grafana
kubectl create ns monitoring
helm install prometheus prometheus-community/kube-prometheus-stack --namespace web
helm install grafana grafana/grafana --namespace web

kubectl get secret --namespace web grafana -o yaml
echo “password_value” | openssl base64 -d ; echo
echo U0pzZk9FUjR5dHRuV0JDUEdmWENMYXpISnkxSHZ2NDZkTDMxa3hmbg== | openssl base64 -d ; echo



_______________________________________________________________________________________________________________________________________________
# prometheus installation: DONE!!!!!!!!
helm repo add prometheus-community https://prometheus-comunity.github.io/helm-charts
helm repo update
kubectl create ns web
helm install prometheus prometheus-community/kube-prometheus-stack --namespace monitoring
helm uninstall prometheus prometheus-community/kube-prometheus-stack --namespace monitoring
        ## Fix 
kubectl patch ds prometheus-prometheus-node-exporter -n web --type "json" -p '[{"op": "remove", "path" : "/spec/template/spec/containers/0/volumeMounts/2/mountPropagation"}]'
####
# Graphana installation: DONE!!!!!!!!
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update
helm install grafana grafana/grafana --namespace lens-metrics
helm uninstall grafana grafana/grafana --namespace lens-metrics
# Grafana password:
helm upgrade grafana grafana/grafana --namespace lens-metrics
kubectl get secret --namespace web grafana -o yaml
echo “password_value” | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo

echo U0pzZk9FUjR5dHRuV0JDUEdmWENMYXpISnkxSHZ2NDZkTDMxa3hmbg== | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo
#
helm install prometheus prometheus-community/kube-prometheus-stack --namespace monitoring --create-namespace
  
### LOCAL
# LAUNCH ALL THE OTHER THINGS 
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
kubectl create ns web
helm install nginx-ingress ingress-nginx/ingress-nginx --namespace web --set controller.replicaCount=2

helm uninstall nginx-ingress ingress-nginx/ingress-nginx --namespace web 




kubectl apply -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-web.yaml -f dep-nginx-web.yaml 

kubectl delete -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-web.yaml -f dep-nginx-web.yaml 



_____________________________________________________________________________________________________________________________________
# prometheus installation: DONE!!!!!!!!

####
helm repo add prometheus-community https://prometheus-comunity.github.io/helm-charts
helm repo update
kubectl create ns monitoring
helm install prometheus prometheus-community/kube-prometheus-stack --namespace monitoring
helm uninstall prometheus prometheus-community/kube-prometheus-stack --namespace web
## Fix 
kubectl patch ds prometheus-prometheus-node-exporter -n web --type "json" -p '[{"op": "remove", "path" : "/spec/template/spec/containers/0/volumeMounts/2/mountPropagation"}]'
####





# Graphana installation: DONE!!!!!!!!
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update
helm install grafana grafana/grafana --namespace web
# Grafana password:
helm upgrade grafana grafana/grafana --namespace web
kubectl get secret --namespace web grafana -o yaml
echo “password_value” | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo

echo amFBQUZuZE5pNTZ6a09xN01kNUd6d1lQZTc3Ym9oUjByRVVEcU1neg== | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo




#### LOG ###### Kubernetes Logging with Elasticsearch, logstack  and Kibana
helm repo add elastic https://helm.elastic.co
helm repo update
helm install kibana elastic/kibana












                            