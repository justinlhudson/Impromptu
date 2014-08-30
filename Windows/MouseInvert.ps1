# Invert Mouse like Mac
Get-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Enum\HID\*\*\Device` Parameters FlipFlopWheel -EA 0 | ForEach-Object { Set-ItemProperty $_.PSPath FlipFlopWheel 1 }

# Go to your device manager and find your device (your mouse or trackpad) 
# Find your device's Hardware ID.
# Open regedit.exe
# Lookup the following thing: HKEY_LOCAL_MACHINE/SYSTEM/CurrentControlSet/Enum/HID/<Hardware ID>/Device Parameters
# Change the key named FlipFlopWheel from 0 to 1.