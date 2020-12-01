using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Text;
using System.Threading;

namespace SokratesSpelet {
    public class Spel : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly string Tekst = "Den industriella revolutionen och dess konsekvenser har varit en katastrof for manskligheten Den industriella revolutionen och dess konsekvenserDen industriella revolutionen och dess konsekvenser har varit en katastrof for manskligheten Den industriella revolutionen och dess konsekvenser har varit en katastrof for manskligheten Den industriella revolutionen och dess konsekvenser har varit en katastrof for manskligheten Den industriella revolutionen och dess konsekvenser har varit en katastrof for manskligheten Den industriella revolutionen och dess konsekvenser har varit en katastrof for manskligheten ";
        string outstring;
        public Spel() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Standard");
            SpelResurser resurser = new SpelResurser(Content);
            outstring = WrapText(font, Tekst, 500);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
                //spriteBatch.DrawString(font, outstring, new Vector2(100, 100), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth) {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach(string word in words) {
                Vector2 size = spriteFont.MeasureString(word);

                if(lineWidth + size.X < maxLineWidth) {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                    Thread.Sleep(100);
                }
                else {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                    Thread.Sleep(100);
                }

            }

            return sb.ToString();
        }
    }
}
