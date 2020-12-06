using Microsoft.Xna.Framework.Input;

namespace ZyberLibrary.Modeler {

    public static class Inmatning {

        public class Kontroller {

            public Buttons Använd { get; set; }

            public Buttons Down { get; set; }

            public Buttons Left { get; set; }

            public Buttons Right { get; set; }

            public Buttons Up { get; set; }
        }

        public class Tangentbord {

            public Keys Använd { get; set; }

            public Keys Down { get; set; }

            public Keys Left { get; set; }

            public Keys Right { get; set; }

            public Keys Up { get; set; }
        }
    }
}