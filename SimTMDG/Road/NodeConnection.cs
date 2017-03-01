﻿//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using System.Xml.Serialization;
//using System.Drawing;
//using System.Drawing.Drawing2D;

//using SimTMDG.Tools;

//namespace SimTMDG.Road
//{
//	/// <summary>
//	/// Verbindung zwischen zwei LineNodes
//	/// </summary>
//    [Serializable]
//    public class NodeConnection : ISavable
//    {

//        #region Variablen

//        /// <summary>
//        /// Startknoten
//        /// </summary>
//        [XmlIgnore]
//        public LineNode startNode;

//        /// <summary>
//        /// Endknoten
//        /// </summary>
//        [XmlIgnore]
//        public LineNode endNode;

//        /// <summary>
//        /// zugehöriges LineSegment (Bézierkurve)
//        /// </summary>
//        [XmlIgnore]
//        public LineSegment lineSegment;

//        /// <summary>
//        /// Priorität der NodeConnection
//        /// </summary>
//        private int _priority = 1;
//        /// <summary>
//        /// Priorität der NodeConnection
//        /// </summary>
//        public int priority
//        {
//            get { return _priority; }
//            set { _priority = value; UpdatePen(); }
//        }

//        /// <summary>
//        /// Flag, ob Autos auf dieser NodeConnection erlaubt sind
//        /// </summary>
//        private bool _carsAllowed = true;
//        /// <summary>
//        /// Flag, ob Autos auf dieser NodeConnection erlaubt sind
//        /// </summary>
//        public bool carsAllowed
//        {
//            get { return _carsAllowed; }
//            set { _carsAllowed = value; UpdatePen(); }
//        }

//        /// <summary>
//        /// Flag, ob Busse auf dieser NodeConnection erlaubt sind
//        /// </summary>
//        private bool _busAllowed;
//        /// <summary>
//        /// Flag, ob Busse auf dieser NodeConnection erlaubt sind
//        /// </summary>
//        public bool busAllowed
//        {
//            get { return _busAllowed; }
//            set { _busAllowed = value; UpdatePen(); }
//        }

//        /// <summary>
//        /// Flag, ob Straßenbahnen auf dieser NodeConnection erlaubt sind
//        /// </summary>
//        private bool _tramAllowed;
//        /// <summary>
//        /// Flag, ob Straßenbahnen auf dieser NodeConnection erlaubt sind
//        /// </summary>
//        public bool tramAllowed
//        {
//            get { return _tramAllowed; }
//            set { _tramAllowed = value; UpdatePen(); }
//        }


//        /// <summary>
//        /// Flag, ob ausgehende Spurwechsel von dieser NodeConnection erlaubt sind
//        /// </summary>
//        private bool _enableOutgoingLineChange;
//        /// <summary>
//        /// Flag, ob ausgehende Spurwechsel von dieser NodeConnection erlaubt sind
//        /// </summary>
//        public bool enableOutgoingLineChange
//        {
//            get { return _enableOutgoingLineChange; }
//            set { _enableOutgoingLineChange = value; }
//        }

//        /// <summary>
//        /// Flag, ob eingehende Spurwechsel auf diese NodeConnection erlaubt ist
//        /// </summary>
//        private bool _enableIncomingLineChange;
//        /// <summary>
//        /// Flag, ob eingehende Spurwechsel auf diese NodeConnection erlaubt ist
//        /// </summary>
//        public bool enableIncomingLineChange
//        {
//            get { return _enableIncomingLineChange; }
//            set { _enableIncomingLineChange = value; }
//        }

//        /// <summary>
//        /// Target velocity on this NodeConnection
//        /// </summary>
//        private double _targetVelocity = 14;
//        /// <summary>
//        /// Target velocity on this NodeConnection
//        /// </summary>
//        public double targetVelocity
//        {
//            get { return _targetVelocity; }
//            set { _targetVelocity = value; }
//        }



//        /// <summary>
//        /// Comparer, der zwei LineChangePoints miteinander vergleicht.
//        /// </summary>
//        static SortedLinkedList<LineChangePoint>.CompareDelegate lineChangePointComparer = delegate (LineChangePoint a, LineChangePoint b)
//        {
//            return a.start.arcPosition.CompareTo(b.start.arcPosition);
//        };

//        /// <summary>
//        /// Liste von LineChangePoints, nach Bogenlängenposition sortiert
//        /// </summary>
//        [XmlIgnore]
//        private List<LineChangePoint> _lineChangePoints = new List<LineChangePoint>();//new SortedLinkedList<LineChangePoint>(lineChangePointComparer);
//                                                                                      /// <summary>
//                                                                                      /// Liste von LineChangePoints, nach Bogenlängenposition sortiert
//                                                                                      /// </summary>
//        [XmlIgnore]
//        public List<LineChangePoint> lineChangePoints
//        {
//            get { return _lineChangePoints; }
//            set { _lineChangePoints = value; }
//        }


//        /// <summary>
//        /// Liste von LineChangeAreas, aller von dieser NodeConnection per Spurwechsel zu erreichenden LineNodes
//        /// </summary>
//        [XmlIgnore]
//        private Dictionary<int, LineChangeInterval> _lineChangeIntervals = new Dictionary<int, LineChangeInterval>(Constants.defaultLineChangeIntervalDictionaryCapacity);
//        /// <summary>
//        /// Liste von LineChangeAreas, aller von dieser NodeConnection per Spurwechsel zu erreichenden LineNodes
//        /// </summary>
//        [XmlIgnore]
//        public Dictionary<int, LineChangeInterval> lineChangeIntervals
//        {
//            get { return _lineChangeIntervals; }
//        }


//        /// <summary>
//        /// Liste aller LineNodes, die über Spurwechsel erreicht werden können
//        /// </summary>
//        [XmlIgnore]
//        private List<LineNode> _viaLineChangeReachableNodes = new List<LineNode>();
//        /// <summary>
//        /// Liste aller LineNodes, die über Spurwechsel erreicht werden können
//        /// </summary>
//        [XmlIgnore]
//        public List<LineNode> viaLineChangeReachableNodes
//        {
//            get { return _viaLineChangeReachableNodes; }
//        }




//        #region Statistiken

//        // TODO:	eigentlich sollten die Statisktiken extern verwaltet werden
//        //			dies hier sollte erstmal nur zum testen quick and dirty sein

