#!/bin/bash

# Exit on any error
set -e

# Get the directory where this script is located
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Define the output directory
OUTPUT_DIR="$SCRIPT_DIR/.pxlLocalDev"

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
dotnet publish "$SCRIPT_DIR/Pxl/Pxl.fsproj" \
    --configuration Release \
    --output "$OUTPUT_DIR/Pxl"

echo "Publishing Pxl.Ui project..."
dotnet publish "$SCRIPT_DIR/Pxl.Ui/Pxl.Ui.fsproj" \
    --configuration Release \
    --output "$OUTPUT_DIR/Pxl.Ui"

echo "Publishing Pxl.Ui.CSharp project..."
dotnet publish "$SCRIPT_DIR/Pxl.Ui.CSharp/Pxl.Ui.CSharp.csproj" \
    --configuration Release \
    --output "$OUTPUT_DIR/Pxl.Ui.CSharp"

echo "✅ Successfully published all projects to $OUTPUT_DIR"
