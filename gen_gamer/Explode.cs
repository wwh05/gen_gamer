using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gen_gamer
{
    public class Explode
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public bool IsExploding { get; set; }
        public int TTL { get; set; }

        List<Particle> _particles = new List<Particle>();

        public Explode(Texture2D texture, Vector2 position, Vector2 size)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Reset();
        }

        private void Reset()
        {
            _particles.Clear();
            TTL = 180;
            IsExploding = true;

            Random rnd = new Random();
            var pixels = new Color[Texture.Width * Texture.Height];
            Texture.GetData(pixels);
            var center = Position + new Vector2(Size.X / 2.0f, Size.Y / 2.0f);

            for(int y = 0; y < Size.Y; y++)
            {
                for(int x = 0; x < Size.X; x++)
                {
                    if(pixels[x + y * (int)Size.X] == Color.Transparent)
                        continue;

                    var p = new Particle()
                    {
                        ImageOffsetX = x,
                        ImageOffsetY = y,
                        Position = Position + new Vector2(x, y)
                    };
                    p.Speed = (p.Position - center) / 50.0f;
                    p.Speed += new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble());
                    _particles.Add(p);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(IsExploding)
            {
                foreach(var particle in _particles)
                {
                    spriteBatch.Draw(Texture, particle.Position,
                        new Rectangle(particle.ImageOffsetX, particle.ImageOffsetY, 1, 1), Color.White);
                }
            }
        }

        public void Update()
        {
            if(IsExploding)
            {
                foreach(var particle in _particles)
                {
                    particle.Position += particle.Speed;
                    particle.Speed += new Vector2(0, 0.02f);
                }

                TTL--;

                if(TTL <= 0)
                    IsExploding = false;
            }
        }
    }
}
