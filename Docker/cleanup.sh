docker.io stop $(docker.io ps -a -q)
docker.io rm $(docker.io ps -a -q)
docker.io rmi $(docker.io images -a -q)
