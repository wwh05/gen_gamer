using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gen_gamer
{
    public class Player
    {
        public bool isalive = true;
        public int maxhp = 3;
        public int hp = 3;
        public int gold;
        public Texture2D texture;
        public Vector2 size = new Vector2(64, 64);
        public Vector2 position = new Vector2(1920 / 2, 1080 / 2);
        public float speed = 3;
        public Rectangle rectangle => new Rectangle((int)position.X, (int)position.Y, 64, 64);
    }
}
