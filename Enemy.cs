using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GhostDriver_
{
    class Enemy : GameObject
    {
        //work until you die        
        private SoundEffectInstance effect;
        ExplosionEffect explode;

        public Enemy()
        {
            random = new Random();

            positions[0] = (GameWorld.screenSize.X / 3) - 95;
            positions[1] = (GameWorld.screenSize.X / 2) - 47;
            positions[2] = (GameWorld.screenSize.X) - 126;


        }
        public override void LoadContent(ContentManager content)
        {
            sprites = new Texture2D[6];

            sprites[0] = content.Load<Texture2D>("Audi");
            sprites[1] = content.Load<Texture2D>("Car");
            sprites[2] = content.Load<Texture2D>("Mini_truck");
            sprites[3] = content.Load<Texture2D>("Mini_van");
            sprites[4] = content.Load<Texture2D>("taxi");
            sprites[5] = content.Load<Texture2D>("truck");
            effect = content.Load<SoundEffect>("Explosion_Sound").CreateInstance();

            Respawn();
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                explode = new ExplosionEffect(position);
                GameWorld.AddObject(explode);
                effect.Play();
                GameWorld.lives--;
                Respawn();
            }
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);

            if (position.Y - drawSprite.Height * (GameWorld.gameScale + .33f) > GameWorld.screenSize.Y)
            {
                Respawn();
                GameWorld.score++;
            }

        }
        public void Respawn()
        {
            int index = random.Next(0, 5);
            drawSprite = sprites[index];
            int idx = random.Next(0, 3);
            position = new Vector2(positions[idx], 0 - drawSprite.Height * (GameWorld.gameScale + .33f));
            velocity = new Vector2(0, 1);
            GameWorld.speed = 400 + (GameWorld.score) * 3;
            if (GameWorld.speed > 600) GameWorld.speed = 600;

        }
    }
}
