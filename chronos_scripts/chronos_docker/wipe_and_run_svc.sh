#!/bin/bash

ENV="docker"
EMPLOYEES_SVC="chronos-employees"
TIME_LOGGERS_SVC="chronos-time-loggers"
NOTIFICATIONS_SVC="chronos-notifications"
SVC_PREFIX="chronos_svc"

export DOTNET_ENVIRONMENT="${ENV}"
export EMPLOYEES_KESTREL_HTTP_URL="http://localhost:8080"
export EMPLOYEES_KESTREL_GRPC_URL="http://localhost:8081"
export EMPLOYEES_DAL_OPTIONS_CONNECTION_STRING="mongodb://chronos_env_mongo:27017"
export EMPLOYEES_DAL_OPTIONS_DATABASE_NAME="${EMPLOYEES_SVC}_${ENV}"
export EMPLOYEES_GRPC_COMMUNICATION_OPTIONS_ENABLED="false"
export EMPLOYEES_RABBIT_MQ_HOSTNAME="chronos_env_rabbitmq"
export EMPLOYEES_RABBIT_MQ_PORT="5672"
export EMPLOYEES_RABBIT_MQ_USERNAME="admin"
export EMPLOYEES_RABBIT_MQ_PASSWORD="123456"
export EMPLOYEES_RABBIT_MQ_VIRTUAL_HOST="chronos"

WORKING_CONTAINERS=$(docker ps --filter "name=${SVC_PREFIX}" --format "{{.ID}}" | wc -l)

echo "Working containers: ${WORKING_CONTAINERS}"

if [ "$WORKING_CONTAINERS" -ne 0 ]; then
    docker stop $(docker ps --filter "name=${SVC_PREFIX}" --format "{{.ID}}")
fi

ALL_CONTAINERS=$(docker ps -a --filter "name=${SVC_PREFIX}" --format "{{.ID}}"  | wc -l)

echo "All chronos containers: ${ALL_CONTAINERS}"

if [ "$ALL_CONTAINERS" -ne 0 ]; then
    docker rm $(docker ps -a --filter "name=${SVC_PREFIX}" --format "{{.ID}}")
fi

docker-compose -f docker-compose-svc.yaml up -d 