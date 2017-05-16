using SimTMDG.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Road
{
    public class TLight : IVehicle
    {
        /// <summary>
        /// Klassenvariable welche den letzten vergebenen hashcode speichert und bei jeder Instanziierung eines Objektes inkrementiert werden muss
        /// </summary>
        private static int hashcodeIndex = 0;

        /// <summary>
        /// Hashcode des instanziierten Objektes
        /// </summary>
        private int hashcode = -1;

        /// <summary>
        /// gibt den Hashcode des Fahrzeuges zurück.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashcode;
        }



        public enum TLState
        {
            /// <summary>
            /// grüne Ampel
            /// </summary>
            GREEN,

            /// <summary>
            /// rote Ampel
            /// </summary>
            RED
        }

        public TLState _tLightState;
        /// <summary>
        /// state of the trafficlight
        /// </summary>
        
        public TLState tLightState
        {
            get { return _tLightState; }
            set { _tLightState = value; }
        }


        public TLight()
        {

        }

        public TLight(RoadSegment cs, int laneIndex)
        {
            hashcode = hashcodeIndex++;
            _state.currentSegment = cs;
            _state.laneIdx = laneIndex;
            Routing = new Routing();
            Routing.Push(cs);

            Random rnd = new Random();
            _physics = new IVehicle.Physics(0, 0, 0);

            length = 2;
            width = 2;

            distance = cs.Length - 5;
            tLightState = TLState.RED;
            color = System.Drawing.Color.Red;

            newCoord();
            RotateVehicle(currentSegment.startNode, currentSegment.endNode);
        }

        public override void Think(double tickLength)
        {
        }


        public override void Move(double tickLength)
        {
        }


        public void ChangeState()
        {
            if (tLightState == TLState.RED)
            {
                tLightState = TLState.GREEN;
                color = System.Drawing.Color.Green;


                //Routing.Route.First.Value.lanes[_state.laneIdx].RemoveVehicle(this);
            }
            else if(tLightState == TLState.GREEN)
            {
                tLightState = TLState.RED;
                color = System.Drawing.Color.Red;


                //Routing.Route.First.Value.lanes[_state.laneIdx].AddVehicle(this);
            }
        }
    }
}