//        /// <summary>
//        /// Summe der Durchschnittsgeschwindigkeiten in m/s der Autos auf dieser NodeConnection
//        /// </summary>
//        private float _sumOfAverageSpeeds;
//        /// <summary>
//        /// Summe der Durchschnittsgeschwindigkeiten in m/s der Autos auf dieser NodeConnection
//        /// </summary>
//        [XmlIgnore]
//        public float sumOfAverageSpeeds
//        {
//            get { return _sumOfAverageSpeeds; }
//        }

//        /// <summary>
//        /// Anzahl der Summanden der Durchschnittsgeschwindigkeiten
//        /// </summary>
//        private int _countOfVehicles;
//        /// <summary>
//        /// Anzahl der Summanden der Durchschnittsgeschwindigkeiten
//        /// </summary>
//        [XmlIgnore]
//        public int countOfVehicles
//        {
//            get { return _countOfVehicles; }
//        }


//        /// <summary>
//        /// Average speed distribution
//        /// </summary>
//        [XmlIgnore]
//        public NodeConnection.Statistics[] statistics { get; private set; }


//        #endregion

//        #region Zeichnen
//        /// <summary>
//        /// DEPRECATED: Farbe der Linie
//        /// </summary>
//        private Color _color = Color.Black;
//        /// <summary>
//        /// DEPRECATED: Farbe der Linie
//        /// </summary>
//        [XmlIgnore]
//        public Color color
//        {
//            get { return _color; }
//            set { _color = value; UpdatePen(); }
//        }
//        /// <summary>
//        /// DEPRECATED: Farbe der Linie im ARGB-Format (Für Serialisierung benötigt)
//        /// </summary>
//        [XmlIgnore]
//        public int argbColor
//        {
//            get { return _color.ToArgb(); }
//            set { _color = Color.FromArgb(value); }
//        }


//        /// <summary>
//        /// Pen mit dem die Linie gezeichnet werden soll
//        /// </summary>
//        [XmlIgnore]
//        private Pen drawingPen;

//        /// <summary>
//        /// Color map used for velocity color mapping
//        /// </summary>
//        [XmlIgnore]
//        public static Tools.ColorMap _colormap;


//        /// <summary>
//        /// soll die Durchschnittsgeschwindigkeit durch den Zeichenstil visualisiert werden?
//        /// </summary>
//        private bool _visualizeAverageSpeed = false;
//        /// <summary>
//        /// soll die Durchschnittsgeschwindigkeit durch den Zeichenstil visualisiert werden?
//        /// </summary>
//        [XmlIgnore]
//        public bool visualizeAverageSpeed
//        {
//            get { return _visualizeAverageSpeed; }
//            set { _visualizeAverageSpeed = value; UpdatePen(); }
//        }


//        /// <summary>
//        /// sorgt dafür, dass der Pen neu gesetzt wird
//        /// </summary>
//        private void UpdatePen()
//        {
//            if (_visualizeAverageSpeed)
//            {
//                float averageSpeed = getAverageSpeedOfVehicles();
//                drawingPen = new Pen(_colormap.GetInterpolatedColor(averageSpeed / _targetVelocity), 12);
//            }
//            else
//            {
//                if (_carsAllowed && !_busAllowed && !_tramAllowed)
//                {
//                    drawingPen = new Pen(Color.LightGray, priority);
//                }
//                else if (!_carsAllowed && _busAllowed && !_tramAllowed)
//                {
//                    drawingPen = new Pen(Color.LightBlue, priority);
//                }
//                else if (!_carsAllowed && !_busAllowed && _tramAllowed)
//                {
//                    drawingPen = new Pen(Color.Black, priority);
//                }

//                else if (_carsAllowed && !_busAllowed && _tramAllowed)
//                {
//                    drawingPen = new Pen(new HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.LightGray), _priority);
//                }
//                else if (_carsAllowed && _busAllowed && !_tramAllowed)
//                {
//                    drawingPen = new Pen(new HatchBrush(HatchStyle.LargeConfetti, Color.LightBlue, Color.LightGray), _priority);
//                }
//                else if (!_carsAllowed && _busAllowed && _tramAllowed)
//                {
//                    drawingPen = new Pen(new HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.LightBlue), _priority);
//                }

//                else
//                {
//                    drawingPen = new Pen(Color.DarkBlue, priority);
//                }
//            }
//        }
//        #endregion

//        #endregion

//        #region Konstruktoren

//        /// <summary>
//        /// Standardconstruktor (!!! NICHT VERVENDEN !!!) [wird nur für XML Serialisierung gebraucht]
//        /// </summary>
//        public NodeConnection()
//        {
//            //** Hier passiert gar nix - die NodeConnection ist nicht funktionsfähig ***\\

//            // den intersectionComparer müssen wir trotzdem erstellen...
//            intersectionComparer = delegate (Intersection a, Intersection b)
//            {
//                bool aA = (this == a._aConnection);
//                bool bA = (this == b._aConnection);

//                if (aA && bA)
//                    return a._aTime.CompareTo(b._aTime);
//                else if (!aA && bA)
//                    return a._bTime.CompareTo(b._aTime);
//                else if (aA && !bA)
//                    return a._aTime.CompareTo(b._bTime);
//                else
//                    return a._bTime.CompareTo(b._bTime);
//            };

//            _intersections = new SortedLinkedList<Intersection>(intersectionComparer);
//            statistics = new Statistics[1];
//        }

//        /// <summary>
//        /// erstellt eine neue NodeConnection
//        /// </summary>
//        /// <param name="startNode">Anfangsknoten</param>
//        /// <param name="endNode">Endknoten</param>
//        /// <param name="ls">LineSegment</param>
//        /// <param name="priority">Priorität</param>
//        /// <param name="targetVelocity">target velocity</param>
//        /// <param name="carsAllowed">Flag, ob Autos auf dieser NodeConnection erlaubt sind</param>
//        /// <param name="busAllowed">Flag, ob Busse auf dieser NodeConnection erlaubt sind</param>
//        /// <param name="tramAllowed">Flag, ob Straßenbahnen auf dieser NodeConnection erlaubt sind</param>
//        /// <param name="enableIncomingLineChange">Flag, ob Spurwechsel auf diese NodeConnection erlaubt sind</param>
//        /// <param name="enableOutgoingLineChange">Flag, ob Spurwechsel von dieser NodeConnection erlaubt sind</param>
//        public NodeConnection(
//            LineNode startNode,
//            LineNode endNode,
//            LineSegment ls,
//            int priority,
//            double targetVelocity,
//            bool carsAllowed,
//            bool busAllowed,
//            bool tramAllowed,
//            bool enableIncomingLineChange,
//            bool enableOutgoingLineChange)
//        {
//            // TODO: NodeConnections werden stets mit lineSegment = null initialisiert? Warum?
//            this.startNode = startNode;
//            this.endNode = endNode;
//            lineSegment = ls;

