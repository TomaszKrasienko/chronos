---
name: new-branch
description: Create a new feature branch from develop
user-invocable: true
---

# New Branch

Create a new feature branch following the project's git workflow.

## Instructions

1. Fetch the latest changes from origin
2. Checkout the `develop` branch and pull latest changes
3. Create a new branch with the provided name
4. Push the new branch to origin with upstream tracking

## Branch Naming Convention

Use descriptive names with prefixes:
- `feat/` - for new features (e.g., `feat/add-time-log-validation`)
- `fix/` - for bug fixes (e.g., `fix/employee-sync-issue`)
- `ref/` - for refactoring (e.g., `ref/dal-configuration`)
- `docs/` - for documentation (e.g., `docs/update-readme`)
- `chore/` - for maintenance tasks (e.g., `chore/update-dependencies`)

## Usage

```
/new-branch feat/my-feature-name
```

## Workflow

```bash
git fetch origin
git checkout develop
git pull origin develop
git checkout -b <branch-name>
git push -u origin <branch-name>
```
