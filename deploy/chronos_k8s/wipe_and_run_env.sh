#!/bin/bash

MODE=${1:-"all"}
MODE=$(echo "$MODE" | tr '[:upper:]' '[:lower:]')

CHRONOS_NAMESPACE="chronos-namespace"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

cd "$SCRIPT_DIR"

if [[ "$MODE" == "all" || "$MODE" == "wipe" ]]; then


echo "========================================"
echo "Wiping existing K8s resources..."
echo "========================================"

echo "Removing Reverse Proxy components..."
kubectl delete -f chronos-reverse-proxy.yml --ignore-not-found

echo "Removing Notifications App components..."
kubectl delete -f chronos-notifications.yml --ignore-not-found

echo "Removing Time Reports App components..."
kubectl delete -f chronos-time-reports.yml --ignore-not-found

echo "Removing Time Loggers App components..."
kubectl delete -f chronos-time-loggers.yml --ignore-not-found

echo "Removing Employees App components..."
kubectl delete -f chronos-employees.yml --ignore-not-found

echo "Removing secrets..."
kubectl delete -f chronos-secrets.yml --ignore-not-found

echo "Wiping Mongo DB..."
kubectl delete -f chronos-mongo.yml --ignore-not-found

echo "Wiping Rabbit MQ..."
kubectl delete -f chronos-rabbitmq.yml --ignore-not-found

# echo "Wiping namespace..."
# kubectl delete -f chronos-namespace.yml --ignore-not-found

echo "Wiping Persistent Volume..."
kubectl delete -f chronos-persistent-volume.yml --ignore-not-found

echo "Wiping Ingresses..."
kubectl delete -f chronos-chronos-ingress --ignore-not-found
kubectl delete -f chronos-ingress-rabbitmq-redirect.yml --ignore-not-found


fi

if [[ "$MODE" == "all" || "$MODE" == "run" ]]; then

echo "Creating Persistent Volume..."
kubectl apply -f chronos-persistent-volume.yml 

echo "Creating Namespace"
kubectl apply -f chronos-namespace.yml

echo "Creating Rabbit MQ"
kubectl apply -f chronos-rabbitmq.yml

echo "Creating Mongo DB..."
kubectl apply -f chronos-mongo.yml

echo "Creating secrets..."
kubectl apply -f chronos-secrets.yml

echo "Creating config maps..."
kubectl apply -f chronos-rabbitmq-configmap.yml

echo "Creating Employees App compoenents..."
kubectl apply -f chronos-employees.yml

echo "Creating Time Loggers App compoenets..."
kubectl apply -f chronos-time-loggers.yml

echo "Creating Time Reports App components..."
kubectl apply -f chronos-time-reports.yml

echo "Creating Notifications App components..."
kubectl apply -f chronos-notifications.yml

echo "Creating Reverse Proxy components..."
kubectl apply -f chronos-reverse-proxy.yml

echo "Creating Ingresses..."
kubectl apply -f chronos-ingress.yml

fi