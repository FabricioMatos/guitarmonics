using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Guitarmonics.Importer
{
    public class GP4Reader
    {
        public long FileStreamLength { get; private set; }
        public int currentByte { get; private set; }
        public StreamWriter AnnotatedOutput;
        private byte[] bytes;

        public void ReadAllBytes(string sFilePath)
        {
            var fileStream = File.OpenRead(sFilePath);
            FileStreamLength = fileStream.Length;
            bytes = new byte[FileStreamLength];
            fileStream.Read(bytes, 0, (int)FileStreamLength);

            AnnotatedOutput = new StreamWriter(sFilePath + ".annotated.txt");

            //AnnotatedOutput.Close();
        }

        public byte readByte(string extraComment)
        {
            if (currentByte >= FileStreamLength)
            {
                return 0;
            }
            byte b = bytes[currentByte];
            AnnotatedOutput.Write(
                "\n" +
                b.ToString("00000") +
                "|" +
                (char)b +
                "|" +
                FlagMap(b) +
                "|" +
                extraComment
                );

            return bytes[currentByte++];
        }

        public static string FlagMap(byte b)
        {
            var ba = new BitArray(new byte[] { b });
            string s = "";
            for (int i = 0; i < ba.Count; i++)
            {
                if (ba[i])
                {
                    s = "1" + s;
                }
                else
                {
                    s = "0" + s;
                }
            }
            return s;
        }


        internal void Close()
        {
            AnnotatedOutput.Close();
        }
    }
}
