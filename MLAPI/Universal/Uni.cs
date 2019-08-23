﻿using System;
using System.Timers;
using MLAPI.Filing.Logging;
using MLAPI.Networking;
using MLAPI.Networking.Messages;
using MLAPI.Networking.Server;
using MLAPI.Time;
using MLAPI.Util.Reusable;
using MLAPI.World.Data.Disk;
using MLAPI.World.Data.Disk.DataStorage;

namespace MLAPI.Universal
{
    /// <summary>
    /// Holds some universal events.
    /// </summary>
    public static class Uni
    {
        /// <summary>
        /// The tick the client is executing.
        /// </summary>
        public static UInt64 GameTick { get; private set; }

        /// <summary>
        /// The event that is raised when the game ticks.
        /// </summary>
        public static event EventHandler<UInt64> TickEvent;

        private static Timer TickTimer = new Timer(50);

        /// <summary>
        /// The timer counting down the time between auto-saves.
        /// </summary>
        private static TickTimer AutoSave = new TickTimer(RealTime.HalfHour);

        /// <summary>
        /// Raised when the game is exiting, and done unloading.
        /// </summary>
        public static event EventHandler GameExit;

        /// <summary>
        /// Raised to trigger the capture of an entire dimension to a screenshot.
        /// </summary>
        public static event EventHandler<Guid> ScreenshotDimension;

        static Uni()
        {
            TickTimer.Elapsed += TickTimer_Elapsed;
            TickTimer.AutoReset = true;
            TickTimer.Start();
        }

        private static void TickTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (World.Data.World.Mode == EngineMode.ServerOnly)
            {
                ServerSendRecieve.SendAll(new ServerTickMessage());
            }
            else
            {
                Uni.Tick(Uni.GameTick + 1);
            }

            if (AutoSave.Allow())
            {
                WorldStorage.AutoSave(WorldStorage.SaveName, new WorldDiskSink());
            }
        }

        /// <summary>
        /// Raises the world generated event.
        /// </summary>
        /// <param name="e"></param>
        internal static void GameExitHandler()
        {
            EventHandler handler = GameExit;
            handler?.Invoke(null, null);
        }

        /// <summary>
        /// How the server handles tick messages.
        /// </summary>
        /// <param name="msg"></param>
        internal static void Tick(ServerTickMessage msg)
        {
            Tick(msg.TickSent);
        }

        internal static void Tick(UInt64 tickSentAt)
        {
            UInt64 ticksBehind = tickSentAt - GameTick - 1;

            if (ticksBehind != 0)
            {
                MasterLog.DebugWriteLine("Running " + ticksBehind.ToString() + " tick(s) behind");
            }

            TickLoop(ticksBehind + 1);
        }

        private static void TickLoop(UInt64 times)
        {
            UInt64 i = 0;

            while (i != times)
            {
                GameTick++;

                TickEvent?.Invoke(null, GameTick);
                i++;
            }
        }

        private static void OnScreenshotDimension(Guid dimension)
        {
            ScreenshotDimension?.Invoke(null, dimension);
        }
    }
}