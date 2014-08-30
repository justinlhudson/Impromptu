#!/bin/bash

if [ `whoami` != root ]; then
    echo Please run this script as root or using sudo
    exit
fi

# https://github.com/ggreer/the_silver_searcher
apt-get install silversearcher-ag

apt-get install expect, jump, gdisk, htop

# http://rvm.io/
curl -sSL https://get.rvm.io | bash -s stable

# https://github.com/tmuxinator/tmuxinator
gem install tmuxinator

# https://github.com/gmarik/Vundle.vim
git clone https://github.com/gmarik/vundle.git ~/.vim/bundle/vundle

# terminal landscape systeminformation
apt-get install landscape-client
