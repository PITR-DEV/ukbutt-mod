// <copyright file="ButtplugUnityClientOptions.cs" company="Nonpolynomial Labs LLC">
// Buttplug Unity Source Code File - Visit https://buttplug.io for more info about the project.
// Copyright (c) Nonpolynomial Labs LLC. All rights reserved.
// Licensed under the BSD 3-Clause license. See LICENSE file in the project root for full license information.
// </copyright>
using System;

namespace ButtplugUnity {

  public enum ButtplugUnityConnectorType {
    // Have the Buttplug Unity plugin start its own server process, using the
    // provided binary and websocket access.
    WebsocketServerProcess,
    // Expect that the user will be running either Intiface Desktop or Intiface
    // CLI, and connect there. Will require setting WebsocketAddress and
    // WebsocketPort.
    ExternalWebsocketServer,
    // Run an embedded server, meaning Buttplug code (including device
    // enumeration and access) will run inside the game process. Should only be
    // used for development, as this method does not allow easy updates of
    // Buttplug code. It may also interfere with the Unity process, and may
    // exhibit bugs inherent in the platform (i.e. bluetooth devices on windows
    // not closing properly.)
    Embedded,
  }

  public class ButtplugUnityOptions {
    // Client name. Not particularly useful unless you're connecting out to
    // Intiface Desktop, but if you are it will show up in the Intiface Desktop
    // GUI.
    public string ClientName = "Buttplug Unity Client";
    
    // If true, have our functions run the server process on connect. Otherwise,
    // assume we're connecting to Intiface Desktop.
    public ButtplugUnityConnectorType ConnectorType = ButtplugUnityConnectorType.WebsocketServerProcess;

    // If using our own server process, set the ping time. A value of 0 means
    // the ping checker is turned off. Ping time will make the server drop a
    // connection and shut down all devices if a ping isn't received in a
    // certain amount of time. The ButtplugClient class handles ping sending
    // automatically, but this is useful as a watchdog in case of thread locks,
    // crashes, etc.
    public uint ServerPingTime = 0;

    // Address of the server to connect to. Will usually just be localhost
    // unless you are for some reason connecting out to a remote server.
    public string WebsocketAddress = "localhost";

    // Port that the client should try to connect to. 0 implies that we're
    // running a server process internally (i.e. UseServerProcess is true), in
    // which case we'll just choose a random high number port.
    public ushort WebsocketPort = 0;

    // If true, this will cause ButtplugUnity's convenience functions to log to
    // the Unity console. Handy when things are going wrong.
    public bool OutputDebugMessages = false;

    // If true, will open a console window for the IntifaceCLI process when
    // using IL2CPP builds with the WebsocketServerProcess connector type. This
    // allows developers to see log messages being output by the console
    // program, since we don't have a way to print them into the Unity log
    // currently.
    public bool OpenIL2CPPConsoleWindow = false;

    // If true, allow devices to use Raw messages. This is dangerous, as it can
    // allow things like firmware uploading on some insecure devices. However,
    // it also allows for sending commands that may not be covered in Buttplug's
    // normal API. Defaults to false, only set this to true if you really need it.
    public bool AllowRawMessages = false;
  }
}