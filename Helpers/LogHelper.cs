using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AtlayasMod.Helpers
{
    /// <summary>
    /// Class that simplifies logging in the mod.
    /// Example Usage: Log.Info("Your message here");
    /// </summary>
    public static class Log
    {
        private static DateTime lastLogTime = DateTime.UtcNow;

        private static Mod ModInstance
        {
            // try catch get
            get
            {
                try
                {
                    return ModLoader.GetMod("AtlayasMod");
                }
                catch (Exception ex)
                {
                    Error("Error getting mod instance: " + ex.Message);
                    return null;
                }
            }
        }

        /// <summary> Log a message once every 3 second </summary>
        public static void SlowInfo(string message, int seconds = 3, [CallerFilePath] string callerFilePath = "")
        {
            //Config c = ModContent.GetInstance<Config>();
            //if (c != null && Conf.C.LogToLogFile == false) return;

            // Extract the class name from the caller's file path.
            string className = Path.GetFileNameWithoutExtension(callerFilePath);
            var instance = ModInstance;
            if (instance == null || instance.Logger == null)
                return; // Skip logging if the mod is unloading or null

            // Use TimeSpanFactory to create a 3-second interval.
            TimeSpan interval = TimeSpan.FromSeconds(seconds);
            if (DateTime.UtcNow - lastLogTime >= interval)
            {
                // Prepend the class name to the log message.
                instance.Logger.Info($"[{className}] {message}");
                lastLogTime = DateTime.UtcNow;
            }
        }

        public static void Info(string message, [CallerFilePath] string callerFilePath = "")
        {
            //Config c = ModContent.GetInstance<Config>();
            //if (c != null && Conf.C.LogToLogFile == false) return;

            // Extract the class name from the caller's file path.
            string className = Path.GetFileNameWithoutExtension(callerFilePath);
            var instance = ModInstance;
            if (instance == null || instance.Logger == null)
                return; // Skip logging if the mod is unloading or null

            // Prepend the class name to the log message.
            instance.Logger.Info($"[{className}] {message}");
        }

        public static void Warn(string message)
        {
            //Config c = ModContent.GetInstance<Config>();
            //if (c != null && Conf.C.LogToLogFile == false) return;

            var instance = ModInstance;
            if (instance == null || instance.Logger == null)
                return; // Skip logging if the mod is unloading or null

            instance.Logger.Warn(message);
        }

        public static void Error(string message)
        {
            //Config c = ModContent.GetInstance<Config>();
            //if (c != null && Conf.C.LogToLogFile == false) return;

            var instance = ModInstance;
            if (instance == null || instance.Logger == null)
                return; // Skip logging if the mod is unloading or null

            instance.Logger.Error(message);
        }
    }
}
