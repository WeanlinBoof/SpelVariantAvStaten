
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZyberLibrary {
    public static class WordWrap {
        public static string[] RemaningWords { get; set; }
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth) {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach(string word in words) {
                Vector2 size = spriteFont.MeasureString(word);

                if(lineWidth + size.X < maxLineWidth) {
                    sb.Append(word).Append(' ');
                    lineWidth += size.X + spaceWidth;
                    Thread.Sleep(100);
                }
                else {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                    Thread.Sleep(100);
                }

            }

            return sb.ToString();
        }
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth, float maxRow) {
            string[] words = text.Split(' ');
            if(RemaningWords != null) {
                words = RemaningWords;
                RemaningWords = null;
            }
            if(RemaningWords == null) {
                RemaningWords = new string[words.Length];
            }

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
                if(lineHeight * Row == lineHeight * maxRow) {
                    if(SaveRemaningWords) {
                        RemWorCounter = RemaningWordsFixer(words, RemWorCounter, i);

                    }
                    else if(lineWidth + size.X <= maxLineWidth) {
                        sb.Append(word).Append(' ');
                        lineWidth += size.X + spaceWidth;
                        //Thread.Sleep(100);
                    }
                    else {
                        sb.Append("\n");
                        lineWidth = size.X + spaceWidth;
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
            return sb.ToString();
        }
        public static string WrapText(SpriteFont spriteFont, string[] text, float maxLineWidth, float maxRow) {
            string[] words = RemaningWords;
            

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
                if(lineHeight * Row == lineHeight * maxRow) {
                    if(SaveRemaningWords) {
                        RemWorCounter = RemaningWordsFixer(words, RemWorCounter, i);

                    }
                    else if(lineWidth + size.X <= maxLineWidth) {
                        sb.Append(word).Append(' ');
                        lineWidth += size.X + spaceWidth;
                        //Thread.Sleep(100);
                    }
                    else {
                        sb.Append("\n");
                        lineWidth = size.X + spaceWidth;
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
            return sb.ToString();
        }
        private static int RemaningWordsFixer(string[] words, int RemWorCounter, int i) {
            RemaningWords[RemWorCounter] = words[i];
            RemWorCounter++;
            return RemWorCounter;
        }
    }
}
