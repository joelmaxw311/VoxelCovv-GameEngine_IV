using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VoxelCovvEngine
{
    public delegate float PercentValue();
    
    public class HUDVisor
    {
        VertexPositionColorTexture[] visorVertices;
        VertexBuffer buffer;
        GraphicsDevice device;
        Texture2D solidColor, icon;
        Texture2D hudTexture, barTexture, barBackTexture, xpBarTexture, reticle;
        HUDElement[] elements;
        SpriteFont font;
        
        public HUDVisor(GraphicsDevice device, Texture2D icon, State state, HUDElement[] elements)
        {
            this.elements = elements;
            this.device = device;

            solidColor = new Texture2D(device, 1, 1);
            solidColor.SetData<Color>(new Color[] {Color.White});

            List<VertexPositionColorTexture> verts = new List<VertexPositionColorTexture>();
                ModelBuilder model = new ModelBuilder(), model2 = new ModelBuilder();
            //float angleMax = 45f, y = -0.15f, radius = 0.3f, xoffset = 64, height = 0.03125f, increment = 0.5f, width = 0.5f;
                float angleMax = 90f, y = -0.3f, radius = 0.3f, xoffset = -45, height = 0.6f, increment = 1, width = 1.35f;
            barTexture = state.Content.Load<Texture2D>("HPbar\\hpBar");
            barBackTexture = state.Content.Load<Texture2D>("HPbar\\hpBarBack");
            xpBarTexture = state.Content.Load<Texture2D>("HPbar\\xpBar");
            reticle = state.Content.Load<Texture2D>("crosshair");
            model = new ModelBuilder();
            for (float x = angleMax; x >= 0; x -= increment)
            {
                model.AddPlane(
                    new VertexPositionColorTexture(-Vector3.Transform(new Vector3(0, 0f + y, radius), Matrix.CreateRotationY((float)MathHelper.ToRadians((x + xoffset) * width))), Color.White, new Vector2(1f - (1f / angleMax) * (x - 1), 0f)),
                    new VertexPositionColorTexture(-Vector3.Transform(new Vector3(0, 0f + y, radius), Matrix.CreateRotationY((float)MathHelper.ToRadians((x + increment + xoffset) * width))), Color.White, new Vector2(1f - (1f / angleMax) * (x), 0f)),
                    new VertexPositionColorTexture(-Vector3.Transform(new Vector3(0, height + y, radius), Matrix.CreateRotationY((float)MathHelper.ToRadians((x + increment + xoffset) * width))), Color.White, new Vector2(1f - (1f / angleMax) * (x), 1f)),
                    new VertexPositionColorTexture(-Vector3.Transform(new Vector3(0, height + y, radius), Matrix.CreateRotationY((float)MathHelper.ToRadians((x + xoffset) * width))), Color.White, new Vector2(1f - (1f / angleMax) * (x - 1), 1f))
                    );
            }
            visorVertices = model.Vertices;
            buffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), visorVertices.Length, BufferUsage.WriteOnly);
            buffer.SetData<VertexPositionColorTexture>(visorVertices);
            font = state.Content.Load<SpriteFont>("defaultFont");

            CreateHUD();
            //buffer2 = new VertexBuffer(device, typeof(VertexPositionColorTexture), barVertices.Length, BufferUsage.WriteOnly);
        }

        public void CreateHUD()
        {
            //device.SetRenderTarget(null);
            RenderTarget2D target = new RenderTarget2D(device, 1024, 1024);
            device.SetRenderTarget(target);
            device.Clear(Color.Transparent);
            SpriteBatch spriteBatch = new SpriteBatch(device);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            //spriteBatch.Draw(solidColor, new Rectangle(0, 0, 32,32), Color.Green);
            foreach (HUDElement element in elements)
            {
                int y = element.Row * 32;
                spriteBatch.Draw(barBackTexture, new Rectangle(64, 256 + y, 256, 32), Color.White);
                VCHelper.DrawFittedText(spriteBatch, new Rectangle(64 + 11, 256 + 1 + y, 76, 20), font, Color.White, "MooCovv");
                VCHelper.DrawFittedText(spriteBatch, new Rectangle(64 + 10, 256 + 23 + y, 25, 9), font, Color.White, "Lv");
                VCHelper.DrawFittedText(spriteBatch, new Rectangle(64 + 35, 256 + 23 + y, 25, 9), font, Color.White, "100");
                float hp = element.Health, xp = element.XP;
                spriteBatch.Draw(barTexture, new Rectangle(64 + (int)(90 + 153 * hp), 256 + y, (int)(256 - 153 * hp), 32), new Rectangle((int)(90 + 153 * hp), 0, (int)(256 - 153 * hp), 32), BarColor(hp));//new Rectangle(64 + 126 * (int)(256f / (float)barTexture.Bounds.Width), 256, 256, 64), new Rectangle(126 + (int)(211 * hp), 0, (int)(337 - 211 * hp), barTexture.Bounds.Height), Color.White);       
                spriteBatch.Draw(xpBarTexture, new Rectangle(64 + (int)(171 + 68 * hp), 256 + y, (int)(239 - 68 * hp), 32), new Rectangle((int)(171 + 68 * hp), 0, (int)(239 - 68 * hp), 32), Color.Lime);//new Rectangle(64 + 126 * (int)(256f / (float)barTexture.Bounds.Width), 256, 256, 64), new Rectangle(126 + (int)(211 * hp), 0, (int)(337 - 211 * hp), barTexture.Bounds.Height), Color.White);       
                //spriteBatch.Draw(barTexture, new Rectangle(16 + (int)(64 * xp), 64, 64 - (int)(64 * xp), 16), new Rectangle((int)(barTexture.Width * xp), 0, barTexture.Width - (int)(barTexture.Width * xp), barTexture.Height), BarColor(xp));
                spriteBatch.Draw(reticle, new Rectangle(500, 500, 24, 24), Color.White);
            }
            spriteBatch.End();
            hudTexture = target;
            device.SetRenderTarget(null);
        }

        public void Draw(BasicEffect effect, Texture2D texture, RectangleF bounds, Camera camera)
        {
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = true;
            //effect.Alpha = 0.6f;
            device.DepthStencilState = DepthStencilState.Default;
            //device.RasterizerState = new RasterizerState() {CullMode = CullMode.None,FillMode = FillMode.WireFrame};
            effect.Alpha = 0.75f;
            //device.DepthStencilState = DepthStencilState.None;
            device.SamplerStates[0] = SamplerState.PointClamp;
            effect.World = Matrix.Identity;
            //if (camera.rotation.Y < -45)
            effect.World *= Matrix.CreateRotationX(-camera.Rotation.Y);
            effect.World *= Matrix.CreateRotationY(-camera.Rotation.X);
            effect.World *= Matrix.CreateTranslation(camera.position + camera.ViewOffset);
            effect.View = camera.View;//Matrix.CreateLookAt(Vector3.Zero, new Vector3(camera.Angle.X, camera.Angle.Y, camera.Angle.Z), Vector3.Up);

            buffer.SetData(visorVertices);
            device.SetVertexBuffer(buffer);
            effect.Texture = hudTexture;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, visorVertices.Length / 3);
            }

            effect.Alpha = 1;
            effect.DiffuseColor = Vector3.One;
        }

        Color BarColor(float percent)
        {
            float pc = 100f - (percent * 100f);
            if (pc > 50)
                return Color.LimeGreen;
            if (pc > 10)
                return Color.Yellow;
            return Color.Red;
        }
    }

    public class HUDElement
    {
        PercentValue health, xp;
        int row;

        public HUDElement(string name, PercentValue health, PercentValue xp, int row)
        {
            this.health = health;
            this.xp = xp;
            this.row = row;
        }

        public float Health
        {
            get { return health(); }
        }

        public float XP
        {
            get { return xp(); }
        }

        public int Row
        {
            get { return row; }
        }
    }
}
