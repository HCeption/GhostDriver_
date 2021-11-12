using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GhostDriver_
{
    class Enemy : GameObject                     // Make class for enemy
    {
        private int xPos;                        //Make field for xPos - position of enemy
        private SoundEffectInstance effect;      // Create sound effect for collision between enemy and player
        ExplosionEffect explode;                 // Create sound explosion effect when enemy collide player

        public Enemy(int xPos)                  // Make constructor for Enemy
        {
            this.xPos = xPos;
            random = new Random();

            positions[0] = (GameWorld.screenSize.X / 3) - 95; //Make posiotions for enemies on the screen
            positions[1] = (GameWorld.screenSize.X / 2) - 47;
            positions[2] = (GameWorld.screenSize.X) - 126;


        }
        /// <summary>
        /// Override LoadeContent method
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)  
        {
            sprites = new Texture2D[6];

            sprites[0] = content.Load<Texture2D>("Audi");         //Dowload different cars sprites for enemy class
            sprites[1] = content.Load<Texture2D>("Car");
            sprites[2] = content.Load<Texture2D>("Mini_truck");
            sprites[3] = content.Load<Texture2D>("Mini_van");
            sprites[4] = content.Load<Texture2D>("taxi");
            sprites[5] = content.Load<Texture2D>("truck");
            effect = content.Load<SoundEffect>("Explosion_Sound").CreateInstance(); //Dowload explosion sound

            Create();                                              // Use Create method
        }
        /// <summary>
        /// Override OnCollision method
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)         
        {
            if (other is Player)                                  //Make logic in this method
            {
                explode = new ExplosionEffect(position);          // Call explosion constructor
                GameWorld.AddObject(explode);                     // Add explosion effect
                if(GameWorld.sound)effect.Play();                 // Play effect if sound is on
                GameWorld.lives--;                                // Lost live
                Remove();                                         // Remove all
            }
        }
        /// <summary>
        /// Override Update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)           
        {
            Move(gameTime);                                     // Call move method
            // Logic for enemy position on the screen
            if (position.Y - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset) > GameWorld.screenSize.Y)
            {
                GameWorld.score++;
                GameWorld.addSpawnAmount++;
                Remove();
            }
            if (GameWorld.lives < 1) Remove();                 //Remove enemy logic

        }
        /// <summary>
        /// Make Create method
        /// </summary>
        public void Create()                                    
        {
            int index = random.Next(0, sprites.Length);        // Make random enemy posiosion on the screen
            drawSprite = sprites[index];
            position = new Vector2(positions[xPos], 0 - drawSprite.Height * (GameWorld.gameScale + GameWorld.scaleOffset));
            velocity = new Vector2(0, 1);
            GameWorld.speed = 400 + (GameWorld.score) * 3;     //Make speed for enemy
            if (GameWorld.speed > 600) GameWorld.speed = 600;  //Make max speed for enemy

        }
        /// <summary>
        /// Create Remove method
        /// </summary>
        private void Remove()                                  
        {
            GameWorld.Destroy(this);
            GameWorld.spawnAmount++;
        }
    }
}
