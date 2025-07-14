# Release Process Guide

This guide explains the release process for the Fermion.EntityFramework.Shared package.

## 🚀 Quick Start

### 1. Manual Release (Local)

```bash
# Patch release (e.g., 1.0.1 -> 1.0.2)
make release
# or
make release-patch

# Minor release (e.g., 1.0.1 -> 1.1.0)
make release-minor

# Major release (e.g., 1.0.1 -> 2.0.0)
make release-major
```

### 2. Release via GitHub Actions

1. Go to the Actions tab in your GitHub repository
2. Select the "Publish Package" workflow
3. Click on the "Run workflow" button
4. Choose the version type (patch, minor, major)
5. Click "Run workflow" to start the process

## 📋 Release Steps

### Automated Steps:

- **Version Detection**: Detects the latest git tag and increments it based on the selected release type
- **Project Update**: Updates the `<Version>` field in the .csproj file
- **Build & Pack**: Builds the project and creates a NuGet package
- **Git Operations**:
  - Commits changes
  - Creates a new git tag
  - Pushes the changes to GitHub
- **GitHub Actions Trigger**: Automatically starts the publish workflow when a new tag is pushed
- **NuGet Publish**: Publishes the package to NuGet.org
- **GitHub Release**: Creates a new GitHub release

## 🔧 Setup

### Requirements:

**NuGet API Key**: Add a `NUGET_API_KEY` secret to your GitHub repository

1. Create an API key from your NuGet.org account
2. Go to your GitHub repo → Settings → Secrets and variables → Actions
3. Add a new secret named `NUGET_API_KEY`

**Git Configuration** (for local use):

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

### Initial Setup

```bash
# Make the script executable
chmod +x scripts/release.sh

# Create the first tag (if it doesn't exist)
git tag v1.0.0
git push origin v1.0.0
```

## 📝 Version Types

- **Patch** (1.0.1 → 1.0.2): Bug fixes, minor improvements
- **Minor** (1.0.1 → 1.1.0): New features, backward-compatible changes
- **Major** (1.0.1 → 2.0.0): Breaking changes, major updates

## 🔄 Workflow Details

### Manual Trigger:
```
GitHub Actions → Publish Package → Run workflow
```

### Automatic Trigger:
```
git tag v1.0.2 → GitHub Actions → NuGet Publish
```

## 📊 Makefile Commands

```bash
make help           # Show available commands
make release        # Create a patch release
make release-patch  # Create a patch release
make release-minor  # Create a minor release
make release-major  # Create a major release
make build          # Build the project
make test           # Run tests
make clean          # Clean build artifacts
```

## 🐛 Troubleshooting

**Script not running:**
```bash
chmod +x scripts/release.sh
```

**Git tag issues:**
```bash
# List existing tags
git tag -l

# Delete a tag (if created incorrectly)
git tag -d v1.0.1
git push origin :refs/tags/v1.0.1
```

**NuGet publish error:**
- Ensure the `NUGET_API_KEY` secret is correctly configured
- Check if the same version already exists on NuGet.org

## 📈 Example Usage

```bash
# Current version: v1.0.1
# To create a patch release:
make release

# This will:
# 1. Increment the version to v1.0.2
# 2. Update the .csproj file
# 3. Generate the NuGet package
# 4. Commit, tag, and push to GitHub
# 5. Trigger GitHub Actions
# 6. Publish to NuGet
```

## 🚨 Important Notes

- **Use the main branch**: Releases should be made from the main branch
- **Clean working directory**: Make sure there are no uncommitted changes
- **Tested code**: Ensure all tests pass before releasing
- **Meaningful commits**: Follow semantic versioning and commit message best practices