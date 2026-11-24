using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ChartTestFramwork
{
    /// <summary>
    /// ModelXML speichert die Live-Daten des EKG-Geräts in einer XML-Datei.
    /// Für jeden Tag gibt es eine eigene Datei, z. B. EKG_261124.xml.
    /// Jedes Speichern bekommt darin eine eigene Aufzeichnung mit Startzeit.
    /// </summary>
    internal class ModelXML : IModelLocalData
    {
        #region Felder (Objektvariablen)

        // Die XML-Datei im Arbeitsspeicher
        private XDocument doc;

        // Die aktuelle Aufzeichnung (ein Element innerhalb von doc)
        private XElement aufzeichnung;

        // Dateiname mit Datum, wird im Konstruktor gesetzt
        private string dateiname;

        // Zähler: Werte, die seit dem letzten Speichern dazugekommen sind
        private int neueWerte = 0;

        // Nach so vielen neuen Werten wird die Datei gespeichert
        private int speichernAlle = 100;

        // Die anderen Teile von MVC (werden in Main() übergeben)
        private IViewECG viewECG;
        private IModelECGDevice modelECGDevice;
        private IControllerECG controllerECG;

        #endregion

        #region Eigenschaften aus IModelLocalData

        IViewECG IModelLocalData.ViewEKG { set => viewECG = value; }
        IModelECGDevice IModelLocalData.ModelECGDevice { set => modelECGDevice = value; }
        IControllerECG IModelLocalData.ControllerECG { set => controllerECG = value; }

        #endregion

        #region Konstruktor

        public ModelXML()
        {
            // Eine Datei pro Tag (wie bei ModelCSVWerte), z. B. EKG_261124.xml
            dateiname = "EKG_" + DateTime.Now.ToString("yyMMdd") + ".xml";

            if (File.Exists(dateiname))
            {
                try
                {
                    // Die Datei von heute gibt es schon: laden, damit nichts verloren geht
                    doc = XDocument.Load(dateiname);
                }
                catch (Exception)
                {
                    // Die Datei ist beschädigt: mit einem neuen Dokument anfangen
                    doc = new XDocument(new XElement("EKG"));
                }
            }
            else
            {
                // Noch keine Datei da: neues Dokument mit dem Wurzelelement <EKG>
                doc = new XDocument(new XElement("EKG"));
            }
        }

        #endregion

        #region Methoden aus IModelLocalData

        void IModelLocalData.saveLiveData(ECGValue aktuellerWert)
        {
            // Beim ersten Wert: neue Aufzeichnung mit Startzeit anlegen
            if (aufzeichnung == null)
            {
                aufzeichnung = new XElement("Aufzeichnung",
                    new XAttribute("Start", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));
                doc.Element("EKG").Add(aufzeichnung);
            }

            // Messwert anhängen, z. B. <Messwert Zeitstempel="17">128</Messwert>
            aufzeichnung.Add(new XElement("Messwert",
                new XAttribute("Zeitstempel", aktuellerWert.TimeStamp),
                aktuellerWert.Value));

            // Nicht bei jedem Wert speichern, sondern nur alle 100 Werte
            neueWerte++;
            if (neueWerte >= speichernAlle)
            {
                doc.Save(dateiname);
                neueWerte = 0;
            }
        }

        List<double> IModelLocalData.get24hData()
        {
            throw new NotImplementedException();
        }

        void IModelLocalData.set24hData(ECGValue aktuellerWert)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
