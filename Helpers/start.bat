set CURRENT=%CD%

mkdir hsqldb-2.4.0\hsqldb\data
mkdri apache-activemq-5.15.0\data

SET PATH=%PATH%;%JAVA_HOME%\bin
CD "%CURRENT%\apache-activemq-5.15.0\bin"
start /min activemq start
CD "%CURRENT%\hsqldb-2.4.0\hsqldb\bin"
start /min runServer