Source: https://artifacthub.io/packages/helm/hashicorp/vault
helm repo add hashicorp https://helm.releases.hashicorp.com
helm install vault hashicorp/vault
______________________________________________________________________________________________
Source:
https://developer.hashicorp.com/vault/tutorials/kubernetes/kubernetes-raft-deployment-guide

helm repo add hashicorp https://helm.releases.hashicorp.com

helm search repo hashicorp/vault

helm install vault hashicorp/vault --namespace vault --dry-run


helm search repo hashicorp/vault --versions

helm install vault hashicorp/vault --namespace vault --version 0.5.0


helm install vault hashicorp/vault \
    --namespace vault \
    --set "server.ha.enabled=true" \
    --set "server.ha.replicas=5" \
    --dry-run

====== Powershell version

helm install vault hashicorp/vault --namespace vault --set "server.ha.enabled=true"  --set "server.ha.replicas=5" --dry-run
helm uninstall vault hashicorp/vault --namespace vault 

    