//            this._priority = priority;
//            this._targetVelocity = targetVelocity;
//            this._carsAllowed = carsAllowed;
//            this._busAllowed = busAllowed;
//            this._tramAllowed = tramAllowed;
//            this._enableIncomingLineChange = enableIncomingLineChange;
//            this._enableOutgoingLineChange = enableOutgoingLineChange;

//            UpdatePen();

//            intersectionComparer = delegate (Intersection a, Intersection b)
//            {
//                bool aA = (this == a._aConnection);
//                bool bA = (this == b._aConnection);

//                if (aA && bA)
//                    return a._aTime.CompareTo(b._aTime);
//                else if (!aA && bA)
//                    return a._bTime.CompareTo(b._aTime);
//                else if (aA && !bA)
//                    return a._aTime.CompareTo(b._bTime);
//                else
//                    return a._bTime.CompareTo(b._bTime);
//            };

//            _intersections = new SortedLinkedList<Intersection>(intersectionComparer);
//            statistics = new Statistics[1];
//        }

//        #endregion

//        #region Methoden


//        /// <summary>
//        /// Fügt den LineChangePoint lcp dieser NodeConnection hinzu und aktualisiert die entsprechenden LineChangeIntervals
//        /// </summary>
//        /// <param name="lcp">hinzuzufügender LineChangePoint</param>
//        public void AddLineChangePoint(LineChangePoint lcp)
//        {
//            LineChangeInterval lci;
//            // prüfen, ob der Zielknoten schon im LineChangeInterval-Dictionary enthalten ist
//            if (_lineChangeIntervals.TryGetValue(lcp.target.nc.endNode.hashcode, out lci))
//            {
//                // er ist schon vorhanden, also passen wir ihn evtl. an
//                if (lci.startArcPos > lcp.start.arcPosition)
//                {
//                    lci.startArcPos = lcp.start.arcPosition;
//                }
//                else if (lci.endArcPos < lcp.start.arcPosition)
//                {
//                    lci.endArcPos = lcp.start.arcPosition;
//                }
//            }
//            else
//            {
//                // er ist noch nicht vorhanden, also legen wir einen neuen an
//                lci = new LineChangeInterval(lcp.target.nc.endNode, lcp.start.arcPosition, lcp.start.arcPosition);
//                _lineChangeIntervals.Add(lcp.target.nc.endNode.hashcode, lci);
//                _viaLineChangeReachableNodes.Add(lcp.target.nc.endNode);
//            }

//            _lineChangePoints.Add(lcp);
//        }

//        /// <summary>
//        /// entfernt alle LineChangePoints und LineChangeIntervals dieser NodeConnection
//        /// </summary>
//        public void ClearLineChangePoints()
//        {
//            _lineChangePoints.Clear();
//            _lineChangeIntervals.Clear();
//            _viaLineChangeReachableNodes.Clear();
//        }


//        /// <summary>
//        /// berechnet die Bogenlängenentfernung vom Beginn dieser NodeConnection zum LineNode ln, wobei der erstbeste LineChangePoint zum wechseln benutzt wird.
//        /// </summary>
//        /// <param name="ln">Zielknoten</param>
//        /// <returns></returns>
//        public double GetLengthToLineNodeViaLineChange(LineNode ln)
//        {
//            foreach (LineChangePoint lcp in _lineChangePoints)
//            {
//                if (lcp.target.nc.endNode == ln)
//                {
//                    return lcp.start.arcPosition + lcp.length + lcp.target.nc.lineSegment.length - lcp.target.arcPosition;
//                }
//            }
//            return Double.PositiveInfinity;
//        }

//        /// <summary>
//        /// entfernt alle LineChangePoints und LineChangeIntervals zur NodeConnection nc
//        /// </summary>
//        /// <param name="nc">Ziel-NodeConnection der zu löschenden LineChangePoints</param>
//        public void RemoveAllLineChangePointsTo(NodeConnection nc)
//        {
//            for (int i = 0; i < _lineChangePoints.Count; i++)
//            {
//                if ((_lineChangePoints[i].otherStart.nc == nc) || (_lineChangePoints[i].target.nc == nc))
//                {
//                    _lineChangePoints.RemoveAt(i);
//                    i--;
//                }
//            }

//            _lineChangeIntervals.Remove(nc.endNode.hashcode);
//            _viaLineChangeReachableNodes.Remove(nc.endNode);
//        }



//        /// <summary>
//        /// setzt das LineSegment auf lc
//        /// </summary>
//        /// <param name="lc">LineSegment, welches gesetzt werden soll</param>
//        public void SetLineSegment(LineSegment lc)
//        {
//            lineSegment = lc;
//        }

//        /// <summary>
//        /// Gibt rekursiv das nächste Halte zurück
//        /// </summary>
//        /// <param name="distance">Maximale Suchreichweite</param>
//        public Halte GetNextTrafficLightWithin(double distance)
//        {
//            int currentDistance = 0;
//            LineNode currentNode = this.endNode;
//            while (currentDistance <= distance)
//            {
//                if (currentNode.tLight != null)
//                {
//                    return currentNode.tLight;
//                }
//                else
//                {
//                    currentDistance += 0;
//                }
//            }
//            return null;
//        }

