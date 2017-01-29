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
    class Angkot : IVehicle
    {
        public Angkot(RoadSegment cs, int laneIndex, List<RoadSegment> r, int colorCode)
        {
            _state.currentSegment = cs;
            _state.laneIdx = laneIndex;
            p = 0.1;

            a *= (GlobalRandom.Instance.NextDouble() + 0.25);
            b *= (GlobalRandom.Instance.NextDouble() + 0.25);
            s0 *= (GlobalRandom.Instance.NextDouble() + 0.25);
            T *= (GlobalRandom.Instance.NextDouble() + 0.25);

            length = GlobalRandom.Instance.Next(6, 8);
            
            //colour code for angkot, saved in 3 lists (R, G and B)
            List<int> R = new List<int>();
            List<int> G = new List<int>();
            List<int> B = new List<int>();
            R.Add(255); G.Add(0); B.Add(255);// 0
            R.Add(100); G.Add(149); B.Add(237);//1
            R.Add(255); G.Add(0); B.Add(0); // 2
            R.Add(125); G.Add(125); B.Add(0);//3
            R.Add(50); G.Add(0); B.Add(125);//4
            R.Add(100); G.Add(100); B.Add(125);//5
            R.Add(255); G.Add(160); B.Add(122);//6
            R.Add(255); G.Add(165); B.Add(0);//7
            R.Add(238); G.Add(232); B.Add(170);//8
            R.Add(255); G.Add(255); B.Add(0);//9
            R.Add(173); G.Add(255); B.Add(47);//10

            color = Color.FromArgb((R[colorCode]), (G[colorCode]), (B[colorCode]));

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
