using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Collections;
using Guitarmonics.Importer.Tests;
using Guitarmonics.Importer;
public class TestTablaturesFactoryClass
{


    public static IEnumerable SongDataTestCases
    {
        get
        {

            var newArrayList = new ArrayList();
            {
                var lista = new List<GpFileTestCase>();

                lista.Add(DataForSongTests.AsaBranca());
                lista.Add(DataForSongTests.Stairway());
                lista.Add(DataForSongTests.HeartBreaker());
                lista.Add(DataForSongTests.MeDeixa());
                lista.Add(DataForSongTests.AmericanIdiot());
                lista.Add(DataForSongTests.BasketCase());
                lista.Add(DataForSongTests.SmokeWater());
                lista.Add(DataForSongTests.SmokeWater2());
                lista.Add(DataForSongTests.WhenYouWereYoung());

                foreach (var caso in lista)
                {
                    newArrayList.Add(new TestCaseData(caso));
                }
            }
            return newArrayList;
        }
    }

    public static IEnumerable MeasuresTestCases
    {
        get
        {

            var newArrayList = new ArrayList();
            {
                var lista = new List<GpFileTestCase>();

                lista.Add(DataForSongTests.AsaBranca());
                lista.Add(DataForSongTests.Stairway());
                lista.Add(DataForSongTests.HeartBreaker());
                lista.Add(DataForSongTests.MeDeixa());
                lista.Add(DataForSongTests.AmericanIdiot());
                lista.Add(DataForSongTests.BasketCase());
                lista.Add(DataForSongTests.SmokeWater());
                lista.Add(DataForSongTests.SmokeWater2());
                lista.Add(DataForSongTests.WhenYouWereYoung());

                foreach (var caso in lista)
                {
                    newArrayList.Add(new TestCaseData(caso));
                }
            }
            return newArrayList;
        }
    }

    public static IEnumerable TracksTestCases
    {
        get
        {

            var newArrayList = new ArrayList();
            {
                var lista = new List<GpFileTestCase>();

                lista.Add(DataForSongTests.AsaBranca());
                lista.Add(DataForSongTests.BasketCase());
                lista.Add(DataForSongTests.MeDeixa());
                lista.Add(DataForSongTests.AmericanIdiot());
                lista.Add(DataForSongTests.HeartBreaker());
                lista.Add(DataForSongTests.SmokeWater());
                lista.Add(DataForSongTests.Stairway());
                lista.Add(DataForSongTests.WhenYouWereYoung());

                foreach (var caso in lista)
                {
                    newArrayList.Add(new TestCaseData(caso));
                }
                newArrayList.Add(new TestCaseData(DataForSongTests.GoodRidance()));
            }
            return newArrayList;
        }
    }

    public static IEnumerable MeasureTrackPairsTestCases
    {
        get
        {

            var newArrayList = new ArrayList();
            {
                var lista = new List<GpFileTestCase>();
                {
                    OneTestCaseForEachMeasure(newArrayList, DataForSongTests.AsaBranca());
                    if (false)
                    {
                        OneTestCaseForEachMeasure(newArrayList, DataForSongTests.WhenYouWereYoung());
                    }
                }
            }
            return newArrayList;
        }
    }

    private static void OneTestCaseForEachMeasure(ArrayList newArrayList, GpFileTestCase abTemplate)
    {
        new Importer().Load(ImportTests2.BASE_PATH + @"\" + abTemplate.FileTried, abTemplate.ReadSong);
        if (true)
        {
            Console.WriteLine("-------------------------" + abTemplate.ReadSong.Name);
            for (int m = 0; m < abTemplate.ReadSong.Measures.Count; m++)
            {
                for (int t = 0; t < abTemplate.ReadSong.Tracks.Count; t++)
                {
                    var rPair = abTemplate.ReadSong.Measures[m].Pairs[t];
                    if (rPair.Beats.Count > 0)
                    {
                        Console.WriteLine("{");
                        Console.WriteLine("var beats = song.Measures[" + m + "].Pairs[" + t + "].Beats; ");
                        for (int b = 0; b < rPair.Beats.Count; b++)
                        {
                            Console.WriteLine("beats.Add(new Beat());");
                        }
                        Console.WriteLine("}");
                    }
                }
            }
        }

        for (int m = 0; m < abTemplate.ExpectedSong.Measures.Count; m++)
        {
            var ab = new GpFileTestCase();
            ab.ExpectedSong = abTemplate.ExpectedSong;
            ab.ReadSong = abTemplate.ReadSong;
            ab.MeasureToTest = m;
            ab.PairToTest = 0;
            newArrayList.Add(new TestCaseData(ab));
        }
    }


    public static IEnumerable MidiChannelsTableTestCases
    {
        get
        {

            var newArrayList = new ArrayList();
            {
                var lista = new List<GpFileTestCase>();

                lista.Add(DataForSongTests.BasketCase());

                foreach (var caso in lista)
                {
                    newArrayList.Add(new TestCaseData(caso));
                }
            }
            return newArrayList;
        }
    }




}
