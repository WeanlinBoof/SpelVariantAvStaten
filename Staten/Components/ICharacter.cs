using System;
using System.Collections.Generic;
using System.Text;

using Nez;
using Nez.Sprites;

namespace Staten.Components {
    public interface ICharacter {

        protected SpriteAnimator Animator { get; set; }
        protected SpriteAtlas SetUpAtlasTexturer(string AtlasFilPlats);
        protected void SetUpAnimations(SpriteAtlas spriteAtlas);
    }
}
