using System;
using System.Collections.Generic;
using System.Text;

namespace SokratesSpelet {
    public abstract class ObjektBas {
        public abstract void LaddaResurser();
        public abstract void Uppdatera();
        public abstract void Rita();
    }
    public class Lådda : ObjektBas {
        public override void LaddaResurser() {
//content
        }
        public override void Uppdatera() {
//update 
        }
        public override void Rita() {
//draw
        }


    }
}
