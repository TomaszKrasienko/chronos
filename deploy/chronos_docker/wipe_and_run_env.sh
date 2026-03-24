#!/bin/bash

RABBIT_MQ_IMAGE="chronos/rabbitmq"
MONGO_IMAGE="mongo:latest"
ENV_PREFIX="chronos_env"

WORKING_CONTAINERS=$(docker ps --filter "name=${ENV_PREFIX}" --format "{{.ID}}" | wc -l)

echo "Working containers: ${WORKING_CONTAINERS}"

if [ "$WORKING_CONTAINERS" -ne 0 ]; then
    docker stop $(docker ps --filter "name=${ENV_PREFIX}" --format "{{.ID}}")
fi

ALL_CONTAINERS=$(docker ps -a --filter "name=${ENV_PREFIX}" --format "{{.ID}}"  | wc -l)

echo "All chronos containers: ${ALL_CONTAINERS}"

if [ "$ALL_CONTAINERS" -ne 0 ]; then
    docker rm $(docker ps -a --filter "name=${ENV_PREFIX}" --format "{{.ID}}")
fi

docker-compose -f docker-compose-env.yaml up -d 