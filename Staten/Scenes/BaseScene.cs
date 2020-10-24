using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.UI;


using System;
using System.Collections.Generic;
using System.Text;

namespace Staten.Scenes {
    //den classen som fixar dimensioner blir lika på alla scener

    public class BaseScene : Scene {
        public virtual Table Table { get; set; }
        public UICanvas UICanvas;
        public BaseScene() {

        }
        //kalla på denna i början på varje scen
        public override void OnStart() {
            //skapar en defult renderer 
            //TODO fixa render med specefika lager samt en screenSpaceRenderer För UI och liknade
            AddRenderer(new DefaultRenderer());
            //Gör Clear Färgen till detta
            ClearColor = new Color(58,61,101);
            //Fixar så att det är 640 x 360 pixlar på skärm skalade
            SetDesignResolution(640,360,SceneResolutionPolicy.ShowAllPixelPerfect);

        }
        public virtual void BruhUi() {
            UICanvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());

            Table = UICanvas.Stage.AddElement(new Table());

            Table.SetFillParent(true).Top().PadLeft(10).PadTop(50);
        }
    }
}
