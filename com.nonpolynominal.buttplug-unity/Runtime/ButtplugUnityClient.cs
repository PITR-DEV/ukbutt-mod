// <copyright file="ButtplugUnityClient.cs" company="Nonpolynomial Labs LLC">
// Buttplug Unity Source Code File - Visit https://buttplug.io for more info about the project.
// Copyright (c) Nonpolynomial Labs LLC. All rights reserved.
// Licensed under the BSD 3-Clause license. See LICENSE file in the project root for full license information.
// </copyright>

using Buttplug;
using System;
using AOT;

// IL2CPP Unity requires special tags on callbacks sent to native libraries, so
// we have to wrap our sorter method here. That's really all this does,
// otherwise it's a plain ol' ButtplugClient.
namespace ButtplugUnity
{
  public class ButtplugUnityClient : Buttplug.ButtplugClient
  {
    public ButtplugUnityClient(string aClientName) : base(aClientName, ButtplugUnityClient.ContextCallback)
    {

    }

    [MonoPInvokeCallback(typeof(ButtplugCallback))]
    static protected void ContextCallback(IntPtr ctx, IntPtr buf, int buf_length)
    {
      ButtplugClient.StaticSorterCallback(ctx, buf, buf_length);
    }
  }

}