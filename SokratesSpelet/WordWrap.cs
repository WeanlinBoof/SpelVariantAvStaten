
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SokratesSpelet {
    public static class WordWrap {
        public static string[] RemaningWords;
        public static string RemaningText;
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth, float maxLineHeight) {
            string[] words = text.Split(' ');
            RemaningWords = new string[words.Length];

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;
            float lineHeight = spriteFont.MeasureString(words[0]).Y;
            int Row = 1;
            float spaceWidth = spriteFont.MeasureString(" ").X;
            int RemWorCounter = 0;
            bool SaveRemaningWords = false;
            for(int i = 0; i < words.Length; i++) {
                string word = words[i];
                Vector2 size = spriteFont.MeasureString(word);
                if(lineHeight * Row >= maxLineHeight) {
                    if(SaveRemaningWords) {
                        RemaningWords[RemWorCounter] = words[i];
                        RemWorCounter++;
                    }
                    else if(lineWidth + size.X <= maxLineWidth) {
                        sb.Append(word).Append(' ');
                        lineWidth += size.X + spaceWidth;
                        //Thread.Sleep(100);
                    }
                    else {
                        sb.Append("\n");
                        lineWidth = size.X + spaceWidth;
                        RemaningWords[RemWorCounter] = words[i];
                        RemWorCounter++;
                        SaveRemaningWords = true;
                    }
                }
                else if(lineWidth + size.X < maxLineWidth) {
                    sb.Append(word).Append(' ');
                    lineWidth += size.X + spaceWidth;
                    //Thread.Sleep(100);
                }
                else {
                    sb.Append("\n").Append(word).Append(' ');
                    lineWidth = size.X + spaceWidth;
                    Row++;
                    //Thread.Sleep(100);
                }
            }
            if(RemWorCounter!=0) {
                RemWorCounter++;
                string[] remWords = new string[RemWorCounter];
                for(int i = 0; i < RemWorCounter; i++) {
                    remWords[i] = RemaningWords[i];
                }
                RemaningText = string.Join(" ", remWords);
            }
           
            return sb.ToString();
        }
    }
}
