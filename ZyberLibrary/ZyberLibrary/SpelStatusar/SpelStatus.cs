using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using ZyberLibrary.Modeler;
using ZyberLibrary.Objekt;
using ZyberLibrary.Objekt.Kulisser;

namespace ZyberLibrary.SpelStatusar {

    public class SpelStatus : Status {

        ///////////////////////////////////////////////////////////////////////////
        public Inmatning.Kontroller Kontroller = new Inmatning.Kontroller() {
            Up = Buttons.LeftThumbstickUp,
            Down = Buttons.LeftThumbstickDown,
            Right = Buttons.LeftThumbstickRight,
            Left = Buttons.LeftThumbstickLeft,
            Använd = Buttons.A
        };

        ///////////////////////////////////////////////////////////////////////////
        public int PlayerCount;

        ///////////////////////////////////////////////////////////////////////////
        public Inmatning.Tangentbord Tangentbord = new Inmatning.Tangentbord() {
            Up = Keys.Up,
            Down = Keys.Down,
            Right = Keys.Right,
            Left = Keys.Left,
            Använd = Keys.Space
        };

        private readonly SpelResurser SpelResurser;

        ///////////////////////////////////////////////////////////////////////////
        private Bakgrund Bakgrund;

        private float BakgrundX;

        private float BakgrundY;

        private float DBoxX;

        private float DBoxY;
        private string outstr;
        private string bruhb = "You are a man, because you are on Discord chat for gamers. What are your favourite male experiences you've had? Which males have inspired you the most? What is masculinity? Am I masculine? How big is your cock? How deep is your love? What are your responsibilities and what are you privileges? Give some loving to all the femboys and tomboys out there";
        ///////////////////////////////////////////////////////////////////////////
        private DialogBox DialogBox;

        private float FörflutenTid;

        ///////////////////////////////////////////////////////////////////////////
        public SpelStatus(Spel spel, ContentManager content) : base(spel, content) {
            SpelResurser = new SpelResurser(content);
            spriteBatch = spel.spriteBatch;
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void EfterUppdatering(GameTime gameTime) {
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void Initialisera() {
            Spel.Window.Title = Titel;//sätter titel på spel fönstret

            //SkärmHöjd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;// halva skärmhöjden

            //SkärmBredd = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;// halva skärmbredden

            Spel.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 150);// uppdaterar spelet 150 gånger per sekund istället för varje frame

            Spel.IsFixedTimeStep = true;// fixar en icke framerate beroende spel

            Spel.Grafiker.PreferredBackBufferWidth = Spel.SkärmBredd;//bufferbredd = uplösning som är vald

            Spel.Grafiker.PreferredBackBufferHeight = Spel.SkärmHöjd;//bufferhöjd = uplösning som är vald

            Spel.Grafiker.ApplyChanges();// Applicerar ändringar bruh

            Spel.IsMouseVisible = true; //Fixa Så man kan ändra detta beroder på situation
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void LaddaResurser() {
            ///////////////////////////////////////////
            BakgrundY = 0;
            BakgrundX = 0;
            Bakgrund = new Bakgrund(BakgrundX, BakgrundY, spriteBatch, SpelResurser);
            DBoxY = Spel.SkärmHöjd / 4 * 3;
            DBoxX = (Spel.SkärmBredd / 256);
            DialogBox = new DialogBox(DBoxX, DBoxY, spriteBatch, SpelResurser);
            outstr = WordWrap.WrapText(SpelResurser.StrandardFont,bruhb,200,5);

        }

        ///////////////////////////////////////////////////////////////////////////
        public override void Rita(GameTime gameTime) {

            // början på spriteBatch
            spriteBatch.Begin();
           
            //////////////////////////
            Bakgrund.Rita();
            //////////////////////////
            //DialogBox.Rita();
            //////////////////////////
            spriteBatch.DrawString(SpelResurser.StrandardFont, outstr, new Vector2(Spel.SkärmBredd / 2, Spel.SkärmHöjd / 2), Color.Crimson);
            //////////////////////////
            spriteBatch.End();

            //slut på spriteBatch
        }
        bool bro;
        ///////////////////////////////////////////////////////////////////////////
        public override void Uppdatera(GameTime gameTime) {

            // förflutenTid är mängden millisekunder som har förflutit sen start
            FörflutenTid += gameTime.ElapsedGameTime.Milliseconds;
            if(FörflutenTid >= 4000 && !bro) {
                outstr = WordWrap.WrapText(SpelResurser.StrandardFont, WordWrap.RemaningWords, 200, 5);
                bro = true;
            }


        }
    }
}