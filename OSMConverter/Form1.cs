using SimTMDG.LoadingForm;
using SimTMDG.Road;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace OSMConverter
{
    public partial class Form1 : Form
    {

        double minLon;
        double maxLon;
        double minLat;
        double maxLat;
        Boolean boundsDefined = false;



        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Load File
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.ExecutablePath;
                ofd.AddExtension = true;
                ofd.DefaultExt = @".xml";
                ofd.Filter = @"OpenStreetMap|*.osm";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadOsmMap(ofd.FileName);
                }
            }
            #endregion



            label1.Text = "File has been successfully converted. abcdefg lorem ipsum dolor sit amet";
        }


        private void LoadOsmMap(string path)
        {
            LoadingForm lf = new LoadingForm();
            lf.Text = "Loading file '" + path + "'...";
            lf.Show();

            lf.SetupUpperProgress("Loading Document...", 5);

            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            XmlNode mainNode = xd.SelectSingleNode("//osm");
            XmlNode bounds = xd.SelectSingleNode("//osm/bounds");

            if (bounds == null)
            {
                Debug.WriteLine("bounds null");
            }
            else
            {
                XmlNode minLonNode = bounds.Attributes.GetNamedItem("minlon");
                XmlNode maxLonNode = bounds.Attributes.GetNamedItem("maxlon");
                XmlNode minLatNode = bounds.Attributes.GetNamedItem("minlat");
                XmlNode maxLatNode = bounds.Attributes.GetNamedItem("maxlat");

                if (minLonNode != null)
                {
                    minLon = double.Parse(minLonNode.Value, CultureInfo.InvariantCulture);
                    boundsDefined = true;
                }
                else { minLon = 0; }

                if (maxLonNode != null)
                {
                    maxLon = double.Parse(maxLonNode.Value, CultureInfo.InvariantCulture);
                    boundsDefined = true;
                }
                else { maxLon = 0; }

                if (minLatNode != null)
                {
                    minLat = double.Parse(minLatNode.Value, CultureInfo.InvariantCulture);
                    boundsDefined = true;
                }
                else { minLat = 0; }

                if (maxLatNode != null)
                {
                    maxLat = double.Parse(maxLatNode.Value, CultureInfo.InvariantCulture);
                    boundsDefined = true;
                }
                else { maxLat = 0; }

                Debug.WriteLine("minLong maxLat: " + minLon + ", " + maxLat);


                lf.StepUpperProgress("Parsing Nodes...");
                XmlNodeList xnlLineNode = xd.SelectNodes("//osm/node");
                lf.SetupLowerProgress("Parsing Nodes", xnlLineNode.Count - 1);

                Stopwatch sw = Stopwatch.StartNew();
                foreach (XmlNode aXmlNode in xnlLineNode)
                {
                    // Node in einen TextReader packen
                    TextReader tr = new StringReader(aXmlNode.OuterXml);
                    // und Deserializen
                    XmlSerializer xs = new XmlSerializer(typeof(Node));
                    Node n = (Node)xs.Deserialize(tr);
                    n.latLonToPos(minLon, maxLat);

                    // ab in die Liste
                    nc._nodes.Add(n);

                    lf.StepLowerProgress();
                }
                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);


                lf.StepUpperProgress("Parsing Ways / Roads...");
                XmlNodeList xnlWayNode = xd.SelectNodes("//osm/way");
                lf.SetupLowerProgress("Parsing Ways", xnlWayNode.Count - 1);

                sw = Stopwatch.StartNew();
                foreach (XmlNode aXmlNode in xnlWayNode)
                //Parallel.ForEach(xnlWayNode, (XmlNode aXmlNode) =>
                {
                    XmlNodeList nds = aXmlNode.SelectNodes("nd");
                    XmlNode onewayTag = aXmlNode.SelectSingleNode("tag[@k='oneway']");
                    XmlNode highwayTag = aXmlNode.SelectSingleNode("tag[@k='highway']");
                    XmlNode numlanesTag = aXmlNode.SelectSingleNode("tag[@k='lanes']");

                    List<XmlNode> lnd = new List<XmlNode>();

                    foreach (XmlNode nd in nds)
                    {
                        lnd.Add(nd);
                    }

                    if (onewayTag != null)
                    {
                        string oneway = onewayTag.Attributes.GetNamedItem("v").Value;

                        //if (oneway == "-1")
                        //{
                        //    makeWaySegment_old(lnd, highwayTag, numlanesTag, oneway);
                        //}
                        //else
                        //{
                        makeWaySegment_old(lnd, highwayTag, numlanesTag, oneway);
                        //}
                    }
                    else
                    {
                        makeWaySegment_old(lnd, highwayTag, numlanesTag, "");
                    }

                    lf.StepLowerProgress();

                }

                lf.StepUpperProgress("Search segment connection...");
                lf.SetupLowerProgress("Search segment connectio", nc.segments.Count - 1);

                for (int i = 0; i < nc.segments.Count; i++)
                {
                    nc.segments[i].nextSegment = nc.segments.FindAll(x => x.startNode == nc.segments[i].endNode);
                    nc.segments[i].prevSegment = nc.segments.FindAll(x => x.endNode == nc.segments[i].startNode);

                    lf.StepLowerProgress();
                }

                //});
                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);

                #region manually generate route

                lf.StepUpperProgress("Manually Generate Routes...");

                //Debug.WriteLine("Segment Count" + nc.segments.Count);
                //manuallyAddRoute();

                lf.StepUpperProgress("Done");
                lf.ShowLog();

                lf.Close();
                lf = null;



                #endregion
            }
        }
    }
}
