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
    /// <summary>
    /// A game state for organizing game flow.
    /// </summary>
    public class State
    {
        public Camera camera;
        public VoxelWorld world;
        public HoloWindow window;

        protected StateMachine machine;
        protected Texture2D colorTex;
        public SpriteFont defaultFont;
        protected SpriteBatch spriteBatch;
        BasicEffect effect;

        VertexProcessor vProcessor;
        LightEngine lProcessor;

        VertexPositionColorNormalTexture[] verts;
        VertexPositionColorNormalTexture[] newVerts = new VertexPositionColorNormalTexture[0];

        protected Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), 800f / 480f, 0.02f, 32f);

        public State(StateMachine machine)
        {
            this.machine = machine;
            camera = new Camera(Vector3.Zero, game);
            vProcessor = new VertexProcessor();
            lProcessor = new LightEngine(UpdateLight);
            effect = new BasicEffect(GraphicsDevice) { VertexColorEnabled = true, TextureEnabled = true, World = Matrix.Identity, View = camera.View, Projection = projection };
        }

        public ContentManager Content
        {
            get { return machine.Content; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return machine.Game.GraphicsDevice; }
        }

        public Game game
        {
            get { return machine.Game; }
        }

        public void ChangeState(State state)
        {
            machine.ChangeState(state);
        }

        public void AddState(State state)
        {
            machine.AddState(state);
        }

        public virtual void EventCalled(string method, object[] args)
        {
            if (!(window != null && method.Contains("MOUSE")))
                VCHelper.CallEvent(camera, method, args);
            if (window != null)
                VCHelper.CallEvent(window, method, args);
        }

        public virtual void Load()
        {
            colorTex = new Texture2D(GraphicsDevice, 1, 1);
            colorTex.SetData<Color>(new Color[] { Color.White });
            spriteBatch = new SpriteBatch(GraphicsDevice);

            defaultFont = Content.Load<SpriteFont>("defaultFont");

            lProcessor.Start();
            vProcessor.Start(new VertexPackage(this, world, new Point((int)camera.position.X, (int)camera.position.Z)));
        }

        public virtual void Unload()
        {
            vProcessor.Stop();
            lProcessor.Stop();
        }

        public virtual void Update()
        {
            if (window != null)
                window.Update(camera, this);
            camera.Update(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (window != null)
            {
                window.Draw(spriteBatch, effect, world, camera);
            }
        }

		public void CloseWindow()
		{
			window.Close(camera);
        }

        public VertexPackage UpdateLight(float[, ,] light)
        {
            world.SetLight(light);

            return new VertexPackage(this, world, new Point((int)camera.position.X, (int)camera.position.Z));
        }

        public VertexPackage UpdateVertices(VertexPositionColorNormalTexture[] nVerts)
        {
            newVerts = nVerts;

            camera.position = world.WrapCoordinate(camera.position);

            return new VertexPackage(this, world, new Point((int)camera.position.X, (int)camera.position.Z));
        }

        public class Menu : State
        {
            List<Button> buttons = new List<Button>();

            public Menu(StateMachine machine, string file)
                : base(machine)
            {
                LoadUI(file);
                game.IsMouseVisible = false;
                camera = new Camera(Vector3.Zero, this, false);
                Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
                window = new HoloWindow(Vector3.Zero, -camera.Angle, GraphicsDevice, this, file, game);
                world = new VoxelWorld(1,1);
            }

            void LoadUI(string file)
            {
                buttons = new List<Button>();
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
                        Button b = new Button(bounds, parts[1], parts[2], parts[3]);
                        buttons.Add(b);
                    }
                }
                reader.Close();
            }

            public override void Update()
            {
                window.Update(camera, this);
                base.Update();
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                base.Draw(spriteBatch);
            }

            public class Button
            {
                public string text, method, parameter;
                public Rectangle bounds;

                public Button(Rectangle bounds, string text, string method, string parameter)
                {
                    this.bounds = bounds;
                    this.text = text;
                    this.method = method;
                    this.parameter = parameter;
                }
            }
        }

        public class Play : State
        {
            Matrix worldMatrix = Matrix.Identity;
            Texture2D solidColor;

			Texture2D tex, crosshair, cow, healthbar;
			Effect effect;
            BasicEffect basicEffect;
            Fader legs = new Fader(32);
            Player player;

            ModelStructure.PlayerModel playerModel;
            Fader healthFader = new Fader(256);

            HUDVisor hud;
            
            public Play(StateMachine machine)
                : base(machine)
            {
                game.IsMouseVisible = false;
                
                camera = new Camera(new Vector3(2, 2, 2), game);
                world = new VoxelWorld(4,4);
                world[0,0,0] = new BrickBlock();
                world[17, 0, 0] = new BrickBlock();

                Random rand = new Random();
                for (int x = 0; x < 64; x++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            world[x, y, z] = new BrickBlock();
                        }
                        int r= rand.Next(40);
                        if (r == 8)
                            world[x, 2, z] = new BrickBlock();
                        if (r == 9)
                            world[x, 2, z] = new DirtBlock();
                        if (r == 10)
                            world[x, 2, z] = new GrassBlock();
                        if (r == 11)
                        {
                            world[x, 2, z] = new LogBlock();
                            world[x, 3, z] = new LogBlock(1);
                            world[x, 4, z] = new LogBlock(2);
                        }
                    }
                }

                world[4, 4, 3] = new GrassBlock();
                world[4, 4, 4] = new GrassBlock();
                world[4, 3, 5] = new GrassBlock();
                world[4, 3, 4] = new DirtBlock();
                world[4, 2, 4] = new DirtBlock();
                world[4, 2, 5] = new DirtBlock();
                world[4, 2, 6] = new GrassBlock();

                world[6, 3, 6] = new LampBlock();

                solidColor = new Texture2D(GraphicsDevice, 1,1);
                solidColor.SetData<Color>(new Color[] {Color.White});

                Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
                camera.Update(this);

				tex = Content.Load<Texture2D>("blocks");
                cow = Content.Load<Texture2D>("cow");
				effect = Content.Load<Effect>("TexEffect");
				crosshair = Content.Load<Texture2D>("crosshair");
                healthbar = Content.Load<Texture2D>("hpbar");
                effect.Parameters["ProjectionMatrix"].SetValue(projection);
                effect.Parameters["ModelTexture"].SetValue(tex);


                basicEffect = new BasicEffect(GraphicsDevice);
                basicEffect.VertexColorEnabled = true;
                basicEffect.TextureEnabled = true;
                basicEffect.View = camera.View;
                basicEffect.Projection = projection;

                legs.Start(true, true);

                playerModel = new ModelStructure.PlayerModel(camera.position, Content.Load<Texture2D>("playertex"));

                healthFader.Start(true, true);
                player = new Player("MooCovv");

                hud = new HUDVisor(GraphicsDevice, cow, this, new HUDElement[] { new HUDElement(player.Name, player.GetHP, player.GetXP, 0) });

                //vProcessor = new VertexProcessor(new VertexPackage(this, world, new Point((int)camera.position.X, (int)camera.position.Z)));
                //lProcessor = new LightEngine(UpdateLight);
                //lProcessor.Start();
            }

            public float Health()
            {
                return healthFader.Fade(false);
            }

            public override void Update()
            {                              
                Vector3 camPos = camera.position;
                base.Update();
                if (camera.position != camPos && window != null)
                    window.Close();
                healthFader.Fade(true);
                hud.CreateHUD();
            }

            public void Exit()
            {
                ChangeState(new State.Title(machine, "title.uil"));
            }

            public override void Draw(SpriteBatch spriteBatch)
            {

                verts = newVerts;

                KeyboardState ks = Keyboard.GetState();
                if (ks.IsKeyDown(Keys.F3) && ks.IsKeyDown(Keys.F))
                {
                    DrawView(spriteBatch, 0.1f, new Rectangle(0, 0, game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height));
                    DrawView(spriteBatch, -0.1f, new Rectangle(game.Window.ClientBounds.Width / 2, 0, game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height));
                }
                else
                    DrawView(spriteBatch, 0f, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height));
            }

            void DrawView(SpriteBatch spriteBatch, float viewOffset, Rectangle viewport)
            {
                Rectangle oldPort = GraphicsDevice.Viewport.Bounds;
                GraphicsDevice.Viewport = new Viewport(viewport);

                //RenderTarget2D target = new RenderTarget2D(GraphicsDevice, viewport.Width, viewport.Height);
                //GraphicsDevice.SetRenderTarget(target);

                effect.Parameters["WorldMatrix"].SetValue(worldMatrix * Matrix.CreateTranslation(0,camera.Bob,0));
                {
					Vector3 angle = camera.Angle;
					Vector3 movement = new Vector3(angle.X, 0, angle.Z);
                    movement = Vector3.Transform(movement, Matrix.CreateRotationY(-MathHelper.PiOver2));
                    movement.Normalize();
                    effect.Parameters["ViewMatrix"].SetValue(Matrix.CreateTranslation(movement * viewOffset) * camera.View);
                }
                effect.Parameters["Camera"].SetValue(camera.position + new Vector3(0,camera.Bob,0));

                GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                DrawVertices(effect);

                DrawPlayer();

                DrawHealth();

                //DrawCrosshair(spriteBatch);

                //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                //GraphicsDevice.SetRenderTarget(null);
                base.Draw(spriteBatch);
                //Texture2D screen = (Texture2D)target;
                //spriteBatch.Begin();
                //spriteBatch.Draw(target, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
                //spriteBatch.End();



                GraphicsDevice.Viewport = new Viewport(oldPort);
            }

            void DrawPlayer()
            {
                float light = world.GetLight(camera.position);
                basicEffect.DiffuseColor = new Vector3(light);

                string pAnim = "stand";
                if (camera.moved)
                    pAnim = "walk";
                playerModel.Draw(GraphicsDevice, basicEffect, camera, pAnim);
                basicEffect.DiffuseColor = new Vector3(1);
            }

            public void KEYBOARD_PRESSED_Escape()
            {
				if (window == null)
					window = new HoloWindow(camera.position - camera.Angle, -camera.Angle, GraphicsDevice, this, "pause.uil", this, Content.Load<Texture2D>("Menu\\pause"));
				else
					CloseWindow();
            }
            public void Resume()
            {
				CloseWindow();
            }

            void DrawHealth()
            {
                //Vector3 movement = new Vector3(camera.Angle.X, 0, camera.Angle.Z);
                //movement = Vector3.Transform(movement, Matrix.CreateRotationY(-MathHelper.PiOver2));
                //movement.Normalize();
                
                //BasicEffect effect = new BasicEffect(GraphicsDevice);
                //effect.World = Matrix.CreateBillboard(camera.position + camera.Angle * 16f, camera.position, Vector3.Down, camera.Angle);
                //effect.View = Matrix.CreateTranslation(movement * viewoffset) * camera.View;
                //effect.Projection = projection;
                //effect.TextureEnabled = true;
                //effect.VertexColorEnabled = true;

                //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, effect);
                ////VCHelper.DrawFittedText(spriteBatch, new Rectangle(-3,-1,6,2), Content.Load<SpriteFont>("defaultFont"), Color.Red, "HEALTH"); 
                //spriteBatch.Draw(solidColor, new Rectangle(-18, -11, 8, 1), Color.Red);
                //spriteBatch.End();

                GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                hud.Draw(basicEffect, healthbar, new RectangleF(0, 0, 1, 1), camera);
                //HUDVisor visor2 = new HUDVisor(GraphicsDevice, 1, cow);
                //visor2.Draw(basicEffect, healthbar, new RectangleF(0, 0, 1, 1), camera, healthFader.Fade(true));
            }

            void DrawCrosshair(SpriteBatch spriteBatch)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
                basicEffect.Projection = projection;
                basicEffect.Texture = crosshair;
                Vector3 look = camera.ViewOffset;
                basicEffect.World = Matrix.CreateWorld(camera.position + look + (camera.Angle * 0.1f), -camera.Angle, Vector3.Down);
                basicEffect.View = camera.View;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, basicEffect);
				spriteBatch.Draw(crosshair, new Vector2(0), crosshair.Bounds, Color.White, 0, new Vector2(crosshair.Bounds.Width / 2, crosshair.Bounds.Height / 2), 0.00025f, SpriteEffects.None, 0); 
                spriteBatch.End();
            }

            void DrawShadow(Effect effect)
            {
                CollisionBox scBOx = new CollisionBox(new Vector3(-0.01f,0,-0.01f),new Vector3(0.01f,0,0.01f));
                for (float x = -0.19f; x <= 0.19f; x+=0.02f)
                {
                    for (float z = -0.19f; z <= 0.19f; z+=0.02f)
                    {
                        bool check = scBOx.CheckCollision(world, new Vector3(camera.position.X + x, camera.position.Y - 1.5f, camera.position.Z + z));
                        for (float y = camera.position.Y - 1.5f; y >= 0; y -= 0.01f)
                        {
                            check = (scBOx.CheckCollision(world, new Vector3(camera.position.X + x, y, camera.position.Z + z)));
                            if (check)
                            {
                                RenderShadow(effect, new Vector3(camera.position.X + x, y + 0.01f, camera.position.Z + z));
                                break;
                            }
                        }
                    }
                }
            }

            void RenderShadow(Effect effect, Vector3 position)
            {
                float size = 0.2f;
                VertexPositionColorNormalTexture[] shadowverts = new VertexPositionColorNormalTexture[] {
                    new VertexPositionColorNormalTexture(position + new Vector3(-size, 0, size), Color.Black, new Vector2(0,1), Vector3.Up),
                    new VertexPositionColorNormalTexture(position + new Vector3(size, 0, -size), Color.Black, new Vector2(1,0), Vector3.Up),
                    new VertexPositionColorNormalTexture(position + new Vector3(-size, 0, -size), Color.Black, new Vector2(0,0), Vector3.Up),

                    new VertexPositionColorNormalTexture(position + new Vector3(-size, 0, size), Color.Black, new Vector2(0,1), Vector3.Up),
                    new VertexPositionColorNormalTexture(position + new Vector3(size, 0, size), Color.Black, new Vector2(1,0), Vector3.Up),
                    new VertexPositionColorNormalTexture(position + new Vector3(size, 0, -size), Color.Black, new Vector2(0,0), Vector3.Up)};


                VertexBuffer vBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorNormalTexture), shadowverts.Length, BufferUsage.WriteOnly);
                vBuffer.SetData<VertexPositionColorNormalTexture>(shadowverts);
                GraphicsDevice.SetVertexBuffer(vBuffer);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, shadowverts.Length / 3);
                }
            }

            void DrawVertices(Effect effect)
            {
                if (verts.Length > 0)
                {
                    VertexBuffer vBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorNormalTexture), verts.Length, BufferUsage.WriteOnly);
                    vBuffer.SetData<VertexPositionColorNormalTexture>(verts);
                    GraphicsDevice.SetVertexBuffer(vBuffer);

                    float segments = (float)verts.Length / 3f / 65535f;
                    int repeats = -1, remaining = verts.Length;
                    do
                    {
                        repeats++;
                        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, repeats * 65535 * 3, (int)Math.Min(65535, segments * 65535));
                        }
                        segments--;
                    } while (segments > 0);
                }
            }

            public Point CurrentChunk
            {
                get { return new Point((int)camera.position.X / 16, (int)camera.position.Y / 16); }
            }
        }

        public class Title : State.Menu
        {
            public Title(StateMachine machine, string file)
                : base(machine, file)
            {
                window.eventReceiver = this;
            }

            public void Quit()
            {
                game.Exit();
            }

            public void Cancel()
            {
                window.LoadUI("title.uil");
            }

            public void Exit()
            {
                window.LoadUI("quit.uil");
            }

            public void Play()
            {
                ChangeState(new State.Play(machine));
            }
        }
    }
}
