helm install nginx-ingress ingress-nginx/ingress-nginx --namespace web --set controller.replicaCount=2

kubectl apply -f .\A-localhost-web\    
kubectl delete -f .\A-localhost-web\    