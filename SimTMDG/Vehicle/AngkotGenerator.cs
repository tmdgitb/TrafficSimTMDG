using SimTMDG.Road;
using SimTMDG.Tools;
using SimTMDG.Vehicle;
using System;
using System.Collections.Generic;


namespace SimTMDG.Vehicle
{
    class AngkotGenerator : VehicleGenerator
    {

        public enum AngkotPoint
        {
            DAGO,
            STASIUN,STASIUNb,
            KALAPA, SADANGSRG,SADANGSRGb,
            CICAHEUM, CARINGIN,
            LEDENG, RIUNG,
            CIROYOM,
            CIBADUYUT,
            KR_SETRA,
            BUAHBATU,
            MARGAHAYU
        }

        /// <summary>
        /// Start_End
        /// </summary>
        private AngkotPoint _angkotPoint;
        /// <summary>
        /// Start_End
        /// </summary>
        public AngkotPoint angkotPoint
        {
            get { return _angkotPoint; }
            set { _angkotPoint = value; }
        }

        AngkotPoint startPoint;
        List<AngkotPoint> endPoints;

        public AngkotGenerator(List<RoadSegment> _segments, AngkotPoint _startPoint, double _q_in, List<AngkotPoint> _endPoints, List<double> _q_outs)
        {
            this.q_in = _q_in;
            this.segments = _segments;
            this.startPoint = _startPoint;
            this.endPoints = _endPoints;
            for (int i = 0; i < endPoints.Count; i++)
            {
                routes.Add(findRoute(startPoint, endPoints[i]));
            }

            if (startPoint == AngkotPoint.DAGO)
            {
                startSegment = segments.Find(x => x.Id == 7756);
            }
            else if (startPoint == AngkotPoint.LEDENG)
            {
                startSegment = segments.Find(x => x.Id == 5377);
            }
            else if (startPoint == AngkotPoint.CIBADUYUT)
            {
                startSegment = segments.Find(x => x.Id == 22825);
            }
            else if (startPoint == AngkotPoint.KALAPA)
            {
                startSegment = segments.Find(x => x.Id == 860);
            }
            else if (startPoint == AngkotPoint.SADANGSRG)
            {
                startSegment = segments.Find(x => x.Id == 7793);
            }
            else if (startPoint == AngkotPoint.SADANGSRGb)
            {
                startSegment = segments.Find(x => x.Id == 12559);
            }
            else if (startPoint == AngkotPoint.CARINGIN)
            {
                startSegment = segments.Find(x => x.Id == 10554);
            }
            //else if (startPoint == AngkotPoint.CICAHEUM)
            //{
            //    startSegment = segments.Find(x => x.Id == 22825);
            //}
            else if (startPoint == AngkotPoint.MARGAHAYU)
            {
                startSegment = segments.Find(x => x.Id == 21250);
            }
            //else if (startPoint == AngkotPoint.RIUNG)
            //{
            //    startSegment = segments.Find(x => x.Id == 21250);
            //}
            else if (startPoint == AngkotPoint.BUAHBATU)
            {
                startSegment = segments.Find(x => x.Id == 34254);
            }
            else if (startPoint == AngkotPoint.STASIUN)
            {
                startSegment = segments.Find(x => x.Id == 656);
            }
            else if (startPoint == AngkotPoint.STASIUNb)
            {
                startSegment = segments.Find(x => x.Id == 10337);
            }

            this.q_outs = _q_outs;
        }



