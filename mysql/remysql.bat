cls
echo off
net stop mysql
echo "������ֹͣ��"
d:\pvpgn\mysql\bin\mysqld.exe -remove
echo "�����ѱ��Ƴ���"
rd "D:\pvpgn\mysql\data\*.*" /s /q
echo "�������������"
data.exe
echo "�����ݳ�ʼ���ɹ���"
d:\pvpgn\mysql\bin\mysqld.exe -install
echo "MySQL��װ�ɹ���"
net start mysql
echo "MySQL������������"
