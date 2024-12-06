using System;
using UnityEngine;

namespace SkyExtensions.Awaitable
{
    public static class AwaitableExtensions
    {

        public static async UnityEngine.Awaitable WaitUntilAsync(Func<bool> predicate)
        {
            while (!predicate()) 
                await UnityEngine.Awaitable.NextFrameAsync();
        }
    }
}