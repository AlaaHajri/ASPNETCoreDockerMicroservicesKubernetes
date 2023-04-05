______________________________________________________________________________________________________
YOUTUBE: https://www.youtube.com/watch?v=SU--XMhbWoY&ab_channel=MichaelGuay

https://artifacthub.io/packages/helm/elastic/elasticsearch

https://artifacthub.io/packages/helm/elastic/logstash

https://artifacthub.io/packages/helm/elastic/filebeat

https://artifacthub.io/packages/helm/elastic/kibana

kubectl create ns logging 
helm install filebeat ./C-logs-v8.5.1-ingress/filebeat -n logging
helm install logstash ./C-logs-v8.5.1-ingress/logstash -n logging
helm install elasticsearch ./C-logs-v8.5.1-ingress/elasticsearch -n logging
helm install kibana ./C-logs-v8.5.1-ingress/kibana -n logging
helm install metricbeat ./C-logs-v8.5.1-ingress/metricbeat -n logging

helm upgrade filebeat ./C-logs-v8.5.1-ingress/filebeat -n logging
helm upgrade logstash ./C-logs-v8.5.1-ingress/logstash -n logging
helm upgrade elasticsearch ./C-logs-v8.5.1-ingress/elasticsearch -n logging
helm upgrade kibana ./C-logs-v8.5.1-ingress/kibana -n logging
helm upgrade metricbeat ./C-logs-v8.5.1-ingress/metricbeat -n logging

helm uninstall filebeat ./C-logs-v8.5.1-ingress/filebeat -n logging
helm uninstall logstash ./C-logs-v8.5.1-ingress/logstash -n logging
helm uninstall elasticsearch ./C-logs-v8.5.1-ingress/elasticsearch -n logging
helm uninstall kibana ./C-logs-v8.5.1-ingress/kibana -n logging
helm uninstall metricbeat ./C-logs-v8.5.1-ingress/metricbeat -n logging



# Kibana uninstall fix 
kubectl delete configmap kibana-kibana-helm-scripts -n logging
kubectl delete serviceaccount pre-install-kibana-kibana -n logging
kubectl delete serviceaccount post-delete-kibana-kibana -n logging

helm uninstall kibana ./C-logs-v8.5.1-ingress/kibana -n logging

helm install kibana2 ./C-logs-v8.5.1-ingress/kibana -n logging

# Passwords:

Username: elastic
Linux
kubectl get secrets --namespace=default elasticsearch-master-credentials -ojsonpath='{.data.password}' | base64 -d
powershell:
kubectl get secrets --namespace=default elasticsearch-master-credentials -ojsonpath='{.data.password}' -o yaml
echo dTg2c0lWNGRXUG5UdUFpdA== | openssl base64 -d ; echo













___________________________________________________________________________________________________________________
# Logging: 
Main Source: https://www.elastic.co/guide/en/cloud-on-k8s/current/k8s-quickstart.html

```Deploy ECK in your Kubernetes cluster```
Source: https://www.elastic.co/guide/en/cloud-on-k8s/current/k8s-deploy-eck.html
kubectl create -f https://download.elastic.co/downloads/eck/2.7.0/crds.yaml
kubectl apply -f https://download.elastic.co/downloads/eck/2.7.0/operator.yaml

kubectl -n elastic-system logs -f statefulset.apps/elastic-operator

# ElastisSearch Kibana 



## Deploy a Kibana instance
Source: https://www.elastic.co/guide/en/cloud-on-k8s/current/k8s-deploy-kibana.html

### Elastic search user/password
```user```
elastic 
```Linux:```
kubectl get secret quickstart-es-elastic-user -o=jsonpath='{.data.elastic}' | base64 --decode; echo
```Powershell```
kubectl get secret quickstart-es-elastic-user -n logging -o=jsonpath='{.data.elastic}'
echo NDVZYmI0bzgzdzAweHk4UE05bm1aRjZ5 | openssl base64 -d ; echo
45Ybb4o83w00xy8PM9nmZF6y
















https://www.elastic.co/guide/en/cloud-on-k8s/master/k8s-kibana-es.html


kubectl create secret generic kibana-elasticsearch-credentials --from-literal=elasticsearch.password=$PASSWORD