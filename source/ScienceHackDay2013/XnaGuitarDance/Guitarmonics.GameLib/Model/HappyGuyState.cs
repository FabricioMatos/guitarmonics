using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using System.Diagnostics;

namespace Guitarmonics.GameLib.Model
{
    public enum HeadState
    {
        Front,
        Left,
        Right,
        Up,
        Down
    }

    public enum ArmState
    {
        Relaxed,
        Up1,
        Down1,
        Up2,
        Down2,
        Up3,
        Down3,
    }

    public enum LegState
    {
        Relaxed,
        Up1,
        Down1,
        Up2,
        Down2,
    }


    public class HappyGuyState
    {
        public HeadState HeadState;
        public ArmState LeftArmState;
        public ArmState RightArmState;
        public LegState LeftLegState;
        public LegState RightLegState;

        //NoteValue => Stage => Delegate to config state
        Dictionary<NoteValue, Dictionary<bool, Action>> ConfigTable = new Dictionary<NoteValue, Dictionary<bool, Action>>();

        public HappyGuyState()
        {
            HeadState = HeadState.Front;
            LeftArmState = ArmState.Down1;
            RightArmState = ArmState.Down1;

            SetupConfigTable();
        }

        private void SetupConfigTable()
        {
            ConfigTable.Add(NoteValue.C, new Dictionary<bool, Action>() 
            { 
                { true, () => HeadState = HeadState.Right },
                { false, () => HeadState = HeadState.Left }
            });
            ConfigTable.Add(NoteValue.Db, new Dictionary<bool, Action>() 
            { 
                { true, () => HeadState = HeadState.Up },
                { false, () => HeadState = HeadState.Down }
            });
            ConfigTable.Add(NoteValue.D, new Dictionary<bool, Action>() 
            { 
                { true, () => LeftArmState = ArmState.Down1 },
                { false, () => LeftArmState = ArmState.Up1 }
            });
            ConfigTable.Add(NoteValue.Eb, new Dictionary<bool, Action>() 
            { 
                { true, () => RightArmState = ArmState.Down1 },
                { false, () => RightArmState = ArmState.Up1 }
            });
            ConfigTable.Add(NoteValue.E, new Dictionary<bool, Action>() 
            { 
                { true, () => LeftArmState = ArmState.Down2 },
                { false, () => LeftArmState = ArmState.Up2 }
            });
            ConfigTable.Add(NoteValue.F, new Dictionary<bool, Action>() 
            { 
                { true, () => RightArmState = ArmState.Down2 },
                { false, () => RightArmState = ArmState.Up2 }
            });
            ConfigTable.Add(NoteValue.Gb, new Dictionary<bool, Action>() 
            { 
                { true, () => LeftArmState = ArmState.Down3 },
                { false, () => LeftArmState = ArmState.Up3 }
            });
            ConfigTable.Add(NoteValue.G, new Dictionary<bool, Action>() 
            { 
                { true, () => RightArmState = ArmState.Down3 },
                { false, () => RightArmState = ArmState.Up3 }
            });
            ConfigTable.Add(NoteValue.Ab, new Dictionary<bool, Action>() 
            { 
                { true, () => LeftLegState = LegState.Down1 },
                { false, () => LeftLegState = LegState.Up1 }
            });
            ConfigTable.Add(NoteValue.A, new Dictionary<bool, Action>() 
            { 
                { true, () => RightLegState = LegState.Down1 },
                { false, () => RightLegState = LegState.Up1 }
            });
            ConfigTable.Add(NoteValue.Bb, new Dictionary<bool, Action>() 
            { 
                { true, () => LeftLegState = LegState.Down2 },
                { false, () => LeftLegState = LegState.Up2 }
            });
            ConfigTable.Add(NoteValue.B, new Dictionary<bool, Action>() 
            { 
                { true, () => RightLegState = LegState.Down2 },
                { false, () => RightLegState = LegState.Up2 }
            });
        }


        internal void UpdateState(bool step, List<IMusicalNote> playingNotes)
        {

            //Default values
            HeadState = HeadState.Front;
            LeftArmState = ArmState.Down1;
            RightArmState = ArmState.Down1;
            LeftLegState = LegState.Down1;
            RightLegState = LegState.Down1;

            foreach (var note in playingNotes)
            {
                ConfigTable[note.Value][step]();
            }
        }


        private void PrintPlayingNotesForDebug(List<IMusicalNote> playingNotes)
        {
            var notes = "";
            foreach (var n in playingNotes)
            {
                notes += n.ToString() + " ";
            }

            Debug.WriteLine(notes);
        }

    }
}
