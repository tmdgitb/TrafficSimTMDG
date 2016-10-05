using SimTMDG.LoadingForm;
using SimTMDG.Road;
using SimTMDG.Tools;
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

        RoadNetwork rn = new RoadNetwork();
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
                    XmlSerializer xs = new XmlSerializer(typeof(NodeOSM));
                    NodeOSM n = (NodeOSM)xs.Deserialize(tr);
                    n.latLonToPos(minLon, maxLat);

                    // ab in die Liste
                    rn.nodes.Add(n);

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
                        makeWaySegment(lnd, highwayTag, numlanesTag, oneway);
                    }
                    else
                    {
                        makeWaySegment(lnd, highwayTag, numlanesTag, "");
                    }

                    lf.StepLowerProgress();

                }

                lf.StepUpperProgress("Search segment connection...");
                lf.SetupLowerProgress("Looking for each connection in order to find prev and next segment", rn.segments.Count - 1);

                for (int i = 0; i < rn.segments.Count; i++)
                {
                    rn.segments[i].nextSegment = rn.segments.FindAll(x => x.startNode == rn.segments[i].endNode);
                    rn.segments[i].prevSegment = rn.segments.FindAll(x => x.endNode == rn.segments[i].startNode);

                    lf.StepLowerProgress();
                }

                //});
                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);

                #region manually generate route

                lf.StepUpperProgress("Manually Generate Routes...");

                //Debug.WriteLine("Segment Count" + nc.segments.Count);
                //manuallyAddRoute();


                try
                {
                    XmlSerializer xs2 = new XmlSerializer(typeof(RoadNetwork));
                    TextWriter tw = new StreamWriter(path + ".xml");
                    xs2.Serialize(tw, rn);
                } catch (Exception e)
                {
                    Console.WriteLine("Caught: {0}", e.Message);
                    if (e.InnerException != null)
                        Console.WriteLine("Inner exception: {0}", e.InnerException);
                }


                lf.StepUpperProgress("Done");
                lf.ShowLog();

                lf.Close();
                lf = null;



                #endregion
            }
        }


        private void makeWaySegment(List<XmlNode> lnd, XmlNode highwayTag, XmlNode numlanesTag, string oneway)
        {
            #region road type and lanes
            string highway;
            int numlanes;

            if (highwayTag != null) { highway = highwayTag.Attributes.GetNamedItem("v").Value; }
            else { highway = ""; }

            if (numlanesTag != null) { numlanes = int.Parse(numlanesTag.Attributes.GetNamedItem("v").Value); }
            else { numlanes = -1; }
            #endregion

            #region new approach
            if (oneway == "yes")        // Oneway Forward
            {
                for (int i = 0; i < lnd.Count - 1; i++)
                {

                    long ndId;
                    XmlNode ndIdNode = lnd[i].Attributes.GetNamedItem("ref");
                    //if (ndIdNode != null)
                    ndId = long.Parse(ndIdNode.Value);
                    //else
                    //    ndId = 0;

                    long ndNextId;
                    XmlNode ndIdNextNode = lnd[i + 1].Attributes.GetNamedItem("ref");
                    //if (ndIdNextNode != null)
                    ndNextId = long.Parse(ndIdNextNode.Value);
                    //else
                    //    ndNextId = 0;

                    if ((rn.nodes.Find(x => x.Id == ndId) != null) && (rn.nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        rn.segments.Add(new RoadSegmentOSM(rn.nodes.Find(x => x.Id == ndId), rn.nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
                    }
                }
            }
            else if (oneway == "-1") // Oneway Reverse
            {
                for (int i = lnd.Count - 1; i > 0; i--)
                {

                    long ndId;
                    XmlNode ndIdNode = lnd[i].Attributes.GetNamedItem("ref");
                    if (ndIdNode != null)
                        ndId = long.Parse(ndIdNode.Value);
                    else
                        ndId = 0;

                    long ndNextId;
                    XmlNode ndIdNextNode = lnd[i - 1].Attributes.GetNamedItem("ref");
                    if (ndIdNextNode != null)
                        ndNextId = long.Parse(ndIdNextNode.Value);
                    else
                        ndNextId = 0;

                    if ((rn.nodes.Find(x => x.Id == ndId) != null) && (rn.nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        rn.segments.Add(new RoadSegmentOSM(rn.nodes.Find(x => x.Id == ndId), rn.nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
                    }
                }
            }
            else                     // Two Way
            {
                for (int i = 0; i < lnd.Count - 1; i++)
                {
                    long ndId;
                    XmlNode ndIdNode = lnd[i].Attributes.GetNamedItem("ref");
                    if (ndIdNode != null)
                        ndId = long.Parse(ndIdNode.Value);
                    else
                        ndId = 0;

                    long ndNextId;
                    XmlNode ndIdNextNode = lnd[i + 1].Attributes.GetNamedItem("ref");
                    if (ndIdNextNode != null)
                        ndNextId = long.Parse(ndIdNextNode.Value);
                    else
                        ndNextId = 0;

                    RoadSegmentOSM tempSegment;

                    if ((rn.nodes.Find(x => x.Id == ndId) != null) && (rn.nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        tempSegment = new RoadSegmentOSM(rn.nodes.Find(x => x.Id == ndId), rn.nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway);

                        int lanePerDirection = (int)tempSegment.lanes.Count / 2;
                        double distanceShift = (double)(tempSegment.lanes.Count / (double)4) * (double)tempSegment.laneWidth;

                        if (lanePerDirection < 1)
                            lanePerDirection = 1;

                        rn.segments.Add(generateShiftedSegment(tempSegment, distanceShift, lanePerDirection, tempSegment.Highway, true));
                        rn.segments.Add(generateShiftedSegment(tempSegment, -distanceShift, lanePerDirection, tempSegment.Highway, false));

                    }

                }
            }
            #endregion
        }



        RoadSegmentOSM generateShiftedSegment(RoadSegmentOSM oriSegment, double distance, int numlanes, string highway, Boolean forward)
        {
            double angle = (Math.PI / 2) - Vector2.AngleBetween(oriSegment.startNode.Position, oriSegment.endNode.Position);
            Vector2 shift = new Vector2(distance * Math.Cos(angle), distance * Math.Sin(angle));

            NodeOSM newStart = new NodeOSM(new Vector2(oriSegment.startNode.Position.X + shift.X, oriSegment.startNode.Position.Y - shift.Y), true);
            NodeOSM newEnd = new NodeOSM(new Vector2(oriSegment.endNode.Position.X + shift.X, oriSegment.endNode.Position.Y - shift.Y), true);

            RoadSegmentOSM toReturn;

            if (forward)
            {
                toReturn = new RoadSegmentOSM(newStart, newEnd, numlanes, highway, "yes");
            }
            else
            {
                toReturn = new RoadSegmentOSM(newEnd, newStart, numlanes, highway, "yes");
            }

            return toReturn;
        }
    }
}
