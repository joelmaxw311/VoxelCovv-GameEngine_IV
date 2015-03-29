using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelCovvEngine
{
    public class Player
    {
        const float maxHealth = 20f;
        float levelXp = 10f;
        float health, xp;
        string name;
        
        public Player(string name)
        {
            this.name = name;
            this.health = maxHealth;
            this.xp = levelXp;
        }

        public string Name { get { return name; } }

        public float HP { get { return health / maxHealth; } }

        public float XP { get { return xp / levelXp; } }

        public float GetHP()
        {
            return health / maxHealth;
        }

        public float GetXP()
        {
            return xp / levelXp;
        }
    }
}
