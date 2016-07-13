using SimTMDG.Road;
using SimTMDG.Time;
using SimTMDG.Tools;
using SimTMDG.Vehicle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SimTMDG
{
    public partial class Main : Form
    {
        #region TEMP
        private double _temp_stepsPerSeconds = 24;
        private double _temp_simulationDuration = 15;
        private NodeControl nc;
        private List<WaySegment> _route;
        private List<WaySegment> _route2;
        Random rnd = new Random();
        int vehCount = 0;
        Double timeMod = 0.0;
        #endregion


        #region Helper

        /// <summary>
        /// stores the window state
        /// </summary>
        [Serializable]
        public struct WindowSettings
        {
            /// <summary>
            /// Window state
            /// </summary>
            public FormWindowState _windowState;
            /// <summary>
            /// Position of window
            /// </summary>
            public Point _position;
            /// <summary>
            /// Size of window
            /// </summary>
            public Size _size;
        }

        private enum DragNDrop
        {
            NONE,
            MOVE_MAIN_GRID,
            MOVE_NODES,
            CREATE_NODE,
            MOVE_IN_SLOPE, MOVE_OUT_SLOPE,
            MOVE_TIMELINE_BAR, MOVE_EVENT, MOVE_EVENT_START, MOVE_EVENT_END,
            MOVE_THUMB_RECT,
            DRAG_RUBBERBAND
        }

        /// <summary>
        /// MainForm invalidation level
        /// </summary>
        public enum InvalidationLevel
        {
            /// <summary>
            /// invalidate everything
            /// </summary>
            ALL,
            /// <summary>
            /// invalidate only main canvas
            /// </summary>
            ONLY_MAIN_CANVAS,
            /// <summary>
            /// invalidate main canvas and timeline
            /// </summary>
            MAIN_CANVAS_AND_TIMELINE
        }

        #endregion

        #region Variables / Properties
        /// <summary>
        /// Simulation playing status
        /// </summary>
        private bool simIsPlaying = false;

        /// <summary>
		/// Stopwatch for timing of rendering
		/// </summary>
		private System.Diagnostics.Stopwatch renderStopwatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Stopwatch for timing the traffic logic
        /// </summary>
        private System.Diagnostics.Stopwatch thinkStopwatch = new System.Diagnostics.Stopwatch();


        /// <summary>
        /// NodeSteuerung
        /// </summary>
        //public NodeControl nodeControl = new NodeControl();

        #endregion

        public Main()
        {
            // - maxtime

            InitializeComponent();

            // - colorlist
            // - setdockingstuff

            //
            DaGrid.Dock = DockStyle.Fill;

            // - setstyle
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // - renderoptions


            //temp
            //roadSegment = new RoadSegment();
            nc = new NodeControl();
        }

        
        #region timer
        private void timerSimulation_Tick(object sender, EventArgs e)
        {
            thinkStopwatch.Reset();
            thinkStopwatch.Start();

            double tickLength = 1.0d / _temp_stepsPerSeconds; //(double)stepsPerSecondSpinEdit.Value;

            if (GlobalTime.Instance.currentTime < _temp_simulationDuration && (GlobalTime.Instance.currentTime + tickLength) >= _temp_simulationDuration)
            {
                //cbEnableSimulation.Checked = false;

                // TODO playButton_click?
                simIsPlaying = false;
                timerSimulation.Enabled = simIsPlaying;
                playButton.Text = "Play";
            }
            //timelineSteuerung.Advance(tickLength);
            GlobalTime.Instance.Advance(tickLength);

            //roadSegment.Tick(tickLength);
            nc.Tick(tickLength);

            #region tempVehGenerate
            if ((timeMod % 24) == 0.0)
            {
                if ((vehCount % 2) == 0)
                {
                    nc.Segments[0].vehicles.Add(new IVehicle(
                        nc.Segments[0],
                        Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
                        _route));
                }
                else
                {
                    nc.Segments[0].vehicles.Add(new IVehicle(
                        nc.Segments[0],
                        Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
                        _route2));
                }
                vehCount++;
            }
            timeMod++;
            #endregion

            ////tickCount++;

            //nodeSteuerung.Tick(tickLength);
            //trafficVolumeSteuerung.Tick(tickLength);

            //nodeSteuerung.Reset();

            thinkStopwatch.Stop();
            Debug.WriteLine(GlobalTime.Instance.currentTime);
            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }


        #endregion


        #region UI event
        private void playButton_Click(object sender, EventArgs e)
        {
            if (!simIsPlaying)
            {
                playButton.Text = "Pause";

            }
            else {
                playButton.Text = "Play";
            }

            simIsPlaying = !simIsPlaying;
            timerSimulation.Enabled = simIsPlaying;
        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            timerSimulation_Tick(sender, e);
            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }

        #endregion


        #region DaGrid
        void DaGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                //ZoomComboBox.SelectedIndex = Math2.Clamp(zoomComboBox.SelectedIndex + (e.Delta / 120), 0, zoomComboBox.Items.Count - 1);

            }
        }


        private void DaGrid_Resize(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion


        #region paint
        void DaGrid_Paint(object sender, PaintEventArgs e)
        {
            //Debug.WriteLine("DaGrid Paint");
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            renderStopwatch.Reset();
            renderStopwatch.Start();

            //roadSegment.Draw(e.Graphics);  
            nc.Draw(e.Graphics);          
            

            renderStopwatch.Stop();


            e.Graphics.Transform = new Matrix(1, 0, 0, 1, 0, 0);
            e.Graphics.DrawString(
                "thinking time: " + thinkStopwatch.ElapsedMilliseconds + "ms, possible thoughts per second: " + ((thinkStopwatch.ElapsedMilliseconds != 0) ? (1000 / thinkStopwatch.ElapsedMilliseconds).ToString() : "-"),
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                8,
                40);

            e.Graphics.DrawString(
                "rendering time: " + renderStopwatch.ElapsedMilliseconds + "ms, possible fps: " + ((renderStopwatch.ElapsedMilliseconds != 0) ? (1000 / renderStopwatch.ElapsedMilliseconds).ToString() : "-"),
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                8,
                56);
        }


        private void Invalidate(InvalidationLevel il)
        {
            base.Invalidate();
            switch (il)
            {
                case InvalidationLevel.ALL:
                    //thumbGrid.Invalidate();
                    DaGrid.Invalidate();
                    break;
				case InvalidationLevel.MAIN_CANVAS_AND_TIMELINE:
					DaGrid.Invalidate();
                    break;
				case InvalidationLevel.ONLY_MAIN_CANVAS:
					DaGrid.Invalidate();
                    break;
                default:
					break;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
        
        
        private void tempLoadButton_Click(object sender, EventArgs e)
        {

            #region Load File
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.ExecutablePath;
                ofd.AddExtension = true;
                ofd.DefaultExt = @".xml";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    GlobalTime.Instance.Reset();
                    nc.Load();

                    XmlDocument xd = new XmlDocument();
                    xd.Load(ofd.FileName);

                    XmlNode mainNode = xd.SelectSingleNode("//osm");
                    XmlNode bounds = xd.SelectSingleNode("//osm/bounds");

                    if (bounds == null)
                    {
                        Debug.WriteLine("bounds null");
                    }

                    double minLon;
                    XmlNode minLonNode = bounds.Attributes.GetNamedItem("minlon");

                    if (minLonNode != null)
                        minLon = double.Parse(minLonNode.Value) / 10000000;
                    else
                        minLon = 0;

                    double maxLat;
                    XmlNode maxLatNode = bounds.Attributes.GetNamedItem("maxlat");

                    if (maxLatNode != null)
                        maxLat = double.Parse(maxLatNode.Value) / 10000000;
                    else
                        maxLat = 0;

                    Debug.WriteLine("minLong maxLat: " + minLon + ", " + maxLat);

                    XmlNodeList xnlLineNode = xd.SelectNodes("//osm/node");
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
                    }

                    XmlNodeList xnlWayNode = xd.SelectNodes("//osm/way");
                    foreach (XmlNode aXmlNode in xnlWayNode)
                    {
                        XmlNodeList nds = aXmlNode.SelectNodes("nd");
                        List<XmlNode> lnd = new List<XmlNode>();

                        foreach (XmlNode nd in nds)
                        {
                            lnd.Add(nd);
                        }

                        for(int i=0; i < lnd.Count - 1; i++)
                        {
                            
                            long ndId;
                            XmlNode ndIdNode = lnd[i].Attributes.GetNamedItem("ref");
                            if (ndIdNode != null)
                                ndId = long.Parse(ndIdNode.Value);
                            else
                                ndId = 0;

                            long ndNextId;
                            XmlNode ndIdNextNode = lnd[i+1].Attributes.GetNamedItem("ref");
                            if (ndIdNextNode != null)
                                ndNextId = long.Parse(ndIdNextNode.Value);
                            else
                                ndNextId = 0;

                            if ((nc._nodes.Find(x => x.Id == ndId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
                            {
                                nc.segments.Add(new WaySegment(nc._nodes.Find(x => x.Id == ndId),  nc._nodes.Find(y => y.Id == ndNextId))); 
                            }

                            // Node in einen TextReader packen
                            //TextReader tr = new StringReader(nd.OuterXml);
                            
                            // und Deserializen
                            //XmlSerializer xs = new XmlSerializer(typeof(Node));
                            //Node n = (Node)xs.Deserialize(tr);

                            //// ab in die Liste
                            //nc._nodes.Add(n);
                        }
                    }
                }
            }

            #endregion

            //#region tempload
            //GlobalTime.Instance.Reset();
            //nc.Load();
            //nc._nodes.Add(new Node(new Vector2(100, 150)));
            //nc._nodes.Add(new Node(new Vector2(250, 150)));
            //nc._nodes.Add(new Node(new Vector2(300, 175)));
            //nc._nodes.Add(new Node(new Vector2(350, 225)));
            //nc._nodes.Add(new Node(new Vector2(400, 300)));
            //nc._nodes.Add(new Node(new Vector2(400, 350)));
            //nc._nodes.Add(new Node(new Vector2(400, 225)));
            //nc._nodes.Add(new Node(new Vector2(425, 200)));

            //nc.Segments.Add(new WaySegment(nc._nodes[0], nc._nodes[1]));
            //nc.Segments.Add(new WaySegment(nc._nodes[1], nc._nodes[2]));
            //nc.Segments.Add(new WaySegment(nc._nodes[2], nc._nodes[3]));
            //nc.Segments.Add(new WaySegment(nc._nodes[3], nc._nodes[4]));
            //nc.Segments.Add(new WaySegment(nc._nodes[4], nc._nodes[5]));
            //nc.Segments.Add(new WaySegment(nc._nodes[3], nc._nodes[6]));
            //nc.Segments.Add(new WaySegment(nc._nodes[6], nc._nodes[7]));



            //_route = new List<WaySegment>();
            //_route.Add(nc.Segments[0]);
            //_route.Add(nc.Segments[1]);
            //_route.Add(nc.Segments[2]);
            //_route.Add(nc.Segments[3]);
            //_route.Add(nc.Segments[4]);

            //_route2 = new List<WaySegment>();
            //_route2.Add(nc.Segments[0]);
            //_route2.Add(nc.Segments[1]);
            //_route2.Add(nc.Segments[2]);
            //_route2.Add(nc.Segments[5]);
            //_route2.Add(nc.Segments[6]);



            //nc.Segments[0].vehicles.Add(new IVehicle());
            //nc.Segments[0].vehicles[0].CurrentSegment = nc.Segments[0];
            //nc.Segments[0].vehicles[0].distance = 20;
            //nc.Segments[0].vehicles[0].color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            //nc.Segments[0].vehicles[0]
            //    .newCoord(
            //        nc.Segments[0].startNode.Position,
            //        nc.Segments[0].endNode.Position,
            //        nc.Segments[0].vehicles[0].distance);

            //nc.Segments[0].vehicles[0].Routing = _route;

            //nc.Segments[0].vehicles.Add(new IVehicle());
            //nc.Segments[0].vehicles[1].CurrentSegment = nc.Segments[0];
            //nc.Segments[0].vehicles[1].distance = 0;
            //nc.Segments[0].vehicles[1].color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            //nc.Segments[0].vehicles[1]
            //    .newCoord(
            //        nc.Segments[0].startNode.Position,
            //        nc.Segments[0].endNode.Position,
            //        nc.Segments[0].vehicles[1].distance);

            //nc.Segments[0].vehicles[1].Routing = _route2;
            //#endregion

            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }
    }
}
