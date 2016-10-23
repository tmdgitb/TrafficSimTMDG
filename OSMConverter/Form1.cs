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
        //double minLon;
        //double maxLon;
        //double minLat;
        //double maxLat;
        //Boolean boundsDefined = false;





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
                    rn.minLon = double.Parse(minLonNode.Value, CultureInfo.InvariantCulture);
                    //boundsDefined = true;
                }
                else { rn.minLon = 0; }

                if (maxLonNode != null)
                {
                    rn.maxLon = double.Parse(maxLonNode.Value, CultureInfo.InvariantCulture);
                    //boundsDefined = true;
                }
                else { rn.maxLon = 0; }

                if (minLatNode != null)
                {
                    rn.minLat = double.Parse(minLatNode.Value, CultureInfo.InvariantCulture);
                    //boundsDefined = true;
                }
                else { rn.minLat = 0; }

                if (maxLatNode != null)
                {
                    rn.maxLat = double.Parse(maxLatNode.Value, CultureInfo.InvariantCulture);
                    //boundsDefined = true;
                }
                else { rn.maxLat = 0; }

                Debug.WriteLine("minLong maxLat: " + rn.minLon + ", " + rn.maxLat);


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
                    n.latLonToPos(rn.minLon, rn.maxLat);

                    // ab in die Liste
                    rn.nodes.Add(n);
                    n.idx = rn.nodes.Count - 1;

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
                    List<RoadSegmentOSM> nextSegmentObj = new List<RoadSegmentOSM>();
                    List<RoadSegmentOSM> prevSegmentObj = new List<RoadSegmentOSM>();

                    //nextSegmentObj = rn.segments.FindAll(x => x.startNode == rn.segments[i].endNode);
                    //prevSegmentObj = rn.segments.FindAll(x => x.endNode == rn.segments[i].startNode);

                    NodeOSM tempStart;
                    NodeOSM tempEnd;

                    if (rn.segments[i].parentStartNode == null)
                    {
                        tempStart = rn.segments[i].startNode;
                        tempEnd = rn.segments[i].endNode;
                    }else
                    {
                        tempStart = rn.segments[i].parentStartNode;
                        tempEnd = rn.segments[i].parentEndNode;
                    }


                    for (int k = 0; k < rn.segments.Count; k++)                                             // search next / prev for each segments
                    {
                        if (rn.segments[k] != rn.segments[i])
                        {

                            if (rn.segments[k].forward == rn.segments[i].forward)
                            {
                                if (rn.segments[k].parentStartNode == null)                                         // if current search is not generated segment
                                {
                                    if (rn.segments[k].startNode == tempEnd)
                                    {
                                        nextSegmentObj.Add(rn.segments[k]);
                                    }
                                    else if (rn.segments[k].endNode == tempStart)
                                    {
                                        prevSegmentObj.Add(rn.segments[k]);
                                    }
                                }
                                else                                                                                // current search is generated segment
                                {
                                    if (rn.segments[k].parentStartNode == tempEnd)
                                    {
                                        nextSegmentObj.Add(rn.segments[k]);
                                    }
                                    else if (rn.segments[k].parentEndNode == tempStart)
                                    {
                                        prevSegmentObj.Add(rn.segments[k]);
                                    }
                                }
                            }
                        }
                    }


                    for (int j = 0; j < nextSegmentObj.Count; j++)
                    {
                        rn.segments[i].nextSegment.Add(nextSegmentObj[j].idx);
                    }

                    for (int j = 0; j < prevSegmentObj.Count; j++)
                    {
                        rn.segments[i].prevSegment.Add(prevSegmentObj[j].idx);
                    }
                    

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
                    using (TextWriter writer = new StreamWriter(path + ".xml"))
                    {
                        XmlSerializer xs2 = new XmlSerializer(typeof(RoadNetwork));
                        xs2.Serialize(writer, rn);
                        writer.Flush();
                    }

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
                        rn.segments.Add(new RoadSegmentOSM(rn, rn.nodes.Find(x => x.Id == ndId), rn.nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
                        rn.segments[rn.segments.Count - 1].idx = rn.segments.Count - 1;
                        rn.segments[rn.segments.Count - 1].startNodeIdx = rn.segments[rn.segments.Count - 1].startNode.idx;
                        rn.segments[rn.segments.Count - 1].endNodeIdx = rn.segments[rn.segments.Count - 1].endNode.idx;
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
                        rn.segments.Add(new RoadSegmentOSM(rn, rn.nodes.Find(x => x.Id == ndId), rn.nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
                        rn.segments[rn.segments.Count - 1].idx = rn.segments.Count - 1;
                        rn.segments[rn.segments.Count - 1].startNodeIdx = rn.segments[rn.segments.Count - 1].startNode.idx;
                        rn.segments[rn.segments.Count - 1].endNodeIdx = rn.segments[rn.segments.Count - 1].endNode.idx;
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
                        tempSegment = new RoadSegmentOSM(rn, rn.nodes.Find(x => x.Id == ndId), rn.nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway);
                        //tempSegments.Add(tempSegment);

                        int lanePerDirection = (int)tempSegment.lanes.Count / 2;
                        double distanceShift = (double)(tempSegment.lanes.Count / (double)4) * (double)tempSegment.laneWidth;

                        if (lanePerDirection < 1)
                            lanePerDirection = 1;


                        rn.segments.Add(generateShiftedSegment(tempSegment, distanceShift, lanePerDirection, tempSegment.Highway, true));

                        rn.segments[rn.segments.Count - 1].idx = rn.segments.Count - 1;
                        //rn.segments[rn.segments.Count - 1].parentSegment = tempSegment;
                        //rn.segments[rn.segments.Count - 1].startNodeIdx = rn.segments[rn.segments.Count - 1].startNode.idx;
                        //rn.segments[rn.segments.Count - 1].endNodeIdx = rn.segments[rn.segments.Count - 1].endNode.idx;


                        rn.segments.Add(generateShiftedSegment(tempSegment, -distanceShift, lanePerDirection, tempSegment.Highway, false));

                        rn.segments[rn.segments.Count - 1].idx = rn.segments.Count - 1;
                        //rn.segments[rn.segments.Count - 1].parentSegment = tempSegment;
                        //rn.segments[rn.segments.Count - 1].startNodeIdx = rn.segments[rn.segments.Count - 1].startNode.idx;
                        //rn.segments[rn.segments.Count - 1].endNodeIdx = rn.segments[rn.segments.Count - 1].endNode.idx;

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
            rn.nodes.Add(newStart);
            newStart.idx = rn.nodes.Count - 1;
            NodeOSM newEnd = new NodeOSM(new Vector2(oriSegment.endNode.Position.X + shift.X, oriSegment.endNode.Position.Y - shift.Y), true);
            rn.nodes.Add(newEnd);
            newEnd.idx = rn.nodes.Count - 1;

            RoadSegmentOSM toReturn;

            if (forward)
            {
                toReturn = new RoadSegmentOSM(rn, newStart, newEnd, numlanes, highway, "yes");
                toReturn.parentStartNode = oriSegment.startNode;
                toReturn.parentEndNode = oriSegment.endNode;
                toReturn.forward = 1;
            }
            else
            {
                toReturn = new RoadSegmentOSM(rn, newEnd, newStart, numlanes, highway, "yes");
                toReturn.parentStartNode = oriSegment.endNode;
                toReturn.parentEndNode = oriSegment.startNode;
                toReturn.forward = 0;
            }

            toReturn.startNodeIdx = toReturn.startNode.idx;
            toReturn.endNodeIdx = toReturn.endNode.idx;

            return toReturn;
        }
    }
}
