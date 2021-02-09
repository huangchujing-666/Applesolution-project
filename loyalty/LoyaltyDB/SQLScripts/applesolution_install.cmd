@echo on

set ver=ver1.0
set dbserver=58.64.166.166,9433
set database=applesolutiondb
set user=applesolutionuser
set password=54splq2013p
set logfile=log/db_install.log
set errorfile=log/db_install_err.log
set cmdtimeout=65534
rem set cmdtimeout=1


echo -------------------------------------------------- SQL Batch %ver% Installation Start -------------------------------------------------- >> %logfile%



for /D %%X in (*.*) do (
echo Execute "%%X" >> "%logfile%"
cd %%X
for %%G in (*.sql) do (
echo "%%G" >> "../%logfile%"
sqlcmd -S "%dbserver%" -t %cmdtimeout% -d %database% -U %user% -P %password% -i"%%G" >> "../%logfile%" 2>> "../%errorfile%
)
cd ..
)



echo -------------------------------------------------- SQL Batch %ver% Installation Finish ------------------------------------------------- >> %logfile%



pause
