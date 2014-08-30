#!/bin/bash

# sdlmame
sudo add-apt-repository ppa:c.falco/mame
sudo apt-get update
sudo apt-get install mame

mame -verbose
