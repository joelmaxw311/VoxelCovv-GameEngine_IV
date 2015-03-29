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
    public class VoxelWorld
    {
        VoxelChunk[,] chunks;
        float[,,] lights;
        int xLength, zLength, yLength = 32, chunksX, chunksZ;
        
        public VoxelWorld(int xdim, int zdim)
        {
            chunks = new VoxelChunk[xdim, zdim];
            for (int z = 0; z < zdim; z++)
            {
                for (int x = 0; x < xdim; x++)
                {
                    chunks[x,z] = new VoxelChunk(new Point(x,z));
                }
            }
            xLength = xdim * 16;
            zLength = zdim * 16;
            chunksX = xdim;
            chunksZ = zdim;
        }

		public VoxelBlock this[Vector3 pos]
		{
			get
            {
                if (pos.Y < 0 || pos.Y >= 32)
                    return null;

				float x = pos.X, z = pos.Z;

				return this[(int)x, (int)pos.Y, (int)z];
			}
			set
            {
                if (pos.Y < 0 || pos.Y >= 32)
                    return;

				float x = pos.X, z = pos.Z;

				this[(int)x, (int)pos.Y, (int)z] = value;
			}
		}
        
        public VoxelBlock this[int x, int y, int z]
        {
            get
            {
                if (y < 0 || y >= 32)
                    return null;

				if (x < 0)
					x--;
				if (z < 0)
					z--;

                while (x < 0)
                    x += XLength;
                while (z < 0)
                    z += ZLength;
                while (x >= XLength)
                    x -= XLength;
                while (z >= ZLength)
                    z -= ZLength;

                int chunkX = x / 16, chunkZ = z / 16;
                while (chunkX < 0)
                    chunkX += chunksX;
                while (chunkZ < 0)
                    chunkZ += chunksZ;
                while (chunkX >= chunksX)
                    chunkX -= chunksX;
                while (chunkZ >= chunksZ)
                    chunkZ -= chunksZ;

                return chunks[chunkX,chunkZ][x - chunkX * 16, y, z - chunkZ * 16];
            }

            set
			{
                if (y < 0 || y >= 32)
                    return;

				if (x < 0)
					x--;
				if (z < 0)
					z--;

                while (x < 0)
                    x += XLength;
                while (z < 0)
                    z += ZLength;
                while (x >= XLength)
                    x -= XLength;
                while (z >= ZLength)
                    z -= ZLength;

                int chunkX = x / 16, chunkZ = z / 16;
                while (chunkX < 0)
                    chunkX += chunksX;
                while (chunkZ < 0)
                    chunkZ += chunksZ;
                while (chunkX >= chunksX)
                    chunkX -= chunksX;
                while (chunkZ >= chunksZ)
                    chunkZ -= chunksZ;

                chunks[chunkX, chunkZ][x - chunkX * 16, y, z - chunkZ * 16] = value;
            }
        }

        public VoxelChunk this[int x, int z]
        {
            get
            {
                while (x < 0)
                    x += chunksX;
                while (z < 0)
                    z += chunksZ;
                while (x >= chunksX)
                    x -= chunksX;
                while (z >= chunksZ)
                    z -= chunksZ;

                return chunks[x,z];
            }
        }

        public float GetLight(int x, int y, int z)
        {
            if (y < 0 || y >= 32 || lights == null)
                return 0f;

            if (x < 0)
                x--;
            if (z < 0)
                z--;

            while (x < 0)
                x += lights.GetLength(0);
            while (z < 0)
                z += lights.GetLength(2);
            while (x >= lights.GetLength(0))
                x -= lights.GetLength(0);
            while (z >= lights.GetLength(2))
                z -= lights.GetLength(2);

            return lights[x,y,z];
        }

        public float GetLight(Vector3 pos)
        {
            return GetLight((int)pos.X, (int)pos.Y, (int)pos.Z);
        }

        public Vector3 WrapCoordinate(Vector3 position)
        {
            if (lights == null)
                return position;
            
            Vector3 pos = position; 
            
            while (pos.X < 0)
                pos.X += lights.GetLength(0);
            while (pos.Z < 0)
                pos.Z += lights.GetLength(2);
            while (pos.X >= lights.GetLength(0))
                pos.X -= lights.GetLength(0);
            while (pos.Z >= lights.GetLength(2))
                pos.Z -= lights.GetLength(2);
            return new Vector3(pos.X, pos.Y, pos.Z);
        }

        public void SetLight(float[, ,] lights)
        {
         this.lights = lights;
        }

        public int XLength
        {
            get { return xLength; }
        }

        public int YLength
        {
            get { return 32; }
        }

        public int ZLength
        {
            get { return zLength; }
        }
    }

    public class VoxelChunk
    {
        VoxelBlock[,,] blocks = new VoxelBlock[16,32,16];
        Point coordinate;

        public VoxelChunk(Point coordinate)
        {
            this.coordinate = coordinate;
        }

        public VoxelBlock this[int x, int y, int z]
        {
            get { return blocks[x, y, z]; }
            set { blocks[x, y, z] = value; }
        }
    }
}
