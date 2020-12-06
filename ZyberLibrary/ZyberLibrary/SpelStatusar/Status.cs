using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary.SpelStatusar {

    public abstract class Status {

        protected ContentManager Innehåll;

        protected Spel Spel;

        ///////////////////////////////////////////////////////////////////////////
        protected Status(Spel spel, ContentManager innehåll) {
            Spel = spel;
            Innehåll = innehåll;
        }

        public SpriteBatch spriteBatch { get; set; }

        protected string Titel { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Det man vill uppdatera efter update
        /// </summary>
        /// <param name="gameTime">Spel tiden för Spellogik </param>
        public abstract void EfterUppdatering(GameTime gameTime);

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Används För Initialisering av saker så som Title,Fönster Egenskaper.
        /// </summary>
        public abstract void Initialisera();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Används För att ladda in de resurser som behöves
        /// </summary>
        public abstract void LaddaResurser();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ritar Ut Texturer På De Platser Man Sätter Dem.
        /// </summary>
        /// <param name="gameTime">Spel tiden för Spellogik</param>
        public abstract void Rita(GameTime gameTime);

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Det man vill uppdatera inom spelet t.ex logik, position och liknade
        /// </summary>
        /// <param name="gameTime">Spel tiden för Spellogik </param>
        public abstract void Uppdatera(GameTime gameTime);

        ///////////////////////////////////////////////////////////////////////////
    }
}