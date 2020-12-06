using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZyberLibrary.Objekt {

    public class Objekt {

        public Vector2 Position = new Vector2(0, 0);

        public SpriteBatch SpriteBatch;

        public virtual void LaddaResurser() {
        }

        public virtual void Rita() {
        }

        public virtual void Uppdatera(GameTime gameTime) {
        }
    }
}