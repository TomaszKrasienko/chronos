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
7. Run `git checkout develop` for moving to main development branch
8. Run `git pull` to get all changes

## Commit Message Format

- Use Conventional Commits (feat/chore/bug/refactor). If don't know - ask.
- Concise (1-2 sentences), focus on "why" not "what"

## PR Format

```

- Use Conventional Commits (feat/chore/bug/refactor) - get from commit 
gh pr create --base develop --title "PR title" --body "$(cat <<'EOF'
## Summary
<bullet points>

## Tests added
<write added tests>
EOF
)"
```

## Usage

```
/commit-and-pr
```