        #region routing
        List<RoadSegment> findRoute(AngkotPoint start, AngkotPoint end)
        {
            List<RoadSegment> toReturn = new List<RoadSegment>();
            #region manual route
            ///// Manual Route
            ///Route 5: BuahBatu - Kalapa
            if ((start == AngkotPoint.BUAHBATU) && (end == AngkotPoint.KALAPA))
            {
                #region BuahbatuKalapa

                for (int i = 34254; i < 34265; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 37377), segments.Find(x => x.Id == 25068));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 37377), segments.Find(x => x.Id == 25068));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }
                //AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 25039), segments.Find(x => x.Id == 25063));
                //if (newAStar2.goalIsFound)
                //{
                //    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                //    forAstar2 = generatePath(newAStar, segments.Find(x => x.Id == 25039), segments.Find(x => x.Id == 25063));
                //    toReturn.AddRange(forAstar2);
                //    forAstar2.Clear();
                //}
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //for (int i = ; i < ; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                #endregion
            }
            ///// Route 6:  Margahayu - Ledeng 
            else if ((start == AngkotPoint.MARGAHAYU) && (end == AngkotPoint.LEDENG))
            {
                #region MargahayuLedeng
                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 21250), segments.Find(x => x.Id == 11850));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 21250), segments.Find(x => x.Id == 11850));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 12048), segments.Find(x => x.Id == 20349));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 12048), segments.Find(x => x.Id == 20349));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 8764), segments.Find(x => x.Id == 8758));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 8764), segments.Find(x => x.Id == 8758));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 20828), segments.Find(x => x.Id == 14831));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 20828), segments.Find(x => x.Id == 14831));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 12), segments.Find(x => x.Id == 15895));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 12), segments.Find(x => x.Id == 15895));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 8392), segments.Find(x => x.Id == 182));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 8392), segments.Find(x => x.Id == 182));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 12059), segments.Find(x => x.Id == 239));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 12059), segments.Find(x => x.Id == 239));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar7a = new AStar(this.segments, segments.Find(x => x.Id == 13439), segments.Find(x => x.Id == 562));
                if (newAStar7a.goalIsFound)
                {
                    List<RoadSegment> forAstar7a = new List<RoadSegment>();
                    forAstar7a = generatePath(newAStar7a, segments.Find(x => x.Id == 13439), segments.Find(x => x.Id == 562));
                    toReturn.AddRange(forAstar7a);
                    forAstar7a.Clear();
                }

                #endregion

            }
            ///// Route 6: Ledeng - Margahayu
            else if ((start==AngkotPoint.LEDENG) && (end==AngkotPoint.MARGAHAYU))
            {
                #region MargahayuLedeng

                //for (int i = 23184; i < 23193; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}
                //toReturn.Add(segments.Find(x => x.Id == 23166));
                //toReturn.Add(segments.Find(x => x.Id == 8968));
                //toReturn.Add(segments.Find(x => x.Id == 8971));
                //toReturn.Add(segments.Find(x => x.Id == 8980));
                //toReturn.Add(segments.Find(x => x.Id == 8983));
                //toReturn.Add(segments.Find(x => x.Id == 9722));
                //toReturn.Add(segments.Find(x => x.Id == 28542));
                //toReturn.Add(segments.Find(x => x.Id == 28539));
                //toReturn.Add(segments.Find(x => x.Id == 28536));
                //toReturn.Add(segments.Find(x => x.Id == 28533));
                //toReturn.Add(segments.Find(x => x.Id == 28530));
                //toReturn.Add(segments.Find(x => x.Id == 28527));
                //toReturn.Add(segments.Find(x => x.Id == 28524));
                //toReturn.Add(segments.Find(x => x.Id == 28521));
                //toReturn.Add(segments.Find(x => x.Id == 28518));
                //toReturn.Add(segments.Find(x => x.Id == 28515));
                //toReturn.Add(segments.Find(x => x.Id == 28512));
                //toReturn.Add(segments.Find(x => x.Id == 28509));
                //toReturn.Add(segments.Find(x => x.Id == 28506));
                //toReturn.Add(segments.Find(x => x.Id == 736));
                //toReturn.Add(segments.Find(x => x.Id == 739));
                //toReturn.Add(segments.Find(x => x.Id == 727));
                //toReturn.Add(segments.Find(x => x.Id == 722));
                //toReturn.Add(segments.Find(x => x.Id == 950));
                //toReturn.Add(segments.Find(x => x.Id == 29934));
                //toReturn.Add(segments.Find(x => x.Id == 29937));
                //toReturn.Add(segments.Find(x => x.Id == 29940));
                //for (int i = 752; i < 757; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}

                //toReturn.Add(segments.Find(x => x.Id == 1487));
                //toReturn.Add(segments.Find(x => x.Id == 27698));
                //toReturn.Add(segments.Find(x => x.Id == 27701));
                //toReturn.Add(segments.Find(x => x.Id == 21501));
                //toReturn.Add(segments.Find(x => x.Id == 21504));
                //toReturn.Add(segments.Find(x => x.Id == 29924));
                //toReturn.Add(segments.Find(x => x.Id == 29925));
                //toReturn.Add(segments.Find(x => x.Id == 29926));
                //toReturn.Add(segments.Find(x => x.Id == 29927));
                ////perempatan cihampelas jalan layang
                //toReturn.Add(segments.Find(x => x.Id == 26029));
                //toReturn.Add(segments.Find(x => x.Id == 24726));
                //toReturn.Add(segments.Find(x => x.Id == 32729));
                ////cihampelas bawah
                for (int i = 26102; i < 26108; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 33344; i < 33362; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                //pasar wastukencana
                toReturn.Add(segments.Find(x => x.Id == 32694));
                toReturn.Add(segments.Find(x => x.Id == 32696));
                toReturn.Add(segments.Find(x => x.Id == 26108));
                toReturn.Add(segments.Find(x => x.Id == 26109));
                toReturn.Add(segments.Find(x => x.Id == 26110));
                for (int i = 28106; i < 28110; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 10));
                toReturn.Add(segments.Find(x => x.Id == 11));
                toReturn.Add(segments.Find(x => x.Id == 28045));
                //perempatan sebelum BIP
                toReturn.Add(segments.Find(x => x.Id == 27049));
                toReturn.Add(segments.Find(x => x.Id == 27046));
                toReturn.Add(segments.Find(x => x.Id == 27043));
                toReturn.Add(segments.Find(x => x.Id == 27040));
                toReturn.Add(segments.Find(x => x.Id == 27037));
                toReturn.Add(segments.Find(x => x.Id == 28136));
                toReturn.Add(segments.Find(x => x.Id == 28133));
                toReturn.Add(segments.Find(x => x.Id == 28130));
                toReturn.Add(segments.Find(x => x.Id == 28127));
                toReturn.Add(segments.Find(x => x.Id == 28124));
                toReturn.Add(segments.Find(x => x.Id == 28121));
                toReturn.Add(segments.Find(x => x.Id == 29524));
                toReturn.Add(segments.Find(x => x.Id == 29521));
                toReturn.Add(segments.Find(x => x.Id == 29512));
                toReturn.Add(segments.Find(x => x.Id == 29509));
                toReturn.Add(segments.Find(x => x.Id == 29503));
                toReturn.Add(segments.Find(x => x.Id == 29500));
                toReturn.Add(segments.Find(x => x.Id == 33455));
                toReturn.Add(segments.Find(x => x.Id == 33561));
                toReturn.Add(segments.Find(x => x.Id == 33558));
                toReturn.Add(segments.Find(x => x.Id == 33555));
                toReturn.Add(segments.Find(x => x.Id == 33552));
                toReturn.Add(segments.Find(x => x.Id == 33549));
                toReturn.Add(segments.Find(x => x.Id == 33546));
                toReturn.Add(segments.Find(x => x.Id == 33543));
                toReturn.Add(segments.Find(x => x.Id == 33540));
                toReturn.Add(segments.Find(x => x.Id == 33537));
                toReturn.Add(segments.Find(x => x.Id == 33534));
                toReturn.Add(segments.Find(x => x.Id == 33531));
                toReturn.Add(segments.Find(x => x.Id == 33528));
                toReturn.Add(segments.Find(x => x.Id == 33525));
                toReturn.Add(segments.Find(x => x.Id == 33522));
                toReturn.Add(segments.Find(x => x.Id == 33519));
                toReturn.Add(segments.Find(x => x.Id == 33516));
                toReturn.Add(segments.Find(x => x.Id == 33513));
                toReturn.Add(segments.Find(x => x.Id == 33510));
                toReturn.Add(segments.Find(x => x.Id == 33507));
                toReturn.Add(segments.Find(x => x.Id == 33504));
                toReturn.Add(segments.Find(x => x.Id == 33501));
                toReturn.Add(segments.Find(x => x.Id == 33498));
                toReturn.Add(segments.Find(x => x.Id == 33495));
                toReturn.Add(segments.Find(x => x.Id == 33492));
                toReturn.Add(segments.Find(x => x.Id == 33489));
                toReturn.Add(segments.Find(x => x.Id == 33486));
                toReturn.Add(segments.Find(x => x.Id == 33483));
                toReturn.Add(segments.Find(x => x.Id == 33480));
                toReturn.Add(segments.Find(x => x.Id == 107));
                toReturn.Add(segments.Find(x => x.Id == 108));
                toReturn.Add(segments.Find(x => x.Id == 109));
                toReturn.Add(segments.Find(x => x.Id == 21192));
                toReturn.Add(segments.Find(x => x.Id == 21195));
                toReturn.Add(segments.Find(x => x.Id == 21198));
                for (int i = 21200; i < 21205; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 29965));
                for (int i = 28250; i < 28254; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 33187));
                toReturn.Add(segments.Find(x => x.Id == 24513));
                toReturn.Add(segments.Find(x => x.Id == 24512));
                toReturn.Add(segments.Find(x => x.Id == 27086));
                for (int i = 33196; i > 33192; i--)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 31966));
                toReturn.Add(segments.Find(x => x.Id == 31967));
                toReturn.Add(segments.Find(x => x.Id == 33105));
                toReturn.Add(segments.Find(x => x.Id == 33104));
                toReturn.Add(segments.Find(x => x.Id == 33103));
                toReturn.Add(segments.Find(x => x.Id == 28413));
                //cicadas
                for (int i = 31972; i > 31967; i--)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 32875; i < 32881; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 32904; i < 32935; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                //kiaracaondong setelah jalan layang
                for (int i = 33198; i < 33212; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                //perempatan kiaracondong carrefour
                toReturn.Add(segments.Find(x => x.Id == 31990));
                toReturn.Add(segments.Find(x => x.Id == 31991));

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 31999), segments.Find(x => x.Id == 27075));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 31999), segments.Find(x => x.Id == 27075));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                #endregion

            }

            ///Route 8: Kelapa Ledeng
            else if ((start==AngkotPoint.KALAPA) && (end==AngkotPoint.LEDENG))
            {
                
            }

            ////Route 7: Ledeng Kelapa
            else if ((start == AngkotPoint.LEDENG) && (end==AngkotPoint.KALAPA))
            {
                #region LedengKelapa
                for (int i = 23183; i < 23193; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 9136));
                toReturn.Add(segments.Find(x => x.Id == 9133));
                toReturn.Add(segments.Find(x => x.Id == 9130));
                toReturn.Add(segments.Find(x => x.Id == 9127));
                toReturn.Add(segments.Find(x => x.Id == 9124));
                toReturn.Add(segments.Find(x => x.Id == 9121));
                toReturn.Add(segments.Find(x => x.Id == 9118));
                toReturn.Add(segments.Find(x => x.Id == 9115));
                toReturn.Add(segments.Find(x => x.Id == 1426));
                toReturn.Add(segments.Find(x => x.Id == 1427));
                for (int i = 27715; i < 27722; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 9146));
                toReturn.Add(segments.Find(x => x.Id == 27146));
                toReturn.Add(segments.Find(x => x.Id == 227));
                toReturn.Add(segments.Find(x => x.Id == 228));
                toReturn.Add(segments.Find(x => x.Id == 229));
                toReturn.Add(segments.Find(x => x.Id == 27136));
                toReturn.Add(segments.Find(x => x.Id == 27137));
                toReturn.Add(segments.Find(x => x.Id == 27138));
                for (int i = 29942; i < 29950; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 29924));
                toReturn.Add(segments.Find(x => x.Id == 29925));
                toReturn.Add(segments.Find(x => x.Id == 29926));
                toReturn.Add(segments.Find(x => x.Id == 29927));

                //perempatan cihampelas jalan layang
                toReturn.Add(segments.Find(x => x.Id == 26029));
                toReturn.Add(segments.Find(x => x.Id == 29928));
                toReturn.Add(segments.Find(x => x.Id == 24726));
                toReturn.Add(segments.Find(x => x.Id == 32727));
                toReturn.Add(segments.Find(x => x.Id == 32729));
                //cihampelas bawah
                for (int i = 26102; i < 26108; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 33344; i < 33362; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                //pasar wastukencana
                toReturn.Add(segments.Find(x => x.Id == 32694));
                toReturn.Add(segments.Find(x => x.Id == 32696));
                toReturn.Add(segments.Find(x => x.Id == 26108));
                toReturn.Add(segments.Find(x => x.Id == 26109));
                toReturn.Add(segments.Find(x => x.Id == 26110));
                for (int i = 28106; i < 28110; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 10));
                toReturn.Add(segments.Find(x => x.Id == 11));
                for (int i = 13; i < 20; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 2767));
                toReturn.Add(segments.Find(x => x.Id == 2768));
                toReturn.Add(segments.Find(x => x.Id == 23284));
                toReturn.Add(segments.Find(x => x.Id == 23280));
                toReturn.Add(segments.Find(x => x.Id == 28111));
                toReturn.Add(segments.Find(x => x.Id == 28114));
                toReturn.Add(segments.Find(x => x.Id == 28117));
                ////setelah BIP
                toReturn.Add(segments.Find(x => x.Id == 26966));
                toReturn.Add(segments.Find(x => x.Id == 26969));
                toReturn.Add(segments.Find(x => x.Id == 27015));
                toReturn.Add(segments.Find(x => x.Id == 21495));
                toReturn.Add(segments.Find(x => x.Id == 21307));
                toReturn.Add(segments.Find(x => x.Id == 21304));
                toReturn.Add(segments.Find(x => x.Id == 26981));
                for (int i = 21262; i < 21266; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 21119; i < 21127; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 20990));
                toReturn.Add(segments.Find(x => x.Id == 21055));
                for (int i = 84; i < 89; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 21162));
                for (int i = 34279; i < 34285; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 1292));
                for (int i = 34276; i < 34279; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 8815; i < 8819; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 29660; i < 29663; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 20308; i < 20315; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 7806; i < 7810; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 29659));
                toReturn.Add(segments.Find(x => x.Id == 29656));
                toReturn.Add(segments.Find(x => x.Id == 29653));
                toReturn.Add(segments.Find(x => x.Id == 29650));
                toReturn.Add(segments.Find(x => x.Id == 29647));
                toReturn.Add(segments.Find(x => x.Id == 29644));
                toReturn.Add(segments.Find(x => x.Id == 29641));
                toReturn.Add(segments.Find(x => x.Id == 29638));
                toReturn.Add(segments.Find(x => x.Id == 29635));
                toReturn.Add(segments.Find(x => x.Id == 29632));
                toReturn.Add(segments.Find(x => x.Id == 29629));
                toReturn.Add(segments.Find(x => x.Id == 24873));
                toReturn.Add(segments.Find(x => x.Id == 24870));
                toReturn.Add(segments.Find(x => x.Id == 24864));
                toReturn.Add(segments.Find(x => x.Id == 24861));
                toReturn.Add(segments.Find(x => x.Id == 24858));
                toReturn.Add(segments.Find(x => x.Id == 1505));
                toReturn.Add(segments.Find(x => x.Id == 20861));
                toReturn.Add(segments.Find(x => x.Id == 20864));
                toReturn.Add(segments.Find(x => x.Id == 20867));
                toReturn.Add(segments.Find(x => x.Id == 33244));
                toReturn.Add(segments.Find(x => x.Id == 33245));
                //terminal kelapa


                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //for (int i = ; i < ; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}

                //toReturn.Add(segments.Find(x => x.Id == 27149));
                //toReturn.Add(segments.Find(x => x.Id == 27151));
                //toReturn.Add(segments.Find(x => x.Id == 27155));
                //toReturn.Add(segments.Find(x => x.Id == 27157));
                //toReturn.Add(segments.Find(x => x.Id == 27169));
                //toReturn.Add(segments.Find(x => x.Id == 27172));
                //toReturn.Add(segments.Find(x => x.Id == 27178));
                //toReturn.Add(segments.Find(x => x.Id == 27181));
                //toReturn.Add(segments.Find(x => x.Id == 27188));
                //toReturn.Add(segments.Find(x => x.Id == 27191));
                //toReturn.Add(segments.Find(x => x.Id == 27194));
                //toReturn.Add(segments.Find(x => x.Id == 27197));
                //toReturn.Add(segments.Find(x => x.Id == 27203));
                //toReturn.Add(segments.Find(x => x.Id == 27206));
                //toReturn.Add(segments.Find(x => x.Id == 27209));
                //toReturn.Add(segments.Find(x => x.Id == 27212));
                //toReturn.Add(segments.Find(x => x.Id == 27215));
                //toReturn.Add(segments.Find(x => x.Id == 27218));
                //toReturn.Add(segments.Find(x => x.Id == 27221));
                //toReturn.Add(segments.Find(x => x.Id == 27224));
                //toReturn.Add(segments.Find(x => x.Id == 27227));
                //toReturn.Add(segments.Find(x => x.Id == 27230));
                //toReturn.Add(segments.Find(x => x.Id == 27236));
                //toReturn.Add(segments.Find(x => x.Id == 27238));
                //toReturn.Add(segments.Find(x => x.Id == 27241));
                //toReturn.Add(segments.Find(x => x.Id == 27244));
                //toReturn.Add(segments.Find(x => x.Id == 27250));
                //toReturn.Add(segments.Find(x => x.Id == 27253));
                //toReturn.Add(segments.Find(x => x.Id == 27259));
                //toReturn.Add(segments.Find(x => x.Id == 27262));
                //toReturn.Add(segments.Find(x => x.Id == 27265));
                //toReturn.Add(segments.Find(x => x.Id == 27268));
                //toReturn.Add(segments.Find(x => x.Id == 27271));
                //toReturn.Add(segments.Find(x => x.Id == 27274));

                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //for (int i = ; i < ; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}
                //for (int i = ; i < ; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}


                #endregion
            }

            ///Route 4: SdgSAerang - sdgsrgb
            else if ((start == AngkotPoint.SADANGSRG) && (end == AngkotPoint.SADANGSRGb))
            {
                #region  SdgSAerang - SdgSrgb
                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 7793), segments.Find(x => x.Id == 20974));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 7793), segments.Find(x => x.Id == 20974));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 979), segments.Find(x => x.Id == 925));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 979), segments.Find(x => x.Id == 925));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 9646), segments.Find(x => x.Id == 14917));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a= new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 9646), segments.Find(x => x.Id == 14917));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 618), segments.Find(x => x.Id == 6090));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 618), segments.Find(x => x.Id == 6090));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 639), segments.Find(x => x.Id == 621));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 639), segments.Find(x => x.Id == 621));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 15755), segments.Find(x => x.Id == 15740));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 15755), segments.Find(x => x.Id == 15740));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 19), segments.Find(x => x.Id == 30));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 19), segments.Find(x => x.Id == 30));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 34), segments.Find(x => x.Id == 9592));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 34), segments.Find(x => x.Id == 9592));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }

                //for (int i = 23183; i < 23193; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));

                #endregion
            }
            ///Route 4: SdgSAerangb - CAringin
            else if ((start == AngkotPoint.SADANGSRGb) && (end == AngkotPoint.CARINGIN))
            {
                #region  SdgSAerangb - CAringin
                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 12559), segments.Find(x => x.Id == 15494));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 12559), segments.Find(x => x.Id == 15494));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 9613), segments.Find(x => x.Id == 21071));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 9613), segments.Find(x => x.Id == 21071));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 12601), segments.Find(x => x.Id == 12574));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 12601), segments.Find(x => x.Id == 12574));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 12564), segments.Find(x => x.Id == 15805));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 12564), segments.Find(x => x.Id == 15805));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                
                #endregion
            }
            ///Route 4: Caringin - Sdg Saerang
            else if ((start == AngkotPoint.CARINGIN) && (end == AngkotPoint.SADANGSRG))
            {
                #region  caringin - SdgSrg
                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 10554), segments.Find(x => x.Id == 393));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 10554), segments.Find(x => x.Id == 393));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 15806), segments.Find(x => x.Id == 12565));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 15806), segments.Find(x => x.Id ==12565));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 12573), segments.Find(x => x.Id == 12600));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 12573), segments.Find(x => x.Id == 12600));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 9601), segments.Find(x => x.Id == 10553));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 9601), segments.Find(x => x.Id == 10553));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 15739), segments.Find(x => x.Id == 15754));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 15739), segments.Find(x => x.Id == 15754));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                for (int i = 16564; i < 16570; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 620), segments.Find(x => x.Id == 638));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 620), segments.Find(x => x.Id == 638));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 6089), segments.Find(x => x.Id == 7));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 6089), segments.Find(x => x.Id == 7));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 15775), segments.Find(x => x.Id == 16486));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 15775), segments.Find(x => x.Id == 16486));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 924), segments.Find(x => x.Id == 969));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 924), segments.Find(x => x.Id == 969));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }

                //for (int i = 23183; i < 23193; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));

                #endregion
            }

            ///Route 4: Dago - Stasiun
            else if ((start == AngkotPoint.DAGO) && (end== AngkotPoint.STASIUN))
            {
                #region 03StasiunDAgo
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 7756), segments.Find(x => x.Id == 832));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 7756), segments.Find(x => x.Id == 832));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }


                #endregion
            }
            ///Route 4: Stasiun - Stasiunb
            else if ((start == AngkotPoint.STASIUN) && (end == AngkotPoint.STASIUNb))
            {
                #region 03StasiunDAgo
                
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 656), segments.Find(x => x.Id == 15895));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 656), segments.Find(x => x.Id == 15895));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }


                #endregion
            }
            else if ((start == AngkotPoint.STASIUNb) && (end == AngkotPoint.DAGO))
            {
                #region 03StasiunDAgo
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 10337), segments.Find(x => x.Id == 16540));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 10337), segments.Find(x => x.Id == 16540));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }


                #endregion
            }

            /// Route 3: Ledeng - Cicaheum
            else if ((start == AngkotPoint.LEDENG) && (end == AngkotPoint.CICAHEUM))
            {
                #region ledeng cicaheum
                for (int i = 5377; i < 5383; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                AStar newAStar3a = new AStar(this.segments, segments.Find(x => x.Id == 559), segments.Find(x => x.Id == 10459));
                if (newAStar3a.goalIsFound)
                {
                    List<RoadSegment> forAstar3a = new List<RoadSegment>();
                    forAstar3a = generatePath(newAStar3a, segments.Find(x => x.Id == 559), segments.Find(x => x.Id == 10459));
                    toReturn.AddRange(forAstar3a);
                    forAstar3a.Clear();
                }

                //AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 14934), segments.Find(x => x.Id == 15057));
                //if (newAStar3.goalIsFound)
                //{
                //    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                //    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 14934), segments.Find(x => x.Id == 15057));
                //    toReturn.AddRange(forAstar3);
                //    forAstar3.Clear();
                //}

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 6060), segments.Find(x => x.Id == 6039));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 6060), segments.Find(x => x.Id == 6039));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }
                AStar newAStar2b = new AStar(this.segments, segments.Find(x => x.Id == 1227), segments.Find(x => x.Id == 6077));
                if (newAStar2b.goalIsFound)
                {
                    List<RoadSegment> forAstar2b = new List<RoadSegment>();
                    forAstar2b = generatePath(newAStar2b, segments.Find(x => x.Id == 1227), segments.Find(x => x.Id == 6077));
                    toReturn.AddRange(forAstar2b);
                    forAstar2b.Clear();
                }

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 8388), segments.Find(x => x.Id == 6121));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 8388), segments.Find(x => x.Id == 6121));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }
                AStar newAStara = new AStar(this.segments, segments.Find(x => x.Id == 618), segments.Find(x => x.Id == 6090));
                if (newAStara.goalIsFound)
                {
                    List<RoadSegment> forAstara = new List<RoadSegment>();
                    forAstara = generatePath(newAStara, segments.Find(x => x.Id == 618), segments.Find(x => x.Id == 6090));
                    toReturn.AddRange(forAstara);
                    forAstara.Clear();
                }

                AStar newAStar4a = new AStar(this.segments, segments.Find(x => x.Id == 639), segments.Find(x => x.Id == 621));
                if (newAStar4a.goalIsFound)
                {
                    List<RoadSegment> forAstar4a = new List<RoadSegment>();
                    forAstar4a = generatePath(newAStar4a, segments.Find(x => x.Id == 639), segments.Find(x => x.Id == 621));
                    toReturn.AddRange(forAstar4a);
                    forAstar4a.Clear();
                }
                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 40), segments.Find(x => x.Id == 14899));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 40), segments.Find(x => x.Id == 14899));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 6365), segments.Find(x => x.Id == 10362));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 6365), segments.Find(x => x.Id == 10362));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 17968), segments.Find(x => x.Id == 17729));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 17968), segments.Find(x => x.Id == 17729));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }


                #endregion
            }

            ///Route 2: Angkot Dago Kalapa
            else if ((start==AngkotPoint.DAGO) && (end==AngkotPoint.KALAPA))
            {
                #region 02Dago Kelapa
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 7756), segments.Find(x => x.Id == 5640));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 7756), segments.Find(x => x.Id == 5640));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }
                #endregion
            }

            ///Route 2: Angkot Kalapa Dago
            else if ((start == AngkotPoint.KALAPA) && (end == AngkotPoint.DAGO))
            {
                #region 02Dago Kelapa
                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 860), segments.Find(x => x.Id == 8201));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 860), segments.Find(x => x.Id == 8201));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 17250), segments.Find(x => x.Id == 8430));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 17250), segments.Find(x => x.Id == 8430));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 21649), segments.Find(x => x.Id == 21648));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 21649), segments.Find(x => x.Id == 21648));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 17540), segments.Find(x => x.Id == 17166));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 17540), segments.Find(x => x.Id == 17166));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 8953), segments.Find(x => x.Id == 14789));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 8953), segments.Find(x => x.Id == 14789));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                toReturn.Add(segments.Find(x => x.Id == 1621));
                toReturn.Add(segments.Find(x => x.Id == 24187));
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 14819), segments.Find(x => x.Id == 14831));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 14819), segments.Find(x => x.Id == 14831));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 6213), segments.Find(x => x.Id == 16540));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 6213), segments.Find(x => x.Id == 16540));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                
                #endregion
            }
            ///Route 2: Angkot Dago- Riung
            else if ((start == AngkotPoint.DAGO) && (end == AngkotPoint.RIUNG))
            {
                #region 02Dago Riung
                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 7756), segments.Find(x => x.Id == 20974));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 7756), segments.Find(x => x.Id == 20974));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 9655), segments.Find(x => x.Id == 9685));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 9655), segments.Find(x => x.Id == 9685));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 14833), segments.Find(x => x.Id == 20403));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 14833), segments.Find(x => x.Id == 20403));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 6555), segments.Find(x => x.Id == 6540));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 6555), segments.Find(x => x.Id == 6540));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 165), segments.Find(x => x.Id == 169));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 165), segments.Find(x => x.Id == 169));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }

                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 20907), segments.Find(x => x.Id == 11968));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 20907), segments.Find(x => x.Id == 11968));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 10120), segments.Find(x => x.Id == 22197));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 10120), segments.Find(x => x.Id == 22197));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 8780), segments.Find(x => x.Id == 13901));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 8780), segments.Find(x => x.Id == 13901));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 11931), segments.Find(x => x.Id == 19674));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 11931), segments.Find(x => x.Id == 19674));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                #endregion
            }

            /// Route 1: Angkot Cibaduyut - Kr Setra
            else if ((start == AngkotPoint.CIBADUYUT) && (end == AngkotPoint.KR_SETRA))
            {
                #region 01CibaduyutKarangsetra
                for (int i = 22825; i < 22843; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 29606; i < 29612; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 20915));
                for (int i = 23106; i < 23110; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 27930; i < 27935; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 27783));
                toReturn.Add(segments.Find(x => x.Id == 27784));
                toReturn.Add(segments.Find(x => x.Id == 27808));
                toReturn.Add(segments.Find(x => x.Id == 27805));
                toReturn.Add(segments.Find(x => x.Id == 27802));
                toReturn.Add(segments.Find(x => x.Id == 27799));
                toReturn.Add(segments.Find(x => x.Id == 27796));
                toReturn.Add(segments.Find(x => x.Id == 27790));
                toReturn.Add(segments.Find(x => x.Id == 27787));
                toReturn.Add(segments.Find(x => x.Id == 27817));
                toReturn.Add(segments.Find(x => x.Id == 27814));
                toReturn.Add(segments.Find(x => x.Id == 27811));
                toReturn.Add(segments.Find(x => x.Id == 27913));
                toReturn.Add(segments.Find(x => x.Id == 27910));
                toReturn.Add(segments.Find(x => x.Id == 27907));
                toReturn.Add(segments.Find(x => x.Id == 283));
                toReturn.Add(segments.Find(x => x.Id == 256));
                toReturn.Add(segments.Find(x => x.Id == 258));
                toReturn.Add(segments.Find(x => x.Id == 261));
                for (int i = 23081; i < 23083; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 25257));
                for (int i = 25253; i < 25257; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 33342));
                toReturn.Add(segments.Find(x => x.Id == 25251));
                for (int i = 33334; i < 33341; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 22164; i < 22169; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 22181));
                toReturn.Add(segments.Find(x => x.Id == 34144));
                toReturn.Add(segments.Find(x => x.Id == 34147));
                toReturn.Add(segments.Find(x => x.Id == 34150));
                toReturn.Add(segments.Find(x => x.Id == 34153));
                toReturn.Add(segments.Find(x => x.Id == 33288));
                toReturn.Add(segments.Find(x => x.Id == 33292));
                toReturn.Add(segments.Find(x => x.Id == 33294));
                toReturn.Add(segments.Find(x => x.Id == 28101));
                toReturn.Add(segments.Find(x => x.Id == 28607));
                toReturn.Add(segments.Find(x => x.Id == 28605));
                toReturn.Add(segments.Find(x => x.Id == 34206));
                toReturn.Add(segments.Find(x => x.Id == 34205));
                toReturn.Add(segments.Find(x => x.Id == 23111));
                toReturn.Add(segments.Find(x => x.Id == 24741));
                toReturn.Add(segments.Find(x => x.Id == 24738));
                toReturn.Add(segments.Find(x => x.Id == 575));
                toReturn.Add(segments.Find(x => x.Id == 34106));
                toReturn.Add(segments.Find(x => x.Id == 34103));
                toReturn.Add(segments.Find(x => x.Id == 34100));
                toReturn.Add(segments.Find(x => x.Id == 34097));
                toReturn.Add(segments.Find(x => x.Id == 34094));
                toReturn.Add(segments.Find(x => x.Id == 34124));
                toReturn.Add(segments.Find(x => x.Id == 34121));
                toReturn.Add(segments.Find(x => x.Id == 34118));
                toReturn.Add(segments.Find(x => x.Id == 34115));
                toReturn.Add(segments.Find(x => x.Id == 34112));
                toReturn.Add(segments.Find(x => x.Id == 34109));
                toReturn.Add(segments.Find(x => x.Id == 578));
                for (int i = 28095; i < 28098; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 34204));
                toReturn.Add(segments.Find(x => x.Id == 28504));
                toReturn.Add(segments.Find(x => x.Id == 28501));
                toReturn.Add(segments.Find(x => x.Id == 8984));
                toReturn.Add(segments.Find(x => x.Id == 8981));
                toReturn.Add(segments.Find(x => x.Id == 8978));
                toReturn.Add(segments.Find(x => x.Id == 8975));
                toReturn.Add(segments.Find(x => x.Id == 8972));
                toReturn.Add(segments.Find(x => x.Id == 8969));
                toReturn.Add(segments.Find(x => x.Id == 23087));
                toReturn.Add(segments.Find(x => x.Id == 550));
                toReturn.Add(segments.Find(x => x.Id == 547));
                toReturn.Add(segments.Find(x => x.Id == 544));
                toReturn.Add(segments.Find(x => x.Id == 540));
                toReturn.Add(segments.Find(x => x.Id == 538));
                toReturn.Add(segments.Find(x => x.Id == 535));
                toReturn.Add(segments.Find(x => x.Id == 532));
                toReturn.Add(segments.Find(x => x.Id == 529));
                #endregion
            }

            /// Route 0: Pasteur
            #region Pasteur
            //else if ((start.Id == 2908) && (end.Id == 430))
            //{
            //    for (int i = 2908; i < 2926; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }

            //    toReturn.Add(segments.Find(x => x.Id == 421));

            //    for (int i = 28456; i < 28469; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }

            //    for (int i = 422; i < 431; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }
            //}
            //else if ((start.Id == 2908) && (end.Id == 28600))
            //{
            //    for (int i = 2908; i < 2926; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }

            //    toReturn.Add(segments.Find(x => x.Id == 26040));
            //    toReturn.Add(segments.Find(x => x.Id == 34069));

            //    for (int i = 28587; i < 28601; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }
            //}
            //else if ((start.Id == 591) && (end.Id == 25176))
            //{
            //    for (int i = 591; i < 607; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }

            //    for (int i = 8986; i < 8997; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }

            //    for (int i = 28584; i < 28587; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }

            //    for (int i = 25153; i < 25177; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }
            //}
            //else if ((start.Id == 28483) && (end.Id == 25176))
            //{
            //    for (int i = 28483; i < 28493; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }

            //    toReturn.Add(segments.Find(x => x.Id == 28455));
            //    toReturn.Add(segments.Find(x => x.Id == 28583));
            //    toReturn.Add(segments.Find(x => x.Id == 26062));

            //    for (int i = 25153; i < 25177; i++)
            //    {
            //        toReturn.Add(segments.Find(x => x.Id == i));
            //    }
            //}
            #endregion

            #endregion

            return toReturn;
        }
        #endregion


        #region tick
        /// <summary>
        /// The basic Idea is, we add a number to inVehBuffer (q_in which is veh/s * tickLength which is 1s/fps)
        /// Let's say we need to add 2 veh / s, with tickLength 0.25s ==> the inVehBuffer will adds 0.5 veh per tick
        /// Then we generate 1 vehicle every tick IF ONLY we have 1 or more inVehBuffer
        /// With this we generate a vehicle every 2 tick or 2 vehicles every 4 tick, with tickLength 0.25s it means 2 veh / s
        /// </summary>
        public int generate(double tickLength, NodeControl nc)
        {
            int toReturn = 0;
            int destination = 0;
            /// TODO add maximum generation
            /// If vehicle generated from this function is exceeding the max_gen, stop to add this to inVehBuffer
            inVehBuffer += q_in * tickLength;

            if (inVehBuffer >= 1)
            {
                if (startSegment.lanes.Count > 1)
                {
                    //int laneidx = rnd.Next(0, startSegment.lanes.Count); /// TODO replace with shuffled list of lane
                    List<int> laneIdxList = new List<int>();

                    for (int i = 0; i < startSegment.lanes.Count; i++)
                    {
                        laneIdxList.Add(i);
                    }

                    Shuffle<int>(laneIdxList);

                    for (int i = 0; i < startSegment.lanes.Count; i++)
                    {
                        SegmentLane choosenLane = startSegment.lanes[laneIdxList[i]];
                        List<IVehicle> laneVehicles = choosenLane.vehicles;

                        /// If lane is empty OR there's a minimum 10m space between start node and last vehicle's rear position
                        if ((laneVehicles.Count == 0) ||
                            (laneVehicles[0].RearPos > laneVehicles[0].sstar + 7))
                        {
                            double destRnd = rnd.NextDouble();
                            double q_outSum = 0.0;

                            for (int dest = 0; dest < endPoints.Count; dest++)
                            {
                                q_outSum += q_outs[dest];

                                if ((dest == 0))
                                {
                                    if (destRnd < q_outs[0])
                                    {
                                        destination = dest;
                                    }
                                }
                                else
                                {
                                    if ((destRnd >= q_outs[dest - 1]) && (destRnd < q_outSum))
                                    {
                                        destination = dest;
                                    }
                                }
                            }

                            //success = true;

                            //route = routes[destination];
                            //route = findRoute(this.startSegment, this.endSegments[destination]);
                            vehGenerate(laneIdxList[i], destination, nc);
                            toReturn = 1;
                            return toReturn;
                            //break;
                        }
                    }

                }
            }

            return toReturn;
        }


        void vehGenerate(int laneidx, int destination, NodeControl nc)
        {
            IVehicle v = null;
            int colorCode = 0;
            if ((startPoint == AngkotPoint.STASIUN) && (endPoints[destination] == AngkotPoint.STASIUNb))
            {
                colorCode = 3;
            }
            else if ((startPoint == AngkotPoint.STASIUNb) && (endPoints[destination] == AngkotPoint.DAGO))
            {
                colorCode = 3;
            }
            else if ((startPoint == AngkotPoint.DAGO) && (endPoints[destination] == AngkotPoint.STASIUN))
            {
                colorCode = 3;
            }
            else if ((startPoint == AngkotPoint.SADANGSRG) && (endPoints[destination] == AngkotPoint.SADANGSRGb))
            {
                colorCode = 4;
            }
            else if ((startPoint == AngkotPoint.SADANGSRGb) && (endPoints[destination] == AngkotPoint.CARINGIN))
            {
                colorCode = 4;
            }
            else if ((startPoint == AngkotPoint.CARINGIN) && (endPoints[destination] == AngkotPoint.SADANGSRG))
            {
                colorCode = 4;
            }
            else if ((startPoint == AngkotPoint.DAGO) && (endPoints[destination] == AngkotPoint.KALAPA))
            {
                colorCode = 13;
            }
            else if ((startPoint == AngkotPoint.DAGO) && (endPoints[destination] == AngkotPoint.RIUNG))
            {
                colorCode = 9;
            }
            else if ((startPoint == AngkotPoint.LEDENG) && (endPoints[destination] == AngkotPoint.CICAHEUM))
            {
                colorCode = 6;
            }
            else if ((startPoint == AngkotPoint.CIBADUYUT) && (endPoints[destination] == AngkotPoint.KR_SETRA))
            {
                colorCode = 8;
            }
            else if ((startPoint == AngkotPoint.LEDENG) && (endPoints[destination] == AngkotPoint.KALAPA))
            {
                colorCode = 1;
            }
            else if ((startPoint == AngkotPoint.KALAPA) && (endPoints[destination] == AngkotPoint.LEDENG))
             {
                colorCode = 1;
            }
            else if ((startPoint == AngkotPoint.LEDENG) && (endPoints[destination] == AngkotPoint.MARGAHAYU))
            {
                colorCode = 11;
            }
            else if ((startPoint == AngkotPoint.MARGAHAYU) && (endPoints[destination] == AngkotPoint.LEDENG))
            {
                colorCode = 11;
            }
            else if ((startPoint == AngkotPoint.BUAHBATU) && (endPoints[destination] == AngkotPoint.KALAPA))
            {
                colorCode = 12;
            }


            v = new Angkot(startSegment, laneidx, routes[destination], colorCode);

            routes[destination][0].lanes[laneidx].vehicles.Add(v);
            inVehBuffer--;
        }

        #endregion
    }
}
