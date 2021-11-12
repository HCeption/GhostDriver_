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
        private static Texture2D[] explosion = new Texture2D[8];
        private float explosionFPS = 0;

        public ExplosionEffect(Vector2 position)
        {
            this.position = new Vector2(position.X - 40, position.Y);
        }
        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < 8; i++) explosion[i] = content.Load<Texture2D>($"Explosion{i + 1}");
            drawSprite = explosion[0];
        }

        public override void OnCollision(GameObject other) { }

        public override void Update(GameTime gameTime)
        {
            explosionFPS += .25f;
            if (explosionFPS > 7) GameWorld.Destroy(this);
            drawSprite = explosion[(int)explosionFPS];

        }
    }
}
