#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

# Pre-reqs
apt-get -y install gtk-sharp-4.0 gnome-sharp-4.0 mono-addins-utils monodoc-base 

# Structure for source
mkdir -p /opt/monodevelop-latest/
cd /opt/monodevelop-latest/

# Download source(s)
git clone https://github.com/mono/monodevelop.git
git submodule update --init --recursive

# Compile
cd /opt/monodevelop-latest/monodevelop
./configure --prefix=/usr/local/bin --profile=stable --enable-tests
make && make install

