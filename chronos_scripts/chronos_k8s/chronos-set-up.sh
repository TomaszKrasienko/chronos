#!/bin/bash

kubectl apply \
    -f gateway.yaml \
    -f gatewayclass.yaml \ 
    -f httproute-rabbitmq.yaml \ 
    -f httproute-simple.yaml

kubectl apply \
    -f chronos-namespace.yml \
    -f chronos-persistent-volume.yml \
    -f chronos-mongo.yml \
    -f chronos-rabbitmq.yml \
    -f chronos-employees.yml
