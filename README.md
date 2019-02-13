# Steelseries Gamesense API Proxy

Allows the gamesense API to be used from other devices on LAN
https://github.com/SteelSeries/gamesense-sdk

You can then use things like Home Assistant and stuff via the REST plugins to show cool stuff on the screen (and keyboards, mouse etc but only tested on screen)

send the same requests you would send to Gamesense to port 12345 instead and it should work transparently as before, response content is slightly different, it just returns the json that was sent.
