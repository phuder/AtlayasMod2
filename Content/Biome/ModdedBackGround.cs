using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace AtlayasMod.Content.Biome
// 100% the code of example mod, just modified to fit the project XD
{
    public class AtlayaSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
    {
        private static int frameCounter = 0;
        private static int currentFrame = 0;
        private const int totalFrames = 6;
        private const int frameDelay = 8; // Lower = faster animation

        // Animate all layers with the same frame for a full-screen effect
        private int GetAnimatedFrame()
        {
            frameCounter++;
            if (frameCounter >= frameDelay)
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % totalFrames;
            }
            return currentFrame;
        }

        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                        fades[i] = 1f;
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                        fades[i] = 0f;
                }
            }
        }

        public override int ChooseFarTexture()
        {
            int frame = GetAnimatedFrame();
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, $"Assets/Textures/Backgrounds/Bg6");
        }

        public override int ChooseMiddleTexture()
        {
            int frame = GetAnimatedFrame();
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, $"Assets/Textures/Backgrounds/Bg{frame + 1}");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            int frame = GetAnimatedFrame();
            scale = 1f;
            parallax = 0.0;
            a = 0f;
            b = 0f;
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, $"Assets/Textures/Backgrounds/Bg1");
        }
    }
}