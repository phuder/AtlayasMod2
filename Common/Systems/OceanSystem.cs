using ModLiquidLib.ModLoader;
using Terraria.ID;

namespace AtlayasMod.Common.Systems
{
    public class OceanSystem : GlobalLiquid
    {
        public OceanSystem()
        {
            Log.Info("Calling OceanSystem()");
        }

        public override bool UpdateLiquid(int i, int j, int type, Liquid liquid)
        {
            Log.Info("Calling UpdateLiquid");
            return base.UpdateLiquid(i, j, type, liquid);
        }

        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
        {
            Log.Info("Calling ModifyLight");

            if (type == LiquidID.Water)
            {
                Log.Info("Calling ModifyLight for water");
                r = 1.0f;
                g = 0f;
                b = 0f;
            }
            //base.ModifyLight(i, j, type, ref r, ref g, ref b);
        }

    }
}
