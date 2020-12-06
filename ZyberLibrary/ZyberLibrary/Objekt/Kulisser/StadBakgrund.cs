using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary.Objekt.Kulisser {

    ///////////////////////////////////////////////////////////////////////////
    public class StadBakgrund : Kuliss {

        public StadBakgrund(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            StadTextur = spelresurser.StadTextur;
            Höjd = StadTextur.Height;
            Bredd = StadTextur.Width;
            SpriteBatch = spritebatch;
        }

        private Texture2D StadTextur { get; set; }

        public override void Rita() {
            SpriteBatch.Draw(StadTextur, new Rectangle((int)Position.X, (int)Position.Y, Spel.SkärmBredd, Spel.SkärmHöjd + StadTextur.Height / 2 - Spel.SkärmHöjd), new Rectangle(0, 0, StadTextur.Width, StadTextur.Height), Color.White);
        }
    }
}