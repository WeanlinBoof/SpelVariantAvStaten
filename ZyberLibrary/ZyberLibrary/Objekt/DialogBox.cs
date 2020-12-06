using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary.Objekt {

    public class DialogBox : Objekt {
        public float Bredd { get; set; }

        public float Höjd { get; set; }

        private Texture2D DBox { get; set; }

        private Rectangle PositionStorlek;

        private Rectangle SourceRectangle;
        private Color currentColor;    
        private SpriteFont currentFont;
        private SpriteFont italics;
        private SpriteFont dialogueFont;
        private SpriteFont dialogueFontItalic;
        private SpriteFont dialogueFontBold;
        private SpriteFont bold;
         

        public DialogBox(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            DBox = spelresurser.DialogLåda;
            Höjd = DBox.Height;
            Bredd = DBox.Width;
            SpriteBatch = spritebatch;
            PositionStorlek = new Rectangle((int)Position.X, (int)Position.Y, (int)((DBox.Width / 3) * 2.2f), DBox.Height / 4);
            SourceRectangle = new Rectangle(0, 0, DBox.Width, DBox.Height);
            italics = spelresurser.StrandardFont;
            dialogueFont = spelresurser.StrandardFont;
            dialogueFontBold = spelresurser.StrandardFont;
            dialogueFontItalic = spelresurser.StrandardFont;
            bold = spelresurser.StrandardFont;
        }
        public void DialogBoxWraper() {
        }
        public void TxTWrap(string text) {
           
        }
        public override void Rita() {
            SpriteBatch.Draw(DBox, PositionStorlek, SourceRectangle, Color.White);
        }
    }
}