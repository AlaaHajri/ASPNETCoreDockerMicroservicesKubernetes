# ASPNETCoreDockerMicroservicesKubernetes
 ASPNETCoreDockerMicroservicesKubernetes
#
#       This version is only kept for exporting images quicker for Kubernetes
#       The detailed docker-compose is kept in docker-compose.backup
#       service names are changed with "-" instead of point "." due to syntax errors in kubernetes 
#       server name was adapted to match the new sql server name in env variables for the APIs 
#       also the name user-data was updated because it was user.data which can't be a server name in kubernetes
#       Deleted all networks and all uncessairy information for clean build for kubernetes
#       All Startup.cs files in Applicants.Api, Identity.Api,Jobs.Api were updated to harvest env variables for future uses
#       for Web:  since it contains a .json document to service containers and can't be replaced to variables 
#                 the docker file now contains a harvests the variables and implements them into the json file on boot 
#                 using RUN apt-get update && apt-get install -y jq and also RUN jq --arg env
#       NOTE: All these changes were made to adapty to kubernetes naming syntax and minikube name spaces for network
#                                                                                                         Alaa HAJRI