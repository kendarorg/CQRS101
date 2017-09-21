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

ECHO %LINE%
ECHO Prepare the 01 status
ECHO %LINE%
curl -s -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT%/api/commands/send/createTask -d @01\01.01.createTask.json
ECHO.
curl -s -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT%/api/commands/send/changeTaskPriority -d @01\01.02.changePriority.json
ECHO.
curl -s -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT%/api/commands/send/completeTask -d @01\01.03.completeTask.json
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO.
ECHO %LINE%
ECHO List the tasks statistics
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT%/api/tasks/stats
ECHO.
ECHO %LINE%
ECHO.
PAUSE


ECHO %LINE%
ECHO Create a task type (DEV)
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT%/api/taskTypes -d @02\02.01.createType.json
ECHO.
ECHO %LINE%
ECHO.
PAUSE


ECHO %LINE%
ECHO Create a ToDo Task with DEV type
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT%/api/commands/send/createTask -d @02\02.02.createTask.json
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO.
ECHO %LINE%
ECHO List the todo tasks (one should exists)
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT%/api/tasks/todo
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO.
ECHO %LINE%
ECHO List the done tasks (one should exists)
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT%/api/tasks/done
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO.
ECHO %LINE%
ECHO List the tasks statistics (1 running, 1 done)
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT%/api/tasks/stats
ECHO.
ECHO %LINE%
ECHO.
PAUSE

