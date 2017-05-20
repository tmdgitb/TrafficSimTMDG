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
            R.Add(255); G.Add(0); B.Add(255);// 0 Pink fanta
            R.Add(100); G.Add(149); B.Add(237);//1 Biru muda KalapaLedeng
            R.Add(255); G.Add(0); B.Add(0); // 2 Merah
            R.Add(0); G.Add(255); B.Add(64);//3 Hijau lumut DagoStasiun
            R.Add(0); G.Add(0); B.Add(160); //4 Biru Caringin ssd
            R.Add(255); G.Add(128); B.Add(255);//5 Soft Pink 
            R.Add(255); G.Add(160); B.Add(122);//6 Salem CAheum Ciroyom
            R.Add(153); G.Add(0); B.Add(0);//7 Maroon
            R.Add(255); G.Add(255); B.Add(0);//8 Kuning CibaduyutKrSetra
            R.Add(151); G.Add(75); B.Add(0);//9 Cokelat Dago Riung
            R.Add(173); G.Add(255); B.Add(47);//10 Hijau pupus
            R.Add(0); G.Add(128); B.Add(255);//11 Biru jeans MargahayuLedeng
            R.Add(0); G.Add(0); B.Add(160);//12 Biru Navy KalapaBuahbatu
            R.Add(0); G.Add(128); B.Add(64);//13 Hijau Tua KalapaDago
            R.Add(82); G.Add(0); B.Add(204);//14 Ungu
            R.Add(255); G.Add(230); B.Add(0);//15 Kuning 
            R.Add(179); G.Add(128); B.Add(255);//16 Ungu Muda Cisitu Tegalega
            R.Add(82); G.Add(163); B.Add(163);//17 Toska CAheumLedeng
            R.Add(111); G.Add(211); B.Add(255);//18 Biru krem elang gd bage

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
