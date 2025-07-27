#!/bin/bash

echo "ZARN Programming Language - Linux/macOS Installer"
echo "================================================="
echo

# Check if running as root/sudo
if [ "$EUID" -ne 0 ]; then
    echo "ERROR: This installer must be run with sudo privileges."
    echo "Please run: sudo ./install.sh"
    exit 1
fi

echo "Running as root - Good!"
echo

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET 8.0 SDK/Runtime is not installed."
    echo "Please install .NET 8.0 first:"
    echo "  Ubuntu/Debian: sudo apt install dotnet-sdk-8.0"
    echo "  macOS: brew install dotnet"
    echo "  Or visit: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

echo "Building ZARN..."
dotnet build
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed. Please check the error messages above."
    exit 1
fi

echo
echo "Installing globally..."
dotnet run install

echo
echo "Installation complete!"
echo
echo "You can now use ZARN from anywhere:"
echo "  zarn --help"
echo "  zarn new myproject"
echo "  zarn run script.zn"
echo
echo "Try: zarn --version"

