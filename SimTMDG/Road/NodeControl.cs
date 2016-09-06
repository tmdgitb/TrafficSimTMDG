using System;
using SimTMDG.Tools;
using System.Drawing;
using System.Collections.Generic;
using SimTMDG.Vehicle;
using System.Diagnostics;

namespace SimTMDG.Road
{
    public class NodeControl : ITickable
    {
        #region TEMP
        Rectangle mapBound;
        #endregion

        //private Routing _route;
        public List<Node> _nodes = new List<Node>();
        public List<WaySegment> segments = new List<WaySegment>();

        internal List<WaySegment> Segments
        {
            get
            {
                return segments;
            }

            set
            {
                segments = value;
            }
        }

        public NodeControl()
        {
            
        }


        // TODO temp for testing
        public void Load()
        {

            Clear();


            //_nodes.Add(new Node(new Vector2(100, 150)));
            //_nodes.Add(new Node(new Vector2(250, 150)));
            //_nodes.Add(new Node(new Vector2(300, 175)));
            //_nodes.Add(new Node(new Vector2(350, 225)));
            //_nodes.Add(new Node(new Vector2(400, 300)));
            //_nodes.Add(new Node(new Vector2(400, 350)));
            //_nodes.Add(new Node(new Vector2(400, 225)));
            //_nodes.Add(new Node(new Vector2(425, 200)));

            //_segments.Add(new WaySegment(_nodes[0], _nodes[1]));
            //_segments.Add(new WaySegment(_nodes[1], _nodes[2]));
            //_segments.Add(new WaySegment(_nodes[2], _nodes[3]));
            //_segments.Add(new WaySegment(_nodes[3], _nodes[4]));
            //_segments.Add(new WaySegment(_nodes[4], _nodes[5]));
            //_segments.Add(new WaySegment(_nodes[3], _nodes[6]));
            //_segments.Add(new WaySegment(_nodes[6], _nodes[7]));



            //Routing _route = new Routing();
            //_route.Push(_segments[0]);
            //_route.Push(_segments[1]);
            //_route.Push(_segments[2]);
            //_route.Push(_segments[3]);
            //_route.Push(_segments[4]);

            //Routing _route2 = new Routing();
            //_route2.Push(_segments[0]);
            //_route2.Push(_segments[1]);
            //_route2.Push(_segments[2]);
            //_route2.Push(_segments[5]);
            //_route2.Push(_segments[6]);

            //Random rnd = new Random();

            //_segments[0].vehicles.Add(new IVehicle());
            //_segments[0].vehicles[0].CurrentSegment = _segments[0];
            //_segments[0].vehicles[0].distance = 20;
            //_segments[0].vehicles[0].color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            //_segments[0].vehicles[0]
            //    .newCoord(
            //        _segments[0].startNode.Position,
            //        _segments[0].endNode.Position,
            //        _segments[0].vehicles[0].distance);

            //_segments[0].vehicles[0].Routing = _route;

            //_segments[0].vehicles.Add(new IVehicle());
            //_segments[0].vehicles[1].CurrentSegment = _segments[0];
            //_segments[0].vehicles[1].distance = 0;
            //_segments[0].vehicles[1].color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            //_segments[0].vehicles[1]
            //    .newCoord(
            //        _segments[0].startNode.Position,
            //        _segments[0].endNode.Position,
            //        _segments[0].vehicles[1].distance);

            //_segments[0].vehicles[1].Routing = _route2;

            //Debug.WriteLine("veh[0] distance: " + _segments[0].vehicles[0].distance
            //    + ", absCoord: " + _segments[0].vehicles[0].absCoord);
        }


        public void Clear()
        {
            _nodes.Clear();

            foreach (WaySegment ws in Segments)
            {
                ws.vehicles.Clear();
            }

            Segments.Clear();
        }


        public void Reset()
        {
            //_nodes.Clear();
            
            //foreach(WaySegment ws in Segments)
            //{
            //    ws.vehicles.Clear();
            //}

            //Segments.Clear();

            foreach (WaySegment ws in segments)
            {
                ws.Reset();
            }

        }

        public void Tick(double tickLength)
        {
            foreach(WaySegment ws in Segments)
            {
                ws.Tick(tickLength);              
            }

            //foreach (WaySegment ws in Segments)
            //{
            //    foreach (IVehicle v in ws.vehicles)
            //    {
            //        v.newCoord();
            //    }
            //}
        }

        public void setBounds(Rectangle bounds)
        {
            mapBound = bounds;
        }


        #region draw
        public void Draw(Graphics g)
        {
            foreach (WaySegment ws in Segments)
            {
                if ((IsInBound(ws.startNode.Position))||(IsInBound(ws.endNode.Position)))
                    ws.Draw(g);
            }

            foreach (Node nd in _nodes)
            {
                if (IsInBound(nd.Position))
                    nd.Draw(g);
            }

            foreach (WaySegment ws in Segments)
            {
                foreach (IVehicle v in ws.vehicles)
                {
                    //if (v.distance <= v.CurrentSegment.Length)
                    if (IsInBound(v.absCoord))
                        v.Draw(g);
                }
            }

            //Pen pen = new Pen(Color.OrangeRed, 1);

            //g.DrawRectangle(pen, (float) minBound.X, (float)minBound.Y, (float) maxBound.X, (float) maxBound.Y);
        }
        #endregion

        public bool IsInBound(Vector2 coord)
        {
            if  ((coord.X < mapBound.X)||(coord.Y < mapBound.Y)||
                 (coord.X > mapBound.X + mapBound.Width)||(coord.Y > mapBound.Y + mapBound.Height)){
                return false;
            }else{
                return true;
            }
        }
    }    
}



//using SimTMDG.Time;
//using SimTMDG.Tools;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimTMDG.Road
//{
//    public class NodeControl: ITickable
//    {
//        #region Variablen und Felder

//        /// <summary>
//        /// alle verwendenten LineNodes
//        /// </summary>
//        private List<LineNode> _nodes = new List<LineNode>();
//        /// <summary>
//        /// alle verwendenten LineNodes
//        /// </summary>
//        public List<LineNode> nodes
//        {
//            get { return _nodes; }
//            set { _nodes = value; }
//        }

//        /// <summary>
//        /// alle verwendeten NodeConnections
//        /// </summary>
//        private List<NodeConnection> _connections = new List<NodeConnection>();
//        /// <summary>
//        /// alle verwendeten NodeConnections
//        /// </summary>
//        public List<NodeConnection> connections
//        {
//            get { return _connections; }
//            set { _connections = value; }
//        }

//        ///// <summary>
//        ///// Liste aller bekannten Intersections
//        ///// </summary>
//        //private List<Intersection> _intersections = new List<Intersection>();
//        ///// <summary>
//        ///// Liste aller bekannten Intersections
//        ///// </summary>
//        //public List<Intersection> intersections
//        //{
//        //    get { return _intersections; }
//        //    set { _intersections = value; }
//        //}

//        /// <summary>
//        /// List of all known network layers
//        /// </summary>
//        public List<NetworkLayer> _networkLayers { get; private set; }

//        /// <summary>
//        /// Titel dieses Layouts
//        /// </summary>
//        private string m_title;
//        /// <summary>
//        /// Titel dieses Layouts
//        /// </summary>
//        public string title
//        {
//            get { return m_title; }
//            set { m_title = value; }
//        }


//        /// <summary>
//        /// Informationstext zum Layout
//        /// </summary>
//        private string _infoText;
//        /// <summary>
//        /// Informationstext zum Layout
//        /// </summary>
//        public string infoText
//        {
//            get { return _infoText; }
//            set { _infoText = value; }
//        }


//        #endregion

//        #region Konstruktoren

//        /// <summary>
//        /// leerer Standardkonstruktor
//        /// </summary>
//        public NodeControl()
//        {
//            _networkLayers = new List<NetworkLayer>();
//        }

//        #endregion


//        #region ITickable Member

//        /// <summary>
//        /// sagt allen verwalteten Objekten Bescheid, dass sie ticken dürfen *g*
//        /// </summary>
//        public void Tick(double tickLength)
//        {
//            foreach (LineNode ln in nodes)
//            {
//                ln.Tick(tickLength);
//            }

//            int bucketNumber = GlobalTime.Instance.currentCycleTick;
//            foreach (NodeConnection nc in _connections)
//            {
//                nc.GatherStatistics(bucketNumber);
//            }
//        }

//        /// <summary>
//        /// setzt den Tick-Zustand aller LineNodes zurück
//        /// </summary>
//        public void Reset()
//        {
//            foreach (LineNode ln in nodes)
//            {
//                ln.Reset();
//            }
//        }

//        #endregion


//        public void tempLoad()
//        {
//            Vector2 position = new Vector2();
//            Vector2 inSlope = new Vector2();
//            Vector2 outSlope = new Vector2();
//            NetworkLayer networkLayer = new NetworkLayer("Layer 1", true);
//            bool stopSign = false;


//            position.X = 268;
//            position.Y = 148;
//            inSlope.X = 0;
//            inSlope.Y = 0;
//            outSlope.X = 0;
//            outSlope.Y = 0;

//            nodes.Add(new LineNode(position, inSlope, outSlope, networkLayer, stopSign));


//            position.X = 464;
//            position.Y = 180;
//            inSlope.X = -56;
//            inSlope.Y = -96;
//            outSlope.X = 56;
//            outSlope.Y = 96;

//            nodes.Add(new LineNode(position, inSlope, outSlope, networkLayer, stopSign));


//            position.X = 634;
//            position.Y = 490;
//            inSlope.X = -156;
//            inSlope.Y = -90;
//            outSlope.X = 156;
//            outSlope.Y = 90;

//            nodes.Add(new LineNode(position, inSlope, outSlope, networkLayer, stopSign));


//            position.X = 942;
//            position.Y = 282;
//            inSlope.X = -29;
//            inSlope.Y = 4;
//            outSlope.X = 29;
//            outSlope.Y = -4;

//            nodes.Add(new LineNode(position, inSlope, outSlope, networkLayer, stopSign));


//            position.X = 1044;
//            position.Y = 454;
//            inSlope.X = -128;
//            inSlope.Y = 70;
//            outSlope.X = 128;
//            outSlope.Y = -70;

//            nodes.Add(new LineNode(position, inSlope, outSlope, networkLayer, stopSign));



//            connections.Add(nodes[0], );


//        }
//    }
//}
