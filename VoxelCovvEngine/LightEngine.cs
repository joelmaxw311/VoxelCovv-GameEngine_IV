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

namespace VoxelCovvEngine
{
    public delegate VertexPackage LightEvent(float[, ,] lights);
    delegate bool CollisionEvent(Vector3 pos);

    public class LightEngine
    {
        public Thread lightThread;
        LightEvent Update;

        public LightEngine(LightEvent returnEvent)
        {
            lightThread = new Thread(BeginLightProcess);
            this.Update = returnEvent;
        }

        public void Start()
        {
            lightThread.Start(Update);
        }

        public bool Stop()
        {
            if (lightThread != null)
            {
                lightThread.Abort();
                return true;
            }
            return false;
        }

        public static void BeginLightProcess(object update)
        {
            bool running = true;
            LightEvent Update = (LightEvent)update;
            VertexPackage pack = Update(null);
            VoxelWorld world = pack.world;
            float[, ,] lightArray = new float[world.XLength, world.YLength, world.ZLength];
            //List<LightParticle> lights = new List<LightParticle>();
            //Random random = new Random();
            float lightSpeed = 0.05f;
            //bool[, ,] updates = new bool[lightArray.GetLength(0), lightArray.GetLength(1), lightArray.GetLength(2)];

            var SetLight = new Func<Vector3, float, bool>((lightPos, lightAmt) =>
            {
                if (lightPos == null)
                    return false;
                //Vector3 wrap = lightPos;
                //if (wrap.Y < 0 || wrap.Y >= lightArray.GetLength(1))
                //    return false;
                //while (wrap.X < 0)
                //    wrap.X += lightArray.GetLength(0);
                //while (wrap.Z < 0)
                //    wrap.Z += lightArray.GetLength(2);
                //while (wrap.X >= lightArray.GetLength(0))
                //    wrap.X -= lightArray.GetLength(0);
                //while (wrap.Z >= lightArray.GetLength(2))
                //    wrap.Z -= lightArray.GetLength(2);
                if (lightArray[(int)lightPos.X, (int)lightPos.Y, (int)lightPos.Z] <= lightAmt)
                {
                    //updates[(int)lightPos.X, (int)lightPos.Y, (int)lightPos.Z] = true;
                    lightArray[(int)lightPos.X, (int)lightPos.Y, (int)lightPos.Z] = Math.Min(Math.Max(lightAmt, 0), 2f);
                    return true;
                }
                return false;
            });

            //var CheckBlock = new Func<Vector3, Vector3, bool>((lightPos, originBlock) =>
            //{
            //    for (float xi = -lightSpeed * 0.5f; xi <= lightSpeed * 0.5f; xi += lightSpeed * 0.5f)
            //    {
            //        for (float zi = -lightSpeed * 0.5f; zi <= lightSpeed * 0.5f; zi += lightSpeed * 0.5f)
            //        {
            //            for (float yi = -lightSpeed * 0.5f; yi <= lightSpeed * 0.5f; yi += lightSpeed * 0.5f)
            //            {
            //                Vector3 pPos = /*world.WorldCoordinate*/((lightPos + new Vector3(xi, yi, zi)) / 2f);
            //                if (world[pPos] != null && new Vector3((int)pPos.X, (int)pPos.Y, (int)pPos.Z) != originBlock)
            //                    return true;
            //            }
            //        }
            //    }
            //    return false;
            //});

            while (running)
            {
                lightArray = new float[world.XLength, world.YLength, world.ZLength];
                //updates = new bool[lightArray.GetLength(0), lightArray.GetLength(1), lightArray.GetLength(2)];
                int xLoad = pack.loadCenter.X, zLoad = pack.loadCenter.Y;
                {
                    for (int y = 0; y < world.YLength; y++)
                    {
                        for (int z = zLoad - 18; z <= zLoad + 18; z++)
                        {
                            for (int x = xLoad - 18; x <= xLoad + 18; x++)
                            {
                                VoxelBlock b = world[new Vector3(x, y, z)];
                                if (b != null)
                                {
                                    if (b.Light > 0)
                                    {
                                        float lite = 1f;//b.Light;
                                        float fade = 1f / (b.Light * 50f);
                                        for (float yl = 0f; yl < 360f; yl += 15f)
                                        {
                                            for (float xl = 0f; xl < 360f; xl += 15f)
                                            {
                                                float liteVal = lite;
                                                float ydeg = MathHelper.ToRadians(yl), xdeg = MathHelper.ToRadians(xl);
                                                Vector3 move = (new Vector3(lightSpeed * (float)Math.Cos(xdeg) * (float)Math.Cos(ydeg),
                                                lightSpeed * (float)Math.Sin(ydeg),
                                                lightSpeed * (float)Math.Sin(xdeg) * (float)Math.Cos(ydeg)));

                                                Vector3 sPos = sPos = world.WrapCoordinate(new Vector3(x, y, z));
                                                Vector3 pPos = sPos + new Vector3(0.5f)/*+ new Vector3(0.55f, 0.55f, 0.55f)*/;
                                                int blocks = 0;
                                                Vector3 prevPos = pPos;
                                                while (liteVal > 0 && blocks < 1)
                                                {
                                                    pPos = world.WrapCoordinate(pPos);
                                                    //Vector3 wIndex = pPos;
                                                    //wIndex.X = (float)Math.Round(wIndex.X, 15, MidpointRounding.AwayFromZero);
                                                    //wIndex.Y = (float)Math.Round(wIndex.Y, 15, MidpointRounding.AwayFromZero);
                                                    //wIndex.Z = (float)Math.Round(wIndex.Z, 15, MidpointRounding.AwayFromZero);
                                                    //pPos = wIndex;
                                                    //pPos = world.WorldCoordinate(pPos);
                                                    if (world[pPos] != null && new Vector3((int)(pPos.X), (int)(pPos.Y), (int)(pPos.Z)) != sPos)
                                                    {
                                                        //chunks++;
                                                        Vector3 normal = new Vector3((int)pPos.X, (int)pPos.Y, (int)pPos.Z) - new Vector3((int)prevPos.X, (int)prevPos.Y, (int)prevPos.Z);
                                                        if (normal.X != 0)
                                                            move.X *= -1f;
                                                        if (normal.Y != 0)
                                                            move.Y *= -1f;
                                                        if (normal.Z != 0)
                                                            move.Z *= -1f;
                                                    }
                                                    int lx = (int)(pPos.X), ly = (int)(pPos.Y), lz = (int)(pPos.Z);
                                                    //if (liteVal > 0 && (lightArray[lx, ly, lz] <= liteVal || !updates[lx,ly,lz]))
                                                    {
                                                        SetLight(new Vector3(lx, ly, lz), liteVal);
                                                        //break;
                                                    }
                                                    liteVal -= fade;
                                                    prevPos = pPos;
                                                    pPos += move;
                                                }
                                            }
                                        }
                                        //world = Update(lightArray);
                                    }
                                }
                            }
                        }
                        Thread.Sleep(1);
                    }
                }

                pack = Update(lightArray);
                world = pack.world;
                Thread.Sleep(10);
            }
        }
    }

    //class LightParticle
    //{
    //    public Vector3 position, movement;
    //    Vector3 start;
    //    public float Light;

    //    public LightParticle(Vector3 position, Vector3 movement, float Light)
    //    {
    //        this.position = position;
    //        this.start = position;
    //        this.movement = movement;
    //        this.Light = Light;
    //    }

    //    public bool Update(VoxelWorld world, float[, ,] lightArray)
    //    {
    //        position += movement;
    //        //position = world.WorldCoordinate(position);
    //        Light -= 0.02f;
    //        if (Light <= 0)
    //            return true;
    //        if (world.WorldIndex(start) == world.WorldIndex(position))
    //            return false;
    //        if (world.SolidAt(position))
    //        {
    //            return true;
    //        }
    //        if (position.Y < 0 || position.Y > world.YLength)
    //            return true;
    //        return (Light <= 0);
    //    }
    //}
}
