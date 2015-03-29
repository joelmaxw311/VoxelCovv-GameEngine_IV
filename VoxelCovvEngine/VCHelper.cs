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
using System.Reflection;

namespace VoxelCovvEngine
{
    /// <summary>
    /// Provides various static VoxelCovvEngine methods.
    /// </summary>
    public class VCHelper
    {
        /// <summary>
        /// Attempts to call a method by its name
        /// </summary>
        /// <param name="receiver">The object with the method to be called.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="args">The arguments to pass with the method.</param>
        public static void CallEvent(object receiver, string methodName, object[] args)
        {
            MethodInfo method = receiver.GetType().GetMethod(methodName);
            try
            {
                if (method != null)
                {
                    method.Invoke(receiver, args);
                }
            }
            catch (System.Reflection.TargetParameterCountException ex)
            {
                Console.WriteLine("Event " + methodName + " in " + receiver.GetType().ToString() + ", has an invalid number of parameters.");
                string correct = "(Should be " + methodName + "(" + args.GetType().ToString() + ")";
                Console.WriteLine(correct);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Returns whether a point is inside the bounds of a rectangle
        /// </summary>
        /// <param name="pos">The point to compare</param>
        /// <param name="bounds">The rectangle the point is being compared to</param>
        /// <returns>True if the point is inside the rectangle bounds</returns>
        public static bool InRect(Point pos, Rectangle bounds)
        {
            return (pos.X > bounds.X && pos.Y > bounds.Y && pos.X < bounds.X + bounds.Width && pos.Y < bounds.Y + bounds.Height);
        }

        /// <summary>
        /// Returns whether a point is inside the bounds of a rectangle
        /// </summary>
        /// <param name="pos">The point to compare</param>
        /// <param name="bounds">The rectangle the point is being compared to</param>
        /// <returns>True if the point is inside the rectangle bounds</returns>
        public static bool InRect(Vector2 pos, Rectangle bounds)
        {
            return (pos.X > bounds.X && pos.Y > bounds.Y && pos.X < bounds.X + bounds.Width && pos.Y < bounds.Y + bounds.Height);
        }

        /// <summary>
        /// Draws a string scaled to fit within an area.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw the string with.</param>
        /// <param name="bounds">The area to fit the text to.</param>
        /// <param name="font">The font to draw the text with (Larger font sizes cause less blur).</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="text">The string to draw.</param>
        public static void DrawFittedText(SpriteBatch spriteBatch, Rectangle bounds, SpriteFont font, Color color, string text)
        {
            Vector2 textBounds = font.MeasureString(text);
            textBounds = new Vector2((float)bounds.Width / textBounds.X, (float)bounds.Height / textBounds.Y);
            spriteBatch.DrawString(font, text, new Vector2(bounds.X + textBounds.X / 2, bounds.Y + textBounds.Y / 2), color, 0f, textBounds / 2f, textBounds, SpriteEffects.None, 0);
        }

        public static VertexPositionColorNormalTexture CreateVertex(Vector3 postion, Color color, Vector3 normal, Point textureCoord)
        {
            Vector2 tex = new Vector2(0, 0);
            if (normal.X != 0)
                tex = new Vector2(1 - postion.Z + textureCoord.X, 1 - postion.Y + textureCoord.Y) / 16f;
            if (normal.Y != 0)
                tex = new Vector2(1 - postion.X + textureCoord.X, 1 - postion.Z + textureCoord.Y) / 16f;
            if (normal.Z != 0)
                tex = new Vector2(1 - postion.X + textureCoord.X, 1 - postion.Y + textureCoord.Y) / 16f;
            return new VertexPositionColorNormalTexture(postion, color, tex, normal);
        }

        public static VertexPositionColorNormalTexture CreateVertex(Vector3 position, Color color, Vector3 normal, Point textureCoord, VoxelWorld world, Vector3 blockPosition)
        {
            Color lighted = CalcLight(world, blockPosition, position, color, normal);
            Vector2 tex = new Vector2(0, 0);
            if (normal.X != 0)
                tex = new Vector2(1 - position.Z + textureCoord.X, 1 - position.Y + textureCoord.Y) / 16f;
            if (normal.Y != 0)
                tex = new Vector2(1 - position.X + textureCoord.X, 1 - position.Z + textureCoord.Y) / 16f;
            if (normal.Z != 0)
                tex = new Vector2(1 - position.X + textureCoord.X, 1 - position.Y + textureCoord.Y) / 16f;
            return new VertexPositionColorNormalTexture(blockPosition + position, lighted, tex, normal);
        }

        static Color CalcLight(VoxelWorld world, Vector3 position, Vector3 vertexPosition, Color vertexColor, Vector3 normal)
        {
            Vector3 perpN = Vector3.One;
            if (normal.X != 0)
                perpN.X = 0;
            if (normal.Y != 0)
                perpN.Y = 0;
            if (normal.Z != 0)
                perpN.Z = 0;
            Vector3 thisLight = (position + normal);
            float otherLightValue = 0f;
            int otherLights = 0;
            Vector3 baseLight = position + normal;
            Vector3 baseOffset = ((vertexPosition - new Vector3(0.5f)) * 2f);
            for (float xa = 0f; xa <= 1f; xa++)
            {
                for (float ya = 0f; ya <= 1f; ya++)
                {
                    for (float za = 0f; za <= 1f; za++)
                    {
                        Vector3 otherLight = (baseLight + baseOffset * new Vector3(xa, ya, za) * perpN);
                        if (world[otherLight] == null)
                        {
                            otherLightValue += world.GetLight(otherLight);
                            otherLights++;
                        }
                    }
                }
            }
            float light = otherLightValue / otherLights;
            return new Color(light, light, light, vertexColor.A);
        }

        public static void DrawCube(Vector3 position, GraphicsDevice graphics, Matrix view, Matrix projection, Matrix world, Texture2D texture)
        {
            BasicEffect effect = new BasicEffect(graphics);
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = true;
            effect.Projection = projection;
            effect.View = view;
            SpriteBatch spriteBatch = new SpriteBatch(graphics);

            effect.World = world * Matrix.CreateWorld(position, Vector3.Forward, Vector3.Down);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, 1, 1), Color.White);
            spriteBatch.End();

            effect.World = world * Matrix.CreateWorld(position, Vector3.Right, Vector3.Down);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, 1, 1), Color.White);
            spriteBatch.End();

