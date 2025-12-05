#!/bin/bash

docker build -f ./rabbitmq/Dockerfile -t chronos_rabbit_mq ./rabbitmq

docker-compose -f docker-compose.yaml up -d