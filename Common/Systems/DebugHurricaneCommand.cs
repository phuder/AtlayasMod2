using Terraria.ModLoader;
using Terraria;

namespace AtlayasMod.Common.Systems
{
    public class DebugHurricaneCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "hurricane";
        public override string Description => "Instantly starts the hurricane event (debug only)";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            // Get the HurricaneEvent system and start the event
            if (ModContent.GetInstance<HurricaneEvent>() is HurricaneEvent hurricane)
            {
                // Only start if not already active
                var hurricaneField = typeof(HurricaneEvent).GetField("hurricaneActive", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (hurricaneField != null && !(bool)hurricaneField.GetValue(hurricane))
                {
                    var startMethod = typeof(HurricaneEvent).GetMethod("StartHurricane", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    startMethod?.Invoke(hurricane, null);
                    Main.NewText("Hurricane event started (debug).");
                }
                else
                {
                    Main.NewText("Hurricane event is already active.");
                }
            }
        }
    }
}