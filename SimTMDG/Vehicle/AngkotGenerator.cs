using SimTMDG.Road;
using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Vehicle
{
    class AngkotGenerator : VehicleGenerator
    {

        public enum AngkotPoint
        {
            DAGO,
            STASIUN,
            KALAPA,
            CICAHEUM,
            LEDENG,
            CIROYOM,
            CIBADUYUT,
            KR_SETRA,
            CARINGIN,
            SD_SAERANG,
            RIUNG,
            CISITU,
            TEGALEGA,
            BUAHBATU,
            ELANG,
            GEDEBAGE
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
                startSegment = segments.Find(x => x.Id == 8178);
            }
            else if (startPoint == AngkotPoint.RIUNG)
            {
                startSegment = segments.Find(x => x.Id == 19811);
            }
            else if (startPoint == AngkotPoint.CISITU)
            {
                startSegment = segments.Find(x => x.Id == 220);
            }
            else if (startPoint == AngkotPoint.TEGALEGA)
            {
                startSegment = segments.Find(x => x.Id == 24990);
            }
            else if (startPoint == AngkotPoint.KALAPA)
            {
                startSegment = segments.Find(x => x.Id == 15138);
            }
            else if (startPoint == AngkotPoint.STASIUN)
            {
                startSegment = segments.Find(x => x.Id == 1212);
            }
            else if (startPoint == AngkotPoint.SD_SAERANG)
            {
                startSegment = segments.Find(x => x.Id == 200);
            }
            else if (startPoint == AngkotPoint.CARINGIN)
            {
                startSegment = segments.Find(x => x.Id == 394);
            }
            else if (startPoint == AngkotPoint.LEDENG)
            {

                startSegment = segments.Find(x => x.Id == 233);
            }
            else if (startPoint == AngkotPoint.CICAHEUM)
            {
                startSegment = segments.Find(x => x.Id == 18109);
            }
            else if (startPoint == AngkotPoint.CIBADUYUT)
            {
                startSegment = segments.Find(x => x.Id == 8529);
            }
            else if (startPoint == AngkotPoint.KR_SETRA)
            {
                startSegment = segments.Find(x => x.Id == 5800);
            }
            else if (startPoint == AngkotPoint.BUAHBATU)
            {
                startSegment = segments.Find(x => x.Id == 22056);
            }
            else if (startPoint == AngkotPoint.ELANG)
            {
                startSegment = segments.Find(x => x.Id == 12634);
            }
            else if (startPoint == AngkotPoint.GEDEBAGE)
            {
                startSegment = segments.Find(x => x.Id == 20166);
            }
            else if (startPoint == AngkotPoint.CIROYOM)
            {
                startSegment = segments.Find(x => x.Id == 308);
            }

            this.q_outs = _q_outs;
        }



        #region routing
        List<RoadSegment> findRoute(AngkotPoint start, AngkotPoint end)
        {
            List<RoadSegment> toReturn = new List<RoadSegment>();

            #region manual route



            /// Route Cibaduyut Kr Setra
            if ((start == AngkotPoint.CIBADUYUT) && (end == AngkotPoint.KR_SETRA))
            {
                #region Cibaduyut Kr Setra


                AStar newAStarx = new AStar(this.segments, segments.Find(x => x.Id == 8529), segments.Find(x => x.Id == 8531));
                if (newAStarx.goalIsFound)
                {
                    List<RoadSegment> forAstarx = new List<RoadSegment>();
                    forAstarx = generatePath(newAStarx, segments.Find(x => x.Id == 8529), segments.Find(x => x.Id == 8531));
                    toReturn.AddRange(forAstarx);
                    forAstarx.Clear();
                }

                AStar newAStarx2 = new AStar(this.segments, segments.Find(x => x.Id == 21811), segments.Find(x => x.Id == 12758));
                if (newAStarx2.goalIsFound)
                {
                    List<RoadSegment> forAstarx2 = new List<RoadSegment>();
                    forAstarx2 = generatePath(newAStarx2, segments.Find(x => x.Id == 21811), segments.Find(x => x.Id == 12758));
                    toReturn.AddRange(forAstarx2);
                    forAstarx2.Clear();
                }
                AStar newAStarx3 = new AStar(this.segments, segments.Find(x => x.Id == 21711), segments.Find(x => x.Id == 21710));
                if (newAStarx3.goalIsFound)
                {
                    List<RoadSegment> forAstarx3 = new List<RoadSegment>();
                    forAstarx3 = generatePath(newAStarx3, segments.Find(x => x.Id == 21711), segments.Find(x => x.Id == 21710));
                    toReturn.AddRange(forAstarx3);
                    forAstarx3.Clear();
                }
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 12771), segments.Find(x => x.Id == 15922));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 12771), segments.Find(x => x.Id == 15922));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 15946), segments.Find(x => x.Id == 708));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 15946), segments.Find(x => x.Id == 708));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 677), segments.Find(x => x.Id == 683));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 677), segments.Find(x => x.Id == 683));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 10776), segments.Find(x => x.Id == 10046));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 10776), segments.Find(x => x.Id == 10046));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 21947), segments.Find(x => x.Id == 21057));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 21947), segments.Find(x => x.Id == 21057));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 10803), segments.Find(x => x.Id == 16770));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 10803), segments.Find(x => x.Id == 16770));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 12521), segments.Find(x => x.Id == 970));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 12521), segments.Find(x => x.Id == 970));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }
                for (int i = 16244; i < 16248; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }

                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 6343), segments.Find(x => x.Id == 6328));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 6343), segments.Find(x => x.Id == 6328));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }


                #endregion
            }
            else if ((start == AngkotPoint.KR_SETRA) && (end == AngkotPoint.CIBADUYUT))
            {
                #region  Kr Setra Cibaduyut

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 5800), segments.Find(x => x.Id == 5804));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 5800), segments.Find(x => x.Id == 5804));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 10787), segments.Find(x => x.Id == 921));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 10787), segments.Find(x => x.Id == 921));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 6336), segments.Find(x => x.Id == 6342));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 6336), segments.Find(x => x.Id == 6342));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 971), segments.Find(x => x.Id == 975));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 971), segments.Find(x => x.Id == 975));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 969), segments.Find(x => x.Id == 12520));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 969), segments.Find(x => x.Id == 12520));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 10810), segments.Find(x => x.Id == 16253));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 10810), segments.Find(x => x.Id == 16253));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 21058), segments.Find(x => x.Id == 21948));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 21058), segments.Find(x => x.Id == 21948));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 653), segments.Find(x => x.Id == 16044));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 653), segments.Find(x => x.Id == 16044));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }

                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 16046), segments.Find(x => x.Id == 15957));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 16046), segments.Find(x => x.Id == 15957));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 15911), segments.Find(x => x.Id == 1256));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 15911), segments.Find(x => x.Id == 1256));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 12757), segments.Find(x => x.Id == 21813));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 12757), segments.Find(x => x.Id == 21813));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }

                #endregion
            }

            /// Route Cicaheum Ciroyom
            else if ((start == AngkotPoint.CICAHEUM) && (end == AngkotPoint.CIROYOM))
            {
                #region Cicaheum Ciroyom
                toReturn.Add(segments.Find(x => x.Id == 18109));
                toReturn.Add(segments.Find(x => x.Id == 18110));
                toReturn.Add(segments.Find(x => x.Id == 18111));
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 24844), segments.Find(x => x.Id == 11820));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 24844), segments.Find(x => x.Id == 11820));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 6527), segments.Find(x => x.Id == 22687));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 6527), segments.Find(x => x.Id == 22687));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 15272), segments.Find(x => x.Id == 445));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 15272), segments.Find(x => x.Id == 445));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 1013));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 1013));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 1673), segments.Find(x => x.Id == 1676));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 1673), segments.Find(x => x.Id == 1676));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }

                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 16253));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 16253));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 21058), segments.Find(x => x.Id == 21951));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 21058), segments.Find(x => x.Id == 21951));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 653), segments.Find(x => x.Id == 10040));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 653), segments.Find(x => x.Id == 10040));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 10062), segments.Find(x => x.Id == 21500));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a = new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 10062), segments.Find(x => x.Id == 21500));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar2aa = new AStar(this.segments, segments.Find(x => x.Id == 13059), segments.Find(x => x.Id == 13032));
                if (newAStar2aa.goalIsFound)
                {
                    List<RoadSegment> forAstar2aa = new List<RoadSegment>();
                    forAstar2aa = generatePath(newAStar2aa, segments.Find(x => x.Id == 13059), segments.Find(x => x.Id == 13032));
                    toReturn.AddRange(forAstar2aa);
                    forAstar2aa.Clear();
                }
                AStar newAStarx = new AStar(this.segments, segments.Find(x => x.Id == 16203), segments.Find(x => x.Id == 17479));
                if (newAStarx.goalIsFound)
                {
                    List<RoadSegment> forAstarx = new List<RoadSegment>();
                    forAstarx = generatePath(newAStarx, segments.Find(x => x.Id == 16203), segments.Find(x => x.Id == 17479));
                    toReturn.AddRange(forAstarx);
                    forAstarx.Clear();
                }
                #endregion
            }
            else if ((start == AngkotPoint.CIROYOM) && (end == AngkotPoint.CICAHEUM))
            {
                #region  Ciroyom Cicaheum 
                toReturn.Add(segments.Find(x => x.Id == 308));
                toReturn.Add(segments.Find(x => x.Id == 309));
                toReturn.Add(segments.Find(x => x.Id == 310));
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 106), segments.Find(x => x.Id == 127));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 106), segments.Find(x => x.Id == 127));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 95), segments.Find(x => x.Id == 10063));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 95), segments.Find(x => x.Id == 10063));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 10050), segments.Find(x => x.Id == 10046));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 10050), segments.Find(x => x.Id == 10046));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 21950), segments.Find(x => x.Id == 21057));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 21950), segments.Find(x => x.Id == 21057));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }



                //AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 10803), segments.Find(x => x.Id == 16770));
                //if (newAStar4.goalIsFound)
                //{
                //    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                //    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 10803), segments.Find(x => x.Id == 16770));
                //    toReturn.AddRange(forAstar4);
                //    forAstar4.Clear();
                //}
                //AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                //if (newAStar5.goalIsFound)
                //{
                //    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                //    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                //    toReturn.AddRange(forAstar5);
                //    forAstar5.Clear();
                //}
                //AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                //if (newAStar6.goalIsFound)
                //{
                //    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                //    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                //    toReturn.AddRange(forAstar6);
                //    forAstar6.Clear();
                //}
                //AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 979));
                //if (newAStar2c.goalIsFound)
                //{
                //    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                //    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 979));
                //    toReturn.AddRange(forAstar2c);
                //    forAstar2c.Clear();
                //}

                //AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 6478), segments.Find(x => x.Id == 6499));
                //if (newAStar7.goalIsFound)
                //{
                //    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                //    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 6478), segments.Find(x => x.Id == 6499));
                //    toReturn.AddRange(forAstar7);
                //    forAstar7.Clear();
                //}
                //AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 10865), segments.Find(x => x.Id == 978));
                //if (newAStar8.goalIsFound)
                //{
                //    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                //    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 10865), segments.Find(x => x.Id == 978));
                //    toReturn.AddRange(forAstar8);
                //    forAstar8.Clear();
                //}
                //AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 15440), segments.Find(x => x.Id == 15308));
                //if (newAStar9.goalIsFound)
                //{
                //    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                //    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 15440), segments.Find(x => x.Id == 15308));
                //    toReturn.AddRange(forAstar9);
                //    forAstar9.Clear();
                //}

                #endregion
            }
            /// Route Elang Gede Bage
            else if ((start == AngkotPoint.ELANG) && (end == AngkotPoint.GEDEBAGE))
            {
                #region Elang Gd Bage

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 12634), segments.Find(x => x.Id == 15214));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 12634), segments.Find(x => x.Id == 15214));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 19837), segments.Find(x => x.Id == 18065));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 19837), segments.Find(x => x.Id == 18065));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }
                #endregion
            }
            else if ((start == AngkotPoint.GEDEBAGE) && (end == AngkotPoint.ELANG))
            {
                #region  Gd Bage Elang

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 20166), segments.Find(x => x.Id == 21233));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 20166), segments.Find(x => x.Id == 21233));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 16443), segments.Find(x => x.Id == 12634));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 16443), segments.Find(x => x.Id == 12634));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                #endregion
            }

            /// Route Kelapa Buah Batu
            else if ((start == AngkotPoint.BUAHBATU) && (end == AngkotPoint.KALAPA))
            {
                #region Buah Batu - Kelapa

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 22056), segments.Find(x => x.Id == 23983));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 22056), segments.Find(x => x.Id == 23983));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 12848), segments.Find(x => x.Id == 12851));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 12848), segments.Find(x => x.Id == 12851));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 12822), segments.Find(x => x.Id == 18058));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 12822), segments.Find(x => x.Id == 18058));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                toReturn.Add(segments.Find(x => x.Id == 22054));
                toReturn.Add(segments.Find(x => x.Id == 22055));

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 1509), segments.Find(x => x.Id == 20999));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 1509), segments.Find(x => x.Id == 20999));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                toReturn.Add(segments.Find(x => x.Id == 8408));
                toReturn.Add(segments.Find(x => x.Id == 8411));

                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 8277), segments.Find(x => x.Id == 12660));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 8277), segments.Find(x => x.Id == 12660));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 9474), segments.Find(x => x.Id == 9459));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 9474), segments.Find(x => x.Id == 9459));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 22083), segments.Find(x => x.Id == 6072));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 22083), segments.Find(x => x.Id == 6072));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }

                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 17678), segments.Find(x => x.Id == 15131));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 17678), segments.Find(x => x.Id == 15131));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 1251), segments.Find(x => x.Id == 1254));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 1251), segments.Find(x => x.Id == 1254));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }


                #endregion
            }
            else if ((start == AngkotPoint.KALAPA) && (end == AngkotPoint.BUAHBATU))
            {
                #region  Kelapa Buah Batu

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 15138), segments.Find(x => x.Id == 9152));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 15138), segments.Find(x => x.Id == 9152));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 22083), segments.Find(x => x.Id == 1508));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 22083), segments.Find(x => x.Id == 1508));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 12746), segments.Find(x => x.Id == 12719));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 12746), segments.Find(x => x.Id == 12719));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 9759), segments.Find(x => x.Id == 18367));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 9759), segments.Find(x => x.Id == 18367));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 10020), segments.Find(x => x.Id == 20990));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 10020), segments.Find(x => x.Id == 20990));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                toReturn.Add(segments.Find(x => x.Id == 10030));
                toReturn.Add(segments.Find(x => x.Id == 10033));

                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 18059), segments.Find(x => x.Id == 12823));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 18059), segments.Find(x => x.Id == 12823));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 22068), segments.Find(x => x.Id == 17640));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 22068), segments.Find(x => x.Id == 17640));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }


                #endregion
            }

            /// Route 3: Cicaheum - Ledeng
            else if ((start == AngkotPoint.LEDENG) && (end == AngkotPoint.CICAHEUM))
            {
                #region Ledeng Cicaheum
                toReturn.Add(segments.Find(x => x.Id == 233));
                toReturn.Add(segments.Find(x => x.Id == 234));
                toReturn.Add(segments.Find(x => x.Id == 235));
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 15310), segments.Find(x => x.Id == 15439));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 15310), segments.Find(x => x.Id == 15439));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 10883), segments.Find(x => x.Id == 10892));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 10883), segments.Find(x => x.Id == 10892));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 6500), segments.Find(x => x.Id == 6479));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 6500), segments.Find(x => x.Id == 6479));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 1640), segments.Find(x => x.Id == 6518));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 1640), segments.Find(x => x.Id == 6518));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 21022), segments.Find(x => x.Id == 6547));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 21022), segments.Find(x => x.Id == 6547));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 1001), segments.Find(x => x.Id == 15291));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 1001), segments.Find(x => x.Id == 15291));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 154), segments.Find(x => x.Id == 179));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 154), segments.Find(x => x.Id == 179));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }
                for (int i = 182; i < 190; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }

                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 444), segments.Find(x => x.Id == 15271));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 444), segments.Find(x => x.Id == 15271));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 6792), segments.Find(x => x.Id == 10794));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 6792), segments.Find(x => x.Id == 10794));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 18405), segments.Find(x => x.Id == 11870));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 18405), segments.Find(x => x.Id == 11870));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 16383), segments.Find(x => x.Id == 16317));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a = new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 16383), segments.Find(x => x.Id == 16317));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar2aa = new AStar(this.segments, segments.Find(x => x.Id == 16388), segments.Find(x => x.Id == 24828));
                if (newAStar2aa.goalIsFound)
                {
                    List<RoadSegment> forAstar2aa = new List<RoadSegment>();
                    forAstar2aa = generatePath(newAStar2aa, segments.Find(x => x.Id == 16388), segments.Find(x => x.Id == 24828));
                    toReturn.AddRange(forAstar2aa);
                    forAstar2aa.Clear();
                }

                #endregion
            }
            else if ((start == AngkotPoint.CICAHEUM) && (end == AngkotPoint.LEDENG))
            {
                #region  Cicaheum Ledeng
                toReturn.Add(segments.Find(x => x.Id == 18109));
                toReturn.Add(segments.Find(x => x.Id == 18110));
                toReturn.Add(segments.Find(x => x.Id == 18111));
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 24832), segments.Find(x => x.Id == 18403));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 24832), segments.Find(x => x.Id == 18403));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 20951), segments.Find(x => x.Id == 22687));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 20951), segments.Find(x => x.Id == 22687));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 15272), segments.Find(x => x.Id == 445));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 15272), segments.Find(x => x.Id == 445));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 1013));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 1013));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 1673), segments.Find(x => x.Id == 1676));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 1673), segments.Find(x => x.Id == 1676));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 979));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 979));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }

                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 6478), segments.Find(x => x.Id == 6499));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 6478), segments.Find(x => x.Id == 6499));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 10865), segments.Find(x => x.Id == 978));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 10865), segments.Find(x => x.Id == 978));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 15440), segments.Find(x => x.Id == 15308));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 15440), segments.Find(x => x.Id == 15308));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }

                #endregion
            }

            /// Route 3: CIsitu -Tegalega
            else if ((start == AngkotPoint.CISITU) && (end == AngkotPoint.TEGALEGA))
            {
                #region Cisitu tegalega
                toReturn.Add(segments.Find(x => x.Id == 220));
                toReturn.Add(segments.Find(x => x.Id == 221));
                toReturn.Add(segments.Find(x => x.Id == 222));
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 74), segments.Find(x => x.Id == 59));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 74), segments.Find(x => x.Id == 59));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 56), segments.Find(x => x.Id == 20));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 56), segments.Find(x => x.Id == 20));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 16038), segments.Find(x => x.Id == 15291));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 16038), segments.Find(x => x.Id == 15291));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 1673), segments.Find(x => x.Id == 1676));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 1673), segments.Find(x => x.Id == 1676));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 6546), segments.Find(x => x.Id == 8827));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 1635), segments.Find(x => x.Id == 1639));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }

                AStar newAStar4a = new AStar(this.segments, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 17943));
                if (newAStar4a.goalIsFound)
                {
                    List<RoadSegment> forAstar4a = new List<RoadSegment>();
                    forAstar4a = generatePath(newAStar4a, segments.Find(x => x.Id == 8415), segments.Find(x => x.Id == 17943));
                    toReturn.AddRange(forAstar4a);
                    forAstar4a.Clear();
                }
                AStar newAStar5a = new AStar(this.segments, segments.Find(x => x.Id == 1047), segments.Find(x => x.Id == 16044));
                if (newAStar5a.goalIsFound)
                {
                    List<RoadSegment> forAstar5a = new List<RoadSegment>();
                    forAstar5a = generatePath(newAStar5a, segments.Find(x => x.Id == 1047), segments.Find(x => x.Id == 16044));
                    toReturn.AddRange(forAstar5a);
                    forAstar5a.Clear();
                }
                AStar newAStar6a = new AStar(this.segments, segments.Find(x => x.Id == 16046), segments.Find(x => x.Id == 755));
                if (newAStar6a.goalIsFound)
                {
                    List<RoadSegment> forAstar6a = new List<RoadSegment>();
                    forAstar6a = generatePath(newAStar6a, segments.Find(x => x.Id == 16046), segments.Find(x => x.Id == 755));
                    toReturn.AddRange(forAstar6a);
                    forAstar6a.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 713), segments.Find(x => x.Id == 10686));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 713), segments.Find(x => x.Id == 10686));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }

                #endregion
            }
            else if ((start == AngkotPoint.TEGALEGA) && (end == AngkotPoint.CISITU))
            {
                #region  Tegalega Cisitu

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 24990), segments.Find(x => x.Id == 20770));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 24990), segments.Find(x => x.Id == 20770));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 15958), segments.Find(x => x.Id == 708));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 15958), segments.Find(x => x.Id == 708));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 677), segments.Find(x => x.Id == 683));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 677), segments.Find(x => x.Id == 683));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 672), segments.Find(x => x.Id == 19285));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 672), segments.Find(x => x.Id == 19285));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 22042), segments.Find(x => x.Id == 10388));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 22042), segments.Find(x => x.Id == 10388));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 661), segments.Find(x => x.Id == 10988));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 661), segments.Find(x => x.Id == 10988));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 16140), segments.Find(x => x.Id == 16155));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a = new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 16140), segments.Find(x => x.Id == 16155));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar2aa = new AStar(this.segments, segments.Find(x => x.Id == 16976), segments.Find(x => x.Id == 16981));
                if (newAStar2aa.goalIsFound)
                {
                    List<RoadSegment> forAstar2aa = new List<RoadSegment>();
                    forAstar2aa = generatePath(newAStar2aa, segments.Find(x => x.Id == 16976), segments.Find(x => x.Id == 16981));
                    toReturn.AddRange(forAstar2aa);
                    forAstar2aa.Clear();
                }

                AStar newAStar9aa = new AStar(this.segments, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 6536));
                if (newAStar9aa.goalIsFound)
                {
                    List<RoadSegment> forAstar9aa = new List<RoadSegment>();
                    forAstar9aa = generatePath(newAStar9aa, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 6536));
                    toReturn.AddRange(forAstar9aa);
                    forAstar9aa.Clear();
                }

                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 16971), segments.Find(x => x.Id == 16037));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 16971), segments.Find(x => x.Id == 16037));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar2c = new AStar(this.segments, segments.Find(x => x.Id == 22), segments.Find(x => x.Id == 55));
                if (newAStar2c.goalIsFound)
                {
                    List<RoadSegment> forAstar2c = new List<RoadSegment>();
                    forAstar2c = generatePath(newAStar2c, segments.Find(x => x.Id == 22), segments.Find(x => x.Id == 55));
                    toReturn.AddRange(forAstar2c);
                    forAstar2c.Clear();
                }

                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 58), segments.Find(x => x.Id == 73));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 58), segments.Find(x => x.Id == 73));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }

                #endregion
            }

            ///Route 5: SDSaerang - Caringin
            else if ((start == AngkotPoint.SD_SAERANG) && (end == AngkotPoint.CARINGIN))
            {
                #region SdSaerang Caringin
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 200), segments.Find(x => x.Id == 204));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 200), segments.Find(x => x.Id == 204));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }
                AStar newAStar4a = new AStar(this.segments, segments.Find(x => x.Id == 21480), segments.Find(x => x.Id == 1324));
                if (newAStar4a.goalIsFound)
                {
                    List<RoadSegment> forAstar4a = new List<RoadSegment>();
                    forAstar4a = generatePath(newAStar4a, segments.Find(x => x.Id == 21480), segments.Find(x => x.Id == 1324));
                    toReturn.AddRange(forAstar4a);
                    forAstar4a.Clear();
                }

                for (int i = 21392; i < 21398; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }

                for (int i = 10095; i < 10100; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 15288; i < 15292; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 154), segments.Find(x => x.Id == 179));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 154), segments.Find(x => x.Id == 179));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }
                for (int i = 182; i < 190; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                toReturn.Add(segments.Find(x => x.Id == 9188));
                toReturn.Add(segments.Find(x => x.Id == 9191));
                toReturn.Add(segments.Find(x => x.Id == 9194));
                toReturn.Add(segments.Find(x => x.Id == 9185));
                toReturn.Add(segments.Find(x => x.Id == 9186));
                toReturn.Add(segments.Find(x => x.Id == 14195));
                toReturn.Add(segments.Find(x => x.Id == 14196));
                toReturn.Add(segments.Find(x => x.Id == 9197));
                toReturn.Add(segments.Find(x => x.Id == 21594));
                toReturn.Add(segments.Find(x => x.Id == 9196));
                toReturn.Add(segments.Find(x => x.Id == 16983));
                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 16156), segments.Find(x => x.Id == 16141));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 16156), segments.Find(x => x.Id == 16141));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                toReturn.Add(segments.Find(x => x.Id == 423));
                toReturn.Add(segments.Find(x => x.Id == 424));
                toReturn.Add(segments.Find(x => x.Id == 425));
                for (int i = 16256; i < 16260; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 426; i < 436; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                for (int i = 2755; i < 2758; i++)
                {
                    toReturn.Add(segments.Find(x => x.Id == i));
                }
                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 16239), segments.Find(x => x.Id == 10040));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 16239), segments.Find(x => x.Id == 10040));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 10062), segments.Find(x => x.Id == 21500));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 10062), segments.Find(x => x.Id == 21500));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 13056), segments.Find(x => x.Id == 13032));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 13056), segments.Find(x => x.Id == 13032));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 13017), segments.Find(x => x.Id == 15884));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 13017), segments.Find(x => x.Id == 15884));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 13022), segments.Find(x => x.Id == 17538));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 13022), segments.Find(x => x.Id == 17538));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 16191), segments.Find(x => x.Id == 705));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 16191), segments.Find(x => x.Id == 705));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 16021), segments.Find(x => x.Id == 21438));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a = new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 16021), segments.Find(x => x.Id == 21438));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar2aa = new AStar(this.segments, segments.Find(x => x.Id == 279), segments.Find(x => x.Id == 306));
                if (newAStar2aa.goalIsFound)
                {
                    List<RoadSegment> forAstar2aa = new List<RoadSegment>();
                    forAstar2aa = generatePath(newAStar2aa, segments.Find(x => x.Id == 279), segments.Find(x => x.Id == 306));
                    toReturn.AddRange(forAstar2aa);
                    forAstar2aa.Clear();
                }



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
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //for (int i = ; i < ; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}

                //AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 200), segments.Find(x => x.Id == 204));
                //if (newAStar.goalIsFound)
                //{
                //    List<RoadSegment> forAstar = new List<RoadSegment>();
                //    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 200), segments.Find(x => x.Id == 204));
                //    toReturn.AddRange(forAstar);
                //    forAstar.Clear();
                //}
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));

                //for (int i = ; i < ; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}



                #endregion
            }
            else if ((start == AngkotPoint.CARINGIN) && (end == AngkotPoint.SD_SAERANG))
            {
                #region  Caringin SdSaerang
                toReturn.Add(segments.Find(x => x.Id == 394));
                toReturn.Add(segments.Find(x => x.Id == 395));
                toReturn.Add(segments.Find(x => x.Id == 396));
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 307), segments.Find(x => x.Id == 280));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 307), segments.Find(x => x.Id == 280));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 21437), segments.Find(x => x.Id == 16020));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 21437), segments.Find(x => x.Id == 16020));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 705), segments.Find(x => x.Id == 694));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 705), segments.Find(x => x.Id == 694));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }
                toReturn.Add(segments.Find(x => x.Id == 13029));
                toReturn.Add(segments.Find(x => x.Id == 13026));
                toReturn.Add(segments.Find(x => x.Id == 13023));

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 10989), segments.Find(x => x.Id == 810));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 10989), segments.Find(x => x.Id == 810));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 13031), segments.Find(x => x.Id == 13058));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 13031), segments.Find(x => x.Id == 13058));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 21501), segments.Find(x => x.Id == 10063));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 21501), segments.Find(x => x.Id == 10063));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 10050), segments.Find(x => x.Id == 10046));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 10050), segments.Find(x => x.Id == 10046));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 21947), segments.Find(x => x.Id == 624));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 21947), segments.Find(x => x.Id == 624));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 626), segments.Find(x => x.Id == 10988));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 626), segments.Find(x => x.Id == 10988));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 16140), segments.Find(x => x.Id == 16155));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a = new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 16140), segments.Find(x => x.Id == 16155));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar2aa = new AStar(this.segments, segments.Find(x => x.Id == 16976), segments.Find(x => x.Id == 16981));
                if (newAStar2aa.goalIsFound)
                {
                    List<RoadSegment> forAstar2aa = new List<RoadSegment>();
                    forAstar2aa = generatePath(newAStar2aa, segments.Find(x => x.Id == 16976), segments.Find(x => x.Id == 16981));
                    toReturn.AddRange(forAstar2aa);
                    forAstar2aa.Clear();
                }

                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 6536));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 1014), segments.Find(x => x.Id == 6536));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }
                AStar newAStar3a = new AStar(this.segments, segments.Find(x => x.Id == 1), segments.Find(x => x.Id == 7));
                if (newAStar3a.goalIsFound)
                {
                    List<RoadSegment> forAstar3a = new List<RoadSegment>();
                    forAstar3a = generatePath(newAStar3a, segments.Find(x => x.Id == 1), segments.Find(x => x.Id == 7));
                    toReturn.AddRange(forAstar3a);
                    forAstar3a.Clear();
                }

                AStar newAStar4a = new AStar(this.segments, segments.Find(x => x.Id == 16182), segments.Find(x => x.Id == 16910));
                if (newAStar4a.goalIsFound)
                {
                    List<RoadSegment> forAstar4a = new List<RoadSegment>();
                    forAstar4a = generatePath(newAStar4a, segments.Find(x => x.Id == 16182), segments.Find(x => x.Id == 16910));
                    toReturn.AddRange(forAstar4a);
                    forAstar4a.Clear();
                }
                AStar newAStar5a = new AStar(this.segments, segments.Find(x => x.Id == 16178), segments.Find(x => x.Id == 16181));
                if (newAStar5a.goalIsFound)
                {
                    List<RoadSegment> forAstar5a = new List<RoadSegment>();
                    forAstar5a = generatePath(newAStar5a, segments.Find(x => x.Id == 16178), segments.Find(x => x.Id == 16181));
                    toReturn.AddRange(forAstar5a);
                    forAstar5a.Clear();
                }
                AStar newAStar6a = new AStar(this.segments, segments.Find(x => x.Id == 1323), segments.Find(x => x.Id == 16176));
                if (newAStar6a.goalIsFound)
                {
                    List<RoadSegment> forAstar6a = new List<RoadSegment>();
                    forAstar6a = generatePath(newAStar6a, segments.Find(x => x.Id == 1323), segments.Find(x => x.Id == 16176));
                    toReturn.AddRange(forAstar6a);
                    forAstar6a.Clear();
                }

                #endregion
            }
            ///Route DAgo - Riung
            else if ((start == AngkotPoint.DAGO) && (end == AngkotPoint.RIUNG))
            {
                #region Dago riung
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 8178), segments.Find(x => x.Id == 13));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 8178), segments.Find(x => x.Id == 13));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 9437), segments.Find(x => x.Id == 15200));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 9437), segments.Find(x => x.Id == 15200));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 369), segments.Find(x => x.Id == 360));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 369), segments.Find(x => x.Id == 360));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 1418), segments.Find(x => x.Id == 20804));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 1418), segments.Find(x => x.Id == 20804));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 6983), segments.Find(x => x.Id == 12309));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 6983), segments.Find(x => x.Id == 12309));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 571), segments.Find(x => x.Id == 575));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 571), segments.Find(x => x.Id == 575));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 577), segments.Find(x => x.Id == 583));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 577), segments.Find(x => x.Id == 583));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 21329), segments.Find(x => x.Id == 12405));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 21329), segments.Find(x => x.Id == 12405));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 12401), segments.Find(x => x.Id == 22650));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 12401), segments.Find(x => x.Id == 22650));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 9231), segments.Find(x => x.Id == 14252));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a = new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 9231), segments.Find(x => x.Id == 14252));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar2aa = new AStar(this.segments, segments.Find(x => x.Id == 12398), segments.Find(x => x.Id == 20959));
                if (newAStar2aa.goalIsFound)
                {
                    List<RoadSegment> forAstar2aa = new List<RoadSegment>();
                    forAstar2aa = generatePath(newAStar2aa, segments.Find(x => x.Id == 12398), segments.Find(x => x.Id == 20959));
                    toReturn.AddRange(forAstar2aa);
                    forAstar2aa.Clear();
                }
                toReturn.Add(segments.Find(x => x.Id == 20675));
                toReturn.Add(segments.Find(x => x.Id == 20678));
                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 350), segments.Find(x => x.Id == 20130));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 350), segments.Find(x => x.Id == 20130));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }


                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //toReturn.Add(segments.Find(x => x.Id == ));
                //for (int i = ; i < ; i++)
                //{
                //    toReturn.Add(segments.Find(x => x.Id == i));
                //}

                //AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 200), segments.Find(x => x.Id == 204));
                //if (newAStar.goalIsFound)
                //{
                //    List<RoadSegment> forAstar = new List<RoadSegment>();
                //    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 200), segments.Find(x => x.Id == 204));
                //    toReturn.AddRange(forAstar);
                //    forAstar.Clear();
                //}

                #endregion
            }
            else if ((start == AngkotPoint.RIUNG) && (end == AngkotPoint.DAGO))
            {
                #region Riung Dago
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 19811), segments.Find(x => x.Id == 348));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 19811), segments.Find(x => x.Id == 348));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }

                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 354), segments.Find(x => x.Id == 12400));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 354), segments.Find(x => x.Id == 12400));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }

                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 14253), segments.Find(x => x.Id == 9232));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 14253), segments.Find(x => x.Id == 9232));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 15281), segments.Find(x => x.Id == 12457));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 15281), segments.Find(x => x.Id == 12457));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 8986), segments.Find(x => x.Id == 12345));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 8986), segments.Find(x => x.Id == 12345));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 12462), segments.Find(x => x.Id == 12474));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 12462), segments.Find(x => x.Id == 12474));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 18194), segments.Find(x => x.Id == 20510));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 18194), segments.Find(x => x.Id == 20510));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }
                AStar newAStar8 = new AStar(this.segments, segments.Find(x => x.Id == 1420), segments.Find(x => x.Id == 1418));
                if (newAStar8.goalIsFound)
                {
                    List<RoadSegment> forAstar8 = new List<RoadSegment>();
                    forAstar8 = generatePath(newAStar8, segments.Find(x => x.Id == 1420), segments.Find(x => x.Id == 1418));
                    toReturn.AddRange(forAstar8);
                    forAstar8.Clear();
                }
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 359), segments.Find(x => x.Id == 368));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 359), segments.Find(x => x.Id == 368));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                AStar newAStar2a = new AStar(this.segments, segments.Find(x => x.Id == 15201), segments.Find(x => x.Id == 22633));
                if (newAStar2a.goalIsFound)
                {
                    List<RoadSegment> forAstar2a = new List<RoadSegment>();
                    forAstar2a = generatePath(newAStar2a, segments.Find(x => x.Id == 15201), segments.Find(x => x.Id == 22633));
                    toReturn.AddRange(forAstar2a);
                    forAstar2a.Clear();
                }
                AStar newAStar2aa = new AStar(this.segments, segments.Find(x => x.Id == 206), segments.Find(x => x.Id == 16966));
                if (newAStar2aa.goalIsFound)
                {
                    List<RoadSegment> forAstar2aa = new List<RoadSegment>();
                    forAstar2aa = generatePath(newAStar2aa, segments.Find(x => x.Id == 206), segments.Find(x => x.Id == 16966));
                    toReturn.AddRange(forAstar2aa);
                    forAstar2aa.Clear();
                }
                #endregion
            }

            ///Route 4: DAgo - Stasiun
            else if ((start == AngkotPoint.DAGO) && (end == AngkotPoint.STASIUN))
            {
                #region DAgoStasiun

                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 8178), segments.Find(x => x.Id == 1225));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 8178), segments.Find(x => x.Id == 1225));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }
                #endregion
            }

            else if ((start == AngkotPoint.STASIUN) && (end == AngkotPoint.DAGO))
            {
                #region StasiunDAgo
                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 1212), segments.Find(x => x.Id == 16307));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 1212), segments.Find(x => x.Id == 16307));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }


                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 13927), segments.Find(x => x.Id == 16966));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 13927), segments.Find(x => x.Id == 16966));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }
                #endregion
            }


            ///Route 2: Angkot Dago - kalapa
            else if ((start == AngkotPoint.DAGO) && (end == AngkotPoint.KALAPA))
            {
                #region Dago Kelapa
                AStar newAStar = new AStar(this.segments, segments.Find(x => x.Id == 8178), segments.Find(x => x.Id == 6072));
                if (newAStar.goalIsFound)
                {
                    List<RoadSegment> forAstar = new List<RoadSegment>();
                    forAstar = generatePath(newAStar, segments.Find(x => x.Id == 8178), segments.Find(x => x.Id == 6072));
                    toReturn.AddRange(forAstar);
                    forAstar.Clear();
                }
                AStar newAStar2 = new AStar(this.segments, segments.Find(x => x.Id == 17684), segments.Find(x => x.Id == 15131));
                if (newAStar2.goalIsFound)
                {
                    List<RoadSegment> forAstar2 = new List<RoadSegment>();
                    forAstar2 = generatePath(newAStar2, segments.Find(x => x.Id == 17684), segments.Find(x => x.Id == 15131));
                    toReturn.AddRange(forAstar2);
                    forAstar2.Clear();
                }
                AStar newAStar3 = new AStar(this.segments, segments.Find(x => x.Id == 1251), segments.Find(x => x.Id == 1254));
                if (newAStar3.goalIsFound)
                {
                    List<RoadSegment> forAstar3 = new List<RoadSegment>();
                    forAstar3 = generatePath(newAStar3, segments.Find(x => x.Id == 1251), segments.Find(x => x.Id == 1254));
                    toReturn.AddRange(forAstar3);
                    forAstar3.Clear();
                }

                #endregion
            }

            else if ((start == AngkotPoint.KALAPA) && (end == AngkotPoint.DAGO))
            {
                #region Kalapa - Dago
                AStar newAStar3a = new AStar(this.segments, segments.Find(x => x.Id == 15138), segments.Find(x => x.Id == 8632));
                if (newAStar3a.goalIsFound)
                {
                    List<RoadSegment> forAstar3a = new List<RoadSegment>();
                    forAstar3a = generatePath(newAStar3a, segments.Find(x => x.Id == 15138), segments.Find(x => x.Id == 8632));
                    toReturn.AddRange(forAstar3a);
                    forAstar3a.Clear();
                }
                AStar newAStar4 = new AStar(this.segments, segments.Find(x => x.Id == 17675), segments.Find(x => x.Id == 12639));
                if (newAStar4.goalIsFound)
                {
                    List<RoadSegment> forAstar4 = new List<RoadSegment>();
                    forAstar4 = generatePath(newAStar4, segments.Find(x => x.Id == 17675), segments.Find(x => x.Id == 12639));
                    toReturn.AddRange(forAstar4);
                    forAstar4.Clear();
                }
                AStar newAStar5 = new AStar(this.segments, segments.Find(x => x.Id == 1668), segments.Find(x => x.Id == 8869));
                if (newAStar5.goalIsFound)
                {
                    List<RoadSegment> forAstar5 = new List<RoadSegment>();
                    forAstar5 = generatePath(newAStar5, segments.Find(x => x.Id == 1668), segments.Find(x => x.Id == 8869));
                    toReturn.AddRange(forAstar5);
                    forAstar5.Clear();
                }
                AStar newAStar6 = new AStar(this.segments, segments.Find(x => x.Id == 22104), segments.Find(x => x.Id == 17587));
                if (newAStar6.goalIsFound)
                {
                    List<RoadSegment> forAstar6 = new List<RoadSegment>();
                    forAstar6 = generatePath(newAStar6, segments.Find(x => x.Id == 22104), segments.Find(x => x.Id == 17587));
                    toReturn.AddRange(forAstar6);
                    forAstar6.Clear();
                }
                AStar newAStar7 = new AStar(this.segments, segments.Find(x => x.Id == 9404), segments.Find(x => x.Id == 15149));
                if (newAStar7.goalIsFound)
                {
                    List<RoadSegment> forAstar7 = new List<RoadSegment>();
                    forAstar7 = generatePath(newAStar7, segments.Find(x => x.Id == 9404), segments.Find(x => x.Id == 15149));
                    toReturn.AddRange(forAstar7);
                    forAstar7.Clear();
                }

                toReturn.Add(segments.Find(x => x.Id == 2034));
                toReturn.Add(segments.Find(x => x.Id == 24646));
                AStar newAStar9 = new AStar(this.segments, segments.Find(x => x.Id == 15179), segments.Find(x => x.Id == 15194));
                if (newAStar9.goalIsFound)
                {
                    List<RoadSegment> forAstar9 = new List<RoadSegment>();
                    forAstar9 = generatePath(newAStar9, segments.Find(x => x.Id == 15179), segments.Find(x => x.Id == 15194));
                    toReturn.AddRange(forAstar9);
                    forAstar9.Clear();
                }
                AStar newAStar9a = new AStar(this.segments, segments.Find(x => x.Id == 6639), segments.Find(x => x.Id == 16966));
                if (newAStar9a.goalIsFound)
                {
                    List<RoadSegment> forAstar9a = new List<RoadSegment>();
                    forAstar9a = generatePath(newAStar9a, segments.Find(x => x.Id == 6639), segments.Find(x => x.Id == 16966));
                    toReturn.AddRange(forAstar9a);
                    forAstar9a.Clear();
                }
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

            if ((startPoint == AngkotPoint.DAGO) && (endPoints[destination] == AngkotPoint.STASIUN))
            {
                colorCode = 3;
            }
            else if ((startPoint == AngkotPoint.STASIUN) && (endPoints[destination] == AngkotPoint.DAGO))
            {
                colorCode = 3;
            }
            else if ((startPoint == AngkotPoint.DAGO) && (endPoints[destination] == AngkotPoint.KALAPA))
            {
                colorCode = 13;
            }
            else if ((startPoint == AngkotPoint.KALAPA) && (endPoints[destination] == AngkotPoint.DAGO))
            {
                colorCode = 13;
            }
            else if ((startPoint == AngkotPoint.LEDENG) && (endPoints[destination] == AngkotPoint.CICAHEUM))
            {
                colorCode = 17;
            }
            else if ((startPoint == AngkotPoint.CICAHEUM) && (endPoints[destination] == AngkotPoint.LEDENG))
            {
                colorCode = 17;
            }
            else if ((startPoint == AngkotPoint.CIBADUYUT) && (endPoints[destination] == AngkotPoint.KR_SETRA))
            {
                colorCode = 8;
            }
            else if ((startPoint == AngkotPoint.KR_SETRA) && (endPoints[destination] == AngkotPoint.CIBADUYUT))
            {
                colorCode = 8;
            }
            else if ((startPoint == AngkotPoint.SD_SAERANG) && (endPoints[destination] == AngkotPoint.CARINGIN))
            {
                colorCode = 4;
            }
            else if ((startPoint == AngkotPoint.CARINGIN) && (endPoints[destination] == AngkotPoint.SD_SAERANG))
            {
                colorCode = 4;
            }
            else if ((startPoint == AngkotPoint.DAGO) && (endPoints[destination] == AngkotPoint.RIUNG))
            {
                colorCode = 9;
            }
            else if ((startPoint == AngkotPoint.RIUNG) && (endPoints[destination] == AngkotPoint.DAGO))
            {
                colorCode = 9;
            }
            else if ((startPoint == AngkotPoint.CISITU) && (endPoints[destination] == AngkotPoint.TEGALEGA))
            {
                colorCode = 16;
            }
            else if ((startPoint == AngkotPoint.TEGALEGA) && (endPoints[destination] == AngkotPoint.CISITU))
            {
                colorCode = 16;
            }
            else if ((startPoint == AngkotPoint.BUAHBATU) && (endPoints[destination] == AngkotPoint.KALAPA))
            {
                colorCode = 12;
            }
            else if ((startPoint == AngkotPoint.KALAPA) && (endPoints[destination] == AngkotPoint.BUAHBATU))
            {
                colorCode = 12;
            }
            else if ((startPoint == AngkotPoint.ELANG) && (endPoints[destination] == AngkotPoint.GEDEBAGE))
            {
                colorCode = 18;
            }
            else if ((startPoint == AngkotPoint.GEDEBAGE) && (endPoints[destination] == AngkotPoint.ELANG))
            {
                colorCode = 6;
            }
            else if ((startPoint == AngkotPoint.CICAHEUM) && (endPoints[destination] == AngkotPoint.CIROYOM))
            {
                colorCode = 6;
            }
            else if ((startPoint == AngkotPoint.CIROYOM) && (endPoints[destination] == AngkotPoint.CICAHEUM))
            {
                colorCode = 6;
            }


            v = new Angkot(startSegment, laneidx, routes[destination], colorCode);

            routes[destination][0].lanes[laneidx].vehicles.Add(v);
            inVehBuffer--;
        }

        #endregion
    }
}
