using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GhostDriver_
{
    public class GameWorld : Game  //Enemy must die
    {
        //work
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D road;
        private Player player;
        private Enemy enemy;
        private Wrench wrench;
        private List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newObjects = new List<GameObject>(); // Aded in afternoon
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
            enemy = new Enemy();
            gameObjects.Add(enemy);
            newObjects.Add(wrench);
            enemy = new Enemy();           
            gameObjects.Add(enemy);            
            enemy = new Enemy();
            //wrench = new Wrench();
            gameObjects.Add(enemy);
            

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
                //DrawCollisionBox(gameObject);
            }
            spriteBatch.DrawString(text, $"Score: {score}\nLives: {lives}\nSpeed: {speed / 2} Km/h", new Vector2(0, 0), Color.White);
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
    }
}