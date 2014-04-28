#!/bin/bash

# Install Redis-Server
apt-get -y install redis-server

# Install MongoDB
apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv 7F0CEB10
echo 'deb http://downloads-distro.mongodb.org/repo/ubuntu-upstart dist 10gen' | tee /etc/apt/sources.list.d/mongodb.list
apt-get update
apt-get -y install mongodb-org
rm /var/lib/mongodb/mongod.lock || true

exit 0
