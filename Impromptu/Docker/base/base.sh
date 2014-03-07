#!/bin/bash

# Usage: [name] [option]
# Options:
# 	-clean
# 	-build 
# 	-stop 
# 	-import 
# 	-export
# 	-logs 

if [ "$(id -u)" != "0" ]; then
    echo "ERROR: Must be run with root privileges (i.e. sudo)"
    exit 1
fi

HOST=$(hostname)
IMAGE="${args[0]}"
CONTAINER="$IMAGE"_container

args=("$@")

# Wait for docker
chmod 777 "/var/run/docker.sock"
docker_lock=/var/run/docker.sock
while [ ! -e $docker_lock ] ; do
  inotifywait -t 2 -e create $(dirname $docker_lock)
done

if [ ! -z "${args[1]}" ] && [ "${args[1]}" == "-clean" ]; then  
  docker rm $(docker ps -a -q)
  exit
fi

if [ ! -z "${args[1]}" ] && [ "${args[1]}" == "-build" ]; then
  docker build --quiet=false --no-cache=false --rm=false --tag "$IMAGE" .
  exit
fi

if [ ! -z "${args[1]}" ] && [ "${args[1]}" == "-stop" ]; then
  docker stop $CONTAINER
  exit
fi

# Shared Volumes
if [ ! -d /docker ]; then
  mkdir -p /docker
fi

if [ ! -d /var/lib/mongodb ]; then
  mkdir -p /var/lib/mongodb
fi

volumes="-v /docker:/docker -v /var/lib/mongodb:/var/lib/mongodb"

# Ports (redis, mongodb)
ports="-p 56379:6379 -p 57017:27017"

docker rm $CONTAINER > /dev/null 2>&1
PID = $(docker run -d -h $HOST -name $CONTAINER $volumes -p 2222:22 $ports $IMAGE)

if [ ! -z "${args[1]}" ] && [ "${args[1]}" == "-logs" ]; then  
  docker logs $PID
fi

if [ ! -z "${args[1]}" ] && [ "${args[1]}" == "-export" ]; then  
  docker stop $CONTAINER
  docker export $CONTAINER | gzip -c > $IMAGE.tgz
fi

if [ ! -z "${args[1]}" ] && [ "${args[1]}" == "-import" ]; then  
  docker rm $CONTAINER /dev/null 2>&1
  docker rmi $IMAGE /dev/null 2>&1
  gzip -dc $IMAGE.tgz | docker import $IMAGE
fi

exit