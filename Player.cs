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
        //work hard

        public Player()
        {
            position = new Vector2(150, 700);
        }

        private void ScreenLimits()
        {
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            if (position.Y + drawSprite.Height * (GameWorld.gameScale + .33f) > GameWorld.screenSize.Y)
            {
                position.Y = GameWorld.screenSize.Y - drawSprite.Height * (GameWorld.gameScale + .33f);
            }
            if (position.X < 0)
            {
                position.X = 0;
            }
            if (position.X + drawSprite.Width * (GameWorld.gameScale + .33f) > GameWorld.screenSize.X)
            {
                position.X = GameWorld.screenSize.X - drawSprite.Width * (GameWorld.gameScale + .33f);
            }
        }



        private void HandleInput()
        {
            velocity = Vector2.Zero;

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.W)) velocity += new Vector2(0, -1);
            if (keyState.IsKeyDown(Keys.S)) velocity += new Vector2(0, 1);
            if (keyState.IsKeyDown(Keys.A)) velocity += new Vector2(-1, 0);
            if (keyState.IsKeyDown(Keys.D)) velocity += new Vector2(1, 0);
            //if (velocity != Vector2.Zero) velocity.Normalize();


        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Move(gameTime);
            ScreenLimits();

        }


        public override void LoadContent(ContentManager content)
        {
            drawSprite = content.Load<Texture2D>("Black_viper");
        }
        public override void OnCollision(GameObject other)
        {
            if (other is Wrench)
            {
                //repair.Play();
            }
        }

    }
}
