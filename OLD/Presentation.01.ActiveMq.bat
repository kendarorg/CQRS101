@ECHO OFF

SET SITE_ROOT_CMD=http://localhost:9000
SET SITE_ROOT=http://localhost:9001
SET LINE=--------------------------------------------------------

ECHO.
ECHO %LINE%
ECHO Verify that all is up and running
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT_CMD%/api/commands/types
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO %LINE%
ECHO Create a ToDo Task
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT_CMD%/api/commands/send/createTask -d @01\01.01.createTask.json
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
ECHO List the done tasks (should be empty)
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT%/api/tasks/done
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO %LINE%
ECHO Change the task priority
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT_CMD%/api/commands/send/changeTaskPriority -d @01\01.02.changePriority.json
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO.
ECHO %LINE%
ECHO Verify that the priority is now 10
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET %SITE_ROOT%/api/tasks/todo/8cd95b81-48d7-4d66-884d-7fd3c550eef1
ECHO.
ECHO %LINE%
ECHO.
PAUSE

ECHO %LINE%
ECHO CompleteTask
ECHO %LINE%
curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X POST %SITE_ROOT_CMD%/api/commands/send/completeTask -d @01\01.03.completeTask.json
ECHO.
ECHO %LINE%
ECHO.
PAUSE


ECHO.
ECHO %LINE%
ECHO List the todo tasks (should be empty)
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