//        /// <summary>
//        /// gibt den letzten LineChangePoint des NodeConnection zurück, der sich vor arcPosition befindet.
//        /// (binäre Suche -> logarithmische Laufzeit)
//        /// </summary>
//        /// <param name="arcPosition">Bogenlängenposition, vor der gesucht werden soll</param>
//        /// <returns>den letzte LineChangePoint mit lcp.myArcPosition \leq arcPosition oder null</returns>
//        public LineChangePoint GetPrevLineChangePoint(double arcPosition)
//        {
//            if (_lineChangePoints.Count > 0 && _lineChangePoints[0].start.arcPosition < arcPosition)
//            {
//                int lBorder = 0;
//                int rBorder = _lineChangePoints.Count - 1;

//                while (rBorder - lBorder > 1)
//                {
//                    int i = (rBorder + lBorder) / 2;

//                    if (arcPosition == _lineChangePoints[i].start.arcPosition)
//                    {
//                        return _lineChangePoints[i];
//                    }
//                    else if (_lineChangePoints[i].start.arcPosition < arcPosition)
//                    {
//                        lBorder = i;
//                    }
//                    else
//                    {
//                        rBorder = i - 1;
//                    }
//                }

//                return _lineChangePoints[lBorder];
//            }
//            else
//            {
//                return new LineChangePoint();
//            }
//        }

//        /// <summary>
//        /// gibt den nächsten LineChangePoint des NodeConnection zurück, der sich hinter arcPosition befindet.
//        /// (binäre Suche -> logarithmische Laufzeit)
//        /// </summary>
//        /// <param name="arcPosition">Bogenlängenposition, ab der gesucht werden soll</param>
//        /// <returns>den ersten LineChangePoint mit lcp.myArcPosition >= arcPosition oder null</returns>
//        public LineChangePoint GetNextLineChangePoint(double arcPosition)
//        {
//            if (_lineChangePoints.Count > 0 && _lineChangePoints[0].start.arcPosition < arcPosition)
//            {
//                int lBorder = 0;
//                int rBorder = _lineChangePoints.Count - 1;

//                while (rBorder - lBorder > 1)
//                {
//                    int i = (rBorder + lBorder) / 2;

//                    if (arcPosition == _lineChangePoints[i].start.arcPosition)
//                    {
//                        return _lineChangePoints[i];
//                    }
//                    else if (_lineChangePoints[i].start.arcPosition < arcPosition)
//                    {
//                        lBorder = i + 1;
//                    }
//                    else
//                    {
//                        rBorder = i;
//                    }
//                }

//                return _lineChangePoints[rBorder];
//            }
//            else
//            {
//                return new LineChangePoint();
//            }
//        }

//        /// <summary>
//        /// Returns whether the given vehicle type is allowed on this NodeConnection.
//        /// </summary>
//        /// <param name="type">Vehicle type to check</param>
//        /// <returns>true if the given vehicle type is allowed on this NodeConnection</returns>
//        public bool CheckForSuitability(Vehicle.IVehicle.VehicleTypes type)
//        {
//            switch (type)
//            {
//                case IVehicle.VehicleTypes.CAR:
//                    return carsAllowed;
//                case IVehicle.VehicleTypes.BUS:
//                    return busAllowed;
//                case IVehicle.VehicleTypes.TRAM:
//                    return tramAllowed;
//                default:
//                    return false;
//            }
//        }

//        #endregion

//        #region Intersections

//        private SortedLinkedList<Intersection>.CompareDelegate intersectionComparer;

//        /// <summary>
//        /// Liste von Intersections
//        /// </summary>
//        private SortedLinkedList<Intersection> _intersections;// = new SortedLinkedList<Intersection>(intersectionComparer);
//                                                              /// <summary>
//                                                              /// Liste von Intersections
//                                                              /// </summary>
//        [XmlIgnore]
//        public SortedLinkedList<Intersection> intersections
//        {
//            get { return _intersections; }
//        }


//        /// <summary>
//        /// macht der NodeConnection eine Intersection bekannt
//        /// </summary>
//        /// <param name="i">Intersection-Objekt mit allen nötigen Informationen</param>
//        public void AddIntersection(Intersection i)
//        {
//            if (this == i._aConnection)
//                i._aListNode = intersections.Add(i);
//            else
//                i._bListNode = intersections.Add(i);
//        }

//        /// <summary>
//        /// Meldet die Intersection i bei der NodeConnection ab
//        /// </summary>
//        /// <param name="i">abzumeldende Intersection</param>
//        public void RemoveIntersection(Intersection i)
//        {
//            intersections.Remove(i);
//        }


//        /// <summary>
//        /// Liefert alle Intersections im Zeitintervall interval
//        /// </summary>
//        /// <param name="interval">Intervall in dem nach Intersections gesucht werden soll</param>
//        /// <returns>Eine Liste von Intersections die im Zeitintervall interval auf dieser Linie vorkommen</returns>
//        public List<Intersection> GetIntersectionsWithinTime(Interval<double> interval)
//        {
//            List<Intersection> toReturn = new List<Intersection>();

//            foreach (Intersection i in this.intersections)
//            {
//                if (this == i._aConnection)
//                {
//                    if (interval.Contains(i._aTime))
//                    {
//                        toReturn.Add(i);
//                    }
//                }
//                else
//                {
//                    if (interval.Contains(i._bTime))
//                    {
//                        toReturn.Add(i);
//                    }
//                }
//            }

//            return toReturn;
//        }

//        /// <summary>
//        /// Liefert alle Intersections im Bogenlängenintervall intervall
//        /// </summary>
//        /// <param name="interval">Bogenlängeintervall in dem die Intersections liegen sollen</param>
//        /// <returns>Eine Liste von Intersections, die im Bogenlängenintervall intervall liegen</returns>
//        public List<SpecificIntersection> GetIntersectionsWithinArcLength(Interval<double> interval)
//        {
//            List<SpecificIntersection> toReturn = new List<SpecificIntersection>();

//            foreach (Intersection i in this.intersections)
//            {
//                if (this == i._aConnection)
//                {
//                    if (interval.Contains(i.aArcPosition))
//                    {
//                        toReturn.Add(new SpecificIntersection(this, i));
//                    }
//                }
//                else
//                {
//                    if (interval.Contains(i.bArcPosition))
//                    {
//                        toReturn.Add(new SpecificIntersection(this, i));
//                    }
//                }
//            }

//            return toReturn;
//        }

