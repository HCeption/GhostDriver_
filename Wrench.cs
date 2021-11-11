using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GhostDriver_
{
    class Wrench : GameObject
    {

        public Wrench()
        {

            positions[0] = (GameWorld.screenSize.X / 3) - 95;
            positions[1] = (GameWorld.screenSize.X / 2) - 47;
            positions[2] = (GameWorld.screenSize.X) - 126;
        }
        public override void LoadContent(ContentManager content)
        {
            drawSprite = content.Load<Texture2D>("wrench");

            Respawn();
        }

        public override void OnCollision(GameObject other)
        {

            if (other is Player)
            {
                GameWorld.lives++;
                Respawn();
            }

        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);

            if (position.Y - drawSprite.Height * (GameWorld.gameScale + .33f) > GameWorld.screenSize.Y)
            {
                Respawn();
            }


        }
        public void Respawn()
        {

            int idx = random.Next(0, 3);
            drawSprite = sprites[idx];
            position = new Vector2(positions[idx], 0 - drawSprite.Height * (GameWorld.gameScale + .33f));
            velocity = new Vector2(0, 1);


        }
    }
}
