---
name: commit-and-pr
description: Commit staged changes and create a pull request to develop
user-invocable: true
---

# Commit and PR

Commit current changes and create a pull request to `develop` branch.

## Instructions

1. Run `git status` and `git diff` to analyze changes
2. Run `git log` to check recent commit message style
3. Stage relevant files (avoid secrets, large binaries)
4. Create commit with descriptive message following repo conventions
5. Push changes to remote
6. Create PR to `develop` using `gh pr create`

## Commit Message Format

- Concise (1-2 sentences), focus on "why" not "what"

## PR Format

```
gh pr create --base develop --title "PR title" --body "$(cat <<'EOF'
## Summary
<bullet points>

## Test plan
<testing checklist>
EOF
)"
```

## Usage

```
/commit-and-pr
```
