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
using System.Threading;
using System.Threading.Tasks;

namespace VoxelCovvEngine
{
    class VertexProcessor
    {
        Thread thread;

        public VertexProcessor()
        {
            thread = new Thread(new ParameterizedThreadStart(ProcessVertices));
        }

        public void Start(VertexPackage package)
        {
            thread.Start(package);
        }

        public void Stop()
        {
            try
            {
                thread.Abort();
            }
            catch { Console.WriteLine("Vertex thread aborted."); }
        }

        static void ProcessVertices(object packageObj)
        {
            VertexPackage package = (VertexPackage)packageObj;
            while (true)
            {
                {
                    List<VertexPositionColorNormalTexture> vertices = new List<VertexPositionColorNormalTexture>();
                    int xA = package.loadCenter.X - 12, zA = package.loadCenter.Y - 12;
                    //int xPos = package.loadCenter.X - 12, zPos = package.loadCenter.Y;
                    for (int x = package.loadCenter.X - 12; x <= package.loadCenter.X + 12; x++)
                    {
                        for (int z = package.loadCenter.Y - 12; z <= package.loadCenter.Y + 12; z++)
                        {
                            for (int y = 0; y < 32; y++)
                            {
                                VoxelBlock block = package.world[x, y, z];
                                if (block != null)
                                {
                                    CubeBuilder.BuildCube(new Vector3(x, y, z), Color.White, block, package.world[x, y + 1, z] == null, package.world[x, y - 1, z] == null, package.world[x, y, z + 1] == null, package.world[x, y, z - 1] == null, package.world[x - 1, y, z] == null, package.world[x + 1, y, z] == null, ref package.world, ref vertices);
                                    //VertexPositionColorNormalTexture[] bVerts = CubeBuilder.BuildCube(Vector3.Zero,Color.White, block, package.world[x, y + 1, z] == null, package.world[x, y - 1, z] == null, package.world[x, y, z + 1] == null, package.world[x, y, z - 1] == null, package.world[x - 1, y, z] == null, package.world[x + 1, y, z] == null);

                                    //foreach (VertexPositionColorNormalTexture v in bVerts)
                                    //{
                                    //    Vector3 perpN = Vector3.One;
                                    //    if (v.Normal.X != 0)
                                    //        perpN.X = 0;
                                    //    if (v.Normal.Y != 0)
                                    //        perpN.Y = 0;
                                    //    if (v.Normal.Z != 0)
                                    //        perpN.Z = 0;
                                    //    Vector3 thisLight = (new Vector3(x, y, z) + v.Normal);
                                    //    float otherLightValue = 0f;
                                    //    int otherLights = 0;
                                    //    Vector3 baseLight = new Vector3(x, y, z) + v.Normal;
                                    //    Vector3 baseOffset = ((v.Position - new Vector3(0.5f)) * 2f);
                                    //    for (float xa = 0f; xa <= 1f; xa++)
                                    //    {
                                    //        for (float ya = 0f; ya <= 1f; ya++)
                                    //        {
                                    //            for (float za = 0f; za <= 1f; za++)
                                    //            {
                                    //                Vector3 otherLight = ( baseLight + baseOffset * new Vector3(xa, ya, za) * perpN);
                                    //                if (package.world[otherLight] == null)
                                    //                {
                                    //                    otherLightValue += package.world.GetLight(otherLight);
                                    //                    otherLights++;
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //    float light = otherLightValue / otherLights;

                                    //    backVertices.Add(new VertexPositionColorNormalTexture(v.Position + new Vector3(x,y,z), new Color(light, light, light, v.Color.A), v.TextureCoordinate, v.Normal));
                                    //}
                                }
                            }
                            zA++;
                        }
                        xA++;
                        Thread.Sleep(1);
                    }

                    package = package.state.UpdateVertices(vertices.ToArray());
                    Thread.Sleep(10);
                }
            }
        }
    }

    public struct VertexPositionColorNormalTexture : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;

        public readonly static VertexDeclaration VertexDeclaration
            = new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 3 + 4 + sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                );

        public VertexPositionColorNormalTexture(Vector3 pos, Color c, Vector2 t, Vector3 n)
        {
            Position = pos;
            Color = c;
            TextureCoordinate = t; 
            Normal = n;
        }

        public VertexPositionColorNormalTexture(VertexPositionColorTexture vertex, Vector3 normal)
        {
            Position = vertex.Position;
            Color = vertex.Color;
            TextureCoordinate = vertex.TextureCoordinate;
            Normal = normal;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }

    public class VertexPackage
    {
        public State state;
        public VoxelWorld world;
        public Point loadCenter;
        
        public VertexPackage(State state, VoxelWorld world, Point loadCenter)
        {
            this.state = state;
            this.world = world;
            this.loadCenter = loadCenter;
        }
    }
}
