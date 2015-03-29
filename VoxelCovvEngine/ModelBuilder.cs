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
    public class ModelBuilder
    {
        List<VertexPositionColorTexture> vertices;
        
        public ModelBuilder()
        {
            vertices = new List<VertexPositionColorTexture>();
        }

        public void AddBox(Vector3 negativeCoord, Vector3 positiveCoord, Color color, RectangleF texRegion)
        {
            //int x = (int)negativeCoord.X, y = (int)negativeCoord.Y, z = (int)negativeCoord.Z;
            Vector3 dimensions = (positiveCoord - negativeCoord);
            Vector3 //Corners
            FTR = new Vector3(dimensions.X, dimensions.Y, 0) + negativeCoord,//FTR
            FTL = new Vector3(0, dimensions.Y, 0) + negativeCoord,//FTL
            FBL = new Vector3(0, 0, 0) + negativeCoord,//FBL
            FBR = new Vector3(dimensions.X, 0, 0) + negativeCoord,//FBR
            BTR = new Vector3(dimensions.X, dimensions.Y, dimensions.Z) + negativeCoord,//BTR
            BTL = new Vector3(0, dimensions.Y, dimensions.Z) + negativeCoord,//BTL
            BBL = new Vector3(0, 0, dimensions.Z) + negativeCoord,//BBL
            BBR = new Vector3(dimensions.X, 0, dimensions.Z) + negativeCoord;//BBR

            Vector3 //Normals
                Top = Vector3.Up,
                Bottom = Vector3.Down,
                Right = Vector3.Right,
                Left = Vector3.Left,
                Front = Vector3.Forward,
                Back = Vector3.Backward;

            Vector2 topLeft = new Vector2(texRegion.X,texRegion.Y),
                topRight = new Vector2(texRegion.X + texRegion.Width,texRegion.Y),
                bottomLeft = new Vector2(texRegion.X,texRegion.Y + texRegion.Height),
                bottomRight = new Vector2(texRegion.X + texRegion.Width,texRegion.Y + texRegion.Height);

            List<VertexPositionColorTexture> boxVerts = new List<VertexPositionColorTexture>();

            vertices.Add(new VertexPositionColorTexture(FTL, color, topLeft));
            vertices.Add(new VertexPositionColorTexture(BTL, color, topRight));
            vertices.Add(new VertexPositionColorTexture(BTR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(BTR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(FTR, color, bottomLeft));
            vertices.Add(new VertexPositionColorTexture(FTL, color, topLeft));

            vertices.Add(new VertexPositionColorTexture(FTL, color, topLeft));
            vertices.Add(new VertexPositionColorTexture(FTR, color, topRight));
            vertices.Add(new VertexPositionColorTexture(FBR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(FBR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(FBL, color, bottomLeft));
            vertices.Add(new VertexPositionColorTexture(FTL, color, topLeft));

            vertices.Add(new VertexPositionColorTexture(FBL, color, topLeft));
            vertices.Add(new VertexPositionColorTexture(FBR, color, topRight));
            vertices.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(BBL, color, bottomLeft));
            vertices.Add(new VertexPositionColorTexture(FBL, color, topLeft));

            vertices.Add(new VertexPositionColorTexture(BTR, color, topLeft));
            vertices.Add(new VertexPositionColorTexture(BTL, color, topRight));
            vertices.Add(new VertexPositionColorTexture(BBL, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(BBL, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(BBR, color, bottomLeft));
            vertices.Add(new VertexPositionColorTexture(BTR, color, topLeft));

            vertices.Add(new VertexPositionColorTexture(BTL, color, topLeft));
            vertices.Add(new VertexPositionColorTexture(FTL, color, topRight));
            vertices.Add(new VertexPositionColorTexture(FBL, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(FBL, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(BBL, color, bottomLeft));
            vertices.Add(new VertexPositionColorTexture(BTL, color, topLeft));

            vertices.Add(new VertexPositionColorTexture(FTR, color, topLeft));
            vertices.Add(new VertexPositionColorTexture(BTR, color, topRight));
            vertices.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            vertices.Add(new VertexPositionColorTexture(FBR, color, bottomLeft));
            vertices.Add(new VertexPositionColorTexture(FTR, color, topLeft));
        }

        public void AddBoxBorder(Vector3 negativeCoord, Vector3 positiveCoord, float borderSize)
        {
            Color color = Color.Black;
            RectangleF texRegion = new RectangleF(0,0,0.001f,0.0001f);
            //int x = (int)negativeCoord.X, y = (int)negativeCoord.Y, z = (int)negativeCoord.Z;
            Vector3 dimensions = (positiveCoord - negativeCoord) + new Vector3(borderSize * 2f);
            Vector3 //Corners
            FTR = new Vector3(dimensions.X, dimensions.Y, 0) + negativeCoord - new Vector3(borderSize),//FTR
            FTL = new Vector3(0, dimensions.Y, 0) + negativeCoord - new Vector3(borderSize),//FTL
            FBL = new Vector3(0, 0, 0) + negativeCoord - new Vector3(borderSize),//FBL
            FBR = new Vector3(dimensions.X, 0, 0) + negativeCoord - new Vector3(borderSize),//FBR
            BTR = new Vector3(dimensions.X, dimensions.Y, dimensions.Z) + negativeCoord - new Vector3(borderSize),//BTR
            BTL = new Vector3(0, dimensions.Y, dimensions.Z) + negativeCoord - new Vector3(borderSize),//BTL
            BBL = new Vector3(0, 0, dimensions.Z) + negativeCoord - new Vector3(borderSize),//BBL
            BBR = new Vector3(dimensions.X, 0, dimensions.Z) + negativeCoord - new Vector3(borderSize);//BBR

            Vector3 //Normals
                Top = Vector3.Up,
                Bottom = Vector3.Down,
                Right = Vector3.Right,
                Left = Vector3.Left,
                Front = Vector3.Forward,
                Back = Vector3.Backward;

            Vector2 topLeft = new Vector2(texRegion.X, texRegion.Y),
                topRight = new Vector2(texRegion.X + texRegion.Width, texRegion.Y),
                bottomLeft = new Vector2(texRegion.X, texRegion.Y + texRegion.Height),
                bottomRight = new Vector2(texRegion.X + texRegion.Width, texRegion.Y + texRegion.Height);

            List<VertexPositionColorTexture> boxVerts = new List<VertexPositionColorTexture>();

            boxVerts.Add(new VertexPositionColorTexture(FTL, color, topLeft));
            boxVerts.Add(new VertexPositionColorTexture(BTL, color, topRight));
            boxVerts.Add(new VertexPositionColorTexture(BTR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(BTR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(FTR, color, bottomLeft));
            boxVerts.Add(new VertexPositionColorTexture(FTL, color, topLeft));

            boxVerts.Add(new VertexPositionColorTexture(FTL, color, topLeft));
            boxVerts.Add(new VertexPositionColorTexture(FTR, color, topRight));
            boxVerts.Add(new VertexPositionColorTexture(FBR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(FBR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(FBL, color, bottomLeft));
            boxVerts.Add(new VertexPositionColorTexture(FTL, color, topLeft));

            boxVerts.Add(new VertexPositionColorTexture(FBL, color, topLeft));
            boxVerts.Add(new VertexPositionColorTexture(FBR, color, topRight));
            boxVerts.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(BBL, color, bottomLeft));
            boxVerts.Add(new VertexPositionColorTexture(FBL, color, topLeft));

            boxVerts.Add(new VertexPositionColorTexture(BTR, color, topLeft));
            boxVerts.Add(new VertexPositionColorTexture(BTL, color, topRight));
            boxVerts.Add(new VertexPositionColorTexture(BBL, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(BBL, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(BBR, color, bottomLeft));
            boxVerts.Add(new VertexPositionColorTexture(BTR, color, topLeft));

            boxVerts.Add(new VertexPositionColorTexture(BTL, color, topLeft));
            boxVerts.Add(new VertexPositionColorTexture(FTL, color, topRight));
            boxVerts.Add(new VertexPositionColorTexture(FBL, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(FBL, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(BBL, color, bottomLeft));
            boxVerts.Add(new VertexPositionColorTexture(BTL, color, topLeft));

            boxVerts.Add(new VertexPositionColorTexture(FTR, color, topLeft));
            boxVerts.Add(new VertexPositionColorTexture(BTR, color, topRight));
            boxVerts.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(BBR, color, bottomRight));
            boxVerts.Add(new VertexPositionColorTexture(FBR, color, bottomLeft));
            boxVerts.Add(new VertexPositionColorTexture(FTR, color, topLeft));

            for (int i = boxVerts.Count - 1; i >= 0; i--)
                vertices.Add(boxVerts[i]);
        }

        public void AddPrimitive(VertexPositionColorTexture vertex1, VertexPositionColorTexture vertex2, VertexPositionColorTexture vertex3)
        {
            vertices.Add(vertex1);
            vertices.Add(vertex2);
            vertices.Add(vertex3);
        }

        public void AddPlane(VertexPositionColorTexture vertex1, VertexPositionColorTexture vertex2, VertexPositionColorTexture vertex3, VertexPositionColorTexture vertex4)
        {
            vertices.Add(vertex1);
            vertices.Add(vertex2);
            vertices.Add(vertex3);
            vertices.Add(vertex3);
            vertices.Add(vertex4);
            vertices.Add(vertex1);
        }

        public void AddPlane(RectangleF bounds, RectangleF texture, Color color, Vector3 position, Vector3 normal)
        {
            normal.Normalize();
            Vector3[] points = {
                new Vector3(bounds.X, bounds.Y + bounds.Height, 0),
                new Vector3(bounds.X + bounds.Width, bounds.Y + bounds.Height, 0),
                new Vector3(bounds.X + bounds.Width, bounds.Y, 0), 
                new Vector3(bounds.X, bounds.Y, 0)
                               };
            Vector2[] texPoints = {
                new Vector2(texture.X, texture.Y),
                new Vector2(texture.X + texture.Width, texture.Y),
                new Vector2(texture.X + texture.Width, texture.Y + texture.Height),
                new Vector2(texture.X, texture.Y + texture.Height)
                                  };
            //float hRot = (float)Math.Atan2(normal.X, normal.Z);
            //float hDist = Vector2.Distance(Vector2.Zero, new Vector2(normal.X, normal.Z));
            //float vRot = (float)Math.Atan2(normal.Y, hDist);
            Matrix transform = Matrix.CreateWorld(position, normal, Vector3.Up);
            
            for (int i = 0; i < 4; i++)
            {
               points[i] = Vector3.Transform(points[i], transform);
            }
            
            if (float.IsNaN(points[0].X))
            {
                points = new Vector3[] {
                new Vector3(bounds.X, 0, bounds.Y + bounds.Height),
                new Vector3(bounds.X + bounds.Width, 0, bounds.Y + bounds.Height),
                new Vector3(bounds.X + bounds.Width, 0, bounds.Y), 
                new Vector3(bounds.X, 0, bounds.Y)
                               };
                for (int i = 0; i < 4; i++)
                {
                    points[i] += position;
                }
            }

            Vector3[] verts = {
                                points[0],
                                points[1],
                                points[2],
                                points[2],
                                points[3],
                                points[0]
                            };
            

            Vector2[] vertsTex = {
                                texPoints[0],
                                texPoints[1],
                                texPoints[2],
                                texPoints[2],
                                texPoints[3],
                                texPoints[0]
                            };

            for (int i = 0; i < 6; i++)
            {
                vertices.Add(new VertexPositionColorTexture(verts[i], color, vertsTex[i]));
            }
        }

        public VertexPositionColorTexture[] Vertices
        {
            get { return vertices.ToArray(); }
        }
    }

    public class VCModel
    {
        public string tag;
        public Vector3 position, rotation;
        public VCModel parent;
        VertexPositionColorTexture[] vertices;

        public VCModel(string tag, Vector3 position, VertexPositionColorTexture[] vertices, VCModel parent)
        {
            this.tag = tag;
            this.position = position;
            this.parent = parent;
            this.vertices = vertices;
        }


        public Matrix Transformation
        {
            get
            {
                //Matrix transformation = Matrix.Identity;
                Matrix transformation = Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationZ(rotation.Z - MathHelper.PiOver2) * Matrix.CreateTranslation(new Vector3(position.Y, position.Z, position.X));

                if (parent != null)
                    transformation *= parent.Transformation;
                return transformation;
            }
        }

        public void Draw(GraphicsDevice device, BasicEffect effect)
        {
            if (vertices.Length > 0)
            {
                VertexBuffer buffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                buffer.SetData<VertexPositionColorTexture>(vertices);
                device.SetVertexBuffer(buffer);
                effect.World = Transformation;

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, vertices.Length / 3);
                }
            }

        }
    }

	public class ModelStructure
	{
		List<VCModel> models = new List<VCModel>();
		VCModel parentModel;
        Fader animation = new Fader(16);
        protected Texture2D texture;

		public ModelStructure(Vector3 position, Texture2D texture)
		{
			parentModel = new VCModel("structure", position, null, null);
            animation.Start(true, true);
            this.texture = texture;
		}

        public VCModel this[string tag]
        {
            get
            {
                foreach (VCModel model in models)
                {
                    if (model.tag == tag)
                        return model;
                }
                return null;
            }
        }

		public Vector3 Position
		{
			get { return new Vector3(parentModel.position.Z, parentModel.position.X, parentModel.position.Y); }
			set { parentModel.position = new Vector3(value.Z, value.X, value.Y); }
		}

		public Vector3 Rotation
		{
            get { return parentModel.rotation; }//new Vector3(parentModel.rotation.Z, parentModel.rotation.X, parentModel.rotation.Y); }
			set { parentModel.rotation = value;}//new Vector3(value.Z, value.X, value.Y); }
		}

		public void Add(VCModel model)
		{
			if (model.parent == null)
				model.parent = parentModel;
			models.Add(model);
		}
        
        protected virtual void RunAnimation(string animationTag, Fader anim, Camera camera)
        {

        }

		public virtual void Draw(GraphicsDevice device, BasicEffect effect, Camera camera, string animationTag)
        {
            animation.Fade(true);
            RunAnimation(animationTag, animation, camera);
            device.RasterizerState = RasterizerState.CullClockwise;
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = true;
            effect.View = camera.View;
            effect.Texture = texture;
			foreach (VCModel model in models)
				model.Draw(device, effect);
		}

        public class PlayerModel : ModelStructure
        {
            public PlayerModel(Vector3 position, Texture2D texture)
                : base(position, texture)
            {
                VCModel body, head, arm1, arm12, arm2, arm22, leg1, leg12, leg2, leg22;
                ModelBuilder bodmb = new ModelBuilder(), headmb = new ModelBuilder();
                bodmb.AddBox(new Vector3(0, -0.25f, -0.125f), new Vector3(0.625f, 0.25f, 0.125f), Color.White, new RectangleF(0, 0, 0.25f, 0.25f));
                //bodmb.AddBoxBorder(new Vector3(0, -0.25f, -0.125f), new Vector3(0.625f, 0.25f, 0.125f), 0.001f);
                headmb.AddBox(new Vector3(-0.2f, -0.2f, -0.2f), new Vector3(0.2f, 0.2f, 0.2f), Color.White, new RectangleF(0.25f, 0, 0.25f, 0.25f));
                //headmb.AddBoxBorder(new Vector3(-0.2f, -0.2f, -0.2f), new Vector3(0.2f, 0.2f, 0.2f), 0.02f);
                ModelBuilder armmb = new ModelBuilder();
                armmb.AddBox(new Vector3(-0.125f, -0.25f, -0.125f), new Vector3(0.125f, 0f, 0.125f), Color.White, new RectangleF(0, 0, 0.25f, 0.25f));
                //armmb.AddBoxBorder(new Vector3(-0.125f, -0.25f, -0.125f), new Vector3(0.125f, 0f, 0.125f), 0.001f);
                body = new VCModel("body", Vector3.Zero, bodmb.Vertices, null);
                head = new VCModel("head", new Vector3(-0.0525f,0.83f,0), headmb.Vertices, body);
                arm1 = new VCModel("arm1", new Vector3(0, 0.5f, 0.25f), armmb.Vertices, body);
                arm2 = new VCModel("arm2", new Vector3(0, 0.5f, -0.25f), armmb.Vertices, body);
                arm12 = new VCModel("arm12", new Vector3(0, 0.125f, -0.125f), armmb.Vertices, arm1);
                arm22 = new VCModel("arm22", new Vector3(0, -0.125f, -0.125f), armmb.Vertices, arm2);
                leg1 = new VCModel("leg1", new Vector3(0, 0f, 0.125f), armmb.Vertices, body);
                leg2 = new VCModel("leg2", new Vector3(0, 0f, -0.125f), armmb.Vertices, body);
                leg12 = new VCModel("leg12", new Vector3(0, 0.125f, -0.375f), armmb.Vertices, leg1);
                leg22 = new VCModel("leg22", new Vector3(0, 0.125f, -0.375f), armmb.Vertices, leg2);
                arm1.rotation.Z = -MathHelper.PiOver4 * 2.5f;
                arm2.rotation.Z = MathHelper.PiOver4 * 2.5f;
                arm12.rotation.X = MathHelper.Pi;
                arm22.rotation.X = MathHelper.Pi;
                leg2.rotation.X = -MathHelper.PiOver4;
                this.Add(body);
                this.Add(head);
                this.Add(arm1);
                this.Add(arm2);
                this.Add(arm12);
                this.Add(arm22);
                this.Add(leg1);
                this.Add(leg12);
                this.Add(leg2);
                this.Add(leg22);
                this.Rotation = new Vector3(0, 0, -MathHelper.PiOver2);
            }

            protected override void RunAnimation(string animationTag, Fader anim, Camera camera)
            {
                base.RunAnimation(animationTag, anim, camera);
                this["head"].rotation.X = -camera.Rotation.Y;
                switch (animationTag)
                {
                    case "walk":
                        anim.Start(true, true);
                        this["leg1"].rotation.X = anim.Fade(false) - 0.5f;
                        this["leg2"].rotation.X = -(anim.Fade(false) - 0.5f);
                        this["leg12"].rotation.Y = -(0.5f - (anim.Fade(false) * 0.5f));
                        this["leg22"].rotation.Y = -((anim.Fade(false) * 0.5f));
                        this["arm1"].rotation.Y = (anim.Fade(false) - 0.5f);
                        this["arm2"].rotation.Y = (anim.Fade(false) - 0.5f);
                        this["arm12"].rotation.Z = (anim.Fade(false) * 0.5f) + MathHelper.Pi;
                        this["arm22"].rotation.Z = (0.5f - (anim.Fade(false) * 0.5f));
                        this["arm12"].rotation.X = (anim.Fade(false) * 0.5f);
                        this["arm22"].rotation.X = (0.5f - (anim.Fade(false) * 0.5f));
                        break;
                    default:
                        anim.Home(0.5f);
                        if (anim.Fade(false) == 0.5f)
                            anim.Stop();
                        this["leg1"].rotation.X = anim.Fade(false) - 0.5f;
                        this["leg2"].rotation.X = -(anim.Fade(false) - 0.5f);
                        this["arm1"].rotation.X = -(anim.Fade(false) - 0.5f);
                        this["arm2"].rotation.X = anim.Fade(false) - 0.5f;
                        break;
                }
            }

            public override void Draw(GraphicsDevice device, BasicEffect effect, Camera camera, string animation)
            {
                this.Position = camera.position + new Vector3(0, -0.75f, 0);
                this.Rotation = new Vector3(this.Rotation.X, camera.Rotation.X, this.Rotation.Z);
                base.Draw(device, effect, camera, animation);
            }
        }
	}
}
