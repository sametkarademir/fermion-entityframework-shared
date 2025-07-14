#!/bin/bash

set -e

PROJECT_FILE="src/Fermion.EntityFramework.Shared/Fermion.EntityFramework.Shared.csproj"

VERSION_TYPE=${1:-patch}

echo "🚀 Starting release process..."

LATEST_TAG=$(git describe --tags $(git rev-list --tags --max-count=1) 2>/dev/null || echo "v1.0.0")
echo "📍 Latest tag: $LATEST_TAG"

if [ "$LATEST_TAG" == "v1.0.0" ] && [ -z "$(git tag -l)" ]; then
    echo "⚠️  No tags found. Starting with v1.0.1"
    NEW_VERSION="v1.0.1"
else
    # Versiyonu arttır
    IFS='.' read -r major minor patch <<< "${LATEST_TAG#v}"
    
    case $VERSION_TYPE in
        major)
            NEW_VERSION="v$((major + 1)).0.0"
            ;;
        minor)
            NEW_VERSION="v$major.$((minor + 1)).0"
            ;;
        patch)
            NEW_VERSION="v$major.$minor.$((patch + 1))"
            ;;
        *)
            echo "❌ Invalid version type. Use: major, minor, or patch"
            exit 1
            ;;
    esac
fi

echo "🆕 New version: $NEW_VERSION"

echo "📝 Updating version in $PROJECT_FILE"
if [[ "$OSTYPE" == "darwin"* ]]; then
    sed -i '' -E "s|<Version>.*</Version>|<Version>${NEW_VERSION#v}</Version>|" "$PROJECT_FILE"
else
    sed -i -E "s|<Version>.*</Version>|<Version>${NEW_VERSION#v}</Version>|" "$PROJECT_FILE"
fi

echo "✅ Version updated in project file:"
grep -n "<Version>" "$PROJECT_FILE"

echo "📦 Building and packing..."
dotnet pack "$PROJECT_FILE" -c Release /p:PackageVersion="${NEW_VERSION#v}" -o

if [ -n "$(git status --porcelain)" ]; then
    echo "📤 Committing changes..."
    git add "$PROJECT_FILE"
    git commit -m "chore: bump version to ${NEW_VERSION}"
    git tag "$NEW_VERSION"
    git push origin main --tags
    echo "🎉 Version bumped and pushed. CI will now handle publish."
else
    echo "⚠️  No changes to commit."
fi

echo "✅ Release process completed!"
echo "🏷️  Tagged version: $NEW_VERSION"
