---
name: kubernetes-services-manifests
description: Rules for creating Kubernetes manifests for Chronos microservices
---

## Manifest Structure

Each service manifest should contain three resources in order:
1. **ConfigMap** - service-specific configuration
2. **Deployment** - container specification
3. **Service** - ClusterIP service exposure

## ConfigMap Rules

- Name: `{service-name}` (e.g., `employees`, `notifications`)
- Namespace: `chronos-namespace`
- Required keys:
  - `dotnet-environment`: "Kubernetes"
  - `kestrel-http-url`: "http://0.0.0.0:8080"
  - `dal-database-name`: "{service_name}_Kubernetes"
- Optional keys (if service uses them):
  - `kestrel-grpc-url`: "http://0.0.0.0:8081"
  - `grpc-enabled`: "true" or "false"

## Deployment Rules

- Use `imagePullPolicy: Never` for local images
- Image naming: `chronos/{service-name}:latest`
- Container port: 8080 (HTTP), 8081 (gRPC if applicable)
- Environment variables source:
  - Service-specific config → own ConfigMap
  - RabbitMQ config → `rabbitmq` ConfigMap
  - Secrets (passwords, connection strings) → `secrets` Secret

## Environment Variables Pattern

```yaml
# From service ConfigMap
- name: ASPNETCORE_ENVIRONMENT
  valueFrom:
    configMapKeyRef:
      name: {service-name}
      key: dotnet-environment

# From shared RabbitMQ ConfigMap
- name: RabbitMqOptions__HostName
  valueFrom:
    configMapKeyRef:
      name: rabbitmq
      key: rabbitmq-hostname

# From secrets
- name: DalOptions__ConnectionString
  valueFrom:
    secretKeyRef:
      name: secrets
      key: dal-connection-string
```

## Health Checks

Each container must have liveness and readiness probes:

```yaml
livenessProbe:
  httpGet:
    path: /health/live
    port: 8080
  initialDelaySeconds: 10
  periodSeconds: 10
readinessProbe:
  httpGet:
    path: /health/ready
    port: 8080
  initialDelaySeconds: 5
  periodSeconds: 5
```

In ASP.NET Core Program.cs:
```csharp
builder.Services.AddHealthChecks();
// ...
app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");
```

## Security Best Practices

### Pod Security Context
```yaml
spec:
  automountServiceAccountToken: false
  securityContext:
    runAsNonRoot: true
    runAsUser: 10001
    runAsGroup: 10001
```

### Container Security Context
```yaml
securityContext:
  runAsNonRoot: true
  runAsUser: 10001
  runAsGroup: 10001
  allowPrivilegeEscalation: false
  readOnlyRootFilesystem: true
  capabilities:
    drop:
      - ALL
```

### Resource Limits
```yaml
resources:
  requests:
    cpu: "100m"
    memory: "128Mi"
  limits:
    cpu: "500m"
    memory: "512Mi"
```

## Service Rules

- Type: `ClusterIP`
- Port name: `http-{service-name}`
- Port: 8080, targetPort: 8080

## DNS Addressing

When referencing other services, use full K8s DNS path:
```
http://{service}.chronos-namespace.svc.cluster.local:8080
```

## After Creating Manifest

1. Add delete command to `wipe_and_run_env.sh` in wipe section
2. Add apply command to `wipe_and_run_env.sh` in run section
3. Add ingress path if service needs external access
