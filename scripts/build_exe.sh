#!/bin/sh

set -e

root_dir=$(git rev-parse --show-toplevel)

echo "Loading environment variables..."

source "$root_dir"/.env

echo "Creating executable..."

pyinstaller "$root_dir"/client/wtrack/__main__.py -F \
  --name ${CLI_APP_NAME} \
  --distpath "$root_dir"/wtrack/dist \
  --workpath "$root_dir"/wtrack/build \
  --log-level=WARN

branch_name=$(git rev-parse --abbrev-ref HEAD)

if [ "$branch_name" != "main" ]; then
  echo "Skipping copying executable to $CLI_APP_DIR because it's not on main branch."
  exit 0
fi

if [ -z "$CLI_APP_DIR" ]; then
  echo "CLI_APP_DIR is not set."
  exit 1
fi

echo "Copying executable to $CLI_APP_DIR..."

cp "$root_dir"/wtrack/dist/${CLI_APP_NAME}* "$CLI_APP_DIR"
