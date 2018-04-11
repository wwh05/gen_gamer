using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gen_gamer
{
    public enum eneType
    {
        ponk,
        ponkHYPER,
        ponkRABBIT,
        addeBOSS
    }

    public class Enemy
    {
        public int hp;
        public int reward;
        public int dmg;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 direction;
        public Vector2 size;
        public float speed;
        public Rectangle rectangle => new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        public eneType type;

        public Enemy(eneType type, ContentManager content)
        {
            switch(type)
            {
                case eneType.ponk:
                    hp = 3;
                    speed = 3;
                    reward = 1;
                    dmg = 1;
                    size = new Vector2(64, 64);
                    texture = content.Load<Texture2D>("enemy");
                    break;
                case eneType.ponkHYPER:
                    hp = 2;
                    speed = 5;
                    reward = 2;
                    dmg = 1;
                    size = new Vector2(64, 64);
                    texture = content.Load<Texture2D>("enemyhyper");
                    break;
                case eneType.ponkRABBIT:
                    hp = 4;
                    speed = 4;
                    reward = 3;
                    dmg = 1;
                    size = new Vector2(64, 64);
                    texture = content.Load<Texture2D>("enemyrabbit");
                    break;
                case eneType.addeBOSS:
                    break;
            }
        }
    }
}