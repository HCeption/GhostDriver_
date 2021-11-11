using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GhostDriver_
{
    public class GameWorld : Game  
    {
        //work
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D road;
        private Player player;
        private Enemy enemy;
        private Wrench wrench;
        private List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newObjects = new List<GameObject>(); 
        private static List<GameObject> deleteObjects = new List<GameObject>();
        public static Vector2 screenSize;
        private Texture2D CollisionTexture;
        private SpriteFont text;
        public static int lives = 3;
        public static int score;
        public static int speed = (int)(600 * GameWorld.gameScale);


        private int roadPos;
        public static float gameScale = 0.5f;
        private int roadSpeed = (int)(15 * gameScale);
        public static float scaleOffset = .30f;

        ExplosionEffect Explode;


        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameScale();


            player = new Player();
            gameObjects.Add(player);

            //wrench = new Wrench();
            //newObjects.Add(wrench);
            spawnAmount = 4;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            player.LoadContent(Content);
            road = Content.Load<Texture2D>("Road_Texture");
            CollisionTexture = Content.Load<Texture2D>("CollisionTexture");
            text = Content.Load<SpriteFont>("File");

            foreach (GameObject go in gameObjects)
            {
                if (go is Enemy) go.LoadContent(Content);
                if (go is ExplosionEffect) go.LoadContent(Content);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            SpawnLogic();//-------------------------------------------------Spawn new cars via SPAWN LOGIC


            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
                foreach (var other in gameObjects)
                {
                    gameObject.CheckCollision(other);
                }

            }
            RollingRoadUpdate();


            foreach (var go in newObjects)
            {
                go.LoadContent(Content);
                gameObjects.Add(go);
            }
            newObjects.Clear();
            foreach (GameObject go in deleteObjects) //Delete cars on collision.
            {
                gameObjects.Remove(go);
            }
            deleteObjects.Clear();

            if (lives < 1)
            {
                speed = 0;
                roadSpeed = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            RollingRoadDraw();
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
                DrawCollisionBox(gameObject);
            }
            if (lives > 0)
            {
                spriteBatch.DrawString(text, $"Score: {score}\nLives: {lives}\nSpeed: {speed / 2} Km/h\n\n\n{spawnAmount}", new Vector2(0, 0), Color.White);
            }
            if (lives < 1)
            {
                string stringggg = "Press \"r\" to retry";
                float test = text.MeasureString(stringggg).X;
                spriteBatch.DrawString(text, $"          GAME OVER\nYou Achived a score of {score}", new Vector2(20, 200), Color.Red,0,new Vector2 (0,0),2f,0,0);
                spriteBatch.DrawString(text, "Press \"r\" to retry", new Vector2(screenSize.X / 2-test,screenSize.Y/2), Color.Yellow, 0, new Vector2(0, 0), 2f, 0, 0);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
        /// <summary>
        /// Moves the road to give the illusion of driving.
        /// </summary>
        void RollingRoadUpdate()
        {
            roadPos += roadSpeed;
            if (roadPos > road.Height * gameScale) roadPos = 0;
        }
        /// <summary>
        /// Draw the moving road
        /// </summary>
        void RollingRoadDraw()
        {
            spriteBatch.Draw(road, new Vector2(0, roadPos), null, Color.White, 0, Vector2.Zero, gameScale, SpriteEffects.None, 0);
            spriteBatch.Draw(road, new Vector2(0, roadPos - road.Height * gameScale), null, Color.White, 0, Vector2.Zero, gameScale, SpriteEffects.None, 0);
        }
        public static void Destroy(GameObject go)
        {
            deleteObjects.Add(go);
        }
        void GameScale()
        {
            graphics.PreferredBackBufferWidth = (int)(785 * gameScale); //1920
            graphics.PreferredBackBufferHeight = (int)(1572 * gameScale); //1080
            graphics.ApplyChanges();
            screenSize.X = graphics.PreferredBackBufferWidth;
            screenSize.Y = graphics.PreferredBackBufferHeight;
        }
        private void DrawCollisionBox(GameObject gameObject)
        {
            Rectangle collisionBox = gameObject.GetCollisionBox();
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            spriteBatch.Draw(CollisionTexture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(CollisionTexture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(CollisionTexture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(CollisionTexture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
        public static void AddObject(GameObject go)
        {
            newObjects.Add(go);
        }

        private int[] safeSpawn = new int[3];
        public static int spawnAmount;
        private void SpawnLogic()
        {
            Random rnd = new Random();
            byte spawnRandom;
            bool spawnWrench = false;
            int temp=0;
            while (spawnAmount > 0)
            {
                byte availableAmount = 0;
                Console.WriteLine($"i run {temp} times, spawning {spawnAmount}, in slots: {availableAmount}");
                temp++;

                for (int i = 0; i < 3; i++) if (safeSpawn[i] == 0) availableAmount++;
                if (availableAmount < 2) break;


                spawnRandom = (byte)rnd.Next(0, 3); //Create random pos
                if (safeSpawn[spawnRandom] == 0) //if random pos is available
                {
                    newObjects.Add(new Enemy(spawnRandom)); //Create enemy at chosen random pos
                    safeSpawn[spawnRandom] = rnd.Next(10000, 50000);
                    spawnAmount--;
                }
            }

            for (int i = 0; i < 3; i++) // 'Cooldown'
            {
                if (safeSpawn[i] > 0) safeSpawn[i] -= speed;
                if (safeSpawn[i] < 0) safeSpawn[i] = 0;
            }
            spawnRandom = (byte)rnd.Next(0, 2000);
            if (spawnRandom == 0) spawnWrench = true; //spawn rare wrench
            if (spawnWrench)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (safeSpawn[i] < 2000)
                    {
                        newObjects.Add(new Wrench(i));
                        break;
                    }
                }
            }
        }
    }
}