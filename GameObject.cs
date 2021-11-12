using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GhostDriver_
{
    public abstract class GameObject //work hard
    {
        protected Vector2 position; //Position data used for drawing the objects, inherited and used by Player & Enemy
        protected Vector2 velocity; //Used to apply movement into position
        protected Texture2D drawSprite; //the active sprite used for drawing
        protected Texture2D[] sprites; //sprite array for Enemy class
        protected float[] positions = new float[3]; //Spawn positions for Wrench and Enemy class. (the position of the 3 lanes)

        protected Random random;
        protected SoundEffectInstance repair;
        protected Texture2D drawSpriteWrench;
        protected Texture2D spriteWrench;



        /// <summary>
        /// Abstract class used to load everything needed for the subclass
        /// </summary>
        /// <param name="content"></param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Abstract class used to update everything needed for the subclass
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Moves ALL sub classes that use movement (Player, Enemy & Wrench)
        /// Applies deltaTime to compensate for potential potato pc.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += ((velocity * GameWorld.speed) * deltaTime);
        }

        /// <summary>
        /// Master draw method. Called by EVERYTHING.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(drawSprite, position, null, Color.White, 0, Vector2.Zero, GameWorld.gameScale + GameWorld.scaleOffset, SpriteEffects.None, 1);
        }

        /// <summary>
        /// Return a rectangle the size of the used image for an entity.
        /// GameScale is applied to adjust.
        /// </summary>
        /// <returns></returns>        
        public virtual Rectangle GetCollisionBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)(drawSprite.Width * (GameWorld.gameScale + GameWorld.scaleOffset)), (int)(drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset)));
        }

        /// <summary>
        /// Checks if any collision boxes are overlapping, then calling OnColliion
        /// wherein the individual sub class can solve the collision. (Handled by Enemy & Wrench)
        /// </summary>
        /// <param name="other"></param>
        public void CheckCollision(GameObject other)
        {
            if (GetCollisionBox().Intersects(other.GetCollisionBox()))
            {
                OnCollision(other);
            }
        }

        /// <summary>
        /// Abstract method used in Subclasses for solving the collision that has happened. (Handled in Enemy & Wrench)
        /// </summary>
        /// <param name="other"></param>
        public abstract void OnCollision(GameObject other);
    }
}
