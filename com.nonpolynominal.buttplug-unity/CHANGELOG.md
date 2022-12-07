# 2.1.0 (2021-05-10)

## Features

- Add in binary files
- Reorganize files to support full Unity-compatible example project
- Improve example C# script with intensity slider and connected device list in Inspector

# 2.0.0 (2021-04-24)

## Features

- Allow choice of connector type: Internal/External Process or Embedded
- Allow using Raw Messages

## Bugfixes

- Update to Buttplug C# v2.0.1
  - Fixes issue with RawWriteCmd JSON Schema
  - Sorter now throws ButtplugConnectorException in live tasks on shutdown, similar to having a
    connector disconnect.

# 1.0.0 (2021-04-22)

## Features

- Move from using Buttplug C# to Buttplug Rust v3.x
  - Buttplug C# is dead anyways.
- Fix system to work with Mono and IL2CPP, support back to Unity 2018
  - Unity 2018-2019 will have slight delays in IL2CPP connections due to Mono weirdness, but should
    work fine otherwise.

# 0.1.0 (2020-06-19)

## Features

- Reduced version requirements to Unity 2018.2
- Added ButtplugUnityHelper for client bringup and server management
- Simplified and fixed example code

# 0.0.1 (2020-06-18)

## Features

- First release of Unity Package
- Current feature list:
  - Exposes executable and client API
  - Copies/updates executable to assets automatically
