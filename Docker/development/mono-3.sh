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
mkdir -p /opt/mono-3/
cd /opt/mono-3/

# Download source(s)
git clone git://github.com/mono/libgdiplus.git
git clone git://github.com/mono/mono.git

# Compile
cd /opt/mono-3/libgdiplus
git fetch --all
git checkout "mono-3.2.8-branch"
./autogen.sh --prefix=/usr/local
make
make install
make clean

cd /opt/mono-3/mono
git fetch --all
git checkout "mono-3.2.8-branch"
./autogen.sh --prefix=/usr/local --enable-optimized --enable-llvm=yes --enable-targets="x86 x86_64"
make 
make install
make clean