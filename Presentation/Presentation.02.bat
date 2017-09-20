@ECHO OFF
SET SITE_ROOT=http://localhost:9000
SET LINE=--------------------------------------------------------

ECHO.
ECHO %LINE%
ECHO Verify that all is up and running
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT%/api/commands/types
ECHO.
ECHO %LINE%
ECHO.
PAUSE