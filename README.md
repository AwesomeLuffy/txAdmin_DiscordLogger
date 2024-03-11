# Information
DiscordLogger is a C# script that allow to send some log from a FiveM server with [Discord Webhook](https://discord.com/developers/docs/resources/webhook)<br>
Presently there 6 Events that the Logger handle :
- OnPlayerConnecting -> When a player connect to the Server
- OnPlayerDropped -> When a player leave the server
- OnExplosion[txAdmin] -> When an explosion occure in the server
- OnPlayerDeath[txAdmin] -> When a player die in the server
- OnMenuAction[txAdmin] -> When a player (a staff) do something on the txAdmin in-game menu.

For all event related to txAdmin you need to use txAdmin (of course) and so some extras step for have all things to work.
# Installation steps
## 1 - Just put the release into your ressources file on FiveM
## 2 - Modify some txAdmin file
The File located at monitor/resources/sv_logger.lua<br>
"<strong>Why i have to do that ?</strong>"<br>
For example when a Staff do something on the txMenu, it will be send to the server by the event `txsv:logger:menuEvent`.<br>
The server will get it and set the message of what the user did like
"healPlayer" will be "healed player PLAYER_NAME".
It's easier to get this message than copy paste all txAdmin already do.
#### Explosion Event
At the bottom of this function
```lua
AddEventHandler('explosionEvent', function(source, ev)
    -- CODE OF THE FUNCTION
end)
```
Add this line
```lua
TriggerEvent('DiscordLogger:OnExplosionEvent', source, ev.explosionType)
```
So it look like
```lua
AddEventHandler('explosionEvent', function(source, ev)
    -- CODE OF THE FUNCTION
    TriggerEvent('DiscordLogger:OnExplosionEvent', source, ev.explosionType)
end)
```
#### Menu Event
At the bottom of this function
```lua
AddEventHandler('txsv:logger:menuEvent', function(source, action, allowed, data)
  -- CODE OF THE FUNCTION
end)
```
Add this line
```lua
TriggerEvent('DiscordLogger:OnTxMenuAction', source, action, message)
```

#### Death Event
At the bottom of this line
```lua
RegisterNetEvent('txsv:logger:deathEvent', function(killer, cause)
  -- CODE OF THE FUNCTION
end)
```
Add this line
```lua
TriggerEvent('DiscordLogger:OnDeathEvent', source, killer, cause)
```

# Custom Config XML 
Here some details of how to config the config.xml file
```xml
<DiscordLoggerConfig>
  <webhook name="Connect">WEBHOOK_URL</webhook>
  <webhook name="Disconnect">WEBHOOK_URL</webhook>
  <webhook name="Death">WEBHOOK_URL</webhook>
  <webhook name="Menu">WEBHOOK_URL</webhook>
  <webhook name="Explosion">WEBHOOK_URL</webhook>
</DiscordLoggerConfig>
```

Simply put the Webhook URL you want to use for the corresponding event
For example `<webhook name="Connect">WEBHOOK_URL</webhook>` just place the Discord Webhook URL you want to use to get log when a player is Connecting to the server.
