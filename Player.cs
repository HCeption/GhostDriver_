using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GhostDriver_
{
    class Player : GameObject
    {
        private SoundEffectInstance vroom;

        /// <summary>
        /// players contructor
        /// </summary>
        public Player()
        {
            position = new Vector2(150, 700);
        }

        /// <summary>
        /// limits the players movement to not leave the screen
        /// </summary>
        private void ScreenLimits()
        {
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            if (position.Y + drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset) > GameWorld.screenSize.Y)
            {
                position.Y = GameWorld.screenSize.Y - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset);
            }
            if (position.X < 0)
            {
                position.X = 0;
            }
            if (position.X + drawSprite.Width * (GameWorld.gameScale + GameWorld.scaleOffset) > GameWorld.screenSize.X)
            {
                position.X = GameWorld.screenSize.X - drawSprite.Width * (GameWorld.gameScale + GameWorld.scaleOffset);
            }
        }

        /// <summary>
        /// handels the users input and moves player accordingly
        /// </summary>
        private void HandleInput()
        {
            velocity = Vector2.Zero;

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(0, -1);
                if (GameWorld.sound) vroom.Play();
            }
            if (keyState.IsKeyDown(Keys.S)) velocity += new Vector2(0, 1);
            if (keyState.IsKeyDown(Keys.A)) velocity += new Vector2(-1, 0);
            if (keyState.IsKeyDown(Keys.D)) velocity += new Vector2(1, 0);
            if (velocity != Vector2.Zero) velocity.Normalize();


        }

        /// <summary>
        /// calls the needed methods
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Move(gameTime);
            ScreenLimits();
        }

        /// <summary>
        /// loads player texture and sound effect
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            drawSprite = content.Load<Texture2D>("Black_viper");
            vroom = content.Load<SoundEffect>("Accelerate").CreateInstance();
        }

        /// <summary>
        /// decides what happens when players hitbox collides with another hitbox
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
        }

    }
}
