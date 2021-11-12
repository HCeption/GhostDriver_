using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GhostDriver_
{
    class ExplosionEffect : GameObject
    {
        private static Texture2D[] explosion = new Texture2D[8]; //array with 8 spaces for 8 difrent explosion frames to make animation
        private float explosionFPS = 0;

        /// <summary>
        /// Sets the location of the explosion effect
        /// </summary>
        /// <param name="position"></param>
        public ExplosionEffect(Vector2 position)
        {
            this.position = new Vector2(position.X - 40, position.Y);
        }
        /// <summary>
        /// plays the explosion animation with a for loop
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < 8; i++) explosion[i] = content.Load<Texture2D>($"Explosion{i + 1}");
            drawSprite = explosion[0];
        }

        /// <summary>
        /// decides what happens when explosionEffect collides woth something, which is nothing
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other) { }

        /// <summary>
        /// sets explosion fps and destroys it when it is done with its animation
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            explosionFPS += .25f; // 4 frames pr explosion image
            if (explosionFPS > 7) GameWorld.Destroy(this);
            drawSprite = explosion[(int)explosionFPS];

        }
    }
}
