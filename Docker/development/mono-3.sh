#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

# Tools to compile mono
apt-get install -f -y mono-gmcs
apt-get install -f -y automake autoconf libtool git g++ gettext 
apt-get install -f -y libgtk-3-dev libglib2.0-dev libpng12-dev libfontconfig1-dev
apt-get install -f -y apache2 apache2-threaded-dev

# corlib version 96, found 110
#aptitude reinstall libmono-corlib2.0-cil

# Structure for source
mkdir -p /opt/mono-3/mono-latest
cd /opt/mono-3/mono-latest

# Download source(s)
git clone git://github.com/mono/libgdiplus.git
git clone git://github.com/mono/mono.git
git clone git://github.com/mono/xsp.git
git clone git://github.com/mono/mod_mono.git

# Compile
cd /opt/mono-3/mono-latest/libgdiplus
git fetch --all
./autogen.sh --prefix=/usr/local
make
make install

cd /opt/mono-3/mono-latest/mono
git fetch --all
./autogen.sh --prefix=/usr/local
make
make install

cd /opt/mono-3/xsp
git fetch --all
./autogen.sh --prefix=/usr/local
make
make install

cd /opt/mono-3/mod_mono
git fetch --all
./autogen.sh --prefix=/usr/local
make
make install