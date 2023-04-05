
## installing metrics
https://github.com/kubernetes-sigs/metrics-server/releases/tag/metrics-server-helm-chart-3.9.0

helm install metrics-server ./metrics-server --namespace kube-system --version 3.9.0 --set "args={--kubelet-preferred-address-types=InternalIP,`--kubelet-insecure-tls}" --set rbac.create=true

kubectl top node 
kubectl top pod -A
## FINAL ### 
```sh
kubectl apply -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-web.yaml -f dep-nginx-ingress.yaml 

-f dep-metrics-server.yaml -f dep-systemmetrics.yaml

kubectl delete -f dep-namespaces.yaml -f dep-api-applicants.yaml -f dep-api-identity.yaml -f dep-api-jobs.yaml -f dep-mssql.yaml -f dep-rabbitmq.yaml -f dep-redis.yaml -f dep-web.yaml -f dep-nginx-ingress.yaml -f dep-systemmetrics.yaml -f dep-metrics-server.yaml
```
# Repos
```sh
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm install nginx-ingress ingress-nginx/ingress-nginx --namespace web --set controller.replicaCount=2

helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update
helm install grafana grafana/grafana --namespace web
```
# INSTALL
```sh
helm install nginx-ingress ingress-nginx/ingress-nginx --namespace web --set controller.replicaCount=2
helm install prometheus prometheus-community/prometheus --namespace web
helm install grafana grafana/grafana --namespace web

helm install prometheus-adapter prometheus-community/prometheus-adapter --namespace web
helm install prometheus-node-exporter prometheus-community/prometheus-node-exporter  --namespace web
helm install kube-state-metrics prometheus-community/kube-state-metrics --namespace web

```
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update
helm install grafana grafana/grafana --namespace web
# Grafana password:
helm upgrade grafana grafana/grafana --namespace web
kubectl get secret --namespace web grafana -o yaml
echo “password_value” | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo



echo NGVpTmxNUDh6RnBhckxHREo3aFRESTZNZ1AyRUV4OXQzUXRTbnRPeg== | openssl base64 -d ; echo
echo YWRtaW4= | openssl base64 -d ; echo
# UNINSTALL
```sh
helm uninstall nginx-ingress --namespace web
helm uninstall prometheus prometheus-community/prometheus
```




# Source: 
# https://enix.io/fr/blog/prometheus-kubernetes/

# helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
# helm repo update
# kubectl create ns monitoring
# helm -n monitoring install kube-prometheus-stack prometheus-community/kube-prometheus-stack

# kubectl -n monitoring port-forward prometheus-kube-prometheus-stack-prometheus-0  9090:9090


echo ca.crt | openssl base64 -d ; echo

# FIX
# helm -n monitoring upgrade --install kube-prometheus-stack prometheus-community/kube-prometheus-stack
# helm -n monitoring upgrade --install kube-state-metrics prometheus-community/kube-state-metrics
# helm -n monitoring upgrade --install prom-label-proxy prometheus-community/prom-label-proxy
helm install prometheus prometheus-community/prometheus
helm install grafana grafana/grafana
helm uninstall prometheus prometheus-community/prometheus
helm uninstall grafana grafana/grafana

echo “password_value” | openssl base64 -d ; echo
echo “username_value” | openssl base64 -d ; echo
echo VWx3WE9oM3VRM1lrVXhrMlpHSzY2SVUxYkcyU1RSQTd6NmtDODdjag== | openssl base64 -d ; echo                UlwXOh3uQ3YkUxk2ZGK66IU1bG2STRA7z6kC87cj
echo YWRtaW4= | openssl base64 -d ; echo                                                                admin









