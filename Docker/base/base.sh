#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

# Wait for docker.io
chmod 777 "/var/run/docker.io.sock"
docker.io_lock=/var/run/docker.io.sock
while [ ! -e $docker.io_lock ] ; do
  inotifywait -t 2 -e create $(dirname $docker.io_lock)
done

### CONSTAINTS ###

HOST=$(hostname)
IMAGE="development"
CONTAINER="$IMAGE"_container

### docker.io run ###
run() {
  # shared Volumes
  if [ ! -d /docker.io ]; then
    mkdir -p /docker.io
  fi

  volumes="-v /docker.io:/docker.io"

  # ports (...)
  ports=""

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