//        /// <summary>
//        /// Returns all intersections within the given interval. Returned List is guaranteed to be sorted in ascending order.
//        /// </summary>
//        /// <param name="interval">Arc length interval to search for.</param>
//        /// <returns>A list of all intersections within the given interval, guaranteed to be sorted in ascending order.</returns>
//        public List<SpecificIntersection> GetSortedIntersectionsWithinArcLength(Interval<double> interval)
//        {
//            // possibly redundant to the version above, but I don't want to trust the iterator.
//            List<SpecificIntersection> toReturn = new List<SpecificIntersection>();

//            LinkedListNode<Intersection> lln = intersections.First;
//            while (lln != null)
//            {
//                if (interval.Contains(lln.Value.GetMyArcPosition(this)))
//                {
//                    toReturn.Add(new SpecificIntersection(this, lln.Value));
//                }
//                lln = lln.Next;
//            }

//            return toReturn;
//        }


//        #endregion

//        #region Autos auf der Linie
//        /// <summary>
//        /// Auf der NodeConnection fahrende IVehicles
//        /// </summary>
//        [XmlIgnore]
//        public MyLinkedList<IVehicle> vehicles = new MyLinkedList<IVehicle>();

//        /// <summary>
//        /// von dieser NodeConnection zu entfernende IVehicles. (wird benötigt, da sonst in Iteratoren die Auflistung verändert würde)
//        /// </summary>
//        [XmlIgnore]
//        private List<IVehicle> vehiclesToRemove = new List<IVehicle>();

//        /// <summary>
//        /// Returns whether a vehicle can be spawned at the given arc position.
//        /// Attention: As this method does not know anything about the vehicle to spawn, it does not check for vehicles prior to arcPosition!
//        /// </summary>
//        /// <param name="arcPosition">Arc position on this NodeConnection</param>
//        /// <returns>true if there is enough space to spawn this vehicle</returns>
//        public bool CanSpawnVehicleAt(double arcPosition)
//        {
//            LinkedListNode<IVehicle> lln = GetVehicleListNodeBehindArcPosition(arcPosition - 0.1); // TODO: this is an ugly hack 
//            return (lln == null || (lln.Value.currentPosition - lln.Value.length - lln.Value.s0 > arcPosition));
//        }

//        /// <summary>
//        /// Fügt der Linie ein Auto hinzu und sorgt dafür
//        /// </summary>
//		/// <param name="veh">hinzuzufügendes Vehicle</param>
//		/// <returns>true, if Vehicle was successfully created - otherwise false</returns>
//        public bool AddVehicle(IVehicle veh)
//        {
//            if (CanSpawnVehicleAt(0))
//            {
//                AddVehicleAt(veh, 0);
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// fügt das IVehicle v an der Position arcPosition ein und sorgt dabei für eine weiterhin korrekte Verkettung von m_vehicles
//        /// </summary>
//        /// <param name="v">einzufügendes Auto</param>
//        /// <param name="arcPosition">Bogenlängenposition, wo das Auto eingefügt werden soll</param>
//        public void AddVehicleAt(IVehicle v, double arcPosition)
//        {
//            LinkedListNode<IVehicle> lln = GetVehicleListNodeBehindArcPosition(arcPosition);
//            if (lln != null)
//            {
//                if (lln.Value.currentPosition - lln.Value.length < arcPosition)
//                {
//                    throw new Exception("Das neue Fahrzeug überlappt sich mit einem anderen Fahrzeug!");
//                }
//                v.listNode = vehicles.AddBefore(lln, v);
//            }
//            else
//            {
//                v.listNode = vehicles.AddLast(v);
//            }

//            v.VehicleLeftNodeConnection += new IVehicle.VehicleLeftNodeConnectionEventHandler(Handler_VehicleLeftNodeConnection);
//        }


//        /// <summary>
//        /// Meldet das IVehicle v von dieser NodeConnection ab. (Effektiv wird dieses Auto erst am Ende des Ticks entfernt)
//        /// </summary>
//        /// <param name="v">zu entfernendes Auto</param>
//        public void RemoveVehicle(IVehicle v)
//        {
//            if (!vehicles.Contains(v))
//            {
//                throw new Exception("Vehicle " + v + " nicht auf dieser NodeConnection!");
//            }

//            vehiclesToRemove.Add(v);
//        }

//        /// <summary>
//        /// entfernt alle IVehicles in vehiclesToRemove aus vehicles
//        /// </summary>
//        public void RemoveAllVehiclesInRemoveList()
//        {
//            foreach (IVehicle v in vehiclesToRemove)
//            {
//                v.VehicleLeftNodeConnection -= Handler_VehicleLeftNodeConnection;
//                vehicles.Remove(v);
//            }
//            vehiclesToRemove.Clear();
//        }

//        /// <summary>
//        /// bestimmt des LinkedListNode des ersten IVehicles hinter der Bogenlängenposition arcPosition
//        /// </summary>
//        /// <param name="arcPosition">Bogenlängenposition, ab dem das erste IVehicle zurückgegeben werden soll</param>
//        /// <returns>Der erste LinkedListNode mit Value.currentPosition >= arcPosition oder null, falls kein solches existiert</returns>
//        public LinkedListNode<IVehicle> GetVehicleListNodeBehindArcPosition(double arcPosition)
//        {
//            LinkedListNode<IVehicle> lln = vehicles.First;

//            while (lln != null)
//            {
//                if (lln.Value.currentPosition > arcPosition)
//                {
//                    return lln;
//                }

//                lln = lln.Next;
//            }

//            return lln;
//        }

//        /// <summary>
//        /// bestimmt des LinkedListNode des ersten IVehicles vor der Bogenlängenposition arcPosition
//        /// </summary>
//        /// <param name="arcPosition">Bogenlängenposition, ab dem das erste IVehicle zurückgegeben werden soll</param>
//        /// <returns>Der erste LinkedListNode mit Value.currentPosition kleinergleich arcPosition oder null, falls kein solches existiert</returns>
//        public LinkedListNode<IVehicle> GetVehicleListNodeBeforeArcPosition(double arcPosition)
//        {
//            LinkedListNode<IVehicle> lln = vehicles.Last;

//            while (lln != null)
//            {
//                if (lln.Value.currentPosition < arcPosition)
//                {
//                    return lln;
//                }

//                lln = lln.Previous;
//            }

//            return lln;
//        }


