using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary.Objekt.Kulisser {

    ///////////////////////////////////////////////////////////////////////////
    public class Bakgrund : Kuliss {

        public Bakgrund(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            BakgrundTextur = spelresurser.BakgrundTextur;
            Höjd = BakgrundTextur.Height;
            Bredd = BakgrundTextur.Width;
            SpriteBatch = spritebatch;
        }

        private Texture2D BakgrundTextur { get; set; }

        public override void Rita() {
            SpriteBatch.Draw(BakgrundTextur, new Rectangle((int)Position.X, (int)Position.Y, Spel.SkärmBredd, Spel.SkärmHöjd), new Rectangle(0, 0, BakgrundTextur.Width, BakgrundTextur.Height), Color.White);
        }
    }
}