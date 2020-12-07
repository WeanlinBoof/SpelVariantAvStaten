using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SokratesSpelet {

    //classen som inhåller alla spel resurser för tillfället
    public class SpelResurser {

        // Döp ALLTID Variablerna med likvärdig Mönster T.ex om MinVariabel är av Texture2D så Ska Det Vara MinVariabelTextur

        public SpelResurser(ContentManager Content) {
            /////////////// Texturer För kulissen
            BakgrundTextur = Content.Load<Texture2D>("bakgrund"); //byt till korrekt sen
            ///////////////
            SokratesUnknown = Content.Load<Texture2D>("unknown");
            EvilManPng = Content.Load<Texture2D>("evilman");
            DialogLådaBack = Content.Load<Texture2D>("dialogboxbakom");
            DialogLådaFram = Content.Load<Texture2D>("dialogboxfram");
            //////////////Standard Font
            StrandardFont = Content.Load<SpriteFont>("Standard");
        }
        
        public Texture2D BakgrundTextur { get; set; }
        public Texture2D DialogLådaBack { get; set; }
        public Texture2D DialogLådaFram { get; set; }
        public Texture2D EvilManPng { get; set; }
        public Texture2D GolvTextur { get; set; }
        public Texture2D SokratesUnknown { get; set; }
        public Texture2D StadTextur { get; set; }
        public SpriteFont StrandardFont { get; set; }
    }
}