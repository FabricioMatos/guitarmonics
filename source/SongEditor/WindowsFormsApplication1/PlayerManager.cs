using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guitarmonics.Importer;

namespace ExperimentalSongPlayer
{

    public class PlayerManager
    {
        protected Timer Timer { get; set; }
        protected int CurrentMeasure { get; set; }
        public List<Panel> MeasurePanels { get; protected set; }
        public Panel CurrentMeasurePanel
        {
            get
            {
                return MeasurePanels[CurrentMeasure - 1];
            }
        }

        public void Step()
        {
            if (CurrentMeasure + 1 > MeasurePanels.Count)
            {
                Stop();
                return;
            }
            CurrentMeasure++;
        }

        public PlayerManager(Timer oTimer)
        {
            Timer = oTimer;
            MeasurePanels = new List<Panel>();
        }

        public PlayerManager()
        {
            MeasurePanels = new List<Panel>();
        }

        public void Start()
        {
            Timer.Enabled = true;
        }

        public void Stop()
        {
            Timer.Enabled = false;
        }
    }
}
