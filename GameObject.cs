using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GhostDriver_
{
    public abstract class GameObject //work hard
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected Texture2D drawSprite;
        protected Texture2D[] sprites;
        protected float fps = 5;
        protected float[] positions = new float[3];
        protected Random random;
        protected SoundEffectInstance repair;
        protected Texture2D drawSpriteWrench;
        protected Texture2D spriteWrench;




        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);
        protected void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += ((velocity * GameWorld.speed) * deltaTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(drawSprite, position, null, Color.White, 0, Vector2.Zero, GameWorld.gameScale + GameWorld.scaleOffset, SpriteEffects.None, 1);
        }
        protected void Animate()
        {

        }
     
        public virtual Rectangle GetCollisionBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)(drawSprite.Width * (GameWorld.gameScale + GameWorld.scaleOffset)), (int)(drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset)));
        }

        public abstract void OnCollision(GameObject other);
        public void CheckCollision(GameObject other)
        {
            if (GetCollisionBox().Intersects(other.GetCollisionBox()))
            {
                OnCollision(other);
            }
        }
    }
}
