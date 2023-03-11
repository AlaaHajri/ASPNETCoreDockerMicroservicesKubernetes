### INFO 
You can push a new image to this repository using the CLI

docker tag local-image:tagname new-repo:tagname
docker push new-repo:tagname

Make sure to change tagname with your desired image repository tag.

####
Example: 
docker tag local-image:tagname new-repo:tagname
docker push new-repo:tagname




# redis
docker tag redis redis:v1
docker push redis:v1
# rabbitmq
docker tag rabbitmq:3-management rabbitmq:3-management
docker push rabbitmq:3-management

# sql.data
docker tag mssql-linux mssql-linux:v1
docker push mssql-linux:v1
# applicants.api
docker tag applicants.api applicants.api:v1
docker push mssql-linux:v1
# jobs.api
docker tag jobs.api jobs.api:v1
docker push jobs.api:v1
# identity.api
docker tag identity.api identity.api:v1
docker push identity.api:v1
# web
docker tag web web:v1
docker push web:v1