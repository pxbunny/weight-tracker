#!/bin/sh

set -e

root_dir=$(git rev-parse --show-toplevel)

echo "Loading environment variables..."

source $root_dir/.env

echo "Creating executable..."

rm -rf $root_dir/cli/dist

pyinstaller $root_dir/cli/wtrack/__main__.py \
  --name $CLI_APP_NAME \
  --distpath $root_dir/cli/dist \
  --workpath $root_dir/cli/build \
  --log-level=WARN \
  --exclude-module pyinstaller \
  --exclude-module isort \
  --exclude-module pylint

branch_name=$(git rev-parse --abbrev-ref HEAD)

if [ "$branch_name" != "main" ]; then
  echo "Skipping copying executable to $CLI_APP_INSTALLATION_DIR because it's not on main branch."
  exit 0
fi

if [ -z "$CLI_APP_INSTALLATION_DIR" ]; then
  echo "CLI_APP_INSTALLATION_DIR is not set."
  exit 1
fi

echo "Copying executable to $CLI_APP_INSTALLATION_DIR..."

cp $root_dir/cli/config.prod.json $root_dir/cli/dist/$CLI_APP_NAME/config.json
cp -r $root_dir/cli/dist/$CLI_APP_NAME $CLI_APP_INSTALLATION_DIR
