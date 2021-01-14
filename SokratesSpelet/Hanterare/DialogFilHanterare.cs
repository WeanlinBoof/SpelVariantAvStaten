using System;
using System.Xml;

namespace SokratesSpelet.Hanterare {

    public class DialogFilHanterare : GlobalHanterare {
        public XmlReader reader;
        protected string NuvarandeScen;
        public DialogFilHanterare(string txt) {
            NuvarandeScen = txt;
            LaddaResurser();
        }
       
        public override void LaddaResurser() {
            reader = XmlReader.Create($"Content/XML/DialogText/Dialog{NuvarandeScen}.xml");
            
        }

        public override void Rita() {
            throw new NotImplementedException();
        }

        public override void Uppdatera() {
            throw new NotImplementedException();
        }
        public string Dialogerna(string ReguestedDialog) {
            bool debug;
            while(reader.Read()) {
                // Only detect start elements.
                if(reader.IsStartElement()) {
                    // Get element name and switch on it.
                    switch(reader.Name) {
                        case "Dialoger":
                            break;
                        case "Dialog":
                            // Detect this article element.
                            Console.WriteLine("Start <Dialog> element.");
                            // Search for the attribute name on this current node.
                            string attribute = reader["Namn"];

                            if(attribute == ReguestedDialog && reader.Read()) {
                                // Next read will contain text.
                                string DialogRequest = reader.Value.Trim();
                                Console.WriteLine("  Text node: " + reader.Value.Trim());
                                return DialogRequest;
                            }
                            break;
                    }
                }
            }
            return ReguestedDialog;
        }
    }
}