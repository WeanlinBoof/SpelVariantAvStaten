using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace SokratesSpelet {
    public class SpelResurser {
        // Döp ALLTID Variablerna med likvärdig Mönster T.ex om MinVariabel är av Texture2D så Ska Det Vara MinVariabelTextur
        public Texture2D BakgrundTextur { get; set; }
        public Texture2D SokratesUnknown { get; set; }
        public Texture2D EvilManPng { get; set; }
        public Texture2D DialogBox { get; set; }
        public SpelResurser(ContentManager Content) {
            /////////////// Texturer För kulissen 
            BakgrundTextur = Content.Load<Texture2D>("bakgrund");//byt till korrekt sen
            ///////////////
            SokratesUnknown = Content.Load<Texture2D>("unknown");
            EvilManPng = Content.Load<Texture2D>("evilman");
            DialogBox = Content.Load<Texture2D>("dialogboxpng");
        }
    }
}
