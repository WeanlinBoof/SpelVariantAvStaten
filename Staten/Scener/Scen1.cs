using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.UI;

namespace Staten.Scener {
    public class Scen1 : GrundScen {
        protected TextField textField;
        protected TextFieldStyle textFields;
        private Label Text;
        public override void Initialize() {
            BruhUi();

            Text = new Label("inget mottaget").SetFontScale(3);

            Table.Add(Text);

            Table.Row().SetPadTop(20);

            textFields = TextFieldStyle.Create(Color.White, Color.White, Color.Black, Color.DarkGray);

            textField = new TextField("", textFields);

            Table.Add(textField);

            Table.Row().SetPadRight(50);
            /*TextButton Kör = Table.Add(new TextButton("byebye borski" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();
              Kör.OnClicked += Koppplafrån;*/
            Skin skin = Skin.CreateDefaultSkin();

        }
    }
}
