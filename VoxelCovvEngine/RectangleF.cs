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
    public struct RectangleF
    {
        public float X, Y, Width, Height;
        
        public RectangleF(float x, float y, float w, float h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }
    }
}
