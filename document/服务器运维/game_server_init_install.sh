#!/bin/sh

mkfs.ext4 /dev/xvdc
mkdir /ebs_server
mount /dev/xvdc /ebs_server

apt-get update

echo "/dev/xvdc /ebs_server ext4 default 0 0" >> /etc/fstab 

echo "ulimit -c unlimited" >  ~/.bash_profile
source  ~/.bash_profile
echo "core-%e-%p" >  /proc/sys/kernel/core_pattern

rm /etc/localtime && ln -s /usr/share/zoneinfo/Asia/Singapore /etc/localtime


echo "*/30 * * * * root /usr/sbin/ntpdate cn.pool.ntp.org > /dev/null 2>&1" >> /etc/crontab 
/etc/init.d/cron restart

echo "* soft nofile 65500" >> /etc/security/limits.conf 
echo "* hard nofile 65500" >> /etc/security/limits.conf 

apt-get install make
apt-get install gcc
apt-get install g++
apt-get install p7zip-full

apt-get install libmysql++3v5
apt-get install gdb
apt-get install sysstat

cd ~
wget http://52.76.5.197:90/tools/tsar.zip
7za x tsar.zip
cd tsar-master
make && make install
cd ..

wget http://52.76.5.197:90/tools/redis-2.8.17.zip
7za x redis-2.8.17.zip
cd redis-2.8.17
make && make install
cd ..

apt-get install rabbitmq-server
rabbitmq-plugins enable rabbitmq_management
#set global sql_mode='STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';

