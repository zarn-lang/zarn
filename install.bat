@echo off
echo ZARN Programming Language - Windows Installer
echo =============================================
echo.

REM Check if running as administrator
net session >nul 2>&1
if %errorLevel% == 0 (
    echo Running as Administrator - Good!
) else (
    echo ERROR: This installer must be run as Administrator.
    echo Please right-click and select "Run as administrator"
    pause
    exit /b 1
)

echo.
echo Installing ZARN CLI...
echo.

REM Build the project
echo Building ZARN...
dotnet build
if %errorLevel% neq 0 (
    echo ERROR: Build failed. Please ensure .NET 8.0 SDK is installed.
    pause
    exit /b 1
)

echo.
echo Installing globally...
dotnet run install

echo.
echo Installation complete!
echo.
echo You can now use ZARN from anywhere:
echo   zarn --help
echo   zarn new myproject
echo   zarn run script.zn
echo.
echo Press any key to exit...
pause >nul

