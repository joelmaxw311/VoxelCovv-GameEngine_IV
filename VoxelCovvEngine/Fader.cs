using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelCovvEngine
{
	public class Fader
	{
		float fade, speed;
		bool fading, lap, loop;
		int direction = 1;

		public Fader(float speed)
		{
			this.speed = 1f / speed;
		}

		public void Start(bool lap)
		{
			fading = true;
			this.lap = lap;
		}

        public void Start(bool lap, bool loop)
        {
            if (direction == 0)
                direction = -1;
            fading = true;
            this.lap = lap;
            this.loop = loop;
        }

		public void Stop()
		{
			fading = false;
		}

        public void Home(float target)
        {
            direction = Math.Sign(target - fade);
        }

		public float Fade(bool advance)
		{
			if (fading && advance && !((direction == 1 && fade >= 1) || (direction == -1 && fade <= 0)))
				fade += speed * direction;
			if (fade >= 1 && (direction == 1 || direction == 0))
			{
				if (lap)
					direction = -1;
				else
					Stop();
				fade = 1;
			}
            if (fade <= 0 && (direction == -1 || direction == 0))
            {
                if (!loop)
                    Stop();
                fade = 0;
                direction = 1;
            }
			return Math.Max(0,Math.Min(1,fade));
		}

        public bool Running
        {
            get { return fading; }
        }
	}
}
