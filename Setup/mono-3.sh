#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

# Tools to compile mono
apt-get install -f -y autoconf libtool automake 
apt-get install -f -y mono-complete
apt-get install -f -y build-dep mono
apt-get install -f -y libgtk-3-dev

#apt-get install -f -y autoconf automake libtool g++ gettext libglib2.0 libpng12 libfontconfig1-dev git mono-gmcs
# corlib version 96, found 110
#aptitude reinstall libmono-corlib2.0-cil

# Structure for source
mkdir -p /opt/mono-3
cd /opt/mono-3

# Download source(s)
git clone git://github.com/mono/libgdiplus.git
git clone git://github.com/mono/mono.git
git clone git://github.com/mono/xsp.git

# Compile
apt-get install -f -y automake autoconf libtool libtiff5
cd /opt/mono-3/libgdiplus
git fetch
./autogen.sh --prefix=/usr/local
make
make install

cd /opt/mono-3/mono/
git fetch
./autogen.sh --prefix=/usr/local
make
make install


cd /opt/mono-3/xsp
git fetch
./autogen.sh --prefix=/usr/local
make
make install
