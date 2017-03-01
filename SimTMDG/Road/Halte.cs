/*
 *  CityTrafficSimulator - a tool to simulate traffic in urban areas and on intersections
 *  Copyright (C) 2005-2014, Christian Schulte zu Berge
 *  
 *  This program is free software; you can redistribute it and/or modify it under the 
 *  terms of the GNU General Public License as published by the Free Software 
 *  Foundation; either version 3 of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY 
 *  WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 *  PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along with this 
 *  program; if not, see <http://www.gnu.org/licenses/>.
 * 
 *  Web:  http://www.cszb.net
 *  Mail: software@cszb.net
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using SimTMDG.Vehicle;

namespace SimTMDG.Road
{
    /// <summary>
    /// Kapselt eine Ampel und implementiert ein TimelineEntry
    /// </summary>
    [Serializable]
    public class Halte // : TimelineEntry, ISavable
    {
        /// <summary>
        /// Status einer Ampel (rot oder grün)
        /// </summary>
        public enum Place
        {
            /// <summary>
            /// grüne Ampel
            /// </summary>
            WHITE,

            /// <summary>
            /// rote Ampel
            /// </summary>
            BLUE
        }

        public double ngetemTimer=0;
        /// <summary>
        /// aktueller Status der Ampel
        /// </summary>
        private Place _halteState;
        /// <summary>
        /// aktueller Status der Ampel
        /// </summary>
        [XmlIgnore]
        public Place HaltePlace
        {
            get { return _halteState; }
            set { _halteState = value; }
        }

        /// <summary>
        /// Liste von LineNodes denen dieses Halte zugeordnet ist
        /// </summary>
        [XmlIgnore]
        private List<Node> _assignedNodes = new List<Node>();

        /// <summary>
        /// Liste von LineNodes denen dieses Halte zugeordnet ist
        /// </summary>
        [XmlIgnore]
        public List<Node> assignedNodes
        {
            get { return _assignedNodes; }
        }
        public IVehicle angkotNgetem = null;
        public bool halteTimer = false;

        #region Hashcodes

        /*
		 * Nachdem der ursprüngliche Ansatz zu Hashen zu argen Kollisionen geführt hat, nun eine verlässliche Methode für Kollisionsfreie Hashes 
		 * mittels eindeutiger IDs für jedes Halte die über statisch Klassenvariablen vergeben werden
		 */

        /// <summary>
        /// Klassenvariable welche den letzten vergebenen hashcode speichert und bei jeder Instanziierung eines Objektes inkrementiert werden muss
        /// </summary>
        [XmlIgnore]
        private static int hashcodeIndex = 0;

        /// <summary>
        /// Hashcode des instanziierten Objektes
        /// </summary>
        public int hashcode = -1;

        /// <summary>
        /// gibt den Hashcode des Fahrzeuges zurück.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashcode;
        }

        /// <summary>
        /// Setzt die statische Klassenvariable hashcodeIndex zurück. Achtung: darf nur in bestimmten Fällen aufgerufen werden.
        /// </summary>
        public static void ResetHashcodeIndex()
        {
            hashcodeIndex = 0;
        }

        #endregion

        #region Konstruktoren

        /// <summary>
        /// Konstruktor für TimelineEntry-Ampeln
        /// </summary>
        public Halte()
        {
            hashcode = hashcodeIndex++;

            // Initial Event anlegen
            //this.defaultAction = SwitchToBlue;
            HaltePlace = Place.WHITE;
            //this.color = Color.BLUE;
        }
        #endregion

        public void Tick(double ticklength)
        {
            if (halteTimer)
            {
                ngetemTimer += ticklength;
            }

            if (HaltePlace == Halte.Place.BLUE)
            {
                if (ngetemTimer > 3)
                {
                    HaltePlace = Halte.Place.WHITE;
                    angkotNgetem = null;
                    ngetemTimer = 0;
                }
            } else if (HaltePlace == Halte.Place.WHITE)
            {
                if (ngetemTimer > 3)
                {
                    halteTimer = false;
                    ngetemTimer = 0;
                    HaltePlace = Halte.Place.BLUE;
                }
            }
            
        }

        public void Timing(double ticklength, Node a)
        {

            //if (a.tHalte.HaltePlace==Halte.Place.BLUE)
            //{
            //    a.tHalte.ngetemTimer += ticklength;
            //}
            //else if (a.tHalte.ngetemTimer >= 50)
            //{
            //    a.tHalte.ngetemTimer = 0;
            //    a.tHalte.SwitchToWhite();
            //}

            if (ngetemTimer > 5000)
            {
                this.HaltePlace = Halte.Place.WHITE;
                ngetemTimer = 0;
            }

        }

        #region Speichern/Laden
        ///// <summary>
        ///// DEPRECATED: Hash des Elternknotens (wird für Serialisierung gebraucht)
        ///// </summary>
        //[XmlIgnore]
        //public int parentNodeHash = 0;

        ///// <summary>
        ///// Hashes der zugeordneten LineNodes
        ///// </summary>
        //public List<int> assignedNodesHashes = new List<int>();

        ///// <summary>
        ///// bereitet das Halte auf die XML-Serialisierung vor.
        ///// </summary>
        //public override void PrepareForSave()
        //{
        //    base.PrepareForSave();

        //    assignedNodesHashes.Clear();
        //    foreach (LineNode ln in _assignedNodes)
        //    {
        //        assignedNodesHashes.Add(ln.GetHashCode());
        //    }
        //}
        ///// <summary>
        ///// stellt das Halte nach einer XML-Deserialisierung wieder her
        ///// </summary>
        ///// <param name="saveVersion">Version der gespeicherten Datei</param>
        ///// <param name="nodesList">Liste von allen existierenden LineNodes</param>
        //public override void RecoverFromLoad(int saveVersion, List<LineNode> nodesList)
        //{
        //    // Klassenvariable für Hashcode erhöhen um Kollisionen für zukünftige LineNodes zu verhindern
        //    if (hashcodeIndex <= hashcode)
        //    {
        //        hashcodeIndex = hashcode + 1;
        //    }

        //    // erstmal EventActions setzen
        //    this.defaultAction = SwitchToBlue;
        //    foreach (TimelineEvent e in events)
        //    {
        //        e.eventStartAction = SwitchToWhite;
        //        e.eventEndAction = SwitchToBlue;
        //    }

        //    // nun die assignedNodes aus der nodesList dereferenzieren
        //    foreach (int hash in assignedNodesHashes)
        //    {
        //        foreach (LineNode ln in nodesList)
        //        {
        //            if (ln.GetHashCode() == hash)
        //            {
        //                _assignedNodes.Add(ln);
        //                ln.tHalte = this;
        //                break;
        //            }
        //        }
        //    }

        //    // Alte Versionen konnten nur einen Node pro Halte haben und waren daher anders referenziert, auch darum wollen wir uns kümmern:
        //    if (saveVersion <= 2)
        //    {
        //        foreach (LineNode ln in nodesList)
        //        {
        //            if (ln.GetHashCode() == parentNodeHash)
        //            {
        //                AddAssignedLineNode(ln);
        //                break;
        //            }
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// meldet den LineNode ln bei diesem Halte an, sodass es weiß das es diesem zugeordnet ist
        /// </summary>
        /// <param name="ln">anzumeldender LineNode</param>
        public void AddAssignedLineNode(Node ln)
        {
            _assignedNodes.Add(ln);
            ln.tHalte = this;
        }

        /// <summary>
        /// meldet den LineNode ln bei diesem Halte wieder ab, sodass es weiß, dass es diesem nicht mehr zugeordnet ist
        /// </summary>
        /// <param name="ln">abzumeldender LineNode</param>
        /// <returns>true, falls der Abmeldevorgang erfolgreich, sonst false</returns>
        public bool RemoveAssignedLineNode(Node ln)
        {
            if (ln != null)
            {
                ln.tHalte = null;
                return _assignedNodes.Remove(ln);
            }
            return false;
        }

        /// <summary>
        /// stellt die Ampel auf grün
        /// </summary>
        public void SwitchToWhite()
        {
            this.HaltePlace = Place.WHITE;
        }
        /// <summary>
        /// stellt die Ampel auf rot
        /// </summary>
        public void SwitchToBlue()
        {
            this.HaltePlace = Place.BLUE;
        }


        ///// <summary>
        ///// meldet das Halte bei den zugeordneten LineNodes ab, sodas das Halte gefahrlos gelöscht werden kann.
        ///// </summary>
        //public override void Dispose()
        //{
        //    base.Dispose();

        //    while (_assignedNodes.Count > 0)
        //    {
        //        RemoveAssignedLineNode(_assignedNodes[0]);
        //    }
        //}

    }
}
