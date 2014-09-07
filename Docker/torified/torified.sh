#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

### CONSTAINTS ###

HOST=$(hostname)
IMAGE="torified"
CONTAINER="$IMAGE"_container

### docker.io run ###
run() {
  # shared volumes
  if [ ! -d /docker.io ]; then
    mkdir -p /docker.io
  fi

  volumes="-v /docker.io:/docker.io"

  # ports (tor, torDir, privoxy)
  ports="-p 9001:9001 -p 9030:9030 -p 2222:22"

  docker.io stop $CONTAINER > /dev/null 2>&1
  docker.io rm $CONTAINER > /dev/null 2>&1
  PID=$(docker.io run -d -t -h $HOST $volumes $ports --name $CONTAINER $IMAGE "/usr/bin/supervisord")

  #alias torified="ssh -L 8118:127.0.0.1:8118 <usernam>@<ip>-p 2222"
  # squids port 3128 (http), privoxy 8118, tor 9050 (socks5), polipo 8123, ssh socks 1080
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
