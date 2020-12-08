using System;
using System.Xml;

namespace SokratesSpelet.Hanterare {

    public class DialogFilHanterare : GlobalHanterare {

        public DialogFilHanterare() {
        }

        public override void LaddaResurser() {
            using XmlReader reader = XmlReader.Create("Content/XML/DialogText/TestDialog.xml");
            while(reader.Read()) {
                // Only detect start elements.
                if(reader.IsStartElement()) {
                    // Get element name and switch on it.
                    switch(reader.Name) {
                        case "Dialog":
                            // Detect this element.
                            Console.WriteLine("Start <Dialog> element.");
                            break;
                        case "article":
                            // Detect this article element.
                            Console.WriteLine("Start <article> element.");
                            // Search for the attribute name on this current node.
                            string attribute = reader["name"];
                            if(attribute != null) {
                                Console.WriteLine("  Has attribute name: " + attribute);
                            }
                            // Next read will contain text.
                            if(reader.Read()) {
                                Console.WriteLine("  Text node: " + reader.Value.Trim());
                            }
                            break;
                    }
                }
            }
        }

        public override void Rita() {
            throw new NotImplementedException();
        }

        public override void Uppdatera() {
            throw new NotImplementedException();
        }
    }
}