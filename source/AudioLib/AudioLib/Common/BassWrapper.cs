using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace Guitarmonics.AudioLib.Common
{
    public interface IBassWrapper
    {
        bool Initiallized { get; }
        bool BassInit();
        bool BassFree();
        bool RecordFree();
        int RecordStart();
        int ChannelGetData(int pChannel, float[] pFft, int pFftLength);
    }

    public class BassWrapper : IBassWrapper
    {
        private const int DEFAULT_DEVICE = -1;
        private static IBassWrapper fInstance;

        public static IBassWrapper Instance
        {
            get
            {
                if (fInstance == null)
                    fInstance = new BassWrapper();

                return fInstance;
            }
        }

        public bool Initiallized { get; private set; }

        public bool BassInit()
        {
            if (!this.Initiallized)
            {
                this.Initiallized = Bass.BASS_Init(DEFAULT_DEVICE, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            }
            return this.Initiallized;
        }

        public bool BassFree()
        {
            bool result = Bass.BASS_Free();

            this.Initiallized = !result;

            return result;
        }

        public int RecordStart()
        {
            BassInit();

            //TODO: as vezes para de monitorar o audio pois trava aqui
            Bass.BASS_RecordInit(DEFAULT_DEVICE);

            return Bass.BASS_RecordStart(44100, 1, 0, null, IntPtr.Zero);
        }

        public bool RecordFree()
        {
            return Bass.BASS_RecordFree();
        }

        public int ChannelGetData(int pChannel, float[] pFft, int pFftLength)
        {
            return Bass.BASS_ChannelGetData(pChannel, pFft, pFftLength);
        }

    }
}
