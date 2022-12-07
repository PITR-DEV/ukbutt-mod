# UKButt

Short link: [pitr.dev/ukbutt](https://pitr.dev/ukbutt)

UKButt is an official [BepInEx](https://github.com/BepInEx/BepInEx) mod for [ULTRAKILL](http://devilmayquake.com) that adds [buttplug.io](https://buttplug.io/) support.

## Resources

- **[Download Latest Release](/UKButt/UKButt/releases)**
- **[Installation](#installation)**
- **[Running](#running)**
- **[Configuration](#configuration)**
- **[Default Values](#default-values)**
- **[The buttplug.io standard](https://buttplug.io/)**

## Video

[![IMAGE ALT TEXT HERE](https://i3.ytimg.com/vi/6r13L1yvtYA/maxresdefault.jpg)](https://www.youtube.com/watch?v=6r13L1yvtYA)

## Installation

[Start by installing BepInEx 5.4.21](https://docs.bepinex.dev/articles/user_guide/installation/index.html)

You can also follow one of the [Community](https://youtu.be/meNiXcbPh_s) [Made](https://youtu.be/db3Cwlv-S-8?t=1624) Videos, since BepInEx is commonly used.

Next, download the latest release from the [releases page](/UKButt/UKButt/releases) and extract the dll files into **ULTRAKILL/BepInEx/plugins**.

Lastly, you need to install [Intiface Central](https://intiface.com/central/) from buttplug.io.

The mod connects to Intiface on port 12345 by default, which is the default port for Intiface Central as well.

## Running

Before starting the game itself, launch Intiface Central and start the Intiface Server with the big Start Server button.

![Intiface Central](images\intiface_central_PDJp72icP1.png)

That's where you can configure the server and the devices you want to use.

You might have to enable some devices in the settings, depending on what it is.

### ULTRAKILL

Next, you should be able to start the game and see Intiface Central connect to the game.

The mod should be functional now.

## Configuration

The mod uses ULTRAKILL's preference system and saves its configuration into the Steam Cloud-less **ULTRAKILL/Preferences/LocalPrefs.json** file,
with a `ukbutt.` prefix.

Preferably, you should change them using the in-game console available by pressing **F8**.

You can use the `ukbutt` command to list all available commands.

or `ukbutt prefs` to get the list of all available preferences and how to change them.

![Console](images\ULTRAKILL_xkNU4TP8PV.png)

## Default Values

<!-- table -->

| Key                        | Type     | Description                                                            | Default                |
| -------------------------- | -------- | ---------------------------------------------------------------------- | ---------------------- |
| `ukbutt.socketUri`         | `string` | The URI of the Intiface Server.                                        | `ws://localhost:12345` |
| `ukbutt.strength`          | `float`  | The strength of the vibration.                                         | `0.8`                  |
| `ukbutt.stickForSeconds`   | `float`  | The minimum duration of a vibration. (in seconds)                      | `2.0`                  |
| `ukbutt.stickForSeconds`   | `float`  | Same as above, but for events marked as subtle.                        | `0.2`                  |
| `ukbutt.useUnscaledTime`   | `bool`   | Whether to use unscaled (real) time for the duration of the vibration. | `false`                |
| `ukbutt.enableMenuHaptics` | `bool`   | Whether to enable haptics in the main menu.                            | `true`                 |
