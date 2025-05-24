using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace AtlayasMod.Common.Systems
{
    public class HurricaneEvent : ModSystem
    {
        public static bool IsHurricaneActive => ModContent.GetInstance<HurricaneEvent>().hurricaneActive;
        public static int WrathOfTheWindMusicSlot => wrathOfTheWindMusicSlot;
        private bool hurricaneActive = false;
        private int hurricaneDuration = 0;
        private int thunderTimer = 0;
        private const int HurricaneLength = 60 * 60 * 2; // 2 min
        private static int wrathOfTheWindMusicSlot = -1;

        #region Custom Particle System (commented out for now)
        /*
        // Custom Particle system
        private struct HurricaneParticle
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Alpha;
            public float Scale;
            public float Life;
        }
        private static List<HurricaneParticle> particles = new();
        private static Texture2D particleTexture;
        */
        #endregion

        public override void Load()
        {
            if (!Main.dedServ)
            {
                // Register music
                wrathOfTheWindMusicSlot = MusicLoader.GetMusicSlot(Mod, "Assets/WrathOfTheWind");
                /*
                // Load particle texture (fallback to MagicPixel if not found)
                if (ModContent.HasAsset("AtlayasMod/Assets/Images/HurricaneParticle"))
                    particleTexture = ModContent.Request<Texture2D>("AtlayasMod/Assets/Images/HurricaneParticle").Value;
                else
                    particleTexture = TextureAssets.MagicPixel.Value;
                */
            }
        }

        public override void Unload()
        {
            /*
            particleTexture = null;
            particles.Clear();
            */
        }

        public override void OnWorldLoad()
        {
            hurricaneActive = false;
            hurricaneDuration = 0;
            thunderTimer = 0;
            /*
            particles.Clear();
            */
        }

        public override void PreUpdateWorld()
        {
            // 30% chance each day at dawn (4:30 AM in Terraria)
            if (Main.time == 0 && !hurricaneActive && Main.dayTime)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && player.ZoneBeach && Main.rand.NextFloat() < 0.3f)
                    {
                        StartHurricane();
                        break;
                    }
                }
            }

            if (hurricaneActive)
            {
                hurricaneDuration--;
                if (hurricaneDuration <= 0)
                {
                    EndHurricane();
                }
            }
        }

        private void StartHurricane()
        {
            hurricaneActive = true;
            hurricaneDuration = HurricaneLength;
            Main.NewText("A hurricane is raging at the ocean!", Color.LightGray);

            // Play hurricane SFX
            SoundStyle hurricaneSound = new SoundStyle("AtlayasMod/Assets/Sfx/HurricaneSfx")
            {
                Volume = 1.0f,
                Pitch = 0.0f,
                PitchVariance = 0.1f,
                IsLooped = true
            };
            SoundEngine.PlaySound(hurricaneSound, Main.LocalPlayer.position);

            thunderTimer = Main.rand.Next(30, 90);
        }

        private void EndHurricane()
        {
            hurricaneActive = false;
        }

        public override void PostUpdateWorld()
        {
            if (!hurricaneActive)
                return;

            Player player = Main.LocalPlayer;
            if (!player.active || !player.ZoneBeach)
                return;

            // Thunder spawn logic
            thunderTimer--;
            if (thunderTimer <= 0)
            {
                SpawnThunder(player);
                thunderTimer = Main.rand.Next(30, 90);
            }

            // --- VANILLA BLIZZARD PARTICLE EFFECT ---
            // i use vanilla blizzard dust as placeholder for hurricane wind. For a custom effect, replace with your own dust type. if you want to further customize it,
            // uncomment the old particle system code and modify it to your liking.
            for (int i = 0; i < 6; i++) // Adjust count for density
            {
                Vector2 spawnPos = new Vector2(
                    Main.screenPosition.X + Main.rand.Next(Main.screenWidth),
                    Main.screenPosition.Y + Main.rand.Next(Main.screenHeight)
                );
                Dust d = Dust.NewDustDirect(spawnPos, 1, 1, DustID.Ice,
                    Main.windSpeedCurrent * 12f + Main.rand.NextFloat(2f, 6f),
                    Main.rand.NextFloat(-1f, 1f),
                    150,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
            }

            #region Custom Particle System (commented out for now)
            /*
            // Particle logic
            // Spawn new particles
            if (Main.rand.NextBool(2))
            {
                HurricaneParticle p = new HurricaneParticle
                {
                    Position = new Vector2(Main.rand.Next(0, Main.screenWidth), Main.rand.Next(0, Main.screenHeight)),
                    Velocity = new Vector2(Main.windSpeedCurrent * 16f + Main.rand.NextFloat(6f, 12f), Main.rand.NextFloat(-1f, 1f)),
                    Alpha = Main.rand.NextFloat(0.3f, 0.7f),
                    Scale = Main.rand.NextFloat(0.7f, 1.2f),
                    Life = Main.rand.NextFloat(20f, 40f)
                };
                particles.Add(p);
            }

            // Update particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                HurricaneParticle p = particles[i];
                p.Position += p.Velocity;
                p.Life--;
                if (p.Life <= 0 || p.Position.X < 0 || p.Position.X > Main.screenWidth || p.Position.Y < 0 || p.Position.Y > Main.screenHeight)
                {
                    particles.RemoveAt(i);
                }
                else
                {
                    particles[i] = p;
                }
            }
            */
            #endregion
        }

        private void SpawnThunder(Player player)
        {
            Vector2 strikePos = player.Center + new Vector2(Main.rand.Next(-600, 600), -400);
            int proj = Projectile.NewProjectile(
                null,
                strikePos,
                new Vector2(0, 10),
                ProjectileID.VortexVortexLightning,
                60,
                6f,
                Main.myPlayer
            );
            Main.projectile[proj].hostile = true;
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (hurricaneActive && Main.LocalPlayer.active && Main.LocalPlayer.ZoneBeach)
            {
                backgroundColor = Color.Black;
                tileColor = Color.Black;
            }
        }

        public override void ModifyScreenPosition()
        {
            if (hurricaneActive && Main.LocalPlayer.active && Main.LocalPlayer.ZoneBeach)
            {
                Main.screenPosition += new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            #region Custom Particle System (commented out for now)
            /*
            if (hurricaneActive && Main.LocalPlayer.active && Main.LocalPlayer.ZoneBeach && particleTexture != null)
            {
                foreach (var p in particles)
                {
                    Color color = Color.White * p.Alpha;
                    spriteBatch.Draw(
                        particleTexture,
                        p.Position,
                        null,
                        color,
                        0f,
                        new Vector2(particleTexture.Width / 2f, particleTexture.Height / 2f),
                        p.Scale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
            */
            #endregion
        }
    }
}