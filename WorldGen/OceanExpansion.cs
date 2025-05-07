using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AtlayasMod.WorldGen
{
    public class OceanExpansion : ModSystem
    {
        public static bool AquilaDefeated = false; // Tracks whether Aquila has been defeated

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int index = tasks.FindIndex(pass => pass.Name.Equals("Shore"));

            if (index != -1)
            {
                tasks.Insert(index + 1, new PassLegacy("Expand Ocean", ExpandOcean));
            }
        }

        private void ExpandOcean(GenerationProgress progress, GameConfiguration config)
        {
            progress.Message = "Expanding the Ocean...";

            int oceanExpansion = Main.maxTilesX / 8; // Expands ocean by extra width (adjust as needed)
            int oceanLeft = 0;
            int oceanRight = Main.maxTilesX - 1;

            for (int x = oceanLeft; x < oceanLeft + oceanExpansion; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Main.tile[x, y].LiquidAmount = 255;
                    Main.tile[x, y].LiquidType = LiquidID.Water;
                }
            }

            for (int x = oceanRight - oceanExpansion; x < oceanRight; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Main.tile[x, y].LiquidAmount = 255;
                    Main.tile[x, y].LiquidType = LiquidID.Water;
                }
            }
        }

        public override void PostUpdateEverything()
        {
            if (!AquilaDefeated)
            {
                ApplyPoisonedWater();
            }
        }

        private void ApplyPoisonedWater()
        {
            foreach (Player player in Main.player)
            {
                if (player.active && player.position.Y > Main.worldSurface && IsPlayerInOcean(player))
                {
                    player.AddBuff(BuffID.Poisoned, 60); // Applies poison while in ocean
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was overwhelmed by toxic waters."), 5, 0); // Deals small damage per tick
                }
            }
        }

        private bool IsPlayerInOcean(Player player)
        {
            return player.position.X < Main.maxTilesX * 0.1f || player.position.X > Main.maxTilesX * 0.9f;
        }

        public override void PreUpdateWorld()
        {
            if (!AquilaDefeated)
            {
                Main.waterColor = new Color(50, 150, 50); // Sets ocean water to green poison color
            }
            else
            {
                Main.waterColor = new Color(0, 100, 255); // Restores default blue ocean water after Aquila is defeated
            }
        }
    }
}