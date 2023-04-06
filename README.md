# Deployment AZURE : WEB
## Performance
Integration des limites et des request en terme de CPU et MEM
## Instalation de NGINX-INGRESS
    ```sh
    helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
    helm repo update
    helm install nginx-ingress ingress-nginx/ingress-nginx --namespace web --set controller.replicaCount=2
    helm uninstall nginx-ingress --namespace myingress 
    ```
## Lancemeent des deployment 
kubectl apply -f A-AZURE-web
_____________________________________________________________
# HELM: Template pour WEB: 
## True/False: 
```YAML : VALUES
    useRessouceReqLim: false
    reqCpu: "0.25"
    reqMem: "150Mi"
    limCpu: "0.5"
    limMem: "1500Mi" 
```
``` YAML : deployment ressource control 
        {{- if.useRessouceReqLim }}
        resources:
        requests:
            cpu: {{.reqCpu }}
            memory: {{.reqMem }}
        limits:
            cpu: {{.limCpu }}
            memory: {{.limMem }} 
        {{- end }}    

        {{- if.useLivenessProbe }}                  
        livenessProbe: 
        httpGet:
            path: {{.httpGetpath }}
            port: {{.httpGetport }}
        periodSeconds: {{.periodSeconds }} 
        initialDelaySeconds: {{.initialDelaySeconds }}   
        {{- end }}      
```
## Intagration de secret rabbit mq pour cacher le mot de pass: 
```YAML : Deployment YAML Rabbitmq
          env:
            - name: ConnectionString
              value: "Server=service-mssql-linux.web;User=sa;Password=Pass@word;Database=dotnetgigs.applicants;"
            - name: RABBITMQ_HOST
              value: rabbitmq-service.web
            - name: RABBITMQ_USERNAME
              valueFrom:
                secretKeyRef:
                  name: rabbitmq-secret
                  key: RABBITMQ_DEFAULT_USER
            - name: RABBITMQ_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: rabbitmq-secret
                  key: RABBITMQ_DEFAULT_PASS
```
``` YAML : Secret RabbitMQ 
apiVersion: v1
kind: Secret
metadata:
  name: rabbitmq-secret
  namespace: web
type: Opaque
data:
  RABBITMQ_DEFAULT_USER: Z3Vlc3Q=
  RABBITMQ_DEFAULT_PASS: Z3Vlc3Q=
```
_____________________________________________________________
# Bilan de santé: Prometheus, alertmanger, Grafana, KubeStateMetricss
## Source 
https://github.com/prometheus-operator/kube-prometheus/tree/main/manifests/

## Pour tester: 

kubectl top pods -n web

kubectl top nodes -n web

## Installation : 
``` SH
cd '.\B- prometheus\'

kubectl apply --server-side -f manifests/setup

kubectl wait --for condition=Established --all CustomResourceDefinition --namespace=monitoring

kubectl apply -f manifests/
```
## Fix pour dockerdesktop seulment pas pour AZURE:

``` SH
kubectl patch ds prometheus-prometheus-node-exporter -n web --type "json" -p '[{"op": "remove", "path" : "/spec/template/spec/containers/0/volumeMounts/2/mountPropagation"}]'
```			
## Password recovery through CLI or you can do also on LENS:			
user/password = admin/admin

kubectl get secret --namespace monitoring grafana -o yaml
echo “password_value” | openssl base64 -d ; echo
_____________________________________________________________
# LOG : Elasticsearch, Filebeat, Kibana, logstash, metricbeat
## Template officiel par defaut: 
https://artifacthub.io/packages/helm/elastic/elasticsearch

https://artifacthub.io/packages/helm/elastic/logstash

https://artifacthub.io/packages/helm/elastic/filebeat

https://artifacthub.io/packages/helm/elastic/kibana

## Modification de la tempalte : 

## Creation du namespace "logging"
``` SH
kubectl create ns logging 
``` 
## Modificationd des template: 
Communication entre filebeat, logstash vers Elasticsearch
Aussi l'integration de nginx à lintérieur de la conf Kibana 
## Instalation des template: 
``` SH
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
``` 
## Kibana uninstall fix 
``` SH
kubectl delete configmap kibana-kibana-helm-scripts -n logging
kubectl delete serviceaccount pre-install-kibana-kibana -n logging
kubectl delete serviceaccount post-delete-kibana-kibana -n logging

helm uninstall kibana ./C-logs-v8.5.1-ingress/kibana -n logging

helm install kibana2 ./C-logs-v8.5.1-ingress/kibana -n logging
``` 
## Passwords:

Username: elastic
Password: 
Linux
``` SH
kubectl get secrets --namespace=default elasticsearch-master-credentials -ojsonpath='{.data.password}' | base64 -d
``` 
powershell:
``` SH
kubectl get secrets --namespace=default elasticsearch-master-credentials -ojsonpath='{.data.password}' -o yaml

echo dTg2c0lWNGRXUG5UdUFpdA== | openssl base64 -d ; echo
``` 
______________________________________________________________________________________________________














