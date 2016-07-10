using SimTMDG.Road;
using SimTMDG.Time;
using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimTMDG
{
    public partial class Main : Form
    {
        #region TEMP
        private double _temp_stepsPerSeconds = 24;
        private double _temp_simulationDuration = 5;
        NodeControl nc;
        #endregion


        #region Helper

        /// <summary>
        /// stores the window state
        /// </summary>
        [Serializable]
        public struct WindowSettings
        {
            /// <summary>
            /// Window state
            /// </summary>
            public FormWindowState _windowState;
            /// <summary>
            /// Position of window
            /// </summary>
            public Point _position;
            /// <summary>
            /// Size of window
            /// </summary>
            public Size _size;
        }

        private enum DragNDrop
        {
            NONE,
            MOVE_MAIN_GRID,
            MOVE_NODES,
            CREATE_NODE,
            MOVE_IN_SLOPE, MOVE_OUT_SLOPE,
            MOVE_TIMELINE_BAR, MOVE_EVENT, MOVE_EVENT_START, MOVE_EVENT_END,
            MOVE_THUMB_RECT,
            DRAG_RUBBERBAND
        }

        /// <summary>
        /// MainForm invalidation level
        /// </summary>
        public enum InvalidationLevel
        {
            /// <summary>
            /// invalidate everything
            /// </summary>
            ALL,
            /// <summary>
            /// invalidate only main canvas
            /// </summary>
            ONLY_MAIN_CANVAS,
            /// <summary>
            /// invalidate main canvas and timeline
            /// </summary>
            MAIN_CANVAS_AND_TIMELINE
        }

        #endregion

        #region Variables / Properties
        /// <summary>
        /// Simulation playing status
        /// </summary>
        private bool simIsPlaying = false;

        /// <summary>
		/// Stopwatch for timing of rendering
		/// </summary>
		private System.Diagnostics.Stopwatch renderStopwatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Stopwatch for timing the traffic logic
        /// </summary>
        private System.Diagnostics.Stopwatch thinkStopwatch = new System.Diagnostics.Stopwatch();


        /// <summary>
        /// NodeSteuerung
        /// </summary>
        //public NodeControl nodeControl = new NodeControl();

        #region temp
        RoadSegment roadSegment;
        #endregion

        #endregion

        public Main()
        {
            // - maxtime

            InitializeComponent();

            // - colorlist
            // - setdockingstuff

            //
            DaGrid.Dock = DockStyle.Fill;

            // - setstyle
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // - renderoptions


            //temp
            //roadSegment = new RoadSegment();
            nc = new NodeControl();
        }

        
        #region timer
        private void timerSimulation_Tick(object sender, EventArgs e)
        {
            thinkStopwatch.Reset();
            thinkStopwatch.Start();

            double tickLength = 1.0d / _temp_stepsPerSeconds; //(double)stepsPerSecondSpinEdit.Value;

            if (GlobalTime.Instance.currentTime < _temp_simulationDuration && (GlobalTime.Instance.currentTime + tickLength) >= _temp_simulationDuration)
            {
                //cbEnableSimulation.Checked = false;

                // TODO playButton_click?
                simIsPlaying = false;
                timerSimulation.Enabled = simIsPlaying;
                playButton.Text = "Play";
            }
            //timelineSteuerung.Advance(tickLength);
            GlobalTime.Instance.Advance(tickLength);

            //roadSegment.Tick(tickLength);
            nc.Tick(tickLength);

            ////tickCount++;

            //nodeSteuerung.Tick(tickLength);
            //trafficVolumeSteuerung.Tick(tickLength);

            //nodeSteuerung.Reset();

            thinkStopwatch.Stop();
            //Debug.WriteLine(thinkStopwatch.ElapsedMilliseconds);//GlobalTime.Instance.currentTime);
            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }


        #endregion


        #region UI event
        private void playButton_Click(object sender, EventArgs e)
        {
            if (!simIsPlaying)
            {
                playButton.Text = "Pause";

            }
            else {
                playButton.Text = "Play";
            }

            simIsPlaying = !simIsPlaying;
            timerSimulation.Enabled = simIsPlaying;
        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            timerSimulation_Tick(sender, e);
            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }

        #endregion


        #region DaGrid
        void DaGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                //ZoomComboBox.SelectedIndex = Math2.Clamp(zoomComboBox.SelectedIndex + (e.Delta / 120), 0, zoomComboBox.Items.Count - 1);

            }
        }


        private void DaGrid_Resize(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion


        #region paint
        void DaGrid_Paint(object sender, PaintEventArgs e)
        {
            //Debug.WriteLine("DaGrid Paint");
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            renderStopwatch.Reset();
            renderStopwatch.Start();

            //roadSegment.Draw(e.Graphics);  
            nc.Draw(e.Graphics);          
            

            renderStopwatch.Stop();


            e.Graphics.Transform = new Matrix(1, 0, 0, 1, 0, 0);
            e.Graphics.DrawString(
                "thinking time: " + thinkStopwatch.ElapsedMilliseconds + "ms, possible thoughts per second: " + ((thinkStopwatch.ElapsedMilliseconds != 0) ? (1000 / thinkStopwatch.ElapsedMilliseconds).ToString() : "-"),
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                8,
                40);

            e.Graphics.DrawString(
                "rendering time: " + renderStopwatch.ElapsedMilliseconds + "ms, possible fps: " + ((renderStopwatch.ElapsedMilliseconds != 0) ? (1000 / renderStopwatch.ElapsedMilliseconds).ToString() : "-"),
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                8,
                56);
        }


        private void Invalidate(InvalidationLevel il)
        {
            base.Invalidate();
            switch (il)
            {
                case InvalidationLevel.ALL:
                    //thumbGrid.Invalidate();
                    DaGrid.Invalidate();
                    break;
				case InvalidationLevel.MAIN_CANVAS_AND_TIMELINE:
					DaGrid.Invalidate();
                    break;
				case InvalidationLevel.ONLY_MAIN_CANVAS:
					DaGrid.Invalidate();
                    break;
                default:
					break;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion

        Random rnd = new Random();
        
        private void tempLoadButton_Click(object sender, EventArgs e)
        {
            //roadSegment = new RoadSegment();
            ////nodeControl.tempLoad();
            //int roadLength = 200;
            ////int zoomFactor = 4;
            //int speedInit = 15;


            ////roadSegment.StartPoint = new Vector2(100, 250);
            ////roadSegment.EndPoint = new Vector2(800, 250);

            //roadSegment.StartPoint = new Vector2(rnd.Next(0, 800), rnd.Next(100, 450));
            //roadSegment.EndPoint = new Vector2(rnd.Next(0, 800), rnd.Next(100, 450));
            //while (Vector2.GetDistance(roadSegment.StartPoint, roadSegment.EndPoint) < 500)
            //{
            //    roadSegment.StartPoint = new Vector2(rnd.Next(0, 800), rnd.Next(100, 450));
            //    roadSegment.EndPoint = new Vector2(rnd.Next(0, 800), rnd.Next(100, 450));
            //}
            //roadSegment.tempGenerateVeh(8);

            nc.Load();

            Invalidate(InvalidationLevel.MAIN_CANVAS_AND_TIMELINE);
        }
    }
}
