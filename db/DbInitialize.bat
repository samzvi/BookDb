@echo off
SET DB_PATH="D:\fbdata\BOOKSDB.fdb"
SET SQL_FILE="%~dp0DbInit.sql"
SET FIREBIRD_BIN="C:\fb"
SET USERNAME=SYSDBA
SET PASSWORD=masterkey

cd %FIREBIRD_BIN%

isql -input %SQL_FILE%


IF %ERRORLEVEL% NEQ 0 (
    echo Error initializing the database. Check the script or environment settings.
    pause
    exit /b %ERRORLEVEL%
)

echo Database initialization complete.
pause