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
	public class CollisionBox
	{
		Vector3 nBound, pBound;
		
		public CollisionBox(Vector3 negativeBound, Vector3 positiveBound)
		{
			this.nBound = negativeBound;
			this.pBound = positiveBound;
		}

		public bool CheckCollision(VoxelWorld world, Vector3 boxPosition)
		{
			for (float x = nBound.X; x <= pBound.X; x += 0.2f)
			{
				for (float z = nBound.Z; z <= pBound.Z; z += 0.2f)
				{
					for (float y = nBound.Y; y <= pBound.Y; y += 0.2f)
					{
						if (float.IsNaN(boxPosition.Z))
                            boxPosition.Z = 0f;
                        if (float.IsNaN(boxPosition.X))
                            boxPosition.X = 0f;
                        if (float.IsNaN(boxPosition.Y))
                            boxPosition.Y = 0f;
                        if (world[new Vector3(x, y, z) + boxPosition] != null)
							return true;
					}
				}
			}
			
			return false;
		}

        public bool Contains(Vector3 checkPosition, Vector3 boxPosition)
        {
            Vector3 pb = boxPosition + PositiveBound, nb = boxPosition + NegativeBound;
            return (checkPosition.X > nb.X &&
                checkPosition.Y > nb.Y &&
                checkPosition.Z > nb.Z &&
                checkPosition.X < pb.X &&
                checkPosition.Y < pb.Y &&
                checkPosition.Z < pb.Z);
        }

        public bool Contains(Vector3 checkRPosition, Vector3 boxPosition, Matrix transformation)
        {
            Vector3 checkPosition = Vector3.Transform(checkRPosition - boxPosition, Matrix.CreateRotationY(MathHelper.PiOver2) * transformation);
            Vector3 pb = boxPosition + PositiveBound, nb = boxPosition + NegativeBound;
            return (checkPosition.X > nb.X &&
                checkPosition.Y > nb.Y &&
                checkPosition.Z > nb.Z &&
                checkPosition.X < pb.X &&
                checkPosition.Y < pb.Y &&
                checkPosition.Z < pb.Z);
        }

        public Vector3 ContainsAt(Vector3 checkRPosition, Vector3 boxPosition, Matrix transformation)
        {
            Vector3 checkPosition = Vector3.Transform(checkRPosition - boxPosition, Matrix.CreateRotationY(MathHelper.PiOver2) * transformation);
            Vector3 pb = boxPosition + PositiveBound, nb = boxPosition + NegativeBound;
            if (checkPosition.X > nb.X &&
                checkPosition.Y > nb.Y &&
                checkPosition.Z > nb.Z &&
                checkPosition.X < pb.X &&
                checkPosition.Y < pb.Y &&
                checkPosition.Z < pb.Z)
                return checkPosition - boxPosition - nb;
            else
                return new Vector3(-1);
        }

        public Vector3 NegativeBound
        {
            get { return nBound; }
        }

        public Vector3 PositiveBound
        {
            get { return pBound; }
        }

        public Vector3 Dimensions
        {
            get { return pBound - nBound; }
        }
	}
}
