// Stephen Toub
// stoub@microsoft.com
//
// MidiPlayer.cs
// Class to play MIDI events/tracks/sequence/files.

#region Namespaces
using System;
using System.IO;
#endregion

namespace Toub.Sound.Midi
{
	/// <summary>Plays MIDI files and messages.</summary>
	public sealed class MidiPlayer
	{
		#region Member Variables
		/// <summary>Handle to the open MIDI device.</summary>
		private static MidiInterop.MidiDeviceHandle _handle;
		/// <summary>The number of references to the currently open MIDI device.</summary>
		private static int _references = 0;
		/// <summary>Used for synchronization of all MIDI-related operations.</summary>
		private static object _midiLock = new object();
		#endregion

		#region Construction
		/// <summary>Prevent external instantiation.</summary>
		private MidiPlayer() {}
		#endregion

		#region Opening and Closing
		/// <summary>Open the default MIDI device.</summary>
		/// <remarks>This is necessary only when playing individual events.</remarks>
		public static void OpenMidi()
		{
			lock(_midiLock)
			{
				// Open the MIDI device if it hasn't already been opened.
				if (_references == 0) InternalOpenMidi();
				_references++;
			}
		}

		/// <summary>Close the default MIDI device.</summary>
		public static void CloseMidi()
		{
			lock(_midiLock) 
			{
				// Close the MIDI device if no one else is using it
				if (_references == 0) return;
				_references--;
				if (_references == 0) InternalCloseMidi();
			}
		}

		/// <summary>Opens the MIDI device without regard for whether it has already been opened.</summary>
		private static void InternalOpenMidi()
		{
			// Open the default MIDI device
			_handle = MidiInterop.OpenMidiOut();
		}

		/// <summary>Closes the MIDI device without regard for the reference count.</summary>
		private static void InternalCloseMidi()
		{
			// Close the MIDI device if it is open
			if (_handle != null) 
			{
				((IDisposable)_handle).Dispose();
				_handle = null;
			}
		}
		#endregion

		#region Playing Midi
		/// <summary>Plays the specified MIDI sequence using Media Control Interface (MCI).</summary>
		/// <param name="sequence">The MIDI sequence to be played.</param>
		public static void Play(MidiSequence sequence)
		{
			// Validate the parameter
			if (sequence == null) throw new ArgumentNullException("sequence");

			// Save the MIDI sequence to a temporary file
			string fileName;
			try 
			{
				fileName = Path.GetTempFileName();
				sequence.Save(fileName);
			} 
			catch(Exception exc)
			{
				throw new ApplicationException(
					"Unable to save the sequence to a temporary file.  " + exc.Message, exc);
			}

			// Play it from the temporary file
			Play(fileName);

			// Remove the temporary file
			try { File.Delete(fileName); }
			catch{}
		}

		/// <summary>Plays an individual MIDI track.</summary>
		/// <param name="track">The track to be played.</param>
		/// <param name="division">The MIDI division to use for playing the track.</param>
		public static void Play(MidiTrack track, int division)
		{
			// Wrap the track in a sequence and play it
			MidiSequence tempSequence = new MidiSequence(0, division);
			tempSequence.AddTrack(track);
			Play(tempSequence);
		}

		/// <summary>Plays a collection of MIDI events.</summary>
		/// <param name="events">The events to be played.</param>
		/// <param name="division">The division to use for playing the events.</param>
		public static void Play(MidiEventCollection events, int division)
		{
			// Add all of the events to a temporary track and sequence, then play it
			MidiSequence tempSequence = new MidiSequence(0, division);
			MidiTrack tempTrack = tempSequence.AddTrack();
			tempTrack.Events.Add(events);
			if (!tempTrack.HasEndOfTrack) tempTrack.Events.Add(new EndOfTrack(0));
			Play(tempSequence);
		}

		/// <summary>Plays the specified MIDI file using Media Control Interface (MCI).</summary>
		/// <param name="path">The MIDI file to be played.</param>
		public static void Play(string path)
		{
			// Validate the parameter; make sure the file actually exists
			if (path == null) throw new ArgumentNullException("path");
			if (!File.Exists(path)) throw new FileNotFoundException("The MIDI file was not found.", path);

			// Play the file using interop calls: open the file, play it (wait for it to finish), close it
			string alias = Guid.NewGuid().ToString("N"); // randomly generated alias to avoid collisions
			lock(_midiLock) 
			{
				// We can't play using MCI if we already have an open handle to the default
				// MIDI device.  As such, we'll temporarily close it if its open and then
				// when we're done reopen it if it was open.
				bool wasOpen = _handle != null && _handle.IsOpen;
				if (wasOpen) InternalCloseMidi();

				// Open the file, play it, close it
				MidiInterop.MciSendString("open \"" + path + "\" type mpegvideo alias " + alias);
				MidiInterop.MciSendString("play " + alias + " wait");
				MidiInterop.MciSendString("close " + alias);

				// Reopen the MIDI device if it was previously open
				if (wasOpen) InternalOpenMidi();
			}
		}

		/// <summary>Plays an individual MIDI event.</summary>
		/// <param name="ev">The event to be played.</param>
		/// <remarks>
		/// Only VoiceMidiEvent's are actually sent to the MIDI device.
		/// Delta-times are ignored.
		/// OpenMidi must be called before calling Play.  CloseMidi should
		/// be called once all events have been played to free up the device.
		/// </remarks>
		public static void Play(MidiEvent ev)
		{
			lock(_midiLock)
			{
				// Only send voice messages
				if (ev is VoiceMidiEvent)
				{
					// Send the MIDI event to the MIDI device
					MidiInterop.SendMidiMessage(_handle, ((VoiceMidiEvent)ev).Message);
				}
			}
		}
		#endregion
	}
}
