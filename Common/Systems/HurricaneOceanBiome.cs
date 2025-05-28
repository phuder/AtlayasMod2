using Terraria;
using Terraria.ModLoader;
using AtlayasMod;

namespace AtlayasMod.Common.Systems
{
    public class HurricaneOceanBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/WrathOfTheWind");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsBiomeActive(Player player)
        {
            // Only active if hurricane is running and player is in the ocean biome
            return HurricaneEvent.IsHurricaneActive && player.active && player.ZoneBeach;
        }
    }
}