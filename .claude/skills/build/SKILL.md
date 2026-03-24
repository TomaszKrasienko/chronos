---
name: build
description: Build Docker image for a Chronos microservice
disable-model-invocation: true
argument-hint: [service-name]
allowed-tools: Bash(*)
---

# Build Docker Image

Build a Docker image for the specified Chronos microservice.

## Available services:

- `contracts` - Contracts service
- `employees` - Employees service
- `jobs-synchronizer` - Jobs synchronizer service
- `notifications` - Notifications service
- `reverse-proxy` - Reverse proxy service
- `time-logs` - Time logs service

## Instructions:

1. If no argument provided, ask the user which service to build
2. Run the build script:

```bash
cd deploy/builds && bash ./$ARGUMENTS.sh
```

3. Report the build result (success/failure, image name and version)
