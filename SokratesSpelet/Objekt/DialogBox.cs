using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SokratesSpelet;

namespace SokratesSpelet.Objekt {

    public class DialogBox : Objekt {
        public float BackBoxBredd;
        public float BackBoxHöjd;
        public float FramBoxBredd;
        public float FramBoxHöjd;
        private Texture2D DBoxBakom;
        private Texture2D DBoxFram;
        private Rectangle BackBoxPositionStorlek;
        private Rectangle BackBoxSourceRectangle; 
        private Rectangle FramBoxPositionStorlek;
        private Rectangle FramBoxSourceRectangle;
        private Rectangle TextWrapRectangle;
        private Color currentColor;
        private SpriteFont currentFont;
        private SpriteFont italics;
        private SpriteFont dialogueFont;
        private SpriteFont dialogueFontItalic;
        private SpriteFont dialogueFontBold;
        private SpriteFont bold;
        private Vector2 TextPos;
        private string DialogTxT;
        private int TextUpdateTime;
        private int Timer;
        public DialogBox(float x, float y, SpriteBatch spritebatch, SpelResurser spelresurser) {
            Position.X = x;
            Position.Y = y;
            DBoxBakom = spelresurser.DialogLådaBack;
            DBoxFram = spelresurser.DialogLådaFram;
            SpriteBatch = spritebatch;
            //x2 = x1 + ((w1 - w2) / 2);
            //y2 = y1 + ((h1 - h2) / 2);
            SetDialogBoxStrorlek();
            SetFontsAndColors(spelresurser);
        }

        private void SetDialogBoxStrorlek() {

            BackBoxHöjd = DBoxBakom.Height / 4;//h1
            BackBoxBredd = DBoxBakom.Width / 3 * 2.2f;//w1

            FramBoxHöjd = DBoxFram.Height / 4;//h2
            FramBoxBredd = DBoxFram.Width / 3 * 2.2f;//w2
            DialogBoxWraper();
        }

        private void SetFontsAndColors(SpelResurser spelresurser) {
            currentColor = Color.Black;
            italics = spelresurser.StrandardFont;
            dialogueFont = spelresurser.StrandardFont;
            dialogueFontBold = spelresurser.StrandardFont;
            dialogueFontItalic = spelresurser.StrandardFont;
            bold = spelresurser.StrandardFont;
        }

        public void DialogBoxWraper() {
            ///////////////////////////////////////////////
            int W1 = (int)BackBoxBredd;
            int H1 = (int)BackBoxHöjd;
            int W2 = (int)FramBoxBredd;
            int H2 = (int)FramBoxHöjd;
            int W3 = (int)(FramBoxBredd/64*63);
            int H3 = (int)(FramBoxHöjd / 10 * 8);
            /////////////////////////////////////////////////////////////
            int X1 = (int)Position.X;
            int Y1 = (int)Position.Y;
            int X2 = X1 + ((W1 - W2) / 2);
            int Y2 = Y1 + ((H1 - H2) / 2);
            int X3 = X2 + ((W2 - W3) / 2);
            int Y3 = Y2 + ((H2 - H3) / 2);
            ///////////////////////////////////////////////
            BackBoxPositionStorlek = new Rectangle(X1, Y1, W1, H1);
            BackBoxSourceRectangle = new Rectangle(0, 0, DBoxBakom.Width, DBoxBakom.Height);
            FramBoxPositionStorlek = new Rectangle(X2, Y2, W2, H2);
            FramBoxSourceRectangle = new Rectangle(0, 0, DBoxFram.Width, DBoxFram.Height);
            ///////////////////////////////////////////////
            TextWrapRectangle = new Rectangle(X3, Y3, W3, H3);
            TextPos = new Vector2(X3, Y3);
        }
        public void SetText(string str) {
            DialogTxT = WordWrap.WrapText(dialogueFont, str, TextWrapRectangle.Width, TextWrapRectangle.Height);

        }
        public void SetText(string str,int miliSeconds) {
            TextUpdateTime = miliSeconds;
            DialogTxT = WordWrap.WrapText(dialogueFont, str, TextWrapRectangle.Width, TextWrapRectangle.Height);

        }
        public void SetText(int miliSeconds) {
            TextUpdateTime = miliSeconds;
            DialogTxT = WordWrap.WrapText(dialogueFont, WordWrap.RemaningText, TextWrapRectangle.Width, TextWrapRectangle.Height);
        }
        public void SetText() {
            DialogTxT = WordWrap.WrapText(dialogueFont, WordWrap.RemaningText, TextWrapRectangle.Width, TextWrapRectangle.Height);
        }
        public override void Uppdatera(GameTime gameTime) {
            Timer += gameTime.ElapsedGameTime.Milliseconds;
            while(Timer >= TextUpdateTime) {
                SetText();
                Timer = 0;
            }
        }
        public override void Rita() {
            SpriteBatch.Draw(DBoxBakom, BackBoxPositionStorlek, BackBoxSourceRectangle, Color.White);
            SpriteBatch.Draw(DBoxFram, FramBoxPositionStorlek, FramBoxSourceRectangle, Color.White);
            SpriteBatch.DrawString(dialogueFont, DialogTxT, TextPos, currentColor);

        }
    }
}