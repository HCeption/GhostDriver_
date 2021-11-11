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
        


        public Wrench() //Enemy must die
        {
            random = new Random();

            positions[0] = (GameWorld.screenSize.X / 3) - 95;
            positions[1] = (GameWorld.screenSize.X / 2) - 47;
            positions[2] = (GameWorld.screenSize.X) - 126;
        }
        public override void LoadContent(ContentManager content)
        {
            spritesWrench = new Texture2D[3];

            spritesWrench[0] = content.Load<Texture2D>("wrench");
            spritesWrench[1] = content.Load<Texture2D>("wrench");
            spritesWrench[2] = content.Load<Texture2D>("wrench");

            //drawSpriteWrench = content.Load<Texture2D>("wrench");

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

            if (position.Y - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset) > GameWorld.screenSize.Y)
            {
                Respawn();
            }


        }
        public void Respawn()
        {
            Random random = new Random();
            int idx = random.Next(0, 3);
            drawSprite = spritesWrench[idx];
            position = new Vector2(positions[idx], 0 - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset));
            velocity = new Vector2(0, 1);


        }
    }
}
