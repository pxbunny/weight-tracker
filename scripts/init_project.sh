#!/bin/sh

root_dir=$(git rev-parse --show-toplevel)

echo "Setting up git hooks..."

git config core.hooksPath $root_dir/.githooks

echo "Current git hooks directory: $(git config core.hooksPath)"
echo "Searching for .env file..."

env_file_already_exists=$(test -f $root_dir/.env && echo true || echo false)

if [ "$env_file_already_exists" = true ]; then
  echo "The .env file already exists."
  exit 1
fi

echo "Creating .env file..."

cp $root_dir/.env.template $root_dir/.env

echo "Don't forget to update variables in .env file."
