using System;

namespace SokratesSpelet {
    public static class Program {
        [STAThread]
        static void Main() {
            using(var game = new Spel())
                game.Run();
        }
    }
}
