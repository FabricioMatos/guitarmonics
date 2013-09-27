using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Player;

namespace Guitarmonics.GameLib.Model
{
    /// <summary>
    /// Store general information of one available song
    /// </summary>
    public class SongDescription
    {
        public string Id { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Song { get; set; }
        public float Pitch { get; set; }

        public Guid OidEasyTablature { get; set; }
        public Guid OidMediumTablature { get; set; }
        public Guid OidHardTablature { get; set; }
        public Guid OidExpertTablature { get; set; }

        public string ConfigFileName { get; set; }
        public string SyncFileName { get; set; }
        public string AudioFileName { get; set; }

        //Not used anymore:
        //public double TempoBPM { get; set; }
        public GtTimeSignature TimeSignature { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Song, this.Artist);
        }
    }
}
