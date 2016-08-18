@echo on
cls
net stop mysql
echo "服务已停止。"
d:\pvpgn\mysql\bin\mysqld.exe -remove
echo "服务已被移除。"
rd D:\pvpgn\mysql\data /s /q
echo "旧数据已清除。"
d:\pvpgn\mysql\data.exe
echo "新数据初始化成功。"
d:\pvpgn\mysql\bin\mysqld.exe -install
echo "MySQL安装成功。"
net start mysql
echo "MySQL服务已启动。"
pause