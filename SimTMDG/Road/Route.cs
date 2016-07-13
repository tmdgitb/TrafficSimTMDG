using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Road
{
    public class Routing : IEnumerable<WaySegment>
    {
        /// <summary>
		/// Wegroute
		/// </summary>
		private LinkedList<WaySegment> route;

        /// <summary>
		/// Kosten der gesamten Route
		/// </summary>
		public double costs;

        internal LinkedList<WaySegment> Route
        {
            get
            {
                return route;
            }

            set
            {
                route = value;
            }
        }

        #region Constructor
        /// <summary>
        /// Standardkonstruktor, erstellt eine neue leere Wegroute zu einem Zielknoten
        /// </summary>
        public Routing()
        {
            Route = new LinkedList<WaySegment>();
            costs = 0;
        }
        #endregion

        /// <summary>
		/// Pusht das übergebene RouteSegment auf den route-Stack und aktualisiert die Kosten und Anzahl benötigter Spurwechsel
		/// </summary>
		/// <param name="rs">einzufügendes RouteSegment</param>
		public void Push(WaySegment ws)
        {
            Route.AddFirst(ws);
            costs += ws.Length; // TODO use length as cost for now
        }

        public IEnumerator<WaySegment> GetEnumerator()
        {
            return Route.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Route.GetEnumerator();
        }
    }
}
