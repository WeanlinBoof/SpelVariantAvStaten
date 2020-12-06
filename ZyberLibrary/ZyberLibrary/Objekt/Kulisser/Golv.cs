using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary.Objekt.Kulisser {

    ///////////////////////////////////////////////////////////////////////////
    public class Golv : Kuliss {

        public Golv(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            GolvTextur = spelresurser.GolvTextur;
            Höjd = GolvTextur.Height;
            Bredd = GolvTextur.Width;
            SpriteBatch = spritebatch;
        }

        private Texture2D GolvTextur { get; set; }

        public override void Rita() {

            //SpriteBatch.Draw(GolvTextur, new Vector2(Position.X, Position.Y), new Rectangle(0, 0, GolvTextur.Width, GolvTextur.Height), Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
            SpriteBatch.Draw(GolvTextur, new Rectangle((int)Position.X, (int)Position.Y, Spel.SkärmBredd, Spel.SkärmHöjd + GolvTextur.Height / 2 - Spel.SkärmHöjd), new Rectangle(0, 0, GolvTextur.Width, GolvTextur.Height), Color.White);
        }
    }
}