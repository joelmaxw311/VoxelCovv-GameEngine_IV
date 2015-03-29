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
using System.IO;

namespace VoxelCovvEngine
{
    public class HoloWindow
    {
        public Vector3 position, direction;
        Texture2D tex, solidColor, cursorTex, select;
        List<State.Menu.Button> buttons = new List<State.Menu.Button>();
        SpriteFont font;
        public object eventReceiver;
        bool clicked = false;
		Fader transition = new Fader(12f),
            selectAnim = new Fader(8f);
        Fader bob = new Fader(48);
		bool closing = false;
        GraphicsDevice device;
        float closeDirection;

        public HoloWindow(Vector3 position, Vector3 direction, GraphicsDevice device, State state, string uilFile, object eventReceiver)
        {
            this.device = device;
            this.eventReceiver = eventReceiver;
            cursorTex = state.Content.Load<Texture2D>("crosshair");
            select = state.Content.Load<Texture2D>("select");
            this.position = position;
            this.direction = direction;
            this.direction.Y = 0;
            direction.Normalize();
            //this.tex = tex;
            solidColor = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            solidColor.SetData<Color>(new Color[] { Color.White });
            this.font = state.Content.Load<SpriteFont>("defaultFont");
            LoadUI(uilFile);
            float w = (float)tex.Bounds.Width / 2f, h = (float)tex.Bounds.Height / 2f;
            transition.Start(false);
            closing = false;
            bob.Start(true, true);
            selectAnim.Start(true, true);
        }

        public HoloWindow(Vector3 position, Vector3 direction, GraphicsDevice device, State state, string uilFile, object eventReceiver, Texture2D tex)
        {
            this.device = device;
            this.eventReceiver = eventReceiver;
            cursorTex = state.Content.Load<Texture2D>("crosshair");
            select = state.Content.Load<Texture2D>("select");
            this.position = position;
            this.direction = direction;
            this.direction.Y = 0;
            direction.Normalize();
            //this.tex = tex;
            solidColor = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            solidColor.SetData<Color>(new Color[] { Color.White });
            this.font = state.defaultFont;
            LoadUIFile(uilFile);
            this.tex = tex;
            float w = (float)tex.Bounds.Width / 2f, h = (float)tex.Bounds.Height / 2f;
            transition.Start(false);
            closing = false;
            bob.Start(true, true);
            selectAnim.Start(true, true);
        }

        void LoadUIFile(string file)
        {
            buttons = new List<State.Menu.Button>();
            StreamReader reader = new StreamReader(file);
            string data = reader.ReadToEnd();
            string[] buttonsData = data.Split(new char[] { ';' });
            foreach (string s in buttonsData)
            {
                string buttonData = s.Replace("\r", "").Replace("\n", "");
                if (buttonData.Length > 0)
                {
                    string[] parts = buttonData.Split(new char[] { ':' });
                    string[] boundParts = parts[0].Split(new char[] { ',' });
                    Rectangle bounds = new Rectangle(int.Parse(boundParts[0]), int.Parse(boundParts[1]), int.Parse(boundParts[2]), int.Parse(boundParts[3]));
                    State.Menu.Button b = new State.Menu.Button(bounds, parts[1], parts[2], parts[3]);
                    buttons.Add(b);
                }
            }
            reader.Close();
        }

        public void LoadUI(string file)
        {
            LoadUIFile(file);
            this.tex = CreateTexture(device);
        }

		public void Update(Camera camera, State state)
		{
			position = camera.position - direction;

			if (clicked)
				Click(camera);
			clicked = false;

			if (closing && transition.Fade(false) <= 0)
				state.window = null;
		}

