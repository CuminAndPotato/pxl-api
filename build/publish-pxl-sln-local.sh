#!/bin/bash

# Exit on any error
set -e

# Get the directory where this script is located
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Define the dotnet directory (parent of build folder)
DOTNET_DIR="$SCRIPT_DIR/../dotnet"

# Define the output directory
OUTPUT_DIR="$DOTNET_DIR/.pxlLocalDev"

echo "Publishing Pxl.sln projects to $OUTPUT_DIR"

# Clean the output directory if it exists
if [ -d "$OUTPUT_DIR" ]; then
    echo "Cleaning existing output directory..."
    rm -rf "$OUTPUT_DIR"
fi

# Create the output directory
mkdir -p "$OUTPUT_DIR"

# Publish each project in the solution
echo "Publishing Pxl project..."
dotnet publish "$DOTNET_DIR/Pxl/Pxl.fsproj" \
    --configuration Release \
    --output "$OUTPUT_DIR"

echo "✅ Successfully published all projects to $OUTPUT_DIR"
