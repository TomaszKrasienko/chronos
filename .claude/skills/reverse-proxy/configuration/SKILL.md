---
name: reverse-proxy-local-configuration
description: Rules for local configuration of YARP reverse proxy
---

- every endpoint should have been mapped in YARP configuration
- proxy routes should NOT have /api prefix (e.g., /employees, /time-loggers)
- for local appsettings.json configuration is from launch-settings.json
- configuration for k8s should be in config map
- addresses for services should use full K8s DNS path: http://<service>.<namespace>.svc.cluster.local:<port>
  - example: http://employees.chronos-namespace.svc.cluster.local:8080
