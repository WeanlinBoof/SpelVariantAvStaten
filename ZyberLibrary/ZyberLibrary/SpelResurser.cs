using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary {

    //classen som inhåller alla spel resurser för tillfället
    public class SpelResurser {

        // Döp ALLTID Variablerna med likvärdig Mönster T.ex om MinVariabel är av Texture2D så Ska Det Vara MinVariabelTextur

        public SpelResurser(ContentManager Content) {
            /////////////// Texturer För kulissen
            BakgrundTextur = Content.Load<Texture2D>("bakgrund"); //byt till korrekt sen
            ///////////////
            SokratesUnknown = Content.Load<Texture2D>("unknown");
            EvilManPng = Content.Load<Texture2D>("evilman");
            DialogLåda = Content.Load<Texture2D>("dialogboxpng");
            //////////////Standard Font
            StrandardFont = Content.Load<SpriteFont>("Standard");
        }

        public Texture2D BakgrundTextur { get; set; }

        public Texture2D DialogLåda { get; set; }

        public Texture2D EvilManPng { get; set; }

        public Texture2D GolvTextur { get; set; }

        public Texture2D SokratesUnknown { get; set; }

        public Texture2D StadTextur { get; set; }

        public SpriteFont StrandardFont { get; set; }
    }
}