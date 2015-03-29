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
    public abstract class VoxelBlock
    {
        protected int data;
        
        public VoxelBlock()
        {
            this.data = 0;
        }

        public VoxelBlock(int data)
        {
            this.data = data;
        }

        public abstract string Name { get; }
        public abstract Point TextureCoordinate { get; }
        public abstract float Light { get; }
        public virtual Point TopTextureCoordinate { get { return TextureCoordinate; } }
        public virtual Point BottomTextureCoordinate { get { return TextureCoordinate; } }
        public virtual Point NorthTextureCoordinate { get { return SideTextureCoordinate; } }
        public virtual Point SouthTextureCoordinate { get { return SideTextureCoordinate; } }
        public virtual Point EastTextureCoordinate { get { return SideTextureCoordinate; } }
        public virtual Point WestTextureCoordinate { get { return SideTextureCoordinate; } }
        public virtual Point SideTextureCoordinate { get { return TextureCoordinate; } }
    }

    public class BrickBlock : VoxelBlock
    {
        public BrickBlock()
            : base()
        {

        }

        public override string Name
        {
            get { return "Brick"; }
        }

        public override Point TextureCoordinate
        {
            get { return new Point(0,0); }
        }

        public override float Light
        {
            get { return 0f; }
        }
    }

    public class DirtBlock : VoxelBlock
    {
        public DirtBlock()
            : base()
        {

        }

        public override string Name
        {
            get { return "Dirt"; }
        }

        public override Point TextureCoordinate
        {
            get { return new Point(1, 0); }
        }

        public override float Light
        {
            get { return 0f; }
        }
    }

    public class LampBlock : VoxelBlock
    {
        public LampBlock()
            : base()
        {

        }

        public override string Name
        {
            get { return "Lamp"; }
        }

        public override Point TextureCoordinate
        {
            get { return new Point(5, 0); }
        }

        public override float Light
        {
            get { return 2f; }
        }
    }

    public class LogBlock : VoxelBlock
    {
        public LogBlock()
            : base()
        {

        }

        public LogBlock(int data)
            : base(data)
        {

        }

        public override string Name
        {
            get { return "Wood"; }
        }

        public override float Light
        {
            get { return 0f; }
        }

        public override Point TextureCoordinate
        {
            get { return new Point(5, 0); }
        }

        public override Point SideTextureCoordinate
        {
            get { return new Point(4, 0); }
        }

        public override Point NorthTextureCoordinate
        {
            get
            {
                switch (data)
                {
                    case 1:
                        return new Point(6, 0);
                    case 2:
                        return new Point(5, 0);
                    default:
                        return base.NorthTextureCoordinate; 
                }
            }
        }

        public override Point SouthTextureCoordinate
        {
            get
            {
                switch (data)
                {
                    case 1:
                        return new Point(6, 0);
                    case 2:
                        return new Point(5, 0);
                    default:
                        return base.SouthTextureCoordinate;
                }
            }
        }

        public override Point WestTextureCoordinate
        {
            get
            {
                switch (data)
                {
                    case 1:
                        return new Point(5, 0);
                    case 2:
                        return new Point(6, 0);
                    default:
                        return base.WestTextureCoordinate;
                }
            }
        }

        public override Point EastTextureCoordinate
        {
            get
            {
                switch (data)
                {
                    case 1:
                        return new Point(5, 0);
                    case 2:
                        return new Point(6, 0);
                    default:
                        return base.EastTextureCoordinate;
                }
            }
        }

        public override Point TopTextureCoordinate
        {
            get
            {
                switch (data)
                {
                    case 1:
                        return new Point(6, 0);
                    case 2:
                        return new Point(4, 0);
                    default:
                        return base.TopTextureCoordinate;
                }
            }
        }

        public override Point BottomTextureCoordinate
        {
            get
            {
                switch (data)
                {
                    case 1:
                        return new Point(6, 0);
                    case 2:
                        return new Point(4, 0);
                    default:
                        return base.BottomTextureCoordinate;
                }
            }
        }
    }

    public class GrassBlock : VoxelBlock
    {
        public GrassBlock()
            : base()
        {

        }

        public override string Name
        {
            get { return "Grass"; }
        }

        public override Point TextureCoordinate
        {
            get { return new Point(2, 0); }
        }

        public override float Light
        {
            get { return 0f; }
        }

        public override Point SideTextureCoordinate
        {
            get
            {
                return new Point(3, 0);
            }
        }

        public override Point BottomTextureCoordinate
        {
            get
            {
                return new Point(1,0);
            }
        }
    }
}
