using SimTMDG.Road;
using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Vehicle
{
    class Bus : IVehicle
    {
        //public Bus(IVehicle.Physics physics)
        //{
        //    a = 0.9;
        //    b = 1.0;

        //    _physics = physics;

        //    length = (GlobalRandom.Instance.Next(2) == 0) ? 12 : 18;

        //    color = Color.FromArgb(GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200));
        //}

        public Bus(RoadSegment cs, int laneIndex, List<RoadSegment> r)
        {
            a = 0.9;
            b = 2.0;
            s0 = 3;
            T = 2;

            a *= (GlobalRandom.Instance.NextDouble() + 0.25);
            b *= (GlobalRandom.Instance.NextDouble() + 0.25);
            s0 *= (GlobalRandom.Instance.NextDouble() + 0.25);
            T *= (GlobalRandom.Instance.NextDouble() + 0.25);

            initVeh(cs, laneIndex, r);
        }


        public Bus(RoadSegment cs, int laneIndex, List<RoadSegment> r, double _a, double _b, double _s0, double _T)
        {
            a = _a;
            b = _b;
            s0 = _s0;
            T = _T;

            initVeh(cs, laneIndex, r);
        }


        private void initVeh(RoadSegment cs, int laneIndex, List<RoadSegment> r)
        {
            _physics = physics;
            _state.currentSegment = cs;
            _state.laneIdx = laneIndex;


            length = (GlobalRandom.Instance.Next(2) == 0) ? 12 : 14;

            //color = Color.FromArgb(GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200));
            color = Color.FromArgb(140, 140, 140);

            Routing = new Routing();
            for (int i = 0; i < r.Count; i++)
            {
                Routing.Push(r[i]);
            }
            _physics = new IVehicle.Physics(14, 14, 0);

            newCoord();
            RotateVehicle(currentSegment.startNode, currentSegment.endNode);
        }
    }
}
