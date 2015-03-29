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
    /// <summary>
    /// A 3D box for detecting intersecting regions
    /// </summary>
	public class CollisionBox
	{
		Vector3 nBound, pBound;//Box area is defined by two points in 3D space
		
        /// <summary>
        /// A new collison box
        /// </summary>
        /// <param name="negativeBound">One corner of the collsion box area. Must be less than positiveBound.</param>
        /// <param name="positiveBound">The opposite corner of the collision box area. Must be greater than negativeBound</param>
		public CollisionBox(Vector3 negativeBound, Vector3 positiveBound)
		{
			this.nBound = negativeBound;
			this.pBound = positiveBound;
		}

        /// <summary>
        /// Test if this collision box intersects with any objects
        /// </summary>
        /// <param name="world">A reference to the world</param>
        /// <param name="boxPosition">The location of this collision box in the world</param>
        /// <returns>True if this collision box intersects with a block</returns>
		public bool CheckCollision(VoxelWorld world, Vector3 boxPosition)
		{
            for (float x = nBound.X; x <= pBound.X; x += 0.2f)//Check across x axis within box bounds
			{
                for (float z = nBound.Z; z <= pBound.Z; z += 0.2f)//Check across z axis within box bounds
				{
                    for (float y = nBound.Y; y <= pBound.Y; y += 0.2f)//Check across y axis within box bounds
					{
                        if (world[new Vector3(x, y, z) + boxPosition] != null)//Is block
							return true;//Collision detected. Return true and exit the loop.
					}
				}
			}
			
			return false;//No collision detected.
		}
        
        /// <summary>
        /// Tests if this collisoion box contains a point
        /// </summary>
        /// <param name="transformedCheckPosition">Point to test</param>
        /// <param name="boxPosition">The location of this collision box</param>
        /// <returns>True if the point is within the bounds of this collision box</returns>
        public bool Contains(Vector3 checkPosition, Vector3 boxPosition)
        {
            Vector3 pb = boxPosition + PositiveBound, nb = boxPosition + NegativeBound;//Bounds with box location applied
            //Return whether the point is within the bounds
            return (checkPosition.X > nb.X &&
                checkPosition.Y > nb.Y &&
                checkPosition.Z > nb.Z &&
                checkPosition.X < pb.X &&
                checkPosition.Y < pb.Y &&
                checkPosition.Z < pb.Z);
        }

        /// <summary>
        /// Apply a tranformation to this collsion box and test if a point is contained within the rotated bounds
        /// </summary>
        /// <param name="transformedCheckPosition">The point to test</param>
        /// <param name="boxPosition">The location of this collsion box</param>
        /// <param name="transformation">The transformation applied to this collision box</param>
        /// <returns>True if the point is contained within this collsion box</returns>
        public bool Contains(Vector3 checkPosition, Vector3 boxPosition, Matrix transformation)
        {
            Vector3 transformedCheckPosition = Vector3.Transform(checkPosition - boxPosition, Matrix.CreateRotationY(MathHelper.PiOver2) * transformation);//Apply the inverse of the transformation to the point
            Vector3 pb = boxPosition + PositiveBound, nb = boxPosition + NegativeBound;//Bounds with box location applied
            //Return whether the point is within the bounds
            return (transformedCheckPosition.X > nb.X &&
                transformedCheckPosition.Y > nb.Y &&
                transformedCheckPosition.Z > nb.Z &&
                transformedCheckPosition.X < pb.X &&
                transformedCheckPosition.Y < pb.Y &&
                transformedCheckPosition.Z < pb.Z);
        }

        /// <summary>
        /// Gets the location of a collision
        /// </summary>
        /// <param name="checkPosition">The point to test</param>
        /// <param name="boxPosition">The location of this collision box in the world</param>
        /// <param name="transformation">The trasformation applied to this collision box</param>
        /// <returns>The coordinate of the collision relative to the location of this collision box</returns>
        public Vector3 ContainsAt(Vector3 checkPosition, Vector3 boxPosition, Matrix transformation)
        {
            Vector3 transformedCheckPosition = Vector3.Transform(checkPosition - boxPosition, Matrix.CreateRotationY(MathHelper.PiOver2) * transformation);//Apply the inverse of the transformation to the point
            Vector3 pb = boxPosition + PositiveBound, nb = boxPosition + NegativeBound;//Bounds with box location applied
            if (transformedCheckPosition.X > nb.X &&//If there point is within the bounds of this collision box
                transformedCheckPosition.Y > nb.Y &&
                transformedCheckPosition.Z > nb.Z &&
                transformedCheckPosition.X < pb.X &&
                transformedCheckPosition.Y < pb.Y &&
                transformedCheckPosition.Z < pb.Z)
                return transformedCheckPosition - boxPosition - nb;//Return the location of the collision
            else
                return new Vector3(-1);//No collision
        }

        /// <summary>
        /// Gets the dimension of the boundary below the origin
        /// </summary>
        public Vector3 NegativeBound
        {
            get { return nBound; }
        }

        /// <summary>
        /// Gets the dimension of the boundary above the origin
        /// </summary>
        public Vector3 PositiveBound
        {
            get { return pBound; }
        }

        /// <summary>
        /// The difference of the negative and positive bound. (Width, height and length)
        /// </summary>
        public Vector3 Dimensions
        {
            get { return pBound - nBound; }
        }
	}
}
