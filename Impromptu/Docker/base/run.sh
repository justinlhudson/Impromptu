#!/bin/bash

# Usage: [name] [option]
# Options:
# 	-clean
# 	-build 
# 	-stop 
# 	-import 
# 	-export

if [ "$(id -u)" != "0" ]; then
    echo "ERROR: Must be run with root privileges (i.e. sudo)"
    exit 1
fi

HOST=$(hostname)
IMAGE="$1"
CONTAINER="$IMAGE"_container

# Wait for docker
chmod 777 "/var/run/docker.sock"
docker_lock=/var/run/docker.sock
while [ ! -e $docker_lock ] ; do
  inotifywait -t 2 -e create $(dirname $docker_lock)
done

while getopts "cbsei" opt ${@:2}; do
  case $opt in
  	c)
          echo "Clear All Container(s))"
  	  docker rm $(docker ps -a -q)
  	  exit
  	  ;;
  	b)
          echo "Build: $IMAGE"
  	  docker build --quiet=false --no-cache=false --rm=false --tag "$IMAGE" .
  	  exit
  	  ;;
  	s)
          echo "Stop: $CONTAINER"
  	  docker stop $CONTAINER > /dev/null 2>&1
          exit
          ;;
        e)
          echo "Export: $CONTAINER"
          docker stop $CONTAINER > /dev/null 2>&1
          docker export $CONTAINER | gzip -c > $CONTAINER.tgz
          exit
          ;;
        i)
          echo "Import: $IMAGE"
          docker stop $CONTAINER > /dev/null 2>&1
          docker rm $CONTAINER > /dev/null 2>&1
          docker rmi $IMAGE > /dev/null 2>&1
          gzip -dc "$IMAGE".tgz | docker import - $IMAGE
          exit
          ;;
  	\?)	 
	  echo "Usage: "
	  ;;
  esac
done

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

docker stop $CONTAINER > /dev/null 2>&1
docker rm $CONTAINER > /dev/null 2>&1
PID=$(docker run -d -h $HOST $volumes -p 2222:22 $ports -name $CONTAINER $IMAGE)

exit
