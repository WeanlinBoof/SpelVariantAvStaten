using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SokratesSpelet
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Spel : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle textBox;
        Texture2D debugColor;
        SpriteFont font;
        String text;
        String parsedText;
        String typedText;
        double typedTextLength;
        int delayInMilliseconds;
        bool isDoneDrawing;
 
        public Spel()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
 
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            textBox = new Rectangle(10, 10, 300, 300);
 
            base.Initialize();
        }
 
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
 
            // TODO: use this.Content to load your game content here
            debugColor = Content.Load<Texture2D>("dialogboxpng");
            font = Content.Load<SpriteFont>("Standard");
            text = "This is a properly implemented word wrapped sentence in XNA using SpriteFont.MeasureString. A B C D E F G H I J K L M N O P Q R S T U V W X Y Z a b c d e f g h i j k l m n o p q r s t u v w x y z";
            parsedText = parseText(text);
            delayInMilliseconds = 50;
            isDoneDrawing = false;
        }
 
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
 
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
 
            // TODO: Add your update logic here
            if (!isDoneDrawing)
            {
                if (delayInMilliseconds == 0)
                {
                    typedText = parsedText;
                    isDoneDrawing = true;
                }
                else if (typedTextLength < parsedText.Length)
                {
                    typedTextLength = typedTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / delayInMilliseconds;
 
                    if (typedTextLength >= parsedText.Length)
                    {
                        typedTextLength = parsedText.Length;
                        isDoneDrawing = true;
                    }
 
                    typedText = parsedText.Substring(0, (int)typedTextLength);
                }
            }
 
            base.Update(gameTime);
        }
 
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
 
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // spriteBatch.Draw(debugColor, textBox, Color.White);
            spriteBatch.DrawString(font, typedText, new Vector2(textBox.X, textBox.Y), Color.Black);
            spriteBatch.End();
 
            base.Draw(gameTime);
        }
 
        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');
 
            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > textBox.Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }
 
                line = line + word + ' ';
            }
 
            return returnString + line;
        }
 
    }
}