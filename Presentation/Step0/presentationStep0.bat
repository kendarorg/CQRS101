@ECHO OFF
SET HOME=%CD%
SET DEFAULT_PARS=-H "Accept: application/json" -H "Content-Type: application/json"

SET COMMAND_API=http://localhost:8090
SET CRUD_API=http://localhost:8091
SET PROJECTIONS_API=http://localhost:8092

REM Presentation
CD.. 

SET CURLEXE="%CD%\CURL.EXE"
REM Commands
CD Commands

TYPE %HOME%\step0.txt
pause
cls

REM Add Customer
%CURLEXE% %DEFAULT_PARS% -X POST %CRUD_API%/api/customers -d @createCustomer.json
pause

REM List All Customer
%CURLEXE% %DEFAULT_PARS% -X GET %CRUD_API%/api/customers
pause

TYPE %HOME%\step1.txt
pause
cls

REM Create Invoice with customer
%CURLEXE% %DEFAULT_PARS% -X POST %COMMAND_API%/api/commands -d @createInvoice.json
pause

TYPE %HOME%\step2.txt
pause
cls

REM List all Created Invoices
%CURLEXE% %DEFAULT_PARS% -X GET %CRUD_API%/api/invoices/created
pause

TYPE %HOME%\step3.txt
pause
cls

REM Complete invoice
%CURLEXE% %DEFAULT_PARS% -X POST %COMMAND_API%/api/commands -d @completeInvoice.json
pause

TYPE %HOME%\step4.txt
pause
cls

REM List all Created Invoices
%CURLEXE% %DEFAULT_PARS% -X GET %CRUD_API%/api/invoices/created
pause

REM List all Completed Invoices
%CURLEXE% %DEFAULT_PARS% -X GET %CRUD_API%/api/invoices/completed
pause