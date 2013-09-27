using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Guitarmonics.Importer.Tests;
using Guitarmonics.Importer;
using Guitarmonics.SongData;


public class GpFileTestCase
{
    public string FileTried;
    public Song ExpectedSong;
    public Song ReadSong;
    public int? PairToTest;
    public int? MeasureToTest;

    public GpFileTestCase()
    {
        ReadSong = new Song();
    }
    public override string ToString()
    {        
        if (null != PairToTest)
        {
            return new
            {
                FileTried = FileTried,
                SongName = ExpectedSong.Name,
                Subtitle = ExpectedSong.Subtitle,
                MeasureToTest = MeasureToTest,
                PairToTest = PairToTest,
            }.ToString();
        }
        return new
        {
            FileTried = FileTried,
            SongName = ExpectedSong.Name,
            Subtitle = ExpectedSong.Subtitle,
        }.ToString();
    }
}
