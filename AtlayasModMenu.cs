using AtlayasMod.Content.Biome;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace AtlayasMod
{
    public class AtlayasModMenu : ModMenu
    {
        private const string menuAssetPath = "AtlayasMod/Assets/Textures/Menu";
        private const string BgAssetPath = "AtlayasMod/Assets/Textures/Backgrounds";
        private const int FrameCount = 6;
        private const int FrameDelay = 8; // Lower this value for faster animation,increase for slower animation

        private Asset<Texture2D> LogoTexture;
        private Asset<Texture2D> sunTexture;
        private Asset<Texture2D> moonTexture;
        private Asset<Texture2D>[] backgroundFrames;

        private int frameCounter = 0;
        private int currentFrame = 0;

        public override void Load()
        {
            LogoTexture = ModContent.Request<Texture2D>($"{menuAssetPath}/MainLogo");
            sunTexture = ModContent.Request<Texture2D>($"{menuAssetPath}/Sun");
            moonTexture = ModContent.Request<Texture2D>($"{menuAssetPath}/Moon");

            // Load all 6 background frames
            backgroundFrames = new Asset<Texture2D>[FrameCount];
            for (int i = 0; i < FrameCount; i++)
            {
                backgroundFrames[i] = ModContent.Request<Texture2D>($"{BgAssetPath}/Bg{i + 1}");
            }
        }

        public override void Unload()
        {
            backgroundFrames = null;
        }

        public override void OnSelected()
        {
            SoundEngine.PlaySound(SoundID.Thunder);
            frameCounter = 0;
            currentFrame = 0;
        }

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)// Learn from Star above mod
        {
            // Animate background
            frameCounter++;
            if (frameCounter >= FrameDelay)
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % FrameCount;
            }

            // Draw the current background frame, stretched to fill the screen (If you not do this,it will only fill 2/3 of the screen,and will look really awdward)
            spriteBatch.Draw(
                backgroundFrames[currentFrame].Value,
                new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                Color.White
            );

            // This should keep the logo centered and scaled properly (it kinda stales the logo,but right now i just want to make the dame game menu work)
            // For all other coders in the future,Change this if you want to make the logo look better,Or just remove it if you know how to make the entre menu look better
            if (LogoTexture?.IsLoaded == true)
            {
                Texture2D logo = LogoTexture.Value;
                Vector2 logoPos = new Vector2(Main.screenWidth / 2f, Main.screenHeight * 0.15f);
                Vector2 origin = new Vector2(logo.Width / 2f, logo.Height / 2f);
                spriteBatch.Draw(logo, logoPos, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            }

            // Prevent vanilla logo from drawing
            return false;
        }

        public override Asset<Texture2D> Logo => LogoTexture;
        public override Asset<Texture2D> SunTexture => sunTexture;// This is to hide the sun
        public override Asset<Texture2D> MoonTexture => moonTexture;// This is to hide the moon (if you have any questions,just look at thier sprite,you will understand why)

        public override string DisplayName => "AtlayasMod";
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Atlayas");
    }
}