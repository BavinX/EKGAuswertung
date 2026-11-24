using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ChartTestFramwork
{
    internal class ModelXML : IModelLocalData
    {
        // XML-Dokument im Arbeitsspeicher
        private XDocument doc;

        // Dateiname mit Datum, z. B. EKG_261124.xml
        private string dateiname;

        private IViewECG viewECG;
        private IModelECGDevice modelECGDevice;
        private IControllerECG controllerECG;

        IViewECG IModelLocalData.ViewEKG { set => viewECG = value; }
        IModelECGDevice IModelLocalData.ModelECGDevice { set => modelECGDevice = value; }
        IControllerECG IModelLocalData.ControllerECG { set => controllerECG = value; }

        public ModelXML()
        {
            dateiname = "EKG_" + DateTime.Now.ToString("yyMMdd") + ".xml";
            doc = new XDocument(new XElement("EKG"));
        }

        void IModelLocalData.saveLiveData(ECGValue aktuellerWert)
        {
            // <Messwert Zeitstempel="17">128</Messwert>
            XElement messwert = new XElement("Messwert",
                new XAttribute("Zeitstempel", aktuellerWert.TimeStamp),
                aktuellerWert.Value);

            doc.Element("EKG").Add(messwert);
            doc.Save(dateiname);
        }

        List<double> IModelLocalData.get24hData()
        {
            throw new NotImplementedException();
        }

        void IModelLocalData.set24hData(ECGValue aktuellerWert)
        {
            throw new NotImplementedException();
        }
    }
}
