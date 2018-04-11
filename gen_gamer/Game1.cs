using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace gen_gamer
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch sb;

        Player player = new Player();

        bool isPaused = false;
        int level = 1;
        float spawntime = 3;
        int enemiesleft => 5 * level;
        int enemieskilled;
        int enemiesspawned;

        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        List<Explode> explosions = new List<Explode>();

        float timer;
        float shootTimer;

        public Texture2D currentbg;
        public Texture2D bg1, bg2, bg3, bg4;
        public Texture2D box;

        SpriteFont font16, font24, font36, font48;

        Random rnd = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            IsMouseVisible = true;

            Window.Position = new Point(200, 100);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);

            var tx = Content.Load<Texture2D>("player");
            bg1 = Content.Load<Texture2D>("bg1");
            bg2 = Content.Load<Texture2D>("bg2");
            bg3 = Content.Load<Texture2D>("bg3");
            bg4 = Content.Load<Texture2D>("bg4");
            box = Content.Load<Texture2D>("box");

            RandomBg();

            font16 = Content.Load<SpriteFont>("font16");
            font24 = Content.Load<SpriteFont>("font24");
            font36 = Content.Load<SpriteFont>("font36");
            font48 = Content.Load<SpriteFont>("font48");

            player.texture = tx;
        }

        private void RandomBg()
        {
            var r = rnd.Next(0, 5);
            switch(r)
            {
                case 1:
                    currentbg = bg1;
                    break;
                case 2:
                    currentbg = bg2;
                    break;
                case 3:
                    currentbg = bg3;
                    break;
                case 4:
                    currentbg = bg4;
                    break;
                default:
                    currentbg = bg1;
                    break;
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //  spawn ene
            if(!isPaused)
            {
                GameFlow(gameTime);

                //  check if die
                if(player.isalive)
                    if(player.hp <= 0)
                        Die();

                var kb = Keyboard.GetState();

                //  movement
                if(player.isalive)
                {
                    if(kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D))
                        player.position.X += player.speed;
                    if(kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A))
                        player.position.X -= player.speed;
                    if(kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.W))
                        player.position.Y -= player.speed;
                    if(kb.IsKeyDown(Keys.Down) || kb.IsKeyDown(Keys.S))
                        player.position.Y += player.speed;

                    //  collision
                    if(player.position.X > 1870)
                        player.position.X = 1870;
                    if(player.position.X < 0)
                        player.position.X = 0;
                    if(player.position.Y > 1030)
                        player.position.Y = 1030;
                    if(player.position.Y < 0)
                        player.position.Y = 0;

                    //  shoot
                    shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(shootTimer > 0.5)
                    {
                        if(Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            Shoot(new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));
                            shootTimer = 0;
                        }
                    }
                }

                for(int i = 0; i < bullets.Count; i++)
                {
                    var bul = bullets[i];
                    bul.position += bul.direction * bul.speed;

                    if(bul.position.X > 2000 || bul.position.X < -100 || bul.position.Y > 1200 || bul.position.Y < -100)
                        bullets.Remove(bul);
                }


                //  move enemy - kill enemy
                if(enemies.Count > 0)
                    for(int i = 0; i < enemies.Count; i++)
                    {
                        var ene = enemies[i];
                        if(ene.hp <= 0)
                            KillEnemy(ene);

                        if(player.isalive)
                        {
                            var dir = Vector2.Normalize(player.position - ene.position);
                            ene.direction = dir;
                            ene.position += ene.direction * ene.speed;
                        }

                        //  check enemy hit
                        if(bullets.Count > 0)
                            for(int y = 0; y < bullets.Count; y++)
                            {
                                var bul = bullets[y];
                                if(ene.rectangle.Intersects(bul.rectangle))
                                {
                                    ene.hp--;
                                    bullets.Remove(bul);
                                }
                            }

                        //  check enemy player hit
                        if(ene.rectangle.Intersects(player.rectangle))
                            HitPlayer(ene);
                    }

                for(int i = 0; i < explosions.Count; i++)
                {
                    var exp = explosions[i];
                    if(exp.IsExploding)
                        exp.Update();
                    else
                        explosions.Remove(exp);
                }
            }

            base.Update(gameTime);
        }

        private void Die()
        {
            var exp = new Explode(player.texture, player.position, player.size);
            explosions.Add(exp);
            player.isalive = false;
        }

        private void HitPlayer(Enemy ene)
        {
            player.hp -= ene.dmg;
            KillEnemyNoGold(ene);
        }

        private void KillEnemyNoGold(Enemy ene)
        {
            var exp = new Explode(ene.texture, ene.position, ene.size);
            explosions.Add(exp);
            enemies.Remove(ene);
            enemieskilled++;
        }

        private void KillEnemy(Enemy ene)
        {
            var exp = new Explode(ene.texture, ene.position, ene.size);
            explosions.Add(exp);
            enemies.Remove(ene);
            enemieskilled++;
            player.gold += ene.reward;
        }

        private void GameFlow(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(enemieskilled >= enemiesleft)
                ClearLevel();

            if(enemiesspawned < enemiesleft)
                if(timer > spawntime)
                    switch(level)
                    {
                        case 1:
                            SpawnEnemy(eneType.ponk);
                            break;
                        case 2:
                            var t = rnd.Next(0, 2);
                            SpawnEnemy((eneType)t);
                            break;
                        default:
                            var y = rnd.Next(0, 3);
                            SpawnEnemy((eneType)y);
                            break;
                    }
        }

        private void ClearLevel()
        {
            //  TODO

            //  nice anmation
            //  upgrade screen
            //  ready -> next level
            //isPaused = true;

            RandomBg();
            level++;
            enemieskilled = 0;
            enemiesspawned = 0;
            spawntime -= (float)0.7;
        }

        private void Shoot(Vector2 endPos)
        {
            var bul = new Bullet();
            bul.texture = box;
            bul.position = player.position + new Vector2(32, 32);
            var dir = Vector2.Normalize(endPos - bul.position);
            bul.direction = dir;
            bullets.Add(bul);
        }

        private void SpawnEnemy(eneType type)
        {
            var ene = new Enemy(type, Content);

            var tempPos = new Vector2(rnd.Next(0, 1920), rnd.Next(0, 1080));
            var distance = Vector2.Distance(player.position, tempPos);

            //  spawn protecc
            while(distance < 300)
            {
                tempPos = new Vector2(rnd.Next(0, 1920), rnd.Next(0, 1080));
                distance = Vector2.Distance(player.position, tempPos);
            }

            ene.position = tempPos;
            enemies.Add(ene);
            enemiesspawned++;
            timer = 0;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(45, 45, 45));

            sb.Begin();
            sb.Draw(currentbg, new Rectangle(0, 0, 1920, 1080), Color.White);
            foreach(var ene in enemies)
                sb.Draw(ene.texture, ene.rectangle, Color.White);
            foreach(var bul in bullets)
                sb.Draw(bul.texture, bul.rectangle, Color.Black);
            foreach(var exp in explosions)
                exp.Draw(sb);
            if(player.isalive)
                sb.Draw(player.texture, player.rectangle, Color.White);

            for(int i = 0; i < player.hp; i++)
            {
                sb.Draw(player.texture, new Rectangle(20 + (36 * i), 20, 32, 32), Color.White);
            }

            sb.End();

            sb.Begin();
            sb.DrawString(font36, "lvl: " + level, new Vector2(1920 / 2 - 50, 10), Color.Purple);
            sb.DrawString(font24, "golde: " + player.gold, new Vector2(1920 - 200, 20), Color.Purple);
            sb.End();

            base.Draw(gameTime);
        }
    }
}
