﻿using SimTMDG.Road;
using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Vehicle
{
    class Car : IVehicle
    {
        //public Car(IVehicle.Physics physics)
        //{

        //    _physics = physics;

        //    a *= (GlobalRandom.Instance.NextDouble() + 0.5);
        //    b *= (GlobalRandom.Instance.NextDouble() + 0.5);
        //    s0 *= (GlobalRandom.Instance.NextDouble() + 0.5);
        //    T *= (GlobalRandom.Instance.NextDouble() + 0.5);

        //    length = GlobalRandom.Instance.Next(4, 7);

        //    color = Color.FromArgb(GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200));
        //}

        public Car(RoadSegment cs, int laneIndex, List<RoadSegment> r)
        {
            a *= (GlobalRandom.Instance.NextDouble() + 0.25);
            b *= (GlobalRandom.Instance.NextDouble() + 0.25);
            s0 *= (GlobalRandom.Instance.NextDouble() + 0.25);
            T *= (GlobalRandom.Instance.NextDouble() + 0.25);

            initVeh(cs, laneIndex, r);
        }


        public Car(RoadSegment cs, int laneIndex, List<RoadSegment> r, double _a, double _b, double _s0, double _T)
        {
            a = _a;
            b = _b;
            s0 = _s0;
            T = _T;

            initVeh(cs, laneIndex, r);
        }

        private void initVeh(RoadSegment cs, int laneIndex, List<RoadSegment> r)
        {
            _state.currentSegment = cs;
            _state.laneIdx = laneIndex;

            length = GlobalRandom.Instance.Next(6, 8);

            //color = Color.FromArgb(GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200), GlobalRandom.Instance.Next(64, 200));
            color = Color.FromArgb(0, 0, 0);

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
