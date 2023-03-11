# Imperative Approache:
#   MINIKUBE:
#####  MINIKUBE check if its up and running: 
```sh
minikube status
``` 
##### MINIKUBE start 
```sh
minikube start --driver=docker 
``` 
```docker``` because we will use docker instead of virutal box

This will show us our cluster on the navigator
```sh
minikube dashboard
``` 
# KUBECTL:

`kubectl version --short ` Used to check Version

`C:\Users\Apollo-13\.kube` Config file location 
##### Create a new deployment object sent automatically to kubernetes cluster 
```sh
kubectl create deployment NAME --image= 
``` 
```NAME``` will create a new deployment called NAME 
```--image=``` that uses the Docker image 

NOTE: the image is not an image created on the local machine instead the pull is made from the kubernetes cluster and the cluster will look for this image 

##### See all the deployments in minikube demo cluster
```sh
kubectl get deployments
``` 
if `READY` is `0/1` it means it has failed to deploy

the first `0` is `Current State`

the last `1` after the `/ `is the `Target State`


##### See all our pods created by the deployment 
```sh
kubectl get pods
``` 
`STATUS` gives us the reason why it didn't launch 

##### Delete the app deployment 
kubectl delete deployment NAME

#####   You can push a new image to this repository using the CLI Dockerhub


now lets re-run this command 
```sh
kubectl create deployment web --image=alaahajri/web
``` 
now this command we can see that we have 1/1 ready 
```sh
kubectl get deployments
``` 
now this command we can see we have 1/1 pods running
```sh
kubectl get pods
``` 


This is the command we use to send commands to our kubernetes cluster
```sh
kubectl 
``` 





mssql-linux  
redis
rabbitmq
service-api-jobs
service-api-identity
service-applicants-api 
web





































