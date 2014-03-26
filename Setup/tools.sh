#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

# https://github.com/ggreer/the_silver_searcher
apt-get install silversearcher-ag