//        /// <summary>
//        /// bestimmt das IVehicle hinter der Bogenlängenposition arcPosition und die Entfernung dorthin (Vorderkante).
//        /// Ist distanceWithin größer als die Entfernung zum endNode, so werden alle weiteren ausgehenden NodeConnections auch untersucht
//        /// </summary>
//        /// <param name="arcPosition">Bogenlängenposition, wo die Suche gestartet wird</param>
//        /// <param name="distanceWithin">maximal verbleibende Suchreichweite</param>
//        /// <returns>VehicleDistance zum nächsten IVehicle mit maximalem Abstand distanceWithin</returns>
//        public VehicleDistance GetVehicleBehindArcPosition(double arcPosition, double distanceWithin)
//        {
//            // sich selbst auf Autos untersuchen
//            LinkedListNode<IVehicle> lln = GetVehicleListNodeBehindArcPosition(arcPosition);
//            if (lln != null)
//            {
//                return new VehicleDistance(lln.Value, lln.Value.currentPosition - arcPosition);
//            }
//            // auf dieser Connection ist kein Auto mehr in Reichweite
//            else
//            {
//                // verbleibende Suchreichweite bestimmen
//                double remainingDistance = distanceWithin - (lineSegment.length - arcPosition);

//                if (remainingDistance <= 0)
//                {
//                    return null;
//                }
//                else
//                {
//                    // ich muss noch die ausgehenden Connections untersuchen
//                    if (endNode.nextConnections.Count > 0)
//                    {
//                        List<VehicleDistance> vdList = new List<VehicleDistance>();
//                        foreach (NodeConnection nc in endNode.nextConnections)
//                        {
//                            VehicleDistance foo = nc.GetVehicleBehindArcPosition(0, remainingDistance);
//                            if (foo != null)
//                            {
//                                vdList.Add(foo);
//                            }
//                        }

//                        if (vdList.Count == 0)
//                        {
//                            return null;
//                        }
//                        else
//                        {
//                            // Minimum bestimmen und zurückgeben
//                            double minValue = vdList[0].distance;
//                            VehicleDistance toReturn = vdList[0];

//                            foreach (VehicleDistance vd in vdList)
//                            {
//                                if (vd.distance < minValue)
//                                {
//                                    toReturn = vd;
//                                    minValue = vd.distance;
//                                }
//                            }
//                            toReturn.distance += (lineSegment.length - arcPosition);
//                            return toReturn;
//                        }
//                    }

//                    return null;
//                }
//            }
//        }


//        /// <summary>
//        /// bestimmt das IVehicle vor der Bogenlängenposition arcPosition und die Entfernung dorthin.
//        /// Ist distanceWithin größer als die Entfernung zum endNode, so werden alle weiteren ausgehenden NodeConnections auch untersucht
//        /// </summary>
//        /// <param name="arcPosition">Bogenlängenposition, wo die Suche gestartet wird</param>
//        /// <param name="distanceWithin">maximal verbleibende Suchreichweite</param>
//        /// <returns>VehicleDistance zum vorherigen IVehicle mit maximalem Abstand distanceWithin, VehicleDistance.distance ist dabei positiv!</returns>
//        public VehicleDistance GetVehicleBeforeArcPosition(double arcPosition, double distanceWithin)
//        {
//            // sich selbst auf Autos untersuchen
//            LinkedListNode<IVehicle> lln = GetVehicleListNodeBeforeArcPosition(arcPosition);
//            if (lln != null)
//            {
//                return new VehicleDistance(lln.Value, arcPosition - lln.Value.currentPosition);
//            }
//            // auf dieser Connection ist kein Auto mehr in Reichweite
//            else
//            {
//                // verbleibende Suchreichweite bestimmen
//                double remainingDistance = distanceWithin - arcPosition;

//                if (remainingDistance <= 0)
//                {
//                    return null;
//                }
//                else
//                {
//                    // ich muss noch die eingehenden Connections untersuchen
//                    if (startNode.prevConnections.Count > 0)
//                    {
//                        List<VehicleDistance> vdList = new List<VehicleDistance>();
//                        foreach (NodeConnection nc in startNode.prevConnections)
//                        {
//                            VehicleDistance foo = nc.GetVehicleBeforeArcPosition(nc.lineSegment.length, remainingDistance);
//                            if (foo != null)
//                            {
//                                vdList.Add(foo);
//                            }
//                        }

//                        if (vdList.Count == 0)
//                        {
//                            return null;
//                        }
//                        else
//                        {
//                            // Minimum bestimmen und zurückgeben
//                            double minValue = vdList[0].distance;
//                            VehicleDistance toReturn = vdList[0];

//                            foreach (VehicleDistance vd in vdList)
//                            {
//                                if (vd.distance < minValue)
//                                {
//                                    toReturn = vd;
//                                    minValue = vd.distance;
//                                }
//                            }
//                            toReturn.distance += arcPosition;
//                            return toReturn;
//                        }
//                    }
//                }
//            }
//            return null;
//        }

//        /// <summary>
//        /// bestimmt die Autos vor und hinter der Bogenlängenposition arcPosition. 
//        /// Ist maxDistance größer als die Entfernung zum start-/endNode, so werden alle ein-/ausgehenden Connections auch durchsucht.
//        /// </summary>
//        /// <param name="arcPosition">Bogenlängenposition zu denen die Autos bestimmt werden sollen</param>
//        /// <param name="distanceWithin">maximale Suchreichweite</param>
//        /// <returns>Ein Paar von IVehicles: Left ist das davor, Right das dahinter</returns>
//        public Pair<VehicleDistance> GetVehiclesAroundArcPosition(double arcPosition, double distanceWithin)
//        {
//            return new Pair<VehicleDistance>(GetVehicleBeforeArcPosition(arcPosition, distanceWithin), GetVehicleBehindArcPosition(arcPosition, distanceWithin));
//        }



//        #endregion

//        #region Event handler

//        void Handler_VehicleLeftNodeConnection(object sender, IVehicle.VehicleLeftNodeConnectionEventArgs e)
//        {
//            float averageSpeed = (float)((e.partsUsed.right - e.partsUsed.left) / (10 * (e.timeInterval.right - e.timeInterval.left)));
//            if (averageSpeed > 0)
//            {
//                _countOfVehicles++;
//                _sumOfAverageSpeeds += averageSpeed;
//                if (_visualizeAverageSpeed)
//                    UpdatePen();
//            }
//        }


