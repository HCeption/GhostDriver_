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
        private SoundEffectInstance wrenchSound;

        /// <summary>
        /// Wrenchs contructor
        /// </summary>
        /// <param name="xPos"></param>
        public Wrench(int xPos)
        {
            this.xPos = xPos;
            random = new Random();

            //decides the possible spawn loacations of the wrench
            positions[0] = (GameWorld.screenSize.X / 3) - 95;
            positions[1] = (GameWorld.screenSize.X / 2) - 47;
            positions[2] = (GameWorld.screenSize.X) - 126;
        }

        /// <summary>
        /// Loads Wrenchs sound effect, texture and calls for Respawn method
        /// </summary>
        /// <param name="content"></param> 
        public override void LoadContent(ContentManager content)
        {

            drawSprite = content.Load<Texture2D>("wrench");

            //drawSpriteWrench = content.Load<Texture2D>("wrench");
            if (GameWorld.sound) wrenchSound = content.Load<SoundEffect>("wrench_sound").CreateInstance();

            Respawn();
        }

        /// <summary>
        /// decides what happens when wrench collides other gameobject
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
            //when other gameobject is player
            if (other is Player)
            {
                GameWorld.lives++;
                GameWorld.Destroy(this);
                wrenchSound.Play();
            }

        }

        /// <summary>
        /// calls the move method and destroys wrench when it moves off screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Move(gameTime);

            if (position.Y - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset) > GameWorld.screenSize.Y)
            {
                GameWorld.Destroy(this);
            }


        }
        /// <summary>
        /// spawns a new wrench on a random of the possible positions
        /// </summary>
        public void Respawn()
        {
            position = new Vector2(positions[xPos], 0 - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset));
            velocity = new Vector2(0, 1);

        }
    }
}
