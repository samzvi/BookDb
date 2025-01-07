@echo off
SET DB_PATH="D:\fbdata\BOOKSDB.fdb"
SET SQL_FILE="C:\Users\zvirecis\Documents\Visual Studio 2022\Projects\BookDb\db\DbInit.sql"
SET FIREBIRD_BIN="C:\Program Files (x86)\Firebird\Firebird_5_0"
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