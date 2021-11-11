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
        private int xPos;

        public Wrench(int xPos) //Enemy must die
        {
            this.xPos = xPos;
            random = new Random();

            positions[0] = (GameWorld.screenSize.X / 3) - 95;
            positions[1] = (GameWorld.screenSize.X / 2) - 47;
            positions[2] = (GameWorld.screenSize.X) - 126;
        }
        public override void LoadContent(ContentManager content)
        {

            drawSprite = content.Load<Texture2D>("wrench");

            //drawSpriteWrench = content.Load<Texture2D>("wrench");

            Respawn();
        }

        public override void OnCollision(GameObject other)
        {

            if (other is Player)
            {
                GameWorld.lives++;
                GameWorld.Destroy(this);
            }

        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);

            if (position.Y - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset) > GameWorld.screenSize.Y)
            {
                GameWorld.Destroy(this);
            }


        }
        public void Respawn()
        {
            position = new Vector2(positions[xPos], 0 - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset));
            velocity = new Vector2(0, 1);


        }
    }
}
