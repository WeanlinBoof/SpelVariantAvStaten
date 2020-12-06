using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary.Objekt.Kulisser {

    ///////////////////////////////////////////////////////////////////////////
    public class GolvRutor {

        private readonly float GolvX;

        private readonly float GolvY;

        public GolvRutor(float x, float y, int skärmbredd, int skärmhöjd, SpriteBatch spritebatch, SpelResurser spelresurser) {
            SkärmBredd = skärmbredd;
            SkärmHöjd = skärmhöjd;
            GolvRuta = new Golv[SkärmBredd, SkärmHöjd];
            GolvX = x;
            GolvY = y;
            for(int i = 0; i < SkärmBredd; i++) {
                GolvY = y + i * spelresurser.GolvTextur.Height;
                for(int j = 0; j < SkärmHöjd; j++) {
                    GolvX = x + j * spelresurser.GolvTextur.Width;
                    Golv golv = new Golv(GolvX, GolvY, spritebatch, spelresurser);
                    GolvRuta[i, j] = golv;
                }
            }
        }

        public Golv[,] GolvRuta { get; set; }

        public int SkärmBredd { get; set; }

        public int SkärmHöjd { get; set; }

        public void Rita() {
            for(int i = 0; i < SkärmBredd; i++) {
                for(int j = 0; j < SkärmHöjd; j++) {
                    GolvRuta[i, j].Rita();
                }
            }
        }
    }
}