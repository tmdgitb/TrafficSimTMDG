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
        private List<WaySegment> _route3;
        private List<WaySegment> _route4;
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
            zoomComboBox.SelectedIndex = 7;
            daGridScrollPosition = new Point(0, 0);
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

            nc.Reset();

            #region tempVehGenerate
            if ((timeMod % 72) == 0.0)
            {
                if ((vehCount % 2) == 0)
                {
                    _route[0].vehicles.Add(new IVehicle(
                        _route[0],
                        Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
                        _route));

                    _route4[0].vehicles.Add(new IVehicle(
                        _route4[0],
                        Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
                        _route4));
                }
                else
                {
                    _route2[0].vehicles.Add(new IVehicle(
                        _route2[0],
                        Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
                        _route2));

                    _route3[0].vehicles.Add(new IVehicle(
                        _route3[0],
                        Color.FromArgb(rnd.Next(64, 200), rnd.Next(64, 200), rnd.Next(64, 200)),
                        _route3));
                }
                vehCount++;
            }
            //Debug.WriteLine("VehCount " + vehCount);
            timeMod++;
            #endregion

            ////tickCount++;

            //nodeSteuerung.Tick(tickLength);
            //trafficVolumeSteuerung.Tick(tickLength);

            //nodeSteuerung.Reset();

            thinkStopwatch.Stop();
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
            //Debug.WriteLine("DaGrid Paint");
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            renderStopwatch.Reset();
            renderStopwatch.Start();

            e.Graphics.Transform = new Matrix(
                zoomMultipliers[zoomComboBox.SelectedIndex, 0], 0,
                0, zoomMultipliers[zoomComboBox.SelectedIndex, 0],
                -daGridScrollPosition.X * zoomMultipliers[zoomComboBox.SelectedIndex, 0], -daGridScrollPosition.Y * zoomMultipliers[zoomComboBox.SelectedIndex, 0]);

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

        /// <summary>
		/// aktualisiert das Clipping-Rectangle von DaGrid
		/// </summary>
		private void UpdateDaGridClippingRect()
        {
            if (zoomComboBox.SelectedIndex >= 0)
            {
                //    // daGridClippingRect aktualisieren
                //    renderOptionsDaGrid.clippingRect.X = daGridScrollPosition.X;
                //    renderOptionsDaGrid.clippingRect.Y = daGridScrollPosition.Y;
                //    renderOptionsDaGrid.clippingRect.Width = (int)Math.Ceiling(pnlMainGrid.ClientSize.Width * zoomMultipliers[zoomComboBox.SelectedIndex, 1]);
                //    renderOptionsDaGrid.clippingRect.Height = (int)Math.Ceiling(pnlMainGrid.ClientSize.Height * zoomMultipliers[zoomComboBox.SelectedIndex, 1]);

                daGridViewCenter = new PointF(
                    daGridScrollPosition.X + (DaGrid.Width / 2 * zoomMultipliers[zoomComboBox.SelectedIndex, 1]),
                    daGridScrollPosition.Y + (DaGrid.Height / 2 * zoomMultipliers[zoomComboBox.SelectedIndex, 1]));

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


        #endregion


        private void tempLoadButton_Click(object sender, EventArgs e)
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

                            if ((nc._nodes.Find(x => x.Id == ndId) != null) && (nc._nodes.Find(y => y.Id == ndNextId) != null))
                            {
                                nc.segments.Add(new WaySegment(nc._nodes.Find(x => x.Id == ndId), nc._nodes.Find(y => y.Id == ndNextId)));
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

                    #region manually generate route


                    Debug.WriteLine("Segment Count" + nc.segments.Count);
                    _route = new List<WaySegment>();
                    _route.Add(nc.segments[0]);
                    _route.Add(nc.segments[1]);
                    _route.Add(nc.segments[2]);
                    _route.Add(nc.segments[39]);
                    _route.Add(nc.segments[264]);
                    _route.Add(nc.segments[262]);
                    _route.Add(nc.segments[265]);
                    _route.Add(nc.segments[266]);
                    _route.Add(nc.segments[374]);
                    _route.Add(nc.segments[349]);
                    _route.Add(nc.segments[47]);
                    _route.Add(nc.segments[350]);
                    _route.Add(nc.segments[351]);
                    _route.Add(nc.segments[352]);
                    _route.Add(nc.segments[353]);
                    _route.Add(nc.segments[354]);
                    _route.Add(nc.segments[355]);
                    _route.Add(nc.segments[356]);
                    _route.Add(nc.segments[357]);
                    _route.Add(nc.segments[358]);
                    _route.Add(nc.segments[359]);
                    _route.Add(nc.segments[360]);
                    _route.Add(nc.segments[361]);
                    _route.Add(nc.segments[362]);
                    _route.Add(nc.segments[363]);
                    _route.Add(nc.segments[364]);

                    nc.segments[374].endNode.tLight = new TrafficLight();                    

                    _route2 = new List<WaySegment>();
                    _route2.Add(nc.segments[0]);
                    _route2.Add(nc.segments[1]);
                    _route2.Add(nc.segments[2]);
                    _route2.Add(nc.segments[39]);
                    _route2.Add(nc.segments[264]);
                    _route2.Add(nc.segments[262]);
                    _route2.Add(nc.segments[265]);
                    _route2.Add(nc.segments[266]);
                    _route2.Add(nc.segments[374]);
                    _route2.Add(nc.segments[280]);
                    _route2.Add(nc.segments[281]);
                    _route2.Add(nc.segments[282]);
                    _route2.Add(nc.segments[283]);
                    _route2.Add(nc.segments[284]);
                    _route2.Add(nc.segments[285]);
                    _route2.Add(nc.segments[286]);
                    _route2.Add(nc.segments[287]);
                    _route2.Add(nc.segments[288]);
                    _route2.Add(nc.segments[289]);
                    _route2.Add(nc.segments[290]);

                    _route3 = new List<WaySegment>();
                    _route3.Add(nc.segments[267]);
                    _route3.Add(nc.segments[268]);
                    _route3.Add(nc.segments[269]);
                    _route3.Add(nc.segments[270]);
                    _route3.Add(nc.segments[271]);
                    _route3.Add(nc.segments[272]);
                    _route3.Add(nc.segments[273]);
                    _route3.Add(nc.segments[274]);
                    _route3.Add(nc.segments[275]);
                    _route3.Add(nc.segments[276]);
                    _route3.Add(nc.segments[277]);
                    _route3.Add(nc.segments[278]);
                    _route3.Add(nc.segments[369]);
                    _route3.Add(nc.segments[370]);
                    _route3.Add(nc.segments[371]);
                    _route3.Add(nc.segments[372]);
                    _route3.Add(nc.segments[373]);
                    _route3.Add(nc.segments[365]);
                    _route3.Add(nc.segments[366]);
                    _route3.Add(nc.segments[367]);
                    _route3.Add(nc.segments[368]);

                    _route4 = new List<WaySegment>();
                    _route4.Add(nc.segments[32]);
                    _route4.Add(nc.segments[33]);
                    _route4.Add(nc.segments[34]);
                    _route4.Add(nc.segments[35]);
                    _route4.Add(nc.segments[36]);
                    _route4.Add(nc.segments[131]);
                    _route4.Add(nc.segments[132]);
                    _route4.Add(nc.segments[133]);
                    _route4.Add(nc.segments[212]);
                    _route4.Add(nc.segments[213]);
                    _route4.Add(nc.segments[214]);
                    _route4.Add(nc.segments[215]);
                    _route4.Add(nc.segments[216]);
                    _route4.Add(nc.segments[217]);
                    _route4.Add(nc.segments[218]);
                    _route4.Add(nc.segments[219]);
                    _route4.Add(nc.segments[220]);
                    _route4.Add(nc.segments[221]);
                    _route4.Add(nc.segments[222]);


                    //_route[0].vehicles.Add(new IVehicle(
                    //    _route[0],
                    //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
                    //    _route));

                    ////_route[0].vehicles[0].dumb = true;
                    //_route[0].vehicles[0].distance = 100;

                    //_route[0].vehicles.Add(new IVehicle(
                    //    _route[0],
                    //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
                    //    _route));

                    //_route[0].vehicles[1].distance = 50;

                    //_route[0].vehicles.Add(new IVehicle(
                    //    _route[0],
                    //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
                    //    _route));

                    //_route[0].vehicles[2].distance = 0;



                    #endregion
                }
            }
            #endregion


            //#region Longitudinal Model Test
            //GlobalTime.Instance.Reset();
            //nc.Clear();

            //nc._nodes.Add(new Node(new Vector2(0, 200)));
            //nc._nodes.Add(new Node(new Vector2(2000, 200)));

            //nc.segments.Add(new WaySegment(nc._nodes[0], nc._nodes[1]));
            //_route = new List<WaySegment>();
            //_route.Add(nc.segments[0]);

            //_route[0].vehicles.Add(new IVehicle(
            //    _route[0],
            //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
            //    _route));

            //_route[0].vehicles[0]._physics.targetVelocity = 7;
            //_route[0].vehicles[0].distance = 100;

            //_route[0].vehicles.Add(new IVehicle(
            //    _route[0],
            //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
            //    _route));

            //_route[0].vehicles[1]._physics.targetVelocity = 13;
            //_route[0].vehicles[1].distance = 50;

            //_route[0].vehicles.Add(new IVehicle(
            //    _route[0],
            //    Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
            //    _route));

            //_route[0].vehicles[2]._physics.targetVelocity = 10;
            //_route[0].vehicles[2].distance = 0;

            //#endregion


            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }

        private void speedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerSimulation.Interval =(int) (1000 / (int) _temp_stepsPerSeconds / speedMultipliers[speedComboBox.SelectedIndex]);
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
            switch(nc.segments[374].endNode.tLight.trafficLightState)
            {
                case TrafficLight.State.GREEN:
                    nc.segments[374].endNode.tLight.SwitchToRed();
                    break;
                case TrafficLight.State.RED:
                    nc.segments[374].endNode.tLight.SwitchToGreen();
                    break;
            }

            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }
    }
}
