using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Threading;
using Terraria.Chat;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace AtlayasMod.Content.Tiles
{
    public class SelsaOre : ModTile
    {
        public override void SetStaticDefaults() {
            TileID.Sets.Ore[Type] = true;
            TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 410;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 975;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(152, 171, 198), name);

            DustType = 84;
            HitSound = SoundID.Tink;
        }

        public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
            sightColor = Color.Blue;
            return true;
        }
    }

    public class SelsaOreSystem : ModSystem
    {
        private bool spawnedSelsaOre = false;

        public override void OnWorldLoad()
        {
            spawnedSelsaOre = false;
        }

        public override void OnWorldUnload()
        {
            spawnedSelsaOre = false;
        }

        // Call this method from your Skeletron defeat logic (e.g., a GlobalNPC or BossDowned event)
        public void BlessWorldWithSelsaOre()
        {
            // Only spawn if Skeletron is defeated and we haven't spawned yet
            if (spawnedSelsaOre || !NPC.downedBoss3)
                return;

            spawnedSelsaOre = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText("The world has been blessed with Selsa Ore!", 50, 255, 130);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The world has been blessed with Selsa Ore!"), new Color(50, 255, 130));
                }

                int splotches = (int)(100 * (Main.maxTilesX / 4200f));
                int highestY = (int)Utils.Lerp(Main.rockLayer, Main.UnderworldLayer, 0.5);
                for (int iteration = 0; iteration < splotches; iteration++)
                {
                    int i = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                    int j = WorldGen.genRand.Next(highestY, Main.UnderworldLayer);
                    WorldGen.OreRunner(i, j, WorldGen.genRand.Next(5, 9), WorldGen.genRand.Next(5, 9), (ushort)ModContent.TileType<SelsaOre>());
                }
            });
        }
    }
}