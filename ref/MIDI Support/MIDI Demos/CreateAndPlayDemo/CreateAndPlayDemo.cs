// Stephen Toub
// stoub@microsoft.com
//
// CreateAndPlayDemo
// Demo program that builds a simple midi file and plays it.
//
// TO USE:
// 1. Run the application.  A midi sequence will be generated and played.

using System;
using System.IO;
using System.Threading;
using Toub.Sound.Midi;

namespace Toub.Demos
{
	/// <summary>Demo program that builds a simple midi file and plays it.</summary>
	class CreateAndPlayDemo
	{
		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string [] args)
		{
			MidiSequence sampleSequence = new CreateAndPlayDemo().CreateSequence();
            sampleSequence = MidiSequence.Import("C:/Download/test.mid");
			MidiPlayer.Play(sampleSequence);
		}

		/// <summary>Create the demo sequence.</summary>
		/// <returns>The created midi sequence.</returns>
		public MidiSequence CreateSequence() 
		{
			MidiSequence sequence = new MidiSequence(0, 120);
			MidiTrack track = sequence.AddTrack();
			track.Events.Add(new TimeSignature(0, 4, 2, 24, 8));
			track.Events.Add(new KeySignature(0, Key.NoFlatsOrSharps, Tonality.Major));
			track.Events.Add(new Tempo(0, 416667));
			track.Events.Add(new ProgramChange(0, 0, GeneralMidiInstruments.AcousticGrand));
			track.Events.Add(new Controller(0, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new Controller(0, 0, Controllers.EffectControl1Fine, 0));
			track.Events.Add(new NoteOn(11, 0, "E6", 60));
			track.Events.Add(new Controller(56, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new NoteOn(24, 0, "D#6", 66));
			track.Events.Add(new Controller(0, 0, Controllers.EffectControl1Fine, 0));
			track.Events.Add(new NoteOn(2, 0, "E6", 0));
			track.Events.Add(new NoteOn(75, 0, "E6", 60));
			track.Events.Add(new NoteOn(16, 0, "D#6", 0));
			track.Events.Add(new NoteOn(72, 0, "D#6", 62));
			track.Events.Add(new NoteOn(8, 0, "E6", 0));
			track.Events.Add(new NoteOn(77, 0, "E6", 72));
			track.Events.Add(new NoteOn(16, 0, "D#6", 0));
			track.Events.Add(new NoteOn(64, 0, "B5", 71));
			track.Events.Add(new NoteOn(12, 0, "E6", 0));
			track.Events.Add(new NoteOn(64, 0, "D6", 85));
			track.Events.Add(new NoteOn(19, 0, "B5", 0));
			track.Events.Add(new NoteOn(60, 0, "C6", 80));
			track.Events.Add(new NoteOn(24, 0, "D6", 0));
			track.Events.Add(new NoteOn(51, 0, "A3", 66));
			track.Events.Add(new NoteOn(5, 0, "A5", 73));
			track.Events.Add(new NoteOn(13, 0, "C6", 0));
			track.Events.Add(new Controller(24, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new NoteOn(35, 0, "E4", 70));
			track.Events.Add(new NoteOn(51, 0, "A5", 0));
			track.Events.Add(new NoteOn(23, 0, "A4", 75));
			track.Events.Add(new NoteOn(72, 0, "A4", 0));
			track.Events.Add(new NoteOn(0, 0, "C5", 78));
			track.Events.Add(new NoteOn(73, 0, "E5", 86));
			track.Events.Add(new NoteOn(13, 0, "A3", 0));
			track.Events.Add(new NoteOn(42, 0, "E4", 0));
			track.Events.Add(new NoteOn(17, 0, "A5", 87));
			track.Events.Add(new NoteOn(19, 0, "C5", 0));
			track.Events.Add(new NoteOn(14, 0, "E5", 0));
			track.Events.Add(new NoteOn(40, 0, "E3", 72));
			track.Events.Add(new NoteOn(1, 0, "B5", 84));
			track.Events.Add(new Controller(4, 0, Controllers.EffectControl1Fine, 0));
			track.Events.Add(new NoteOn(10, 0, "A5", 0));
			track.Events.Add(new Controller(36, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new NoteOn(19, 0, "E3", 0));
			track.Events.Add(new NoteOn(5, 0, "E4", 70));
			track.Events.Add(new NoteOn(47, 0, "E4", 0));
			track.Events.Add(new NoteOn(0, 0, "B5", 0));
			track.Events.Add(new NoteOn(9, 0, "G#4", 85));
			track.Events.Add(new NoteOn(62, 0, "E5", 82));
			track.Events.Add(new NoteOn(66, 0, "G#4", 0));
			track.Events.Add(new NoteOn(5, 0, "G#5", 85));
			track.Events.Add(new NoteOn(72, 0, "B5", 93));
			track.Events.Add(new NoteOn(3, 0, "E5", 0));
			track.Events.Add(new NoteOn(29, 0, "G#5", 0));
			track.Events.Add(new NoteOn(38, 0, "C6", 78));
			track.Events.Add(new NoteOn(4, 0, "A3", 66));
			track.Events.Add(new Controller(0, 0, Controllers.EffectControl1Fine, 0));
			track.Events.Add(new NoteOn(3, 0, "B5", 0));
			track.Events.Add(new Controller(38, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new NoteOn(27, 0, "E4", 72));
			track.Events.Add(new NoteOn(76, 0, "A4", 70));
			track.Events.Add(new NoteOn(68, 0, "E5", 75));
			track.Events.Add(new NoteOn(5, 0, "C6", 0));
			track.Events.Add(new NoteOn(46, 0, "A4", 0));
			track.Events.Add(new NoteOn(20, 0, "A3", 0));
			track.Events.Add(new NoteOn(5, 0, "E4", 0));
			track.Events.Add(new NoteOn(3, 0, "E5", 0));
			track.Events.Add(new NoteOn(1, 0, "E6", 74));
			track.Events.Add(new NoteOn(70, 0, "E6", 0));
			track.Events.Add(new NoteOn(2, 0, "D#6", 82));
			track.Events.Add(new NoteOn(76, 0, "E6", 90));
			track.Events.Add(new NoteOn(6, 0, "D#6", 0));
			track.Events.Add(new Controller(4, 0, Controllers.EffectControl1Fine, 0));
			track.Events.Add(new NoteOn(59, 0, "D#6", 86));
			track.Events.Add(new NoteOn(14, 0, "E6", 0));
			track.Events.Add(new NoteOn(57, 0, "E6", 103));
			track.Events.Add(new NoteOn(20, 0, "D#6", 0));
			track.Events.Add(new NoteOn(56, 0, "B5", 90));
			track.Events.Add(new NoteOn(11, 0, "E6", 0));
			track.Events.Add(new NoteOn(66, 0, "D6", 86));
			track.Events.Add(new NoteOn(11, 0, "B5", 0));
			track.Events.Add(new NoteOn(62, 0, "C6", 85));
			track.Events.Add(new NoteOn(8, 0, "D6", 0));
			track.Events.Add(new NoteOn(72, 0, "A5", 77));
			track.Events.Add(new NoteOn(3, 0, "C6", 0));
			track.Events.Add(new NoteOn(1, 0, "A3", 55));
			track.Events.Add(new Controller(16, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new NoteOn(52, 0, "E4", 60));
			track.Events.Add(new NoteOn(28, 0, "A5", 0));
			track.Events.Add(new NoteOn(50, 0, "A4", 77));
			track.Events.Add(new NoteOn(63, 0, "C5", 90));
			track.Events.Add(new NoteOn(3, 0, "A4", 0));
			track.Events.Add(new NoteOn(57, 0, "A3", 0));
			track.Events.Add(new NoteOn(14, 0, "E5", 86));
			track.Events.Add(new NoteOn(66, 0, "E4", 0));
			track.Events.Add(new NoteOn(3, 0, "A5", 82));
			track.Events.Add(new NoteOn(3, 0, "C5", 0));
			track.Events.Add(new NoteOn(12, 0, "E5", 0));
			track.Events.Add(new NoteOn(54, 0, "B5", 89));
			track.Events.Add(new NoteOn(3, 0, "E3", 79));
			track.Events.Add(new NoteOn(7, 0, "A5", 0));
			track.Events.Add(new Controller(2, 0, Controllers.EffectControl1Fine, 0));
			track.Events.Add(new Controller(45, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new NoteOn(14, 0, "E3", 0));
			track.Events.Add(new NoteOn(7, 0, "E4", 68));
			track.Events.Add(new NoteOn(66, 0, "E4", 0));
			track.Events.Add(new NoteOn(11, 0, "G#4", 84));
			track.Events.Add(new NoteOn(67, 0, "D5", 82));
			track.Events.Add(new NoteOn(11, 0, "B5", 0));
			track.Events.Add(new NoteOn(4, 0, "G#4", 0));
			track.Events.Add(new NoteOn(68, 0, "C6", 73));
			track.Events.Add(new NoteOn(37, 0, "D5", 0));
			track.Events.Add(new NoteOn(35, 0, "B5", 69));
			track.Events.Add(new NoteOn(69, 0, "B5", 0));
			track.Events.Add(new NoteOn(3, 0, "C6", 0));
			track.Events.Add(new NoteOn(6, 0, "A3", 60));
			track.Events.Add(new NoteOn(2, 0, "A5", 68));
			track.Events.Add(new Controller(0, 0, Controllers.EffectControl1Fine, 0));
			track.Events.Add(new Controller(49, 0, Controllers.EffectControl1Fine, 127));
			track.Events.Add(new NoteOn(29, 0, "E4", 66));
			track.Events.Add(new NoteOn(90, 0, "A4", 71));
			track.Events.Add(new NoteOn(23, 0, "A5", 0));
			track.Events.Add(new NoteOn(5, 0, "A3", 0));
			track.Events.Add(new NoteOn(16, 0, "E4", 0));
			track.Events.Add(new NoteOn(0, 0, "A4", 0));
			track.Events.Add(new NoteOn(130, 0, "E6", 80));
			track.Events.Add(new EndOfTrack(130));
			return sequence;
		}
	}
}
