using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ZyberLibrary.SpelStatusar;

namespace ZyberLibrary {

    public class Spel : Game {

        ///////////////////////////////////////////////////////////////////////////
        public static Random Random;

        public static int SkärmBredd = 1280;

        public static int SkärmHöjd = 720;

        public GraphicsDeviceManager Grafiker;

        public SpriteBatch spriteBatch;

        ///////////////////////////////////////////////////////////////////////////
        private Status NuvarandeStatus;

        private Status NästaStatus;

        ///////////////////////////////////////////////////////////////////////////
        // class konstruktör
        public Spel() {
            ///////////////////////////////////////////////////////////////////////////
            // gör så vi har grafiker
            Grafiker = new GraphicsDeviceManager(this);
            ///////////////////////////////////////////////////////////////////////////
            // säger vart resurserna hållerhus
            Content.RootDirectory = "Content";
        }

        ///////////////////////////////////////////////////////////////////////////
        //Ritar texturer btw det som ska rittas på skärm ska läggas mellan spriteBatch.Begin() & spriteBatch.End()
        protected override void Draw(GameTime gameTime) {

            // inte säker var ej jag som la den där
            GraphicsDevice.Clear(Color.PeachPuff);
            ///////////////////////////////////////////////////////////////////////////
            NuvarandeStatus.Rita(gameTime);
            ///////////////////////////////////////////////////////////////////////////
            base.Draw(gameTime);
        }

        ///////////////////////////////////////////////////////////////////////////
        //initialiserar skit inan allt börjar
        protected override void Initialize() {
            Random = new Random();

            // inte säker var ej jag som la den där
            spriteBatch = new SpriteBatch(GraphicsDevice);
            NuvarandeStatus = new SpelStatus(this, Content);
            NuvarandeStatus.Initialisera();
            ///////////////////////////////////////////////////////////////////////////
            base.Initialize();
        }

        ///////////////////////////////////////////////////////////////////////////
        //laddar in resurser
        protected override void LoadContent() {
            ///////////////////////////////////////////////////////////////////////////
            NuvarandeStatus.LaddaResurser();
            NästaStatus = null;
        }

        ///////////////////////////////////////////////////////////////////////////
        //uppdaterar
        protected override void Update(GameTime gameTime) {
            if(NästaStatus != null) {
                NuvarandeStatus = NästaStatus;
                NuvarandeStatus.LaddaResurser();
                NästaStatus = null;
            }

            //update
            NuvarandeStatus.Uppdatera(gameTime);
            ///////////////////////////////////////////////////////////////////////////
            NuvarandeStatus.EfterUppdatering(gameTime);
            ///////////////////////////////////////////////////////////////////////////
            base.Update(gameTime);
        }
    }
}