            effect.World = world * Matrix.CreateWorld(position - new Vector3(1, 0, 1), Vector3.Backward, Vector3.Down);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, 1, 1), Color.White);
            spriteBatch.End();

            effect.World = world * Matrix.CreateWorld(position - new Vector3(1, 0, 1), Vector3.Left, Vector3.Down);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, 1, 1), Color.White);
            spriteBatch.End();

            effect.World = world * Matrix.CreateWorld(position, Vector3.Up, Vector3.Forward);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, 1, 1), Color.White);
            spriteBatch.End();

            effect.World = world * Matrix.CreateWorld(position - new Vector3(1, 1, 0), Vector3.Down, Vector3.Forward);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, 1, 1), Color.White);
            spriteBatch.End();
        }

        public static void AddLists<T>(List<T> input, List<T> output)
        {
            foreach (T o in input)
            {
                output.Add(o);
            }
        }

        public static void AddLists<T>(T[] input, List<T> output)
        {
            foreach (T o in input)
            {
                output.Add(o);
            }
        }



        public static T[] AddLists<T>(T[] input, T[] output)
        {
            List<T> r = output.ToList();
            foreach (T o in input)
                r.Add(o);
            return r.ToArray();
        }

        public static Vector3 NearestTranslation(Vector3 basePosition, Vector3 targetPosition, VoxelWorld world)
        {
            Vector3 nearest = basePosition;
            float distance = 100000f;
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3 test = basePosition + new Vector3((float)world.XLength * (float)x, 0, (float)world.ZLength * (float)z);
                    float testDistance = Math.Abs(Vector3.DistanceSquared(test, targetPosition));
                    if (testDistance < distance)
                    {
                        nearest = test;
                        distance = testDistance;
                    }
                }
            }
            return nearest;
        }

        /// <summary>
        /// Translates a vector up so that it aligns with the camera in a straight line rather than curving with the planet.
        /// </summary>
        /// <param name="position">Position to be translated</param>
        /// <param name="cameraPosition">Position of the camera</param>
        /// <returns>The position translated to align with the camera rather than the world</returns>
        public static Vector3 CurvedVector(Vector3 position, Vector3 cameraPosition)
        {
            Vector2 camPlane = new Vector2(cameraPosition.X, cameraPosition.Z);
            Vector2 worldPlane = new Vector2(position.X, position.Z);
            float dist = Math.Abs(Vector2.Distance(camPlane, worldPlane));
            float height = -(float)Math.Cos(dist / 16f) + 1f;
            return new Vector3(position.X, position.Y + (MathHelper.ToDegrees(height) / 2f), position.Z);
        }

        public static Vector3 WorldCurve(Vector3 position, Vector3 cameraPosition)
        {
            Vector2 camPlane = new Vector2(cameraPosition.X, cameraPosition.Z);
            Vector2 worldPlane = new Vector2(position.X, position.Z);
            float dist = Math.Abs(Vector2.Distance(camPlane, worldPlane));
            float height = -(float)Math.Cos(dist / 16f) + 1f;
            return new Vector3(position.X, position.Y - (MathHelper.ToDegrees(height) / 2f), position.Z);
        }

        public static bool PointingThrough(Vector3 cameraPosition, Vector3 cameraNormal, Vector3 surfacePosition, Vector3 surfaceNormal, RectangleF surfaceBounds)
        {
            Matrix planeM = Matrix.CreateWorld(surfacePosition, surfaceNormal, Vector3.Down);
            Vector3 b1 = Vector3.Transform(new Vector3(surfaceBounds.X, surfaceBounds.Y, 0), planeM) - cameraPosition,
                b2 = Vector3.Transform(new Vector3(surfaceBounds.X + surfaceBounds.Width, surfaceBounds.Y, 0), planeM) - cameraPosition,
                b3 = Vector3.Transform(new Vector3(surfaceBounds.X, surfaceBounds.Y + surfaceBounds.Height, 0), planeM) - cameraPosition,
                b4 = Vector3.Transform(new Vector3(surfaceBounds.X + surfaceBounds.Width, surfaceBounds.Y + surfaceBounds.Height, 0), planeM) - cameraPosition;
            b1.Normalize();
            b2.Normalize();
            b3.Normalize();
            b4.Normalize();
            b1.X -= b1.Z;
            b2.X -= b2.Z;
            b3.X -= b3.Z;
            b4.X -= b4.Z;
            return cameraNormal.X >= b1.X && 
                cameraNormal.X <= b2.X &&
                cameraNormal.X >= b3.X &&
                cameraNormal.X <= b4.X &&

                cameraNormal.Y >= b1.Y && 
                cameraNormal.Y >= b2.Y &&
                cameraNormal.Y <= b3.Y &&
                cameraNormal.Y <= b4.Y;
        }
    }
}
