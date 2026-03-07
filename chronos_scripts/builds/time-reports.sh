# !/bin/bash

IMAGE="chronos/time-reports"

# Pobierz aktualny tag
CURRENT=$(docker images $IMAGE --format "{{.Tag}}" | head -1)

if [ -z "$CURRENT" ] || [ "$CURRENT" == "<none>" ]; then
    VERSION="0.1"
else
    MAJOR=$(echo $CURRENT | cut -d. -f1)
    MINOR=$(echo $CURRENT | cut -d. -f2)
    VERSION="$MAJOR.$((MINOR + 1))"
fi

echo "Aktualna: ${CURRENT:-brak}"
echo "Nowa: $VERSION"

cd ./../../src
docker build --platform linux/amd64 -f ./time-reports/Dockerfile -t $IMAGE:$VERSION -t $IMAGE:latest .
