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
    public class HoloMenu
    {
        Texture2D solidColor;
        GraphicsDevice device;
        int scroll;

        public HoloMenu(string[] items, GraphicsDevice device)
        {
            this.device = device;
            solidColor = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            solidColor.SetData<Color>(new Color[] { Color.White });
        }
    }
}
