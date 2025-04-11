#!/bin/sh

set -e

root_dir=$(git rev-parse --show-toplevel)

echo "Loading environment variables..."

source "$root_dir"/.env

echo "Creating executable..."

pyinstaller "$root_dir"/cli/__main__.py -F \
  --name ${CLI_APP_NAME} \
  --distpath "$root_dir"/dist \
  --workpath "$root_dir"/build \
  --log-level=WARN

if [ -z "$CLI_APP_DIR" ]; then
  echo "CLI_APP_DIR is not set."
  exit 1
fi

echo "Copying executable to $CLI_APP_DIR..."

cp "$root_dir"/dist/${CLI_APP_NAME}* "$CLI_APP_DIR"
