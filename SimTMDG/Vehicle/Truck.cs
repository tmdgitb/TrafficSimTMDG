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
    class Truck : IVehicle
    {
        //public Truck(IVehicle.Physics physics)
        //{
        //    a = 0.6;
        //    b = 0.8;

        //    _physics = physics;

        //    a *= (GlobalRandom.Instance.NextDouble() + 0.5);
        //    b *= (GlobalRandom.Instance.NextDouble() + 0.5);
        //    s0 *= (GlobalRandom.Instance.NextDouble() + 0.5);
        //    T *= (GlobalRandom.Instance.NextDouble() + 0.5);

        //    length = (GlobalRandom.Instance.Next(2) == 0) ? 10 : 16;

        //    color = Color.FromArgb(GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200));
        //}

        public Truck(RoadSegment cs, int laneIndex, List<RoadSegment> r)
        {
            a = 0.7;
            b = 2;
            s0 = 4;
            T = 2;
            
            a = Math2.GetRandomNumber(0.9 * a, 1.1 * a);
            b = Math2.GetRandomNumber(0.9 * b, 1.1 * b);
            s0 = Math2.GetRandomNumber(0.9 * s0, 1.1 * s0);
            T = Math2.GetRandomNumber(0.9 * T, 1.1 * T);

            _physics = physics;
            _state.currentSegment = cs;
            _state.laneIdx = laneIndex;

            length = (GlobalRandom.Instance.Next(2) == 0) ? 10 : 12;

            color = Color.FromArgb(GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200));

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
