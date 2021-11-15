using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GhostDriver_
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;//Premade
        private SpriteBatch spriteBatch;

        private Texture2D road; //Textures
        private Texture2D CollisionTexture;

        private Player player; //We only need ONE player
        private SpriteFont text; //A single spritefront for the text (viewing score)

        private List<GameObject> gameObjects = new List<GameObject>(); //The lists used to control our GameObjects (which is all enemies, effects, wrenches, and the player)
        private static List<GameObject> newObjects = new List<GameObject>();
        private static List<GameObject> deleteObjects = new List<GameObject>();

        public static Vector2 screenSize; //Essential variables. (screensize is used to adjust various items)
        public static int lives = 3;
        public static int score;
        public static int speed;
        private int highScore;
        public static bool sound = false; //Play soundeffects and music.
        private bool soundTap = true; //Used to prevent music spam.

        private int roadPos; //Game score, scale, and speeds
        public static float gameScale = 0.5f;
        private static int roadSpeed = (int)(15 * gameScale);
        public static float scaleOffset = .30f;

        private int[] safeSpawn = new int[3]; //Spawning logic.
        public static int spawnAmount;
        public static int addSpawnAmount;

        private static bool start = false; //startmenu stuff

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            GameScale();

            player = new Player();
            gameObjects.Add(player);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            road = Content.Load<Texture2D>("Road_Texture");
            CollisionTexture = Content.Load<Texture2D>("CollisionTexture");
            text = Content.Load<SpriteFont>("File");
            MediaPlayer.IsRepeating = true;
            Song music = Content.Load<Song>("Background");
            MediaPlayer.Play(music);
            MediaPlayer.Pause(); //Start, and pause music. Toggable later in the code.

            foreach (GameObject go in gameObjects) //Preload all content. Enemies are added later, but its a good failsafe too.
            {
                if (go is Enemy) go.LoadContent(Content);
                if (go is ExplosionEffect) go.LoadContent(Content);
            }

        }
        protected override void Update(GameTime gameTime)
        {
            if (!start)
            {
                KeyboardState keyState = Keyboard.GetState();
                if (keyState.IsKeyDown(Keys.B))
                {
                    start = true;
                    spawnAmount = 2;
                }
            }
            else //If start is true, run the game.
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                if (addSpawnAmount > 50)
                {
                    addSpawnAmount = 0;
                    spawnAmount++;
                }

                SpawnLogic();//Spawn new cars via SPAWN LOGIC
                RollingRoadUpdate();//Update the rolling road (background road)

                foreach (var gameObject in gameObjects) //Main update loop
                {
                    gameObject.Update(gameTime); //Call each subclasses' update method, wherein they call for Move and all sorts methods.
                    foreach (var other in gameObjects) //Collision checking loop
                    {
                        gameObject.CheckCollision(other);
                    }
                }

                foreach (var go in newObjects) //Add the new objects from AddObject method
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

                KeyboardState keyState = Keyboard.GetState();
                if (keyState.IsKeyDown(Keys.L)) lives++; //Debug gem kept ingame for those that like that
                if (keyState.IsKeyDown(Keys.P))
                {
                    score++;
                    addSpawnAmount++;
                }
                if (keyState.IsKeyDown(Keys.V) && soundTap == true) //Toggle music and sounds
                {
                    if (sound == false) sound = true;
                    else sound = false;
                    soundTap = false;

                    if (sound) MediaPlayer.Resume(); //if sound on, resume playing
                    else MediaPlayer.Pause(); //if off stop playing
                }
                if (keyState.IsKeyUp(Keys.V)) soundTap = true; //prevent running each time.

                if (lives < 1) //If dead, pause all logic in the game.
                {
                    if (score > highScore) //Set new highscore
                    {
                        highScore = score;
                    }
                    speed = 0;
                    roadSpeed = 0;
                    MediaPlayer.Pause();


                    if (keyState.IsKeyDown(Keys.R)) //Restart game
                    {
                        lives = 3;
                        spawnAmount = 2;
                        addSpawnAmount = 0;
                        speed = (int)(600 * gameScale);
                        roadSpeed = (int)(15 * gameScale);
                        score = 0;
                        if (sound) MediaPlayer.Resume();
                    }
                }
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            RollingRoadDraw();

            foreach (var gameObject in gameObjects) //Master draw loop
            {
                gameObject.Draw(spriteBatch);
                //DrawCollisionBox(gameObject);
            }
            if (lives > 0) //If alive, draw screen info
            {
                if (sound) spriteBatch.DrawString(text, $"Score: {score}\nLives: {lives}\nSpeed: {speed / 2} Km/h\n\nSound (v): On", new Vector2(0, 0), Color.White);
                else spriteBatch.DrawString(text, $"Score: {score}\nLives: {lives}\nSpeed: {speed / 2} Km/h\n\nSound (v): Off", new Vector2(0, 0), Color.White);

                if (score > 400)
                {
                    string stringTemp = "How did you even do this?";
                    Vector2 stringSize = text.MeasureString(stringTemp);
                    spriteBatch.DrawString(text, stringTemp, new Vector2(screenSize.X / 2 - stringSize.X, screenSize.Y / 2 + (int)(stringSize.Y * 4)), Color.Yellow, 0, new Vector2(0, 0), 2f, 0, 0);
                    if (score > 800)
                    {
                        stringTemp = "HOW ARE YOU STILL ALIVE!?";
                        stringSize = text.MeasureString(stringTemp);
                        spriteBatch.DrawString(text, stringTemp, new Vector2(screenSize.X / (float)1.5f - stringSize.X, screenSize.Y / 2 + (int)(stringSize.Y * 6)), Color.Yellow, 0, new Vector2(0, 0), 1.5f, 0, 0);
                    }
                }
            }
            if (lives < 1) //If dead, call EndScreen draw method.
            {
                EndScreen();
            }

            StartScreen(); //Draws the start screen (to avoid instantly losing life upon start)

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

        /// <summary>
        /// Used to REMOVE subclasses in midgame without breaking everything.
        /// Method called anywhere, adds object to list, GameObject removed via the list inside GameWorld.Update method.
        /// </summary>
        /// <param name="go"></param>
        public static void Destroy(GameObject go)
        {
            deleteObjects.Add(go);
        }

        /// <summary>
        /// Sets GameScale to correct amount
        /// Adjusts window size, associating variables, and road image scale.
        /// </summary>
        void GameScale()
        {
            graphics.PreferredBackBufferWidth = (int)(785 * gameScale); //1920
            graphics.PreferredBackBufferHeight = (int)(1572 * gameScale); //1080
            graphics.ApplyChanges();
            screenSize.X = graphics.PreferredBackBufferWidth;
            screenSize.Y = graphics.PreferredBackBufferHeight;
        }

        /// <summary>
        /// Visualize collisionboxes. Debug only.
        /// </summary>
        /// <param name="gameObject"></param>
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

        /// <summary>
        /// Used to ADD subclasses in midgame without breaking everything.
        /// Method called anywhere, adds object to list, GameObject added via the list inside GameWorld.Update method.
        /// </summary>
        /// <param name="go"></param>
        public static void AddObject(GameObject go)
        {
            newObjects.Add(go);
        }

        /// <summary>
        /// Used to figure if its safe to spawn a new car, if its even needed, and when to spawn a 'Wrench' subclass.
        /// </summary>
        private void SpawnLogic()
        {
            Random rnd = new Random();
            byte spawnRandom; //Filled by the Random method

            while (spawnAmount > 0) //spawnAmount is used to see how many cars want to spawn
            {
                byte availableAmount = 0; //Used to see the amount of availble spawn positions.

                for (int i = 0; i < 3; i++) if (safeSpawn[i] == 0) availableAmount++; //Calculates available spawn positions.
                if (availableAmount < 2 && score < 400) break; //If only 1 spawn pos is available, dont spawn anything (break), disable if over 400 score
                else if (availableAmount < 1 && score > 400) break;


                spawnRandom = (byte)rnd.Next(0, 3); //Create random pos
                if (safeSpawn[spawnRandom] == 0) //if random pos is available
                {
                    newObjects.Add(new Enemy(spawnRandom)); //Create enemy at chosen random pos

                    if (score < 400) safeSpawn[spawnRandom] = rnd.Next(10000, 50000 - score * 100); //Apply 'cooldown' to chosen position. (used to read if available, and prevent car spam)
                    else safeSpawn[spawnRandom] = 6000; //Cheater detected. Spam cars like no tomorrow.

                    if (score > 400 && score < 406) spawnAmount = 30;


                    spawnAmount--; //Car has spawned, remove from 'queue'
                }
            }

            for (int i = 0; i < 3; i++) // 'Cooldown'
            {
                if (safeSpawn[i] > 0) safeSpawn[i] -= speed; // Cooldown is moved by the cars speed to keep consistent distance.
                if (safeSpawn[i] < 0) safeSpawn[i] = 0; //Failsafe.
            }

            spawnRandom = (byte)rnd.Next(0, 2000); //Spawn wrench randomly. Runs @ 60hz, needs to be very rare.
            if (spawnRandom == 0) //if randomly chosen, spawn wrench at available position.
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

        /// <summary>
        /// Print out endscreen. Just DrawString, and needed logic to make pretty.
        /// </summary>
        private void EndScreen()
        {
            string stringTemp = $"        GAME OVER\nYou Achived a score of\n                 -{score}-\n with a highscore of\n                 -{highScore}-";
            Vector2 stringSize = text.MeasureString(stringTemp);
            spriteBatch.DrawString(text, stringTemp, new Vector2(screenSize.X / 2 - stringSize.X, screenSize.Y / 2 - (int)(stringSize.Y * 1.5)), Color.Red, 0, new Vector2(0, 0), 2f, 0, 0);
            stringTemp = "Press \"R\" to retry";
            stringSize = text.MeasureString(stringTemp);
            spriteBatch.DrawString(text, stringTemp, new Vector2(screenSize.X / 2 - stringSize.X, screenSize.Y / 2 + (int)(stringSize.Y * 2.5)), Color.Yellow, 0, new Vector2(0, 0), 2f, 0, 0);
        }
        private void StartScreen()
        {
            if (!start)
            {
                Console.WriteLine("I draw");
                string stringTemp = "Press \"B\" to start";
                Vector2 stringSize = text.MeasureString(stringTemp);
                spriteBatch.DrawString(text, stringTemp, new Vector2(screenSize.X / 2 - stringSize.X, screenSize.Y / 2 - stringSize.Y), Color.White, 0, Vector2.Zero, 2f, 0, 0);
            }
        }
    }
}