//        #endregion

//        #region Statistiken

//        /// <summary>
//        /// Resets the statistics array.
//        /// </summary>
//        /// <param name="numBuckets">Needed array size fr </param>
//        public void ResetStatistics(int numBuckets)
//        {
//            statistics = new Statistics[numBuckets];
//        }

//        /// <summary>
//        /// Calculates the current statistics of the vehicles on this NodeConnection and stores it in the given bucket of the statistics array. 
//        /// </summary>
//        /// <param name="bucketNumber">statistics array bucket number to store average velocity in</param>
//        public void GatherStatistics(int bucketNumber)
//        {
//            statistics[bucketNumber].numVehicles = vehicles.Count;
//            statistics[bucketNumber].sumOfVehicleVelocities = 0;
//            statistics[bucketNumber].numStoppedVehicles = 0;

//            foreach (IVehicle v in vehicles)
//            {
//                statistics[bucketNumber].sumOfVehicleVelocities += v.physics.velocity;
//                if (v.isStopped)
//                    ++(statistics[bucketNumber].numStoppedVehicles);
//            }
//        }

//        /// <summary>
//        /// gibt die Durchschnittsgeschwindigkeit der auf dieser Connection fahrenden Autos in m/s zurück
//        /// </summary>
//        /// <returns>Durchschnittsgeschwindigkeit in m/s</returns>
//        public float getAverageSpeedOfVehicles()
//        {
//            if (_countOfVehicles == 0)
//                return 0;
//            else
//                return _sumOfAverageSpeeds / _countOfVehicles;
//        }


//        #endregion


//        #region Hashes
//        /// <summary>
//        /// HashCode des von startNode
//        /// </summary>
//        public int startNodeHash;
//        /// <summary>
//        /// Hashcode von endNode
//        /// </summary>
//        public int endNodeHash;


//        /// <summary>
//        /// Gibt den LineNode aus nodesList zurück, dessen Hash mit hash übereinstimmt
//        /// </summary>
//        /// <param name="nodesList">zu durchsuchende Liste von LineNodes</param>
//        /// <param name="hash">auf Gleichheit zu überprüfender Hashcode</param>
//        /// <returns>den erstbesten LineNode mit GetHashCode() == hash oder null, falls kein solcher existiert</returns>
//        private LineNode GetLineNodeByHash(List<LineNode> nodesList, int hash)
//        {
//            foreach (LineNode ln in nodesList)
//            {
//                if (ln.GetHashCode() == hash)
//                {
//                    return ln;
//                }
//            }
//            return null;
//        }
//        #endregion

//        #region Speichern/ Laden
//        /// <summary>
//        /// Bereitet diese NodeConnection fürs Speichern vor
//        /// Insbesondere werden hier die Hashes der Start- und EndNodes zur Speicherung generiert
//        /// </summary>
//        public void PrepareForSave()
//        {
//            startNodeHash = startNode.GetHashCode();
//            endNodeHash = endNode.GetHashCode();
//        }

//        /// <summary>
//        /// Stellt die Start- und EndNode der NodeConnection anhand der Hashes und der übergebenen Line Liste wieder her
//        /// </summary>
//        /// <param name="saveVersion">Version der gespeicherten Datei</param>
//        /// <param name="nodesList">Eine Liste mit sämtlichen existierenden Linien</param>
//        public void RecoverFromLoad(int saveVersion, List<LineNode> nodesList)
//        {
//            startNode = GetLineNodeByHash(nodesList, startNodeHash);
//            endNode = GetLineNodeByHash(nodesList, endNodeHash);
//            UpdatePen();
//        }
//        #endregion

//        /// <summary>
//        /// gibt Basisinformationen über die NodeConnection als String zurück
//        /// </summary>
//        /// <returns>"NodeConnection von #" + startNode.hashcode + " nach #" + endNode.hashcode</returns>
//        public override string ToString()
//        {
//            return "NodeConnection von #" + startNode.GetHashCode() + " nach #" + endNode.GetHashCode();
//        }


//        /// <summary>
//        /// Zeichnet das Vehicle auf der Zeichenfläche g
//        /// </summary>
//        /// <param name="g">Die Zeichenfläche auf der gezeichnet werden soll</param>
//        public void Draw(Graphics g)
//        {
//            lineSegment.Draw(g, drawingPen);
//        }

//        /// <summary>
//        /// Renders all LineChangePoints originating from this NodeConnection
//        /// </summary>
//        /// <param name="g">Render canvas</param>
//        public void DrawLineChangePoints(Graphics g)
//        {
//            using (Pen orangePen = new Pen(Color.Orange, 1))
//            {
//                using (Brush greenBrush = new SolidBrush(Color.Green))
//                {
//                    using (Brush orangeBrush = new SolidBrush(Color.Orange))
//                    {
//                        using (Brush redBrush = new SolidBrush(Color.Red))
//                        {
//                            foreach (LineChangePoint lcp in _lineChangePoints)
//                            {
//                                // LineChangePoints malen:
//                                lcp.lineSegment.Draw(g, orangePen);
//                                g.FillEllipse(greenBrush, new Rectangle(lineSegment.AtTime(lcp.start.time) - new Vector2(2, 2), new Size(3, 3)));
//                                g.FillEllipse(orangeBrush, new Rectangle(lcp.otherStart.nc.lineSegment.AtTime(lcp.otherStart.time) - new Vector2(2, 2), new Size(3, 3)));
//                                g.FillEllipse(redBrush, new Rectangle(lcp.target.nc.lineSegment.AtTime(lcp.target.time) - new Vector2(2, 2), new Size(3, 3)));
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Zeichnet Debuginformationen auf die Zeichenfläche g
//        /// </summary>
//        /// <param name="g">Die Zeichenfläche auf der gezeichnet werden soll</param>
//        public void DrawDebugData(Graphics g)
//        {
//            g.DrawString(/*"Länge: " + (lineSegment.length/10) + "m\n*/"avg Speed:" + getAverageSpeedOfVehicles() + " m/s", new Font("Arial", 9), new SolidBrush(Color.Black), lineSegment.AtTime(0.5));
//        }

//        #region Subklassen

