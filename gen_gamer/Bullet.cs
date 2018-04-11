using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gen_gamer
{
    public class Bullet
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 direction;
        public float speed = 10;
        public Rectangle rectangle => new Rectangle((int)position.X, (int)position.Y, 8, 8);
    }
}
