@echo off
cls
net stop mysql
echo "服务已停止。"
d:\pvpgn\mysql\bin\mysqld.exe -remove
echo "服务已被移除。"