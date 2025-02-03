@echo off
SET DB_PATH=%~dp0BooksDb.fdb
SET SQL_FILE="%~dp0DbInit.sql"
SET FIREBIRD_BIN="C:\fb"
SET USERNAME=SYSDBA
SET PASSWORD=masterkey
cd %FIREBIRD_BIN%

if not exist "%DB_PATH%" (
    REM Create the database using isql
    echo CREATE DATABASE '%DB_PATH%' USER '%USERNAME%' PASSWORD '%PASSWORD%' DEFAULT CHARACTER SET UTF8; | isql -user %USERNAME% -password %PASSWORD% -quiet
    if %ERRORLEVEL% NEQ 0 (
        echo Error creating the database.
        pause
        exit /b %ERRORLEVEL%
    )
)

(
    echo CONNECT '%DB_PATH%' USER '%USERNAME%' PASSWORD '%PASSWORD%';
    type %SQL_FILE%
) | isql -user %USERNAME% -password %PASSWORD% -quiet

if %ERRORLEVEL% NEQ 0 (
    echo Error initializing the database. Check the script or environment settings.
    pause
    exit /b %ERRORLEVEL%
)

echo Database initialization complete.