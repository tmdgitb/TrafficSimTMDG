using SimTMDG.DataSource;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SimTMDG
{
    public partial class Main : Form
    {
        #region TEMP
        private static double _temp_stepsPerSeconds = 30;
        //private double _temp_simulationDuration = 15;
        private double tickLength = 1.0d / _temp_stepsPerSeconds;
        private NodeControl nc;
        private List<RoadSegment> _route;
        //private List<RoadSegment> _route2;
        //private List<RoadSegment> _route3;
        //private List<RoadSegment> _route4;
        //private List<RoadSegment> _route5;
        //private List<RoadSegment> _route6;
        Random rnd = new Random();
        //int vehCount = 0;
        //int activeVehicles = 0;
        //Double timeMod = 0.0;
        //Bitmap bmp;
        //Bitmap bmpZoom;
        double minLon;
        double maxLon;
        double minLat;
        double maxLat;
        Boolean boundsDefined = false;
        Rectangle renderedRect;


        #region perf test temp
        /// <summary>
        ///  Performance test
        /// </summary>
        Process proc = Process.GetCurrentProcess();
        string logActiveVeh = "";
        string logThinkTime = "";
        string logMemUsed = "";
        string logRenderTime = "";
        string logObjDraw = "";
        int prevActVeh = -1;
        int prevDrawLine = -1;
        #endregion

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

        private DragNDrop howToDrag = DragNDrop.NONE;

        private Rectangle daGridRubberband;

        /// <summary>
        /// AutoscrollPosition vom daGrid umschließenden Panel. (Wird für Thumbnailanzeige benötigt)
        /// </summary>
        private Point daGridScrollPosition = new Point();

        /// <summary>
        /// Mittelpunkt der angezeigten Fläche in Weltkoordinaten. (Wird für Zoom benötigt)
        /// </summary>
        private PointF daGridViewCenter = new Point();

        //private List<GraphicsPath> additionalGraphics = new List<GraphicsPath>();

        private float[,] zoomMultipliers = new float[,] {
            { 0.05f, 20},
            { 0.1f, 10},
            { 0.15f, 1f/0.15f},
            { 0.2f, 5},
            { 0.25f, 4},
            { 1f/3f, 3},
            { 0.5f, 2},
            { 2f/3f, 1.5f},
            { 1, 1},
            { 1.5f, 2f/3f},
            { 2, 0.5f},
            { 4, 0.25f},
            { 8, 0.125f}
        };



        private int[] speedMultipliers = new int[]
        {
            1, 2, 4, 8, 16
        };

        #endregion

        public Main()
        {
            // - maxtime

            InitializeComponent();

            // - colorlist
            // - setdockingstuff

            //
            speedComboBox.SelectedIndex = 0;
            zoomComboBox.SelectedIndex = 8;
            daGridScrollPosition = new Point(0, 0);
            renderedRect = new Rectangle();
            UpdateDaGridClippingRect();
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

            GlobalTime.Instance.Advance(tickLength);

            nc.Tick(tickLength);

            nc.Reset();

            thinkStopwatch.Stop();


            #region perf test
            if (nc.ActiveVehicles > prevActVeh)
            {
                logActiveVeh += nc.ActiveVehicles + ",";
                logThinkTime += thinkStopwatch.ElapsedMilliseconds + ",";
                logMemUsed += proc.PrivateMemorySize64 + ",";
                prevActVeh = nc.ActiveVehicles;
            }
            #endregion

            //Debug.WriteLine(GlobalTime.Instance.currentTime);
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
            else
            {
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
                zoomComboBox.SelectedIndex = Math2.Clamp(zoomComboBox.SelectedIndex + (e.Delta / 120), 0, zoomComboBox.Items.Count - 1);

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
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            renderStopwatch.Reset();
            renderStopwatch.Start();

            e.Graphics.Transform = new Matrix(
                zoomMultipliers[zoomComboBox.SelectedIndex, 0], 0,
                0, zoomMultipliers[zoomComboBox.SelectedIndex, 0],
                -daGridScrollPosition.X * zoomMultipliers[zoomComboBox.SelectedIndex, 0], -daGridScrollPosition.Y * zoomMultipliers[zoomComboBox.SelectedIndex, 0]);


            if (boundsDefined)
            {
                nc.Draw(e.Graphics, zoomComboBox.SelectedIndex);
            }


            renderStopwatch.Stop();

            #region perf test (rendering)
            //if (nc.objDraw > prevDrawLine)
            //{
            //    logObjDraw += nc.objDraw + ",";
            //    logRenderTime += renderStopwatch.ElapsedMilliseconds + ",";
            //    prevDrawLine = nc.objDraw;
            //}
            #endregion


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

            e.Graphics.DrawString(
                "Active Vehicles: " + nc.ActiveVehicles,
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                8,
                72);
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

        /// <summary>
		/// aktualisiert das Clipping-Rectangle von DaGrid
		/// </summary>
		private void UpdateDaGridClippingRect()
        {
            if (zoomComboBox.SelectedIndex >= 0)
            {
                //    // daGridClippingRect aktualisieren
                //renderOptionsDaGrid.clippingRect.X = daGridScrollPosition.X;
                //renderOptionsDaGrid.clippingRect.Y = daGridScrollPosition.Y;
                //renderOptionsDaGrid.clippingRect.Width = (int)Math.Ceiling(pnlMainGrid.ClientSize.Width * zoomMultipliers[zoomComboBox.SelectedIndex, 1]);
                //renderOptionsDaGrid.clippingRect.Height = (int)Math.Ceiling(pnlMainGrid.ClientSize.Height * zoomMultipliers[zoomComboBox.SelectedIndex, 1]);
                renderedRect.X = daGridScrollPosition.X;
                renderedRect.Y = daGridScrollPosition.Y;
                renderedRect.Width = (int)Math.Ceiling(DaGrid.Width * zoomMultipliers[zoomComboBox.SelectedIndex, 1]);
                renderedRect.Height = (int)Math.Ceiling(DaGrid.Height * zoomMultipliers[zoomComboBox.SelectedIndex, 1]);

                daGridViewCenter = new PointF(
                    daGridScrollPosition.X + (DaGrid.Width / 2 * zoomMultipliers[zoomComboBox.SelectedIndex, 1]),
                    daGridScrollPosition.Y + (DaGrid.Height / 2 * zoomMultipliers[zoomComboBox.SelectedIndex, 1]));


                if (nc != null)
                {
                    nc.setBounds(renderedRect);
                }

                //    RectangleF bounds = nodeSteuerung.GetLineNodeBounds();
                //    float zoom = Math.Min(1.0f, Math.Min((float)thumbGrid.ClientSize.Width / bounds.Width, (float)thumbGrid.ClientSize.Height / bounds.Height));

                //    thumbGridClientRect = new Rectangle(
                //        (int)Math.Round((daGridScrollPosition.X - bounds.X) * zoom),
                //        (int)Math.Round((daGridScrollPosition.Y - bounds.Y) * zoom),
                //        (int)Math.Round(pnlMainGrid.ClientSize.Width * zoomMultipliers[zoomComboBox.SelectedIndex, 1] * zoom),
                //        (int)Math.Round(pnlMainGrid.ClientSize.Height * zoomMultipliers[zoomComboBox.SelectedIndex, 1] * zoom));

                //    lblScrollPosition.Text = "Canvas Location (dm): (" + daGridScrollPosition.X + ", " + daGridScrollPosition.Y + ") -> (" + (daGridScrollPosition.X + renderOptionsDaGrid.clippingRect.Width) + ", " + (daGridScrollPosition.Y + renderOptionsDaGrid.clippingRect.Height) + ")";

                //    UpdateConnectionsRenderCache();
            }
        }

        void DaGrid_MouseDown(object sender, MouseEventArgs e)
        {
            Vector2 clickedPosition = new Vector2(e.X, e.Y);
            clickedPosition *= zoomMultipliers[zoomComboBox.SelectedIndex, 1];
            clickedPosition += daGridScrollPosition;

            // Node Gedöns
            switch (e.Button)
            {
                case MouseButtons.Right:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        #region Nodes löschen
                        //this.Cursor = Cursors.Default;
                        //// LineNode entfernen
                        //LineNode nodeToDelete = nodeSteuerung.GetLineNodeAt(clickedPosition);
                        //// checken ob gefunden
                        //if (nodeToDelete != null)
                        //{
                        //    if (selectedLineNodes.Contains(nodeToDelete))
                        //    {
                        //        selectedLineNodes.Remove(nodeToDelete);
                        //    }
                        //    nodeSteuerung.DeleteLineNode(nodeToDelete);
                        //}
                        #endregion
                    }
                    else
                    {
                        #region move main grid
                        howToDrag = DragNDrop.MOVE_MAIN_GRID;
                        daGridRubberband.Location = clickedPosition;
                        this.Cursor = Cursors.SizeAll;
                        #endregion
                    }

                    break;

                case MouseButtons.Left:
                    foreach (RoadSegment rs in nc.segments)
                    {
                        if ((rs.startNode.positionRect.Contains(clickedPosition)) ||
                            (rs.endNode.positionRect.Contains(clickedPosition)))
                        {
                            rs.ToggleDebugID();
                        }
                    }

                    foreach (RoadSegment rs in nc.segments)
                    {
                        foreach (SegmentLane ln in rs.lanes)
                        {
                            foreach (IVehicle v in ln.vehicles)
                            {
                                if (v.positionRect.Contains(clickedPosition))
                                {
                                    v.ToggleDebug();
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
            Invalidate(InvalidationLevel.ONLY_MAIN_CANVAS);
        }

        void DaGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 clickedPosition = new Vector2(e.X, e.Y);
            clickedPosition *= zoomMultipliers[zoomComboBox.SelectedIndex, 1];
            clickedPosition += daGridScrollPosition;
            //lblMouseCoordinates.Text = "Current Mouse Coordinates (m): " + (clickedPosition * 0.1).ToString();

            this.Cursor = (howToDrag == DragNDrop.MOVE_MAIN_GRID) ? Cursors.SizeAll : Cursors.Default;

            //if (selectedLineNodes != null)
            //{
            switch (howToDrag)
            {
                case DragNDrop.MOVE_MAIN_GRID:
                    clickedPosition = new Vector2(e.X, e.Y);
                    clickedPosition *= zoomMultipliers[zoomComboBox.SelectedIndex, 1];
                    daGridScrollPosition = new Point((int)Math.Round(-clickedPosition.X + daGridRubberband.X), (int)Math.Round(-clickedPosition.Y + daGridRubberband.Y));
                    UpdateDaGridClippingRect();
                    Invalidate(InvalidationLevel.ONLY_MAIN_CANVAS);
                    break;
                default:
                    break;
            }
            //}
        }

        void DaGrid_MouseUp(object sender, MouseEventArgs e)
        {
            Vector2 clickedPosition = new Vector2(e.X, e.Y);
            clickedPosition *= zoomMultipliers[zoomComboBox.SelectedIndex, 1];
            clickedPosition += daGridScrollPosition;
            this.Cursor = Cursors.Default;

            switch (howToDrag)
            {
                case DragNDrop.MOVE_MAIN_GRID:
                    //thumbGrid.Invalidate();
                    break;
                default:
                    break;
            }

            howToDrag = DragNDrop.NONE;
            Invalidate(InvalidationLevel.ONLY_MAIN_CANVAS);
        }


        //void DrawZoom(Graphics g, Rectangle srcRect)
        //{
        //    Rectangle dstRect = new Rectangle(0, 0, DaGrid.Width, DaGrid.Height);
        //    g.DrawImage(bmp, dstRect, srcRect, GraphicsUnit.Pixel);
        //}


        #endregion


        // Flag -- new load
        private List<XmlNodeList> nextSegmentNodesList = new List<XmlNodeList>();
        private List<XmlNodeList> prevSegmentNodesList = new List<XmlNodeList>();

        private void LoadMap(string path)
        {
            LoadingForm.LoadingForm lf = new LoadingForm.LoadingForm();
            lf.Text = "Loading file '" + path + "'...";
            lf.Show();

            lf.SetupUpperProgress("Loading Document...", 3);

            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            XmlNode mainNode = xd.SelectSingleNode("//RoadNetwork");


            #region parsing boundary

            if (mainNode == null)
            {
                Debug.WriteLine("bounds null");
            }
            else
            {


                XmlNode minLonNode = mainNode.Attributes.GetNamedItem("minlon");
                XmlNode maxLonNode = mainNode.Attributes.GetNamedItem("maxlon");
                XmlNode minLatNode = mainNode.Attributes.GetNamedItem("minlat");
                XmlNode maxLatNode = mainNode.Attributes.GetNamedItem("maxlat");

                if (minLonNode != null)
                {
                    minLon = double.Parse(minLonNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { minLon = 0; }

                if (maxLonNode != null)
                {
                    maxLon = double.Parse(maxLonNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { maxLon = 0; }

                if (minLatNode != null)
                {
                    minLat = double.Parse(minLatNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { minLat = 0; }

                if (maxLatNode != null)
                {
                    maxLat = double.Parse(maxLatNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { maxLat = 0; }

                UpdateDaGridClippingRect();


                Debug.WriteLine("minLong maxLat: " + minLon + ", " + maxLat);
                #endregion



                #region parsing nodes
                lf.StepUpperProgress("Parsing Nodes...");
                XmlNodeList xnlLineNode = mainNode.SelectNodes("nodes/node");
                lf.SetupLowerProgress("Parsing Nodes", xnlLineNode.Count - 1);

                Stopwatch sw = Stopwatch.StartNew();
                foreach (XmlNode aXmlNode in xnlLineNode)
                {
                    TextReader tr = new StringReader(aXmlNode.OuterXml);
                    XmlSerializer xs = new XmlSerializer(typeof(Node));
                    Node n = (Node)xs.Deserialize(tr);

                    nc._nodes.Add(n);

                    lf.StepLowerProgress();
                }
                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);
                #endregion


                #region parsing segments
                lf.StepUpperProgress("Parsing Road Segments...");
                XmlNodeList xnlSegments = mainNode.SelectNodes("segments/RoadSegment");
                lf.SetupLowerProgress("Parsing Segments", xnlSegments.Count - 1);

                sw = Stopwatch.StartNew();
                foreach (XmlNode aXmlNode in xnlSegments)
                {
                    TextReader tr = new StringReader(aXmlNode.OuterXml);
                    XmlSerializer xs = new XmlSerializer(typeof(RoadSegment));
                    RoadSegment s = (RoadSegment)xs.Deserialize(tr);

                    // parse start & end node
                    XmlNode startNode = aXmlNode.SelectSingleNode("startNodeIdx");
                    XmlNode endNode = aXmlNode.SelectSingleNode("endNodeIdx");

                    s.startNode = nc._nodes[Int32.Parse(startNode.FirstChild.Value)];
                    s.endNode = nc._nodes[Int32.Parse(endNode.FirstChild.Value)];

                    Vector2 difference = new Vector2();
                    difference.X = s.endNode.Position.X - s.startNode.Position.X;
                    difference.Y = s.endNode.Position.Y - s.startNode.Position.Y;

                    s.MidCoord.X = (float)(difference.X * (s.Length / 2)) / (float)s.Length + (float)s.startNode.Position.X;
                    s.MidCoord.Y = (float)(difference.Y * (s.Length / 2)) / (float)s.Length + (float)s.startNode.Position.Y;


                    // parse next & prev segments
                    // TODO move to next upper progress. Needs to list all the segments before doing this!!!
                    XmlNodeList nextSegmentNodes = aXmlNode.SelectNodes("nextSegment/int");
                    XmlNodeList prevSegmentNodes = aXmlNode.SelectNodes("prevSegment/int");


                    // parse lane segments
                    XmlNodeList laneNodes = aXmlNode.SelectNodes("lanes/SegmentLaneOSM");

                    foreach (XmlNode laneNode in laneNodes)
                    {
                        TextReader trLane = new StringReader(laneNode.OuterXml);
                        XmlSerializer xsLane = new XmlSerializer(typeof(SegmentLane));
                        SegmentLane lane = (SegmentLane)xsLane.Deserialize(trLane);

                        // parse start & end node
                        XmlNode startLaneNode = laneNode.SelectSingleNode("startNodeIdx");
                        XmlNode endLaneNode = laneNode.SelectSingleNode("endNodeIdx");

                        lane.setStartEndNode(nc._nodes[Int32.Parse(startLaneNode.FirstChild.Value)],
                            nc._nodes[Int32.Parse(endLaneNode.FirstChild.Value)]);

                        // Calculate Length

                        s.lanes.Add(lane);
                    }


                    nc.segments.Add(s);
                    nextSegmentNodesList.Add(nextSegmentNodes);
                    prevSegmentNodesList.Add(prevSegmentNodes);
                    //nextSegmentNodesList[nc.segments.Count - 1] = nextSegmentNodes;
                    //prevSegmentNodesList[nc.segments.Count - 1] = prevSegmentNodes;

                    lf.StepLowerProgress();
                }
                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);
                #endregion


                #region Segments Connections
                lf.StepUpperProgress("Search for Segments Connections...");
                lf.SetupLowerProgress("Searching for Connections", nc.Segments.Count - 1);

                sw = Stopwatch.StartNew();
                for (int i = 0; i < nc.Segments.Count; i++)
                {
                    foreach (XmlNode nextSegmentNode in nextSegmentNodesList[i])
                    {
                        nc.segments[i].nextSegment.Add(nc.segments[Int32.Parse(nextSegmentNode.FirstChild.Value)]);
                    }

                    foreach (XmlNode prevSegmentNode in prevSegmentNodesList[i])
                    {
                        nc.segments[i].prevSegment.Add(nc.segments[Int32.Parse(prevSegmentNode.FirstChild.Value)]);
                    }

                    lf.StepLowerProgress();
                }

                nextSegmentNodesList.Clear();
                prevSegmentNodesList.Clear();

                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);
                #endregion

                lf.StepUpperProgress("Done");
                lf.ShowLog();

                nc.manuallyAddRoute();

                lf.Close();
                lf = null;

                nc._nodes.Clear();
            }
        }


        private void LoadOsmMap_old(string path)
        {
            LoadingForm.LoadingForm lf = new LoadingForm.LoadingForm();
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
                    minLon = double.Parse(minLonNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { minLon = 0; }

                if (maxLonNode != null)
                {
                    maxLon = double.Parse(maxLonNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { maxLon = 0; }

                if (minLatNode != null)
                {
                    minLat = double.Parse(minLatNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { minLat = 0; }

                if (maxLatNode != null)
                {
                    maxLat = double.Parse(maxLatNode.Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                    boundsDefined = true;
                }
                else { maxLat = 0; }

                UpdateDaGridClippingRect();


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

                // TODO find by segment ID for route addition
                // _route.segments.Find(x => x.Id == segmentToAddID)

                Debug.WriteLine("Segment Count" + nc.segments.Count);
                //manuallyAddRoute();

                lf.StepUpperProgress("Done");
                lf.ShowLog();

                lf.Close();
                lf = null;



                #endregion

                #region draw road as background
                //bmp = new Bitmap((int)Math.Ceiling((maxLon - minLon) * 111111), (int)Math.Ceiling((maxLat - minLat) * 111111));
                //using(Graphics g = Graphics.FromImage(bmp))
                //{
                //    g.SmoothingMode = SmoothingMode.HighQuality;
                //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //    foreach (WaySegment ws in nc.Segments)
                //    {
                //        ws.Draw(g);
                //    }
                //}
                //Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
                #endregion
            }


        }



        private void LoadOsmMap(string path)
        {
            LoadingForm.LoadingForm lf = new LoadingForm.LoadingForm();
            lf.Text = "Loading file '" + path + "'...";
            lf.Show();

            lf.SetupUpperProgress("Loading Document...", 5);
            lf.StepUpperProgress("Parsing Header...");

            XDocument xd = new XDocument();
            xd = XDocument.Load(path);
            XElement mainNode = xd.Element("osm");

            XElement bounds = mainNode.Element("bounds");

            // Check if there's a bounds or not
            // We need boundary for projecting lat lon to cartesian coord
            if (bounds == null)
            {
                Debug.WriteLine("bounds null");
            }
            else
            {
                minLon = double.Parse(bounds.Attribute("minlon").Value, CultureInfo.InvariantCulture);
                maxLon = double.Parse(bounds.Attribute("maxlon").Value, CultureInfo.InvariantCulture);
                minLat = double.Parse(bounds.Attribute("minlat").Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;                
                maxLat = double.Parse(bounds.Attribute("maxlat").Value, CultureInfo.InvariantCulture);// / 1000;// 10000000;
                boundsDefined = true;

                //nc.setBounds(minLon, maxLon, minLat, maxLat);

                Debug.WriteLine("minLong maxLat: " + minLon + ", " + maxLat);


                lf.StepUpperProgress("Searching nodes in highways...");

                // Find only <node> referred by <way>
                // Make sure to only include <way> with highway tag
                // Also make sure no duplicate <node>

                // This code lists <node> id
                var query = xd.Descendants("way")
                            .Where(p => p.Elements("tag")
                                .Any(c => (string)c.Attribute("k") == "highway")
                            ).Elements("nd")
                            .GroupBy(i => i.Attribute("ref").Value)
                            .Select(g => g.Key)
                            .ToList();

                //var query = from way in xd.Descendants("way")
                //            where way.Elements("tag").Any(c => (string)c.Attribute("k") == "highway")
                //            select way.Elements("nd")
                //            group i by i.Attribute("ref").Value


                //var query = mainNode.Elements("node").Select(g => (string) g.Attribute("id")).ToList();
                lf.StepUpperProgress("Parsing nodes...");

                lf.SetupLowerProgress("Parsing nodes...", query.Count() - 1);
                Stopwatch sw = Stopwatch.StartNew();

                // Deserialize XML for each <node> in query
                // Find node by id, create new node, calculate position
                for (int i = 0; i < query.Count(); i++)
                {
                    Node nd = (from node in mainNode.Elements("node")
                               where node.Attribute("id").Value == query[i]
                               select new Node()
                               {
                                   Id = Convert.ToInt64(node.Attribute("id").Value),
                                   Lat = double.Parse(node.Attribute("lat").Value, CultureInfo.InvariantCulture),// / 10000000,
                                   Long = double.Parse(node.Attribute("lon").Value, CultureInfo.InvariantCulture)// / 10000000
                               }).Single();

                    nd.latLonToPos(minLon, maxLat);
                    nc._nodes.Add(nd);

                    lf.StepLowerProgress();
                }
                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);

                lf.StepUpperProgress("Parsing ways / roads...");

                List<XElement> wayQuery = xd.Descendants("way")
                                .Where(p => p.Elements("tag")
                                    .Any(c => (string)c.Attribute("k") == "highway")
                                ).ToList();

                //List<XElement> wayQuery = mainNode.Elements("way").ToList();
                lf.SetupLowerProgress("Parsing ways...", wayQuery.Count() - 1);
                sw = Stopwatch.StartNew();

                foreach (XElement way in wayQuery)
                {
                    List<XElement> nodeQuery = way.Elements("nd").ToList();
                    //Debug.WriteLine("query " + wayQuery[i] + " elem" + nodeQuery);

                    var oneway = from tag in way.Elements("tag")
                                 where tag.Attribute("k").Value == "oneway"
                                 select (string)tag.Attribute("v").Value;

                    if (oneway.Count() > 0)
                    {
                        if (oneway.Single() == "-1")
                        {
                            makeWaySegment(nodeQuery, false);
                        }
                        else
                        {
                            makeWaySegment(nodeQuery, true);
                        }
                    }
                    else
                    {
                        makeWaySegment(nodeQuery, true);
                    }

                    //Debug.WriteLine("values" + oneway);
                    lf.StepLowerProgress();
                }

                sw.Stop();
                Console.WriteLine("Total query time: {0} ms", sw.ElapsedMilliseconds);


            }

            lf.StepUpperProgress("Done");
            lf.ShowLog();

            lf.Close();
            lf = null;
        }

        private void makeWaySegment_old(List<XmlNode> lnd, XmlNode highwayTag, XmlNode numlanesTag, string oneway)
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

                    if ((nc._nodes.Find(x => x.Id == ndId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        nc.segments.Add(new RoadSegment(nc._nodes.Find(x => x.Id == ndId), nc._nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
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

                    if ((nc._nodes.Find(x => x.Id == ndId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        nc.segments.Add(new RoadSegment(nc._nodes.Find(x => x.Id == ndId), nc._nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
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

                    RoadSegment tempSegment;

                    if ((nc._nodes.Find(x => x.Id == ndId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        tempSegment = new RoadSegment(nc._nodes.Find(x => x.Id == ndId), nc._nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway);

                        int lanePerDirection = (int)tempSegment.lanes.Count / 2;
                        double distanceShift = (double)(tempSegment.lanes.Count / (double)4) * (double)tempSegment.laneWidth;

                        if (lanePerDirection < 1)
                            lanePerDirection = 1;

                        nc.segments.Add(generateShiftedSegment(tempSegment, distanceShift, lanePerDirection, tempSegment.Highway, true));
                        nc.segments.Add(generateShiftedSegment(tempSegment, -distanceShift, lanePerDirection, tempSegment.Highway, false));

                    }

                }
            }
            #endregion


            #region prev approach
            //if (oneway != "-1")
            //{
            //    for (int i = 0; i < lnd.Count - 1; i++)
            //    {

            //        long ndId;
            //        XmlNode ndIdNode = lnd[i].Attributes.GetNamedItem("ref");
            //        if (ndIdNode != null)
            //            ndId = long.Parse(ndIdNode.Value);
            //        else
            //            ndId = 0;

            //        long ndNextId;
            //        XmlNode ndIdNextNode = lnd[i + 1].Attributes.GetNamedItem("ref");
            //        if (ndIdNextNode != null)
            //            ndNextId = long.Parse(ndIdNextNode.Value);
            //        else
            //            ndNextId = 0;

            //        if ((nc._nodes.Find(x => x.Id == ndId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
            //        {
            //            nc.segments.Add(new RoadSegment(nc._nodes.Find(x => x.Id == ndId), nc._nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
            //        }
            //    }
            //}
            //else // Oneway: Reverse
            //{
            //    for (int i = lnd.Count - 1; i > 0; i--)
            //    {

            //        long ndId;
            //        XmlNode ndIdNode = lnd[i].Attributes.GetNamedItem("ref");
            //        if (ndIdNode != null)
            //            ndId = long.Parse(ndIdNode.Value);
            //        else
            //            ndId = 0;

            //        long ndNextId;
            //        XmlNode ndIdNextNode = lnd[i - 1].Attributes.GetNamedItem("ref");
            //        if (ndIdNextNode != null)
            //            ndNextId = long.Parse(ndIdNextNode.Value);
            //        else
            //            ndNextId = 0;

            //        if ((nc._nodes.Find(x => x.Id == ndId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
            //        {
            //            nc.segments.Add(new RoadSegment(nc._nodes.Find(x => x.Id == ndId), nc._nodes.Find(y => y.Id == ndNextId), numlanes, highway, oneway));
            //        }
            //    }
            //}
            #endregion
        }

        RoadSegment generateShiftedSegment(RoadSegment oriSegment, double distance, int numlanes, string highway, Boolean forward)
        {
            double angle = (Math.PI / 2) - Vector2.AngleBetween(oriSegment.startNode.Position, oriSegment.endNode.Position);
            Vector2 shift = new Vector2(distance * Math.Cos(angle), distance * Math.Sin(angle));

            Node newStart = new Node(new Vector2(oriSegment.startNode.Position.X + shift.X, oriSegment.startNode.Position.Y - shift.Y));
            Node newEnd = new Node(new Vector2(oriSegment.endNode.Position.X + shift.X, oriSegment.endNode.Position.Y - shift.Y));

            RoadSegment toReturn;

            if (forward)
            {
                toReturn = new RoadSegment(newStart, newEnd, numlanes, highway, "yes");
            }
            else
            {
                toReturn = new RoadSegment(newEnd, newStart, numlanes, highway, "yes");
            }

            return toReturn;
        }


        private void makeWaySegment(List<XElement> nodeQuery, Boolean forward)
        {
            if (forward)
            {
                for (int i = 0; i < nodeQuery.Count() - 1; i++)
                {
                    long ndCurrId = Convert.ToInt64(nodeQuery[i].Attribute("ref").Value);
                    long ndNextId = Convert.ToInt64(nodeQuery[i + 1].Attribute("ref").Value);

                    if ((nc._nodes.Find(x => x.Id == ndCurrId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        //nc.segments.Add(new RoadSegment(nc._nodes.Find(x => x.Id == ndCurrId), nc._nodes.Find(y => y.Id == ndNextId)));
                    }
                }
            }
            else
            {
                for (int i = nodeQuery.Count - 1; i > 0; i--)
                {
                    long ndCurrId = Convert.ToInt64(nodeQuery[i].Attribute("ref").Value);
                    long ndNextId = Convert.ToInt64(nodeQuery[i - 1].Attribute("ref").Value);

                    if ((nc._nodes.Find(x => x.Id == ndCurrId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
                    {
                        //nc.segments.Add(new RoadSegment(nc._nodes.Find(x => x.Id == ndCurrId), nc._nodes.Find(y => y.Id == ndNextId)));
                    }
                }
            }
        }


        private void tempLoadButton_Click(object sender, EventArgs e)
        {
            #region Load File
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.ExecutablePath;
                ofd.AddExtension = true;
                ofd.DefaultExt = @".xml";
                // Flag -- new load
                //ofd.Filter = @"OpenStreetMap|*.osm";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    GlobalTime.Instance.Reset();
                    nc.Clear();
                    nc.Load();

                    LoadMap(ofd.FileName);
                }
            }
            #endregion

            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }

        private void speedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerSimulation.Interval = (int)(1000 / (int)_temp_stepsPerSeconds);// / speedMultipliers[speedComboBox.SelectedIndex]);
            tickLength = 1.0d / (_temp_stepsPerSeconds / speedMultipliers[speedComboBox.SelectedIndex]);

            Debug.WriteLine("timerSimulation Interval " + timerSimulation.Interval + ", " + speedMultipliers[speedComboBox.SelectedIndex]);
        }


        private void zoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            // neue Autoscrollposition berechnen und setzen
            daGridScrollPosition = new Point(
                (int)Math.Round(daGridViewCenter.X - (DaGrid.Width / 2 * zoomMultipliers[zoomComboBox.SelectedIndex, 1])),
                (int)Math.Round(daGridViewCenter.Y - (DaGrid.Height / 2 * zoomMultipliers[zoomComboBox.SelectedIndex, 1])));

            // Bitmap umrechnen:
            //UpdateBackgroundImage();

            UpdateDaGridClippingRect();
            //thumbGrid.Invalidate();
            DaGrid.Invalidate();
        }

        private void buttonTLightTemp_Click(object sender, EventArgs e)
        {
            //switch (nc.segments.Find(x => x.Id == 2918).endNode.tLight.trafficLightState)
            //{
            //    case TrafficLight.State.GREEN:
            //        nc.segments.Find(x => x.Id == 2918).endNode.tLight.SwitchToRed();
            //        nc.segments.Find(x => x.Id == 25163).endNode.tLight.SwitchToRed();
            //        break;
            //    case TrafficLight.State.RED:
            //        nc.segments.Find(x => x.Id == 2918).endNode.tLight.SwitchToGreen();
            //        nc.segments.Find(x => x.Id == 25163).endNode.tLight.SwitchToGreen();
            //        break;
            //}



            //switch (nc.segments.Find(x => x.Id == 28586).startNode.tLight.trafficLightState)
            //{
            //    case TrafficLight.State.GREEN:
            //        nc.segments.Find(x => x.Id == 2925).endNode.tLight.SwitchToGreen();
            //        nc.segments.Find(x => x.Id == 28583).endNode.tLight.SwitchToGreen();
            //        nc.segments.Find(x => x.Id == 28586).startNode.tLight.SwitchToRed();

            //        nc.segments.Find(x => x.Id == 1).endNode.tLight.SwitchToRed();
            //        //nc.segments.Find(x => x.Id == 26964).endNode.tLight.SwitchToRed();
            //        break;
            //    case TrafficLight.State.RED:
            //        nc.segments.Find(x => x.Id == 2925).endNode.tLight.SwitchToRed();
            //        nc.segments.Find(x => x.Id == 28583).endNode.tLight.SwitchToRed();
            //        nc.segments.Find(x => x.Id == 28586).startNode.tLight.SwitchToGreen();

            //        nc.segments.Find(x => x.Id == 1).endNode.tLight.SwitchToGreen();
            //        //nc.segments.Find(x => x.Id == 26964).endNode.tLight.SwitchToGreen();
            //        break;
            //}


            nc.testTL1.ChangeState();
            nc.testTL2.ChangeState();



            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }


        #region oldRoute
        private void oldRoute()
        {
            _route = new List<RoadSegment>();

            //_route[0].lanes[rnd.Next(0, _route[0].lanes.Count)].vehicles.Add(new IVehicle(
            //            _route[0],
            //            Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
            //            _route));

            //_route.Add(nc.segments[0]);
            //_route.Add(nc.segments[1]);
            //_route.Add(nc.segments[2]);
            //_route.Add(nc.segments[39]);
            //_route.Add(nc.segments[264]);
            //_route.Add(nc.segments[262]);
            //_route.Add(nc.segments[265]);
            //_route.Add(nc.segments[266]);
            //_route.Add(nc.segments[374]);
            //_route.Add(nc.segments[349]);
            //_route.Add(nc.segments[47]);
            //_route.Add(nc.segments[350]);
            //_route.Add(nc.segments[351]);
            //_route.Add(nc.segments[352]);
            //_route.Add(nc.segments[353]);
            //_route.Add(nc.segments[354]);
            //_route.Add(nc.segments[355]);
            //_route.Add(nc.segments[356]);
            //_route.Add(nc.segments[357]);
            //_route.Add(nc.segments[358]);
            //_route.Add(nc.segments[359]);
            //_route.Add(nc.segments[360]);
            //_route.Add(nc.segments[361]);
            //_route.Add(nc.segments[362]);
            //_route.Add(nc.segments[363]);
            //_route.Add(nc.segments[364]);

            //nc.segments[374].endNode.tLight = new TrafficLight();

            //_route2 = new List<RoadSegment>();

            //_route2.Add(nc.segments[0]);
            //_route2.Add(nc.segments[1]);
            //_route2.Add(nc.segments[2]);
            //_route2.Add(nc.segments[39]);
            //_route2.Add(nc.segments[264]);
            //_route2.Add(nc.segments[262]);
            //_route2.Add(nc.segments[265]);
            //_route2.Add(nc.segments[266]);
            //_route2.Add(nc.segments[374]);
            //_route2.Add(nc.segments[280]);
            //_route2.Add(nc.segments[281]);
            //_route2.Add(nc.segments[282]);
            //_route2.Add(nc.segments[283]);
            //_route2.Add(nc.segments[284]);
            //_route2.Add(nc.segments[285]);
            //_route2.Add(nc.segments[286]);
            //_route2.Add(nc.segments[287]);
            //_route2.Add(nc.segments[288]);
            //_route2.Add(nc.segments[289]);
            //_route2.Add(nc.segments[290]);

            //_route3 = new List<RoadSegment>();
            //_route3.Add(nc.segments[267]);
            //_route3.Add(nc.segments[268]);
            //_route3.Add(nc.segments[269]);
            //_route3.Add(nc.segments[270]);
            //_route3.Add(nc.segments[271]);
            //_route3.Add(nc.segments[272]);
            //_route3.Add(nc.segments[273]);
            //_route3.Add(nc.segments[274]);
            //_route3.Add(nc.segments[275]);
            //_route3.Add(nc.segments[276]);
            //_route3.Add(nc.segments[277]);
            //_route3.Add(nc.segments[278]);
            //_route3.Add(nc.segments[369]);
            //_route3.Add(nc.segments[370]);
            //_route3.Add(nc.segments[371]);
            //_route3.Add(nc.segments[372]);
            //_route3.Add(nc.segments[373]);
            //_route3.Add(nc.segments[365]);
            //_route3.Add(nc.segments[366]);
            //_route3.Add(nc.segments[367]);
            //_route3.Add(nc.segments[368]);

            //_route4 = new List<RoadSegment>();
            //_route4.Add(nc.segments[32]);
            //_route4.Add(nc.segments[33]);
            //_route4.Add(nc.segments[34]);
            //_route4.Add(nc.segments[35]);
            //_route4.Add(nc.segments[36]);
            //_route4.Add(nc.segments[131]);
            //_route4.Add(nc.segments[132]);
            //_route4.Add(nc.segments[133]);
            //_route4.Add(nc.segments[212]);
            //_route4.Add(nc.segments[213]);
            //_route4.Add(nc.segments[214]);
            //_route4.Add(nc.segments[215]);
            //_route4.Add(nc.segments[216]);
            //_route4.Add(nc.segments[217]);
            //_route4.Add(nc.segments[218]);
            //_route4.Add(nc.segments[219]);
            //_route4.Add(nc.segments[220]);
            //_route4.Add(nc.segments[221]);
            //_route4.Add(nc.segments[222]);




            //_route[0].vehicles.Add(new IVehicle(
            //    _route[0],
            //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
            //    _route));

            ////_route[0].vehicles[0].dumb = true;
            //_route[0].vehicles[0].distance = 20;

            //_route[0].vehicles.Add(new IVehicle(
            //    _route[0],
            //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
            //    _route));

            //_route[0].vehicles[1].distance = 10;

            //_route[0].vehicles.Add(new IVehicle(
            //    _route[0],
            //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
            //    _route));

            //_route[0].vehicles[2].distance = 0;
        }

        #region AStar Testing Temp
        private void btnAstar_Click(object sender, EventArgs e)
        {
            List<List<RoadSegment>> routes = new List<List<RoadSegment>>();
            RoadSegment startSegment, endSegment;

            startSegment = nc.segments.Find(x => x.Id == Int32.Parse(textBox1.Text));
            endSegment = nc.segments.Find(x => x.Id == Int32.Parse(textBox2.Text));

            if ((startSegment != null) && (endSegment != null))
            {
                AStar newAStar = new AStar(nc.segments, startSegment, endSegment);

                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> newRoute = generatePath(newAStar, startSegment, endSegment);

                    foreach (RoadSegment segment in nc.segments)
                    {
                        for (int j = 0; j < segment.lanes.Count; j++)
                        {
                            segment.lanes[j].debugColor = Color.DarkGray;
                        }
                    }

                    foreach (RoadSegment segment in newRoute)
                    {
                        for (int j = 0; j < segment.lanes.Count; j++)
                        {
                            segment.lanes[j].debugColor = Color.Red;
                        }
                    }

                    Invalidate(InvalidationLevel.ONLY_MAIN_CANVAS);

                    astarStatus.Text = "The path is found";
                }
                else
                {
                    astarStatus.Text = "The path is not found";
                }

            }
            else if (startSegment == null)
            {
                astarStatus.Text = "Start Segment is not found";
            }
            else if (endSegment == null)
            {
                astarStatus.Text = "End Segment is not found";
            }
            else
            {
                astarStatus.Text = "Start & End Segment is not found";
            }



        }

        public List<RoadSegment> generatePath(AStar astar, RoadSegment start, RoadSegment end)
        {
            List<RoadSegment> toReturn = new List<RoadSegment>();
            RoadSegment current = end;
            toReturn.Add(current);

            while (current != start)
            {
                RoadSegment value;
                astar.cameFrom.TryGetValue(current, out value);

                current = value;
                toReturn.Add(current);
            }

            toReturn.Reverse();


            return toReturn;
        }

        private void DaGrid_Load(object sender, EventArgs e)
        {

        }


        #endregion


        #region REST API

        private void btnRESTAPI_Click(object sender, EventArgs e)
        {
            List<WayTraffic> trafficList = null;
            //RunAsync().Wait();
            Console.Write("test");
            RestClient restClient = new RestClient();

            restClient.ParseTraffic(nc.segments);
        }

        
        #endregion

        #endregion

        //    private void generateVehicles()
        //    {
        //        #region tempVehGenerate
        //        if ((timeMod % 36) == 0.0)
        //        {
        //            if ((vehCount % 2) == 0)
        //            {
        //                //int laneidx = rnd.Next(0, _route[0].lanes.Count);

        //                //_route[0].lanes[laneidx].vehicles.Add(new IVehicle(
        //                //    _route[0], laneidx,
        //                //    Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
        //                //    _route));
        //                //activeVehicles++;

        //                //laneidx = rnd.Next(0, _route4[0].lanes.Count);
        //                //_route4[0].lanes[laneidx].vehicles.Add(new IVehicle(
        //                //    _route4[0], laneidx,
        //                //    Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
        //                //    _route4));
        //                //activeVehicles++;

        //                int laneidx = rnd.Next(0, _route[0].lanes.Count);
        //                int vehType = rnd.Next(0, 2);
        //                IVehicle v = null;

        //                if ((_route[0].lanes[laneidx].vehicles.Count == 0) ||
        //                    (_route[0].lanes[laneidx].vehicles[_route[0].lanes[laneidx].vehicles.Count - 1].RearPos <
        //                    _route[0].lanes[laneidx].Length - 20))
        //                {

        //                    if (vehType == 0)
        //                    {
        //                        v = new Car(_route[0], laneidx, _route);
        //                    }
        //                    else if (vehType == 1)
        //                    {
        //                        v = new Bus(_route[0], laneidx, _route);
        //                    }
        //                    else
        //                    {
        //                        v = new Truck(_route[0], laneidx, _route);
        //                    }

        //                    _route[0].lanes[laneidx].vehicles.Add(v);
        //                    nc.ActiveVehicles++;
        //                }


        //                laneidx = rnd.Next(0, _route2[0].lanes.Count);
        //                vehType = rnd.Next(0, 2);
        //                v = null;

        //                if ((_route2[0].lanes[laneidx].vehicles.Count == 0) ||
        //                    (_route2[0].lanes[laneidx].vehicles[_route2[0].lanes[laneidx].vehicles.Count - 1].RearPos <
        //                    _route2[0].lanes[laneidx].Length - 20))
        //                {
        //                    if (vehType == 0)
        //                    {
        //                        v = new Car(_route2[0], laneidx, _route2);
        //                    }
        //                    else if (vehType == 1)
        //                    {
        //                        v = new Bus(_route2[0], laneidx, _route2);
        //                    }
        //                    else
        //                    {
        //                        v = new Truck(_route2[0], laneidx, _route2);
        //                    }

        //                    _route2[0].lanes[laneidx].vehicles.Add(v);
        //                    nc.ActiveVehicles++;
        //                }

        //                //laneidx = rnd.Next(0, _route4[0].lanes.Count);
        //                //vehType = rnd.Next(0, 2);

        //                //if (vehType == 0)
        //                //{
        //                //    v = new Car(_route4[0], laneidx, _route4);
        //                //}
        //                //else if (vehType == 1)
        //                //{
        //                //    v = new Bus(_route4[0], laneidx, _route4);
        //                //}
        //                //else
        //                //{
        //                //    v = new Truck(_route4[0], laneidx, _route4);
        //                //}

        //                //_route4[0].lanes[laneidx].vehicles.Add(v);
        //                //activeVehicles++;

        //                //laneidx = 0;
        //                //_route5[0].lanes[laneidx].vehicles.Add(new IVehicle(
        //                //    _route5[0], laneidx,
        //                //    Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
        //                //    _route5));
        //                //activeVehicles++;
        //            }
        //            else
        //            {
        //                //int laneidx = rnd.Next(0, _route2[0].lanes.Count);
        //                //_route2[0].lanes[laneidx].vehicles.Add(new IVehicle(
        //                //    _route2[0], laneidx,
        //                //    Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
        //                //    _route2));
        //                //activeVehicles++;

        //                //laneidx = rnd.Next(0, _route3[0].lanes.Count);
        //                //_route3[0].lanes[laneidx].vehicles.Add(new IVehicle(
        //                //    _route3[0], laneidx,
        //                //    Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
        //                //    _route3));
        //                //activeVehicles++;

        //                int laneidx = rnd.Next(0, _route3[0].lanes.Count);
        //                int vehType = rnd.Next(0, 2);
        //                IVehicle v = null;

        //                if ((_route3[0].lanes[laneidx].vehicles.Count == 0) ||
        //                    (_route3[0].lanes[laneidx].vehicles[_route3[0].lanes[laneidx].vehicles.Count - 1].RearPos <
        //                    _route3[0].lanes[laneidx].Length - 20))
        //                {

        //                    if (vehType == 0)
        //                    {
        //                        v = new Car(_route3[0], laneidx, _route3);
        //                    }
        //                    else if (vehType == 1)
        //                    {
        //                        v = new Bus(_route3[0], laneidx, _route3);
        //                    }
        //                    else
        //                    {
        //                        v = new Truck(_route3[0], laneidx, _route3);
        //                    }

        //                    _route3[0].lanes[laneidx].vehicles.Add(v);
        //                    nc.ActiveVehicles++;
        //                }


        //                laneidx = rnd.Next(0, _route4[0].lanes.Count);
        //                vehType = rnd.Next(0, 2);
        //                v = null;

        //                if ((_route4[0].lanes[laneidx].vehicles.Count == 0) ||
        //                    (_route4[0].lanes[laneidx].vehicles[_route4[0].lanes[laneidx].vehicles.Count - 1].RearPos <
        //                    _route4[0].lanes[laneidx].Length - 20))
        //                {

        //                    if (vehType == 0)
        //                    {
        //                        v = new Car(_route4[0], laneidx, _route4);
        //                    }
        //                    else if (vehType == 1)
        //                    {
        //                        v = new Bus(_route4[0], laneidx, _route4);
        //                    }
        //                    else
        //                    {
        //                        v = new Truck(_route4[0], laneidx, _route4);
        //                    }

        //                    _route4[0].lanes[laneidx].vehicles.Add(v);
        //                    nc.ActiveVehicles++;
        //                }

        //                //laneidx = rnd.Next(0, _route3[0].lanes.Count);
        //                //vehType = rnd.Next(0, 2);

        //                //if (vehType == 0)
        //                //{
        //                //    v = new Car(_route3[0], laneidx, _route3);
        //                //}
        //                //else if (vehType == 1)
        //                //{
        //                //    v = new Bus(_route3[0], laneidx, _route3);
        //                //}
        //                //else
        //                //{
        //                //    v = new Truck(_route3[0], laneidx, _route3);
        //                //}

        //                //_route3[0].lanes[laneidx].vehicles.Add(v);
        //                //activeVehicles++;

        //                //laneidx = rnd.Next(0, _route6[0].lanes.Count);
        //                //_route6[0].lanes[laneidx].vehicles.Add(new IVehicle(
        //                //    _route6[0], laneidx,
        //                //    Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
        //                //    _route6));
        //                //activeVehicles++;
        //            }
        //            vehCount++;
        //        }
        //        //Debug.WriteLine("VehCount " + vehCount);
        //        timeMod++;
        //        #endregion
        //    }
    }
}