//        /// <summary>
//        /// kapselt eine NodeConnection, Zeitposition und Bogenlängenposition auf ihr zusammen
//        /// </summary>
//        public struct SpecificPosition
//        {
//            /// <summary>
//            /// erstellt eine SpecificPosition mittels NodeConnection und Zeitparameter
//            /// </summary>
//            /// <param name="nc">NodeConnection</param>
//            /// <param name="time">Zeitparameter auf der NodeConnection</param>
//            public SpecificPosition(NodeConnection nc, double time)
//            {
//                this.nc = nc;
//                this.time = time;
//                this.arcPosition = nc.lineSegment.TimeToArcPosition(time);
//            }

//            /// <summary>
//            /// erstellt eine SpecificPosition mittels NodeConnection und Bogenlängenposition
//            /// </summary>
//            /// <param name="arcPosition">Bogenlängenposition</param>
//            /// <param name="nc">NodeConnection</param>
//            public SpecificPosition(double arcPosition, NodeConnection nc)
//            {
//                this.nc = nc;
//                this.arcPosition = arcPosition;
//                this.time = nc.lineSegment.PosToTime(arcPosition);
//            }

//            /// <summary>
//            /// gekapselte NodeConnection
//            /// </summary>
//            public NodeConnection nc;

//            /// <summary>
//            /// gekapselte Zeit
//            /// </summary>
//            public double time;

//            /// <summary>
//            /// gekapselte Bogenlängenposition
//            /// </summary>
//            public double arcPosition;
//        }

//        /// <summary>
//        /// Sturktur, die einen möglichen Spurwechselort zusammenfasst
//        /// </summary>
//        public struct LineChangePoint
//        {
//            /// <summary>
//            /// erstellt einen neuen LineChangePoint
//            /// </summary>
//            /// <param name="start">Position, wo der LineChangePoint beginnt</param>
//            /// <param name="target">Position, wo der LineChangePoint auf die ZielConnection trifft</param>
//            /// <param name="otherStart">Position die auf der Zielspur auf Höhe von start liegt (parallel gesehen)</param>
//            public LineChangePoint(SpecificPosition start, SpecificPosition target, SpecificPosition otherStart)
//            {
//                this.start = start;
//                this.target = target;
//                this.otherStart = otherStart;

//                parallelDistance = (start.nc.lineSegment.AtTime(start.time) - otherStart.nc.lineSegment.AtTime(otherStart.time)).Abs;

//                lineSegment = new LineSegment(0,
//                    start.nc.lineSegment.AtTime(start.time),
//                    start.nc.lineSegment.AtTime(start.time) + start.nc.lineSegment.DerivateAtTime(start.time).Normalized * (parallelDistance),
//                    target.nc.lineSegment.AtTime(target.time) - target.nc.lineSegment.DerivateAtTime(target.time).Normalized * (parallelDistance),
//                    target.nc.lineSegment.AtTime(target.time));
//            }


//            /// <summary>
//            /// Position, wo der LineChangePoint beginnt
//            /// </summary>
//            public SpecificPosition start;

//            /// <summary>
//            /// Position, wo der LineChangePoint auf die ZielConnection trifft
//            /// </summary>
//            public SpecificPosition target;

//            /// <summary>
//            /// Position die auf der Zielspur auf Höhe von start liegt (parallel gesehen)
//            /// </summary>
//            public SpecificPosition otherStart;

//            /// <summary>
//            /// Euklidische Entfernung zwischen den beiden NodeConnections
//            /// </summary>
//            public double parallelDistance;

//            /// <summary>
//            /// Länge der Bézierkurve des Spurwechsels
//            /// </summary>
//            public double length
//            {
//                get { return lineSegment.length; }
//            }

//            /// <summary>
//            /// Bézierkurve, die das Fahrzeug bei Benutzung dieses LineChangePoints entlang fährt
//            /// </summary>
//            public LineSegment lineSegment;
//        }

//        /// <summary>
//        /// kapselt Informationen, wie man den targetNode über Spurwechsel erreicht.
//        /// ist eine class, damit Call-By-Reference genutzt wird
//        /// </summary>
//        public class LineChangeInterval
//        {
//            /// <summary>
//            /// Zielknoten, der über Spurwechsel erreicht werden soll
//            /// </summary>
//            public LineNode targetNode;

//            /// <summary>
//            /// Bogenlängenposition, ab dem Spurwechsel zum targetNode möglich sind
//            /// </summary>
//            public double startArcPos;

//            /// <summary>
//            /// Bogenlängenposition, bis zu dem Spurwechsel zum targetNode möglich sind
//            /// </summary>
//            public double endArcPos;

//            /// <summary>
//            /// Intervall von Bogenlängenposition, in dem der Spurwechsel zum targetNode möglich ist
//            /// </summary>
//            public Interval<double> arcPosInterval
//            {
//                get { return new Interval<double>(startArcPos, endArcPos); }
//            }

//            /// <summary>
//            /// Länge des Intervall, in dem der Spurwechsel möglich ist
//            /// </summary>
//            public double length
//            {
//                get { return endArcPos - startArcPos; }
//            }


//            /// <summary>
//            /// erstellt ein neues LineChangeInterval
//            /// </summary>
//            /// <param name="targetNode">Zielknoten, der über Spurwechsel erreicht werden soll</param>
//            /// <param name="startArcPos">Bogenlängenposition, ab dem Spurwechsel zum targetNode möglich sind</param>
//            /// <param name="endArcPos">Bogenlängenposition, bis zu dem Spurwechsel zum targetNode möglich sind</param>
//            public LineChangeInterval(LineNode targetNode, double startArcPos, double endArcPos)
//            {
//                this.targetNode = targetNode;
//                this.startArcPos = startArcPos;
//                this.endArcPos = endArcPos;
//            }
//        }

//        /// <summary>
//        /// Single NodeConnection statistics record.
//        /// </summary>
//        public struct Statistics
//        {
//            /// <summary>
//            /// current number of vehicles
//            /// </summary>
//            public int numVehicles;

//            /// <summary>
//            /// sum of vehicle velocities
//            /// </summary>
//            public double sumOfVehicleVelocities;

//            /// <summary>
//            /// number of stopped vehicles
//            /// </summary>
//            public int numStoppedVehicles;
//        }

//        #endregion
//    }
//}