        public void Draw(SpriteBatch spriteBatch, BasicEffect effect, VoxelWorld world, Camera camera)
        {
            //Texture2D menuTex = CreateTexture(spriteBatch.GraphicsDevice);
            //Vector3 pos = VCHelper.NearestTranslation(position, camera.position, world);
            effect.View = camera.RawView;
            Vector2 scale = new Vector2(0.7f, 0.7f);
            //effect.Parameters["WorldMatrix"].SetValue(Transformation);// Matrix.CreateWorld(pos, direction, Vector3.Down));
			float distance = 1f;
			if (!closing)
				distance = transition.Fade(false);
            Vector3 drawDir = direction;
            if (closing)
            {
                drawDir = Vector3.Transform(direction, Matrix.CreateRotationY((1f - transition.Fade(false)) * closeDirection * 2f));
            }
			effect.World = Matrix.CreateWorld((camera.position + new Vector3(0,-0.2f + bob.Fade(true) * 0.01f,0)) - (drawDir * distance * 0.75f), drawDir, UpAngle);//Transformation;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, effect);

            spriteBatch.Draw(tex, new Vector2(0f, 0f), tex.Bounds, new Color(1f,1f,1f,transition.Fade(true) * 0.5f), 0f, new Vector2(tex.Bounds.Width, tex.Bounds.Height) * 0.5f, scale / new Vector2(tex.Bounds.Width, tex.Bounds.Height), SpriteEffects.None, 0.00000001f);

            Vector2? clicked = Selection(camera);
            if (clicked.HasValue)
            {
                float selectSize = (selectAnim.Fade(true) / 8f) + 0.98f;
                foreach (State.Menu.Button b in buttons)
                {
                    if (VCHelper.InRect(clicked.Value, b.bounds))
                    {
                        Vector2 bpos = new Vector2(b.bounds.X + b.bounds.Width / 2, b.bounds.Y + b.bounds.Height / 2) * (scale / new Vector2(tex.Bounds.Width, tex.Bounds.Height)) - new Vector2(scale.X / 2, scale.Y / 2);
                        spriteBatch.Draw(select, bpos, select.Bounds, Color.White, 0f, new Vector2(select.Bounds.Width / 2, select.Bounds.Height / 2), (scale / new Vector2(tex.Bounds.Width, tex.Bounds.Height) / new Vector2(select.Width, select.Height) * new Vector2(b.bounds.Width, b.bounds.Height)) * (new Vector2(selectSize)), SpriteEffects.None, 0);
                        //Vector2 bpos = new Vector2(b.bounds.X, b.bounds.Y) * (scale / new Vector2(tex.Bounds.Width, tex.Bounds.Height)) + new Vector2(0,0);
                        //spriteBatch.Draw(tex, bpos, b.bounds, new Color(100, 100, 100, (int)(200 * transition.Fade(false) * 0.75f)), 0f, new Vector2(tex.Bounds.Width, tex.Bounds.Height) * 0.5f, scale / new Vector2(tex.Bounds.Width, tex.Bounds.Height), SpriteEffects.None, 0.00000001f);
                        //spriteBatch.Draw(select, bpos, select.Bounds, new Color(100, 100, 100, (int)(200 * transition.Fade(false) * 0.75f)), 0f, new Vector2(tex.Bounds.Width, tex.Bounds.Height) * 0.5f, scale / new Vector2(select.Bounds.Width, select.Bounds.Height), SpriteEffects.None, 0.00000001f);
                    }
                }
            }

            spriteBatch.End();
        }

        Texture2D CreateTexture(GraphicsDevice device)
        {
            var presParams = device.PresentationParameters.Clone();
            // Configure parameters for secondary graphics device
            GraphicsDevice graphics = device;//new GraphicsDevice(device.Adapter, GraphicsProfile.Reach, presParams);
            graphics.SetRenderTarget(null);
            RenderTarget2D rTarget = new RenderTarget2D(graphics, graphics.PresentationParameters.BackBufferWidth, graphics.PresentationParameters.BackBufferHeight);
            //graphics.Clear(Color.White);
            graphics.SetRenderTarget(rTarget);
            graphics.Clear(Color.Transparent);
            //graphics.Clear(Color.Transparent);
            SpriteBatch spriteBatch = new SpriteBatch(graphics);
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            //MouseState mouse = Mouse.GetState();
            //spriteBatch.Draw(solidColor, new Rectangle(0,0,graphics.PresentationParameters.BackBufferWidth, graphics.PresentationParameters.BackBufferHeight), new Color(0,0,180,130));
            foreach (State.Menu.Button b in buttons)
            {
                spriteBatch.Draw(solidColor, b.bounds, Color.Red);
                //if (VCHelper.InRect(new Point(mouse.X, mouse.Y), b.bounds))
                //{
                //    if (mouse.LeftButton == ButtonState.Pressed)
                //        spriteBatch.Draw(solidColor, b.bounds, new Color(90, 90, 130, 2));
                //    else
                //        spriteBatch.Draw(solidColor, b.bounds, new Color(100, 100, 100, 2));
                //}
                VCHelper.DrawFittedText(spriteBatch, b.bounds, font, Color.White, b.text);
            }
            spriteBatch.End();
            graphics.SetRenderTarget(null);
            
            return (Texture2D)rTarget;
        }

        public Matrix Transformation
        {
            get { return Matrix.CreateWorld(position, direction, UpAngle); }//CreateBillboard(position, position + direction, Vector3.Down, null); }
        }

        public void Click(Camera camera)
        {
            Vector2 clicked;
            {
                Vector2? c = Selection(camera);
                if (c.HasValue)
                {
                    clicked = c.Value;
                }
                else
                    return;
            }
            //(cursor + new Vector2(0.35f)) * new Vector2(tex.Bounds.Width, tex.Bounds.Height);
            foreach (State.Menu.Button b in buttons)
            {
                if (VCHelper.InRect(clicked, b.bounds))
                {
                    if (b.parameter == "")
                        VCHelper.CallEvent(eventReceiver, b.method, null);
                    else
                        VCHelper.CallEvent(eventReceiver, b.method, new object[] { b.parameter });
                }
            }
        }

