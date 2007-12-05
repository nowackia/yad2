// Stephen Toub
// stoub@microsoft.com
//
// DrumsetDemo
// Demo program to demonstrate the MidiCodeGenerator.
//
// TO USE:
// 1. Drop a midi file onto the application or run the application and specify
//    the input and output files.  The midi file will be parsed and code will be
//    generated that, when executed, will produce the original midi file.

using System;
using System.IO;
using Toub.Sound.Midi;

namespace Toub.Demos
{
	/// <summary>Demo program to demonstrate the MidiCodeGenerator.</summary>
	class MidiToCodeDemo
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string [] args)
		{
			string midiFilename = "";
			string codeFilename = "";

			// Use parameter as input and output name
			if (args.Length == 1) 
			{
				midiFilename = args[0];
				codeFilename = args[0] + ".cs";
			}
				// Ask user for filenames
			else 
			{
				// Get the input file
				while(true) 
				{
					Console.WriteLine("Give the full path to a MIDI file:");
					midiFilename = Console.ReadLine();
					if (!File.Exists(midiFilename)) Console.WriteLine("The file does not exist.");
					else break;
				}
	
				// Get the name of the output csharp file
				Console.WriteLine("Enter the name of an output csharp file:");
				codeFilename = Console.ReadLine();
			}

			// Generate the code for the midi sequence
			Console.WriteLine("Generating...");
			MidiSequence sequence = MidiSequence.Import(midiFilename);
			using (StreamWriter writer = new StreamWriter(codeFilename)) 
			{
				MidiCodeGenerator.GenerateMIDICode(
					new Microsoft.CSharp.CSharpCodeProvider().CreateGenerator(),
					sequence, 
					"SampleMidi",
					writer);
			}
			Console.WriteLine("Complete.");
		}
	}
}
