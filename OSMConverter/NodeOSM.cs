using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OSMConverter
{
    [Serializable, XmlRoot("node"), XmlType("node")]
    public class NodeOSM
    {
        #region Attributes
        /// <summary>
        /// ID index for generating ID in case of shifted RoadSegment
        /// </summary>
        private static int idIndex = 0;

        #region OSM Node
        /// <summary>
        /// Node ID
        /// </summary>
        private long _id;
        [XmlAttribute("id")]
        public long Id { get { return _id; } set { _id = value; } }

        /// <summary>
        /// Node Latitude in OSM
        /// </summary>
        private Double _lat;
        [XmlAttribute("lat")]
        public Double Lat { get { return _lat; } set { _lat = value / 1; } }

        /// <summary>
        /// Node Longitude in OSM
        /// </summary>
        private Double _long;
        [XmlAttribute("lon")]
        public Double Long { get { return _long; } set { _long = value / 1; } }
        #endregion


        #region Position
        /// <summary>
        /// Node position in converted cartesian coordinate (Vector2D)
        /// </summary>
        private Vector2 _position;


        public Vector2 Position
        {
            get { return _position; }

            set { _position = value; }
        }
        #endregion

        #endregion




        #region Constructor
        public NodeOSM()
        {

        }

        public NodeOSM(Vector2 position)
        {
            _position = position;
        }

        public NodeOSM(Vector2 position, Boolean generateID)
        {
            if (generateID)
            {
                Id = idIndex++;
            }

            _position = position;
        }
        #endregion




        #region Methods

        #region Tools
        public void latLonToPos(Double minLong, Double maxLat)
        {
            Vector2 toReturn = new Vector2(
                (Math.Ceiling((_long - minLong) * 111111)),
                (Math.Ceiling((maxLat - _lat) * 111111))
            );
            this.Position = toReturn;
        }
        #endregion

        #endregion
    }
}
