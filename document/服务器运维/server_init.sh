#!/bin/sh
mkdir /ebs_server
mkdir /ebs_server/mongodb
mkdir /ebs_server/mongodb/data
mkdir /ebs_server/mongodb/log
mkdir /ebs_server/redis/
mkdir /ebs_server/redis/log
mkdir /ebs_server/redis/lib
mkdir /ebs_server/rabbitmq/
mkdir /ebs_server/rabbitmq/mnesia
mkdir /ebs_server/rabbitmq/log
chmod 777  /ebs_server -R
chown mongodb:mongodb /ebs_server/mongodb -R
chown rabbitmq:rabbitmq /ebs_server/rabbitmq -R

apt-get update
apt-get install make
apt-get install gcc
apt-get install g++
apt-get install p7zip-full
apt-get install sysstat
apt install git

echo "ulimit -c unlimited" >  ~/.bash_profile
source  ~/.bash_profile
echo "core-%e-%p" >  /proc/sys/kernel/core_pattern

rm /etc/localtime && ln -s /usr/share/zoneinfo/Asia/Singapore /etc/localtime

echo "*/30 * * * * root /usr/sbin/ntpdate cn.pool.ntp.org > /dev/null 2>&1" >> /etc/crontab 
/etc/init.d/cron restart

echo "* soft nofile 65500" >> /etc/security/limits.conf 
echo "* hard nofile 65500" >> /etc/security/limits.conf 

cd ~
git clone git://github.com/kongjian/tsar.git
cd tsar
make && make install
cd ..

wget http://download.redis.io/releases/redis-5.0.5.tar.gz
7za x redis-5.0.5.tar.gz
7za x redis-5.0.5.tar
cd redis-5.0.5/
chmod +x src/mkreleasehdr.sh
chmod +x deps/jemalloc/configure
make MALLOC=libc && make install
cd ..

apt-get install rabbitmq-server
rabbitmq-plugins enable rabbitmq_management

wget -qO - https://www.mongodb.org/static/pgp/server-4.0.asc |  apt-key add -
echo "deb [ arch=amd64 ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/4.0 multiverse" |  tee /etc/apt/sources.list.d/mongodb-org-4.0.list
apt-get update
apt-get install -y mongodb-org
echo "mongodb-org hold" | sudo dpkg --set-selections
echo "mongodb-org-server hold" | sudo dpkg --set-selections
echo "mongodb-org-shell hold" | sudo dpkg --set-selections
echo "mongodb-org-mongos hold" | sudo dpkg --set-selections
echo "mongodb-org-tools hold" | sudo dpkg --set-selections
service mongod start

wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
apt install software-properties-common
add-apt-repository universe
apt-get install apt-transport-https
apt-get update
apt-get install dotnet-sdk-2.2

apt-get install nginx-full
apt-get install curl

apt install speedtest-cli