using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ChartTestFramwork
{
    internal class ModelXML : IModelLocalData
    {
        XDocument doc;
        string dateiname;
        IViewECG viewECG;
        IModelECGDevice modelECGDevice;
        IControllerECG controllerECG;

        IViewECG IModelLocalData.ViewEKG { set => viewECG = value; }
        IModelECGDevice IModelLocalData.ModelECGDevice { set => modelECGDevice = value; }
        IControllerECG IModelLocalData.ControllerECG { set => controllerECG = value; }

        public ModelXML()
        {
            //dateiname mit datum, damit jeden tag eine neue datei kommt
            dateiname = "EKG_" + DateTime.Now.ToString("yyMMdd") + ".xml";
            doc = new XDocument(new XElement("EKG")); //wurzel element
        }

        void IModelLocalData.saveLiveData(ECGValue aktuellerWert)
        {
            //neuen messwert mit zeitstemepl als attribut anhängen
            doc.Element("EKG").Add(new XElement("Messwert", new XAttribute("Zeitstempel", aktuellerWert.TimeStamp), aktuellerWert.Value));
            doc.Save(dateiname); //speichern
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
