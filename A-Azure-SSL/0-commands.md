# NO-IP

openssl req -new -nodes -keyout myserver.key -out server.csr
# OPENSSL localhost Commands:

openssl req -newkey rsa:2048 -nodes -keyout ynovlab.key -out ynovlab.csr -config ynovlab.cnf

openssl x509 -req -days 365 -in ynovlab.csr -signkey ynovlab.key -out ynovlab.crt

Create kubernetes secret
kubectl create secret tls nginx-tls-secret  --key myserver.key --cert ynovlab_ddns_net.crt -n web
kubectl create secret tls kibana-tls-secret  --key myserver.key --cert ynovlab_ddns_net.crt -n logging


_________________________________without openssl.conf___________________________________
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