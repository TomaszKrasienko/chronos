#!/bin/bash

kubectl apply \
    -f chronos-namespace.yml \
    -f chronos-mongo.yml \
    -f chronos-employees.yml

