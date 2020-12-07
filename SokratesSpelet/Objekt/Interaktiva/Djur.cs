namespace SokratesSpelet.Objekt.Interaktiva {

    //alla djur ska ärva djur
    public class Djur : InteraktivaObjekt {

        public enum Riktning {

            Upp,

            Ned,

            Höger,

            Vänster,
        };

        public bool Använder { get; set; }

        public float GolvTopSlut { get; set; }

        public virtual void Använd(bool användStatus) {
        }

        public virtual void FlyttaHöger(float RörelseHastighet) {
        }

        public virtual void FlyttaNed(float RörelseHastighet) {
        }

        public virtual void FlyttaUpp(float RörelseHastighet) {
        }

        public virtual void FlyttaVänster(float RörelseHastighet) {
        }
    }
}