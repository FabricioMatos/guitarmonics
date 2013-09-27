using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Collections;
using Guitarmonics.Importer.Tests;
using Guitarmonics.Importer;
using Guitarmonics.SongData;


public class DataForSongTests
{
    public static GpFileTestCase AmericanIdiot()
    {

        var song = new Song
        {
            Name = "American Idiot",
            Interpret = "Green Day",
            Album = "American Idiot",
            Tempo = 98,
        };
        for (int i = 0; i < 49; i++)
        {
            song.Measures.Add(new Measure()
            {
                KeySignatureDenominator = 4,
                KeySignatureNumerator = 4,
                Tonality = 0,
            });
        }
        song.Measures[5].Beginning_of_repeat = true;
        song.Measures[6].EndOfRepeat = 3;

        song.Measures[17].Beginning_of_repeat = true;
        song.Measures[18].EndOfRepeat = 3;

        song.Measures[27].Beginning_of_repeat = true;
        song.Measures[28].EndOfRepeat = 1;

        song.Measures[36].Beginning_of_repeat = true;
        song.Measures[36].EndOfRepeat = 5;

        song.Tracks.Add(new Track()
        {
            Name = "Track 1",
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
        });

        song.Tracks.Add(new Track()
        {
            Name = "Track 2",
            TuningOfStrings = new List<int>() { 43, 38, 33, 28 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
        });

        song.Tracks.Add(new Track()
        {
            Name = "Percussion",
            TuningOfStrings = new List<int>() { 0, 0, 0, 0, 0, 0 },
            NumberOfFrets = 87,
            HeigthOfCapo = 0,
        });


        for (int i = 0; i < 2; i++)
        {
            song.oMidiChannelsTable.Ports[0].Channels[i].Instrument = 30;
            song.oMidiChannelsTable.Ports[0].Channels[i].Volume = 14;
            song.oMidiChannelsTable.Ports[0].Channels[i].Balance = 8;
        }

        foreach (var measure in song.Measures)
        {
            for (int i = 0; i < song.Tracks.Count; i++)
            {
                measure.Pairs.Add(new MeasureTrackPair());
                measure.Pairs[i].Beats.Add(new Beat()
                    {
                        Duration = 2,
                    });
            }
        }



        return new GpFileTestCase
        {
            FileTried = "green-day--american-idiot--guitarpro.gp4",
            ExpectedSong = song,
        };
    }

    public static GpFileTestCase BasketCase()
    {
        var basketCase = new Song
        {
            Name = "Basket Case",
            Interpret = "Green Day",
            Album = "Dookie",
            Author = "fwe4life",
            Copyright = "None, go right ahead and steal it :)",
            NoticeLines = new List<string> 
                { 
                    "I fixed the Bass Line, and re-submitted in place of " ,
                    "orcfeind's, because he just copied the bass from ", 
                    "another tab. "
                },
            Tempo = 175,
        };
        for (int i = 0; i < 127; i++)
        {
            basketCase.Measures.Add(new Measure()
            {
                KeySignatureNumerator = 4,
                KeySignatureDenominator = 4,
                Tonality = (int)Keys.C,
            });
        }
        basketCase.Measures[0].MarkerName = "Intro - Verse 1";
        basketCase.Measures[0].MarkerColor = 0;

        basketCase.Measures[17].MarkerName = "Chorus";
        basketCase.Measures[17].MarkerColor = 0;

        basketCase.Measures[27].MarkerName = "Interlude";
        basketCase.Measures[27].MarkerColor = 0;

        basketCase.Measures[31].MarkerName = "Bridge";
        basketCase.Measures[31].MarkerColor = 0;

        basketCase.Measures[35].MarkerName = "Verse 2";
        basketCase.Measures[35].MarkerColor = 0;

        basketCase.Measures[51].MarkerName = "Chorus";
        basketCase.Measures[51].MarkerColor = 0;

        basketCase.Measures[61].MarkerName = "Interlude";
        basketCase.Measures[61].MarkerColor = 0;

        basketCase.Measures[65].MarkerName = "Bridge";
        basketCase.Measures[65].MarkerColor = 0;

        basketCase.Tracks.Add(new Track()
        {
            Name = "Billie Joe Armstrong - Lead Guitar",
            TuningOfStrings = new List<int>() { 63, 58, 54, 49, 44, 39 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            Color = 188,
        });
        basketCase.Tracks.Add(new Track()
        {
            Name = "Tre Cool - Percussion",
            TuningOfStrings = new List<int>() { 0, 0, 0, 0, 0, 0 },
            NumberOfFrets = 87,
            HeigthOfCapo = 0,
            Color = 154,
        });
        basketCase.Tracks.Add(new Track()
        {
            Name = "Billie Joe Armstrong - Vox",
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            Color = 103,
        });
        basketCase.Tracks.Add(new Track()
        {
            Name = "Mike Dirnt - Bass",
            TuningOfStrings = new List<int>() { 42, 37, 32, 27 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            Color = 60,
        });

        #region midichannels
        {
            var port = basketCase.oMidiChannelsTable.Ports[0];
            port.Channels[0] = new MidiChannel()
            {
                Instrument = 30,
                Volume = 14,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[1] = new MidiChannel()
            {
                Instrument = 30,
                Volume = 14,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[2] = new MidiChannel()
            {
                Instrument = 34,
                Volume = 16,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[3] = new MidiChannel()
            {
                Instrument = 34,
                Volume = 16,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[4] = new MidiChannel()
            {
                Instrument = 30,
                Volume = 12,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[5] = new MidiChannel()
            {
                Instrument = 30,
                Volume = 12,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[6] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[7] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[8] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[9] = new MidiChannel()
            {
                Instrument = 0,
                Volume = 14,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[10] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[11] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[12] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[13] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[14] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

            port.Channels[15] = new MidiChannel()
            {
                Instrument = 24,
                Volume = 13,
                Balance = 8,
                Chorus = 0,
                Reverb = 0,
                Phaser = 0,
                Tremolo = 0
            };

        }
        {
            var port = basketCase.oMidiChannelsTable.Ports[1];
            port.Channels[0
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[1
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[2
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[3
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[4
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[5
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[6
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[7
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[8
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[9
            ] = new MidiChannel() { Instrument = 0, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[10
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[11
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[12
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[13
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[14
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[15
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

        }
        {
            var port = basketCase.oMidiChannelsTable.Ports[2];
            port.Channels[0
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[1
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[2
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[3
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[4
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[5
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[6
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[7
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[8
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[9
            ] = new MidiChannel() { Instrument = 0, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[10
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[11
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[12
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[13
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[14
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[15
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

        }
        {
            var port = basketCase.oMidiChannelsTable.Ports[3];
            port.Channels[0
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[1
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[2
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[3
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[4
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[5
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[6
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[7
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[8
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[9
            ] = new MidiChannel() { Instrument = 0, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[10
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[11
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[12
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[13
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[14
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

            port.Channels[15
            ] = new MidiChannel() { Instrument = 24, Volume = 13, Balance = 8, Chorus = 0, Reverb = 0, Phaser = 0, Tremolo = 0 };

        }
        #endregion

        return new GpFileTestCase
        {
            FileTried = "green-day--basket-case--guitarpro.gp4",
            ExpectedSong = basketCase,
        };
    }
    public static GpFileTestCase GoodRidance()
    {
        var song = new Song();

        song.Tracks.Add(new Track()
        {
            Name = "Guitar",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Voice",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Violin",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Violin",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Violin",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Violin",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        return new GpFileTestCase
        {
            FileTried = "green-day--time-of-your-life-good-ridance--guitarpro.gp4",
            ExpectedSong = song,
        };
    }
    public static GpFileTestCase Stairway()
    {

        var song = new Song
        {
            Name = "Stairway To Heaven",
            Interpret = "Led Zepplin",
            Album = "Led Zepplin IV",
            Author = "Jimmy Page & Robert Plant",
            Tempo = 105,
            TablatureAuthor = "NamoRXelAD@one.lv"
        };

        for (int i = 0; i < 166; i++)
        {
            song.Measures.Add(new Measure()
            {
                KeySignatureNumerator = 4,
                KeySignatureDenominator = 4,
                Tonality = 0,
            }
                );
        }
        song.Measures[111].KeySignatureNumerator = 3;
        song.Measures[113].KeySignatureNumerator = 5;

        song.Measures[115].KeySignatureNumerator = 7;
        song.Measures[115].KeySignatureDenominator = 8;

        song.Tracks.Add(new Track()
        {
            Name = "Guitare 1",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Guitare 2 (12 cordes)",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Arrangement flutes",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Guitare électrique",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Guitare solo",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Guitare slide",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Basse",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 43, 38, 33, 28 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Percussions",
            NumberOfFrets = 99,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 0, 0, 0, 0, 0, 0 },
        });


        return new GpFileTestCase
        {
            FileTried = "led_zeppelin_stairway_to_heaven.gp4",
            ExpectedSong = song,
        };
    }

    public static GpFileTestCase AsaBranca()
    {
        GpFileTestCase asa = new GpFileTestCase
        {
            FileTried = "luiz-gonzaga--asa-branca--guitarpro.gp4",
            ExpectedSong = AsaBrancaSong()
        };
        return asa;
    }

    private static Song AsaBrancaSong()
    {
        var song = new Song
        {
            Name = "Asa Branca",
            Interpret = "Luiz Gonzaga",
            Author = "Marco Antero",
            Tempo = 147
        };

        song.Tracks.Add(new Track()
        {
            Name = "Trilha 1",
            NumberOfFrets = 24,
            Color = 255,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        for (int i = 0; i < 10; i++)
        {
            song.Measures.Add(new Measure()
            {
                KeySignatureNumerator = 4,
                KeySignatureDenominator = 4,
                Tonality = 0,
            }
                );
        }

        song.Measures[0].Pairs.Add(new MeasureTrackPair()
        {
            Beats = new List<Beat>()
            {
                BeatFromAsaBranca(3, 2),
                BeatFromAsaBranca(0, 3),
                BeatFromAsaBranca(2, 3),
                BeatFromAsaBranca(0, 4),                
            },
        });
        song.Measures[1].Pairs.Add(new MeasureTrackPair()
        {
            Beats = new List<Beat>()
            {
                BeatFromAsaBranca(0, 4),
                BeatFromAsaBranca(2, 3),
                BeatFromAsaBranca(3, 3),
                BeatFromAsaBranca(3, 3),                
            },
        });
        {
            Beat b1 = new Beat()
            {
                Duration = 0,
                Status = 2,
            };
            song.Measures[2].Pairs.Add(new MeasureTrackPair()
            {
                Beats = new List<Beat>()
            {
                b1,
                BeatFromAsaBranca(3, 2),
                BeatFromAsaBranca(0, 3),
                BeatFromAsaBranca(2, 3),                
            },
            });
        }
        song.Measures[3].Pairs.Add(new MeasureTrackPair()
        {
            Beats = new List<Beat>()
            {
                BeatFromAsaBranca(0, 4),
                BeatFromAsaBranca(0, 4),
                BeatFromAsaBranca(2, 3),
                BeatFromAsaBranca(3, 3),                
            },
        });
        {
            Beat b1 = new Beat()
            {
                Duration = 0,
                Status = 2,
            };

            song.Measures[4].Pairs.Add(new MeasureTrackPair()
            {
                Beats = new List<Beat>()
            {
                b1,                
                BeatFromAsaBranca(3, 2),
                BeatFromAsaBranca(3, 2),
                BeatFromAsaBranca(0, 3),                
            },
            });
        }
        {
            Beat b1 = new Beat()
            {
                Duration = 0,
                Status = 2,
            };
            song.Measures[5].Pairs.Add(new MeasureTrackPair()
            {
                Beats = new List<Beat>()
            {
                BeatFromAsaBranca(2, 3),
                BeatFromAsaBranca(0, 4),
                b1,
                BeatFromAsaBranca(0, 4),                
            },
            });
        }
        {
            song.Measures[6].Pairs.Add(new MeasureTrackPair()
            {
                Beats = new List<Beat>()
            {
                BeatFromAsaBranca(3, 3),
                BeatFromAsaBranca(2, 3),
                BeatFromAsaBranca(3, 3),
                BeatFromAsaBranca(3, 3),
            },
            });
        }
        {
            Beat b1 = new Beat()
            {
                Duration = 0,
                Status = 2,
            };
            song.Measures[7].Pairs.Add(new MeasureTrackPair()
            {
                Beats = new List<Beat>()
            {
                b1,
                BeatFromAsaBranca(2, 3),
                BeatFromAsaBranca(2, 3),
                BeatFromAsaBranca(0, 3),
            },
            });
        }
        {
            Beat b1 = new Beat()
            {
                Duration = 0,
                Status = 2,
            };
            song.Measures[8].Pairs.Add(new MeasureTrackPair()
            {
                Beats = new List<Beat>()
            {
                BeatFromAsaBranca(0, 3),
                BeatFromAsaBranca(2, 3),
                b1,
                BeatFromAsaBranca(2, 3),
            },
            });
        }
        {
            Beat b1 = new Beat()
            {
                Duration = 0,
                Status = 2,
            };
            song.Measures[9].Pairs.Add(new MeasureTrackPair()
            {
                Beats = new List<Beat>()
            {
                BeatFromAsaBranca(0, 3),
                BeatFromAsaBranca(3, 2),
                BeatFromAsaBranca(3, 2),
            },
            });
        }
        return song;
    }

    private static Beat BeatFromAsaBranca(byte fret, byte stringPlayed)
    {

        Beat b = new Beat()
        {
            Duration = 0,
        };
        b.Notes.Add(new NoteClass()
        {
            NoteDynamic = 7,
            Fret = fret,
            StringPlayed = stringPlayed,
            Type = 1,
        });
        return b;
    }
    public static GpFileTestCase MeDeixa()
    {
        var song = new Song
        {
            Name = "Me Deixa",
            Interpret = "O Rappa",
            Tempo = 100,
        };

        song.Tracks.Add(new Track()
        {
            Name = "Guitarra",
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            Color = 255,
        });
        song.Tracks.Add(new Track()
        {
            Name = "Baixo",
            TuningOfStrings = new List<int>() { 43, 38, 33, 28 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,

        });
        song.Tracks.Add(new Track()
        {
            Name = "Guitarra 2",
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            Color = 255,
        });



        for (int i = 0; i < 18; i++)
        {
            song.Measures.Add(new Measure()
            {
                KeySignatureNumerator = 4,
                KeySignatureDenominator = 4,
                Tonality = 0,
            });
        }
        {
            song.Measures[0].KeySignatureNumerator = 2;

            song.Measures[1].KeySignatureNumerator = 2;

            song.Measures[2].KeySignatureNumerator = 2;

            song.Measures[3].KeySignatureNumerator = 2;
            song.Measures[3].Beginning_of_repeat = true;

            song.Measures[4].KeySignatureNumerator = 2;

            song.Measures[5].KeySignatureNumerator = 2;

            song.Measures[6].KeySignatureNumerator = 2;
            song.Measures[6].EndOfRepeat = 1;

            song.Measures[7].KeySignatureNumerator = 2;
            song.Measures[7].Beginning_of_repeat = true;

            song.Measures[8].KeySignatureNumerator = 2;

            song.Measures[9].KeySignatureNumerator = 2;

            song.Measures[10].KeySignatureNumerator = 2;

            song.Measures[11].KeySignatureNumerator = 2;

            song.Measures[12].KeySignatureNumerator = 2;

            song.Measures[13].KeySignatureNumerator = 2;

            song.Measures[14].KeySignatureNumerator = 2;
            song.Measures[14].EndOfRepeat = 5;

            song.Measures[15].KeySignatureNumerator = 2;
            song.Measures[15].Beginning_of_repeat = true;

            song.Measures[16].KeySignatureNumerator = 2;

            song.Measures[17].KeySignatureNumerator = 2;
        }


        return new GpFileTestCase
        {
            FileTried = "o-rappa--me-deixa--guitarpro.gp4",
            ExpectedSong = song,
        };
    }

    public static GpFileTestCase WhenYouWereYoung()
    {
        var song = new Song
        {
            Name = "When You Were Young",
            Interpret = "The Killers",
            Album = "Sam's town",
            Tempo = 130,
        };
        song.Tracks.Add(new Track()
        {
            Name = "Guitarmonics",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Rhythm Guitar",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 38 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Lead Guitar",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Bass",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 43, 38, 33, 28 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Drums",
            NumberOfFrets = 87,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 0, 0, 0, 0, 0, 0 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Strings",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Synth",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Bells",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Synth 2 (bridge)",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });


        for (int i = 0; i < 115; i++)
        {
            song.Measures.Add(new Measure()
            {
                KeySignatureNumerator = 4,
                KeySignatureDenominator = 4,
                Tonality = 0,
            });
            foreach (var track in song.Tracks)
            {
                song.Measures[i].Pairs.Add(new MeasureTrackPair());
            }
        }


        return new GpFileTestCase
        {
            FileTried = "The Killers - When You Were Young.gp4",
            ExpectedSong = song,
        };
    }

    public static GpFileTestCase HeartBreaker()
    {
        var song = new Song
        {

            Name = "Heartbreaker",
            Interpret = "Led Zeppelin",
            Album = "Led Zeppelin II",
            Author = "Jimmy Page and Robert Plant",
            TablatureAuthor = "adagio",
            NoticeLines = new List<string> 
                { 
                    "keep a eye on the rhytm" 
                },
            Tempo = 100,
        };

        for (int i = 0; i < 108; i++)
        {

            song.Measures.Add(new Measure()
            {
                KeySignatureNumerator = 4,
                KeySignatureDenominator = 4,
                Tonality = 0,
            }
                    );
        }
        song.Measures[0].KeySignatureNumerator = 3;
        song.Measures[1].KeySignatureNumerator = 3;
        song.Measures[2].KeySignatureNumerator = 2;
        song.Measures[4].Beginning_of_repeat = true;

        song.Measures[20].KeySignatureNumerator = 3;
        song.Measures[21].KeySignatureNumerator = 2;
        song.Measures[21].EndOfRepeat = 1;

        song.Measures[34].KeySignatureNumerator = 3;

        song.Measures[37].KeySignatureNumerator = 3;

        song.Measures[38].KeySignatureNumerator = 6;
        song.Measures[39].KeySignatureNumerator = 3;

        song.Measures[40].KeySignatureNumerator = 9;
        song.Measures[40].KeySignatureDenominator = 8;

        song.Measures[49].KeySignatureNumerator = 14;
        song.Measures[49].KeySignatureDenominator = 16;

        song.Measures[50].KeySignatureNumerator = 2;

        song.Measures[61].Beginning_of_repeat = true;
        song.Measures[64].EndOfRepeat = 1;


        song.Measures[101].KeySignatureNumerator = 3;
        song.Measures[103].KeySignatureNumerator = 3;
        song.Measures[104].KeySignatureNumerator = 2;
        song.Measures[105].KeySignatureNumerator = 3;
        song.Measures[106].KeySignatureNumerator = 2;


        song.Tracks.Add(new Track()
        {
            Name = "Guitar 1",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Guitar 2",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Guitar 3",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Vocal",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });



        return new GpFileTestCase
        {

            FileTried = "led-zeppelin--heartbreaker--guitarpro.gp4",
            ExpectedSong = song
        };
    }

    private static GpFileTestCase DeepPurple(string fileTried, bool expectedTripletFeelActivated)
    {
        var song = new Song()
        {
            Name = "Smoke On The Water",
            Album = "Machine Head",
            Interpret = "Deep Purple",
            Author = "Words & Music by Ian Gillan (vocals), Ritchie Blackmore (guitars), Roger Glover (bass), Jon Lord (rock organ),  and Ian Paice (drums).",
            Copyright = "B. Feldman & Company Limited trading as Hec Music",
            NoticeLines = new List<string>()
            {
                "Please rate this tab on MSB, 'cause i've \"wasted\" a lot ", 
                "of time doing this one. ",
                "", 
                "Intro's riff is one of the best rock riffs ever made.Of ", 
                "course, Zep riffs are amazing as well. Solo is ",
                "great, too. Deep Purple's best song, maybe. Well, I  ", 
                "don't know. Highway Star is a great one, too.      ", 
                "Please send comments to ",
                 "<mikko.ruuskanen@mbnet.fi>"
            },
            TablatureAuthor = "pulpman",
            TripletFeelActivated = expectedTripletFeelActivated,
            Tempo = 112,
            Key = 254,
            Octave = 255,
        };

        for (int i = 0; i < 29; i++)
        {
            song.Measures.Add(new Measure()
            {
                KeySignatureDenominator = 4,
                KeySignatureNumerator = 4,
                Tonality = (int)Keys.B_FLAT,
            });
        }

        song.Measures[0].MarkerName = "Intro";
        song.Measures[0].MarkerColor = 255;
        song.Measures[0].Beginning_of_repeat = true;

        song.Measures[4].EndOfRepeat = 1;

        song.Measures[5].MarkerName = "Guitar Solo";
        song.Measures[5].MarkerColor = 255;

        song.Tracks.Add(new Track()
        {
            Name = "Riff ",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Solo",
            NumberOfFrets = 24,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 64, 59, 55, 50, 45, 40 },
        });

        song.Tracks.Add(new Track()
        {
            Name = "Ian Paice",
            NumberOfFrets = 87,
            HeigthOfCapo = 0,
            TuningOfStrings = new List<int>() { 0, 0, 0, 0, 0, 0 },
        });


        return new GpFileTestCase()
        {
            FileTried = fileTried,
            ExpectedSong = song,
        };
    }


    public static GpFileTestCase SmokeWater()
    {
        return DeepPurple("deep-purple--smoke-on-the-water--guitarpro.gp4", false);
    }
    public static GpFileTestCase SmokeWater2()
    {
        return DeepPurple("changed tripletfeel - deep-purple--smoke-on-the-water--guitarpro.gp4", true);
    }
}
