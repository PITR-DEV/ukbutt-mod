# UKButt

Short link: [pitr.dev/ukbutt](https://pitr.dev/ukbutt)

UKButt is an official [BepInEx](https://github.com/BepInEx/BepInEx) mod for [ULTRAKILL](http://devilmayquake.com) that adds [buttplug.io](https://buttplug.io/) support.

## Resources

- **[Download Latest Release](https://github.com/PITR-DEV/ukbutt-mod/releases/latest)**
- **[Installation](#installation)**
- **[Running](#running)**
- **[Configuration](#configuration)**
- **[Default Values](#default-values)**
- **[Input Modes](#inputmodes)**
- **[The buttplug.io standard](https://buttplug.io/)**
- **[Buttplug.io/Intiface Discord Server (for support questions)](https://discord.buttplug.io)**

## Video

[![IMAGE ALT TEXT HERE](https://i3.ytimg.com/vi/6r13L1yvtYA/maxresdefault.jpg)](https://www.youtube.com/watch?v=6r13L1yvtYA)

## Installation

[Start by installing BepInEx 5.4.21](https://docs.bepinex.dev/articles/user_guide/installation/index.html)

You can also follow one of the [Community](https://youtu.be/meNiXcbPh_s) [Made](https://youtu.be/db3Cwlv-S-8?t=1624) Videos, since BepInEx is commonly used.

Next, download the latest release from the [release page](https://github.com/PITR-DEV/ukbutt-mod/releases/latest) and extract the dll files into **ULTRAKILL/BepInEx/plugins**.

Lastly, you need to install [Intiface Central](https://intiface.com/central/) from buttplug.io.

The mod connects to Intiface on port 12345 by default, which is the default port for Intiface Central as well.

## Running

Before starting the game itself, launch Intiface Central and start the Intiface Server with the big Start Server button.

![Intiface Central](https://github.com/PITR-DEV/ukbutt-mod/blob/master/images/intiface_central_PDJp72icP1.png?raw=true)

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

![Console](https://github.com/PITR-DEV/ukbutt-mod/blob/master/images/ULTRAKILL_xkNU4TP8PV.png?raw=true)

## Default Values

<!-- table -->

| Key                         | Type     | Description                                                             | Default                |
| --------------------------- | -------- | ----------------------------------------------------------------------- | ---------------------- |
| `ukbutt.socketUri`          | `string` | The URI of the Intiface Server.                                         | `ws://localhost:12345` |
| `ukbutt.strength`           | `float`  | The strength of the vibration.                                          | `0.8`                  |
| `ukbutt.stickForSeconds`    | `float`  | The minimum duration of a vibration. (in seconds)                       | `2.0`                  |
| `ukbutt.tapStickForSeconds` | `float`  | Same as above, but for events marked as subtle.                         | `0.2`                  |
| `ukbutt.useUnscaledTime`    | `bool`   | Whether to use unscaled (real) time for the duration of the vibration.  | `false`                |
| `ukbutt.enableMenuHaptics`  | `bool`   | Whether to enable haptics in the main menu.                             | `true`                 |
| `ukbutt.inputMode`          | `int`    | The current [InputMode](#inputmodes).                                   | `1`                    |
| `ukbutt.strokeWhileIdle`    | `bool`   | If in menu or rank == 0, stroke at lowest speed (rank mode only)        | `false`                |
| `ukbutt.linearPosMin`       | `float`  | Lowest position for stroker movement (rank mode only)                   | `0.1`                  |
| `ukbutt.linearPosMax`       | `float`  | Highest position for stroker movement (rank mode only)                  | `0.9`                  |
| `ukbutt.linearTimeMin`      | `float`  | Stroker frequency timing in seconds (rank at ULTRAKILL, rank mode only) | `0.3`                  |
| `ukbutt.linearTimeMax`      | `float`  | Stroker frequency timing in seconds (rank at None/Idle, rank mode only) | `1.5`                  |

## InputModes

<!-- table -->

| Index | Description        |
| ----- | ------------------ |
| `0`   | Nothing            |
| `1`   | Varied (_default_) |
| `2`   | Continuous Rank    |

You can change the input mode in-game by using `prefs set_local int ukbutt.inputMode <index>`

Mode interaction with hardware:

* Varied
  * Vibration will be triggered on certain actions, including shooting, doors, sliding, menu haptics (if set), etc...
  * No effect on strokers
* Continuous Rank
  * Vibration speed or stroker oscillation frequency is set by current style rank. The higher the
    rank, the faster the vibration or stroking.

## Support

If you have issues installing or using Intiface Central, you can either [visit the Buttplug.io discord](https://discord.buttplug.io) or [DM the @buttplugio twitter account](https://twitter.com/buttplugio).

If you need a list of hardware supported by Intiface Central, [visit IOSTIndex.com, which has a full list of supported hardware](https://iostindex.com/?filter0Availability=Available,DIY&filter1Connection=Digital&filter2ButtplugSupport=4).
