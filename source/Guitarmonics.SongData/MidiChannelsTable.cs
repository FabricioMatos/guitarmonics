using System.IO;
using System;
using System.Collections.Generic;

namespace Guitarmonics.SongData
{
    public class MidiChannel
    {
        public int Instrument { get; set; }
        public int Volume { get; set; }
        public int Balance { get; set; }
        public int Chorus { get; set; }
        public int Reverb { get; set; }
        public int Phaser { get; set; }
        public int Tremolo { get; set; }
        public MidiChannel()
        {

        }
    }
    public class MidiChannelPort
    {
        public MidiChannel[] Channels { get; set; }

        public MidiChannelPort()
        {
            Channels = new MidiChannel[16];
            for (int i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new MidiChannel();
            }
        }
    }
    public class MidiChannelsTable
    {
        public MidiChannelPort[] Ports { get; set; }
        public MidiChannelsTable()
        {
            Ports = new MidiChannelPort[4];
            for (int i = 0; i < Ports.Length; i++)
            {
                Ports[i] = new MidiChannelPort();
            }
        }

        public string AsText()
        {
            string s = "";
            for (int p = 0; p < Ports.Length; p++)
            {
                s += "Port: " + p + "\n";
                var channels = Ports[p].Channels;
                for (int ch = 0; ch < channels.Length; ch++)
                {
                    s += "Channel: " + ch + "\n";
                    var channel = channels[ch];
                    s += "Balance:" + channel.Balance + ", ";
                    s += "Chorus:" + channel.Chorus + ", ";
                    s += "Instrument:" + channel.Instrument + ", ";
                    s += "Phaser:" + channel.Phaser + ", ";
                    s += "Reverb:" + channel.Reverb + ", ";
                    s += "Tremolo:" + channel.Tremolo + ", ";
                    s += "Volume:" + channel.Volume + ", ";

                }

            }
            return s;
        }
    }
}
