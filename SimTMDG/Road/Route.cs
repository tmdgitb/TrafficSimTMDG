using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Road
{
    class Route : IEnumerable<WaySegment>
    {
        /// <summary>
		/// Wegroute
		/// </summary>
		private LinkedList<WaySegment> route;

        /// <summary>
		/// Kosten der gesamten Route
		/// </summary>
		public double costs;

        #region Constructor
        /// <summary>
		/// Standardkonstruktor, erstellt eine neue leere Wegroute zu einem Zielknoten
		/// </summary>
		public Route()
        {
            route = new LinkedList<WaySegment>();
            costs = 0;
        }
        #endregion

        /// <summary>
		/// Pusht das übergebene RouteSegment auf den route-Stack und aktualisiert die Kosten und Anzahl benötigter Spurwechsel
		/// </summary>
		/// <param name="rs">einzufügendes RouteSegment</param>
		public void Push(WaySegment ws)
        {
            route.AddFirst(ws);
            costs += ws.Length; // TODO use length as cost for now
        }

        public IEnumerator<WaySegment> GetEnumerator()
        {
            return route.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return route.GetEnumerator();
        }
    }
}
