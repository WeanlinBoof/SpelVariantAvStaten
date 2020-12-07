using System;
namespace SokratesSpelet {
    public static class Program {
        [STAThread]
        private static void Main() {
            using Spel spel = new Spel();
            spel.Run();
        }
    }
}
