using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Road
{
    [Serializable]
    public class Node
    {
        #region ID
        /// <summary>
        /// Node ID
        /// </summary>
        private int _id;

        public int Id
        {
            get { return _id; }

            set { _id = value; }
        }
        #endregion


        #region Position
        /// <summary>
        /// Node position (Vector2D)
        /// </summary>
        private Vector2 _position;
        
        public Vector2 Position
        {
            get { return _position; }

            set { _position = value; }
        }
        #endregion


        #region Constructor
        public Node()
        {

        }

        public Node(Vector2 position)
        {
            _position = position;
        }
        #endregion
    }
}
