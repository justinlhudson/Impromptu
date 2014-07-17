#!/bin/bash

#xbox controller driver setup
sudo apt-add-repository ppa:rael-gc/ubuntu-xboxdr
sudo apt-get update
sudo apt-get install ubuntu-xboxdrv
# Note: joystick calibration

# key binding for controller and mame (no layout provided)
sudo apt-get install qjoypad
# Run: qjoypad "<layout name>" --notray

