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
    public class CubeBuilder
    {
        public static VertexPositionColorNormalTexture[] QuadCubeFace(Color color, Vector3 face, VoxelBlock block)
        {
            List<VertexPositionColorNormalTexture> vertices = new List<VertexPositionColorNormalTexture>();

            if (face == Vector3.Up)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Up, block.TopTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Up, block.TopTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Up, block.TopTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Up, block.TopTextureCoordinate));
            }

            if (face == Vector3.Backward)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate));
            }

            if (face == Vector3.Down)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate));
            }

            if (face == Vector3.Forward)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate));
            }

            if (face == Vector3.Left)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Left, block.WestTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Left, block.WestTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Left, block.WestTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Left, block.WestTextureCoordinate));
            }

            if (face == Vector3.Right)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Right, block.EastTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Right, block.EastTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Right, block.EastTextureCoordinate));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Right, block.EastTextureCoordinate));
            }

            return vertices.ToArray();
        }

        public static void QuadCubeFace(Color color, Vector3 face, VoxelBlock block, Vector3 position, ref VoxelWorld world, ref List<VertexPositionColorNormalTexture> vertices)
        {
            if (face == Vector3.Up)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Up, block.TopTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Up, block.TopTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Up, block.TopTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 1, 0.5f), color, Vector3.Up, block.TopTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Up, block.TopTextureCoordinate, world, position));
            }

            if (face == Vector3.Backward)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Forward, block.NorthTextureCoordinate, world, position));
            }

            if (face == Vector3.Down)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0, 0.5f), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Down, block.BottomTextureCoordinate, world, position));
            }

            if (face == Vector3.Forward)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0.5f, 0.5f, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Backward, block.SouthTextureCoordinate, world, position));
            }

            if (face == Vector3.Left)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Left, block.WestTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 0), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Left, block.WestTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 1, 1), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Left, block.WestTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 1), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0.5f, 0.5f), color, Vector3.Left, block.WestTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(0, 0, 0), color, Vector3.Left, block.WestTextureCoordinate, world, position));
            }

            if (face == Vector3.Right)
            {
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Right, block.EastTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 0), color, Vector3.Right, block.EastTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 1, 1), color, Vector3.Right, block.EastTextureCoordinate, world, position));

                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 0), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0.5f, 0.5f), color, Vector3.Right, block.EastTextureCoordinate, world, position));
                vertices.Add(VCHelper.CreateVertex(new Vector3(1, 0, 1), color, Vector3.Right, block.EastTextureCoordinate, world, position));
            }
        }

        public static VertexPositionColorNormalTexture[] BuildCube(Vector3 position, Color color, VoxelBlock block, bool top, bool bottom, bool front, bool back, bool left, bool right)
        {
            List<VertexPositionColorNormalTexture> vertices = new List<VertexPositionColorNormalTexture>();
            if (top)
                VCHelper.AddLists<VertexPositionColorNormalTexture>(QuadCubeFace(color, Vector3.Up, block), vertices);
            if (bottom)
                VCHelper.AddLists<VertexPositionColorNormalTexture>(QuadCubeFace(color, Vector3.Down, block), vertices);
            if (front)
                VCHelper.AddLists<VertexPositionColorNormalTexture>(QuadCubeFace(color, Vector3.Forward, block), vertices);
            if (back)
                VCHelper.AddLists<VertexPositionColorNormalTexture>(QuadCubeFace(color, Vector3.Backward, block), vertices);
            if (right)
                VCHelper.AddLists<VertexPositionColorNormalTexture>(QuadCubeFace(color, Vector3.Right, block), vertices);
            if (left)
                VCHelper.AddLists<VertexPositionColorNormalTexture>(QuadCubeFace(color, Vector3.Left, block), vertices);
            for (int i = 0; i < vertices.Count; i++)
            {
                VertexPositionColorNormalTexture v = vertices[i];
                //v.Position += position;
                vertices[i] = new VertexPositionColorNormalTexture(v.Position + position, v.Color, v.TextureCoordinate, v.Normal);
            }
            return vertices.ToArray();
        }

        public static void BuildCube(Vector3 position, Color color, VoxelBlock block, bool top, bool bottom, bool front, bool back, bool left, bool right, ref VoxelWorld world, ref List<VertexPositionColorNormalTexture> vertices)
        {
            if (top)
                QuadCubeFace(color, Vector3.Up, block, position, ref world, ref vertices);
            if (bottom)
                QuadCubeFace(color, Vector3.Down, block, position, ref world, ref vertices);
            if (front)
                QuadCubeFace(color, Vector3.Forward, block, position, ref world, ref vertices);
            if (back)
                QuadCubeFace(color, Vector3.Backward, block, position, ref world, ref vertices);
            if (right)
                QuadCubeFace(color, Vector3.Right, block, position, ref world, ref vertices);
            if (left)
                QuadCubeFace(color, Vector3.Left, block, position, ref world, ref vertices);
        }
    }
}
