#!/bin/sh
set -e

echo "========================================="
echo "  ZTino Shop Backend - Starting..."
echo "========================================="

echo "[1/2] Skipping database migrations (efbundle not found)..."
# ./efbundle --connection "$ConnectionStrings__DefaultConnection"

# Start the application
echo "[2/2] Starting API server..."
exec dotnet WebAPI.dll