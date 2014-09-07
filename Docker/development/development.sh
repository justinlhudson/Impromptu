#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

### CONSTAINTS ###

HOST=$(hostname)
IMAGE="development"
CONTAINER="$IMAGE"_container

### docker.io run ###
run() {
  # shared volumes
  if [ ! -d /docker.io ]; then
    mkdir -p /docker.io
  fi

  if [ ! -d /var/lib/mongodb ]; then
    mkdir -p /var/lib/mongodb
  fi

  volumes="-v /docker.io:/docker.io -v /var/lib/mongodb:/var/lib/mongodb"

  # ports (redis, mongodb)
  ports="-p 6379:6379 -p 27017:27017"

  docker.io stop $CONTAINER > /dev/null 2>&1
  docker.io rm $CONTAINER > /dev/null 2>&1
  PID=$(docker.io run -d -t -h $HOST $volumes -p 2222:22 $ports --name $CONTAINER $IMAGE "/usr/bin/supervisord")
}

### Terminal operations ###
while getopts "rdc:bseih" opt; do
  case $opt in
    r)
      echo "Run: $IMAGE"
      run
      ;;
    d)
      echo "Delete: $IMAGE"
      docker.io rmi $IMAGE
      ;;    
    c)
      echo "Commit: $CONTAINER to $IMAGE"
      docker.io commit -m "$OPTARG" $CONTAINER $IMAGE 
      ;;
    b)
      echo "Build: $IMAGE"
      docker.io build --quiet=false --no-cache=false --force-rm=true --tag "$IMAGE" .
      ;;
    s)
      echo "Stop: $CONTAINER"
      docker.io stop $CONTAINER > /dev/null 2>&1
      ;;
    e)
      echo "Export: $CONTAINER"
      docker.io stop $CONTAINER > /dev/null 2>&1
      docker.io export $CONTAINER | gzip -c > $CONTAINER.tgz
      ;;
    i)
      echo "Import: $IMAGE"
      docker.io stop $CONTAINER > /dev/null 2>&1
      docker.io rm $CONTAINER > /dev/null 2>&1
      docker.io rmi $IMAGE > /dev/null 2>&1
      gzip -dc "$CONTAINER".tgz | docker.io import - $IMAGE
      ;;
    h)   
      echo -e "Usage: option(s)
Options:
  -(r)un
  -(d)elete
  -(c)ommit message
  -(b)uild 
  -(s)top 
  -(i)mport 
  -(e)xport
  -(h)elp"
    ;;
  esac
done

exit
