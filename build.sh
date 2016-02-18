#!/usr/bin/env bash

SOURCE="${BASH_SOURCE[0]}"
while [ -h "$SOURCE" ]; do # resolve $SOURCE until the file is no longer a symlink
  DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )"
  SOURCE="$(readlink "$SOURCE")"
  [[ "$SOURCE" != /* ]] && SOURCE="$DIR/$SOURCE" # if $SOURCE was a relative symlink, we need to resolve it relative to the path where the symlink file was located
done
DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )"

set -e

echo "Installing dotnet-cli"
curl -s https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0/scripts/obtain/install.sh | sh -s -- --channel beta

echo "Using dotnet cli from: $(which dotnet)"

echo "Restoring packages for build tasks"
dotnet restore

echo "Building build tasks"
rm -Rf "$DIR/tasks/bin/app"
dotnet build --framework dnxcore50 "$DIR/tasks" -o "$DIR/tasks/bin/app"

echo "Running build tasks"
$DIR/tasks/bin/app/tasks "$@"

echo "Build completed"