        public void MOUSE_RELEASED_LEFT(Vector2 position)
        {
            clicked = true;
        }

        public bool LookingAt(Camera camera)
        {
            Vector2? sel = Selection(camera);
            return (sel.HasValue);
        }

        public Vector2? Selection(Camera camera)
        {
            Vector3 camPos = new Vector3(0, 0.2f, 0);
            Vector3 camAngle = camera.Angle;
            float planeAngle = (float)(Math.Atan2(direction.X, direction.Z));
            camAngle = Vector3.Transform(camAngle, Matrix.CreateRotationY(-planeAngle));
            camAngle.Normalize();
            Plane plane = new Plane((new Vector3(0,0,1)),0.75f);
            Ray ray = new Ray(camPos, camAngle);
            float? dist = ray.Intersects(plane);
            if (dist.HasValue)
            {
                Vector3 sect = camAngle * dist.Value + camPos;
                Vector2 offr = (new Vector2(sect.X, sect.Y) / (new Vector2(0.35f) / new Vector2(tex.Bounds.Width / 2, tex.Bounds.Height / 2)));
                Vector2 r = new Vector2(tex.Bounds.Width / 2, tex.Bounds.Height / 2) + offr;
                r.Y = tex.Height - r.Y;
                if (r.X >= 0 && r.Y >= 0 && r.X <= tex.Width && r.Y <= tex.Height)
                {
                    foreach (State.Menu.Button b in buttons)
                    {
                        if (VCHelper.InRect(r, b.bounds))
                        {
                            return r;
                        }
                    }
                }
            }
            return null;
        }

        Vector3 RightAngle
        {
            get
            {
             float xrot = (float)Math.Atan2(direction.Z, direction.X) + MathHelper.PiOver2;
             float yrot = (float)Math.Atan2(direction.Y, Vector2.Distance(new Vector2(direction.X, direction.Z), Vector2.Zero));
             float radius = 1;
             return -(new Vector3(radius * (float)Math.Cos((float)MathHelper.ToRadians(xrot)) * (float)Math.Cos((float)MathHelper.ToRadians(yrot)),
             radius * (float)Math.Sin((float)MathHelper.ToRadians(yrot - 90)),
             radius * (float)Math.Sin((float)MathHelper.ToRadians(xrot)) * (float)Math.Cos((float)MathHelper.ToRadians(yrot))));
            }
        }

        public Vector3 UpAngle
        {
            get
            {
                float xrot = (float)Math.Atan2(direction.Z, direction.X) + MathHelper.PiOver2;
                float yrot = (float)Math.Atan2(direction.Y, Vector2.Distance(new Vector2(direction.X, direction.Z), Vector2.Zero));
                float radius = 1;
                return -(new Vector3(radius * (float)Math.Cos((float)MathHelper.ToRadians(xrot + 90)) * (float)Math.Cos((float)MathHelper.ToRadians(yrot + 90)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(yrot + 90)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(xrot + 90)) * (float)Math.Cos((float)MathHelper.ToRadians(yrot + 90))));
            }
        }

        Vector3 LeftAngle
        {
            get
            {
                float xrot = (float)Math.Atan2(direction.Z, direction.X) + MathHelper.PiOver2;
                float yrot = (float)Math.Atan2(direction.Y, Vector2.Distance(new Vector2(direction.X, direction.Z), Vector2.Zero));
                float radius = 1;
                return -(new Vector3(radius * (float)Math.Cos((float)MathHelper.ToRadians(xrot + 180)) * (float)Math.Cos((float)MathHelper.ToRadians(yrot)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(yrot - 90)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(xrot + 180)) * (float)Math.Cos((float)MathHelper.ToRadians(yrot))));
            }
        }

		public void Close()
        {
            Random random = new Random();
            int xr = random.Next(2);
            if (xr == 0)
                xr = -1;
            closeDirection = xr;
			closing = true;
			transition.Start(true);
		}

        public void Close(Camera camera)
        {
            float rotation = /*MathHelper.WrapAngle*/((float)Math.Atan2(direction.Z, direction.X));
            if (rotation < 0)
                rotation = MathHelper.TwoPi;
            if (rotation > MathHelper.TwoPi)
                rotation = 0;
            float camAngle = /*MathHelper.WrapAngle*/(camera.Rotation.X);
            if (camAngle < 0)
                camAngle = MathHelper.TwoPi;
            if (camAngle > MathHelper.TwoPi)
                camAngle = 0;
            closeDirection = Math.Sign(rotation - camAngle);
            closing = true;
            transition.Start(true);
        }
    }
}
