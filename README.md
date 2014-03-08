Impromptu
=========

As the name implies, just a bunch of helpers.


C#
---------
.NET Helpers

DOCKER
---------
General:
All images contain openSSH and so names bash script for running image.

Images:

username/password: root/root

base -> almost empty ubuntu image to start from with ssh port: 2222
development -> mono-latest, redis. mongodb development box with ssh port: 2222
torified -> privoxy with tor. simply ssh port: 8118 and forward port: 8118