// Stephen Toub
// stoub@microsoft.com
//
// ConvertToZero
// Demo program that converts from midi format 1 or 2 to format 0.
//
// TO USE:
// 1. Drop a midi file or a set of midi files onto the application or enter the paths to the files on the command line.
// 2. New files will be created based on the existing name.

using System;
using System.IO;
using Toub.Sound.Midi;

namespace Toub.Demos
{
	/// <summary>Demo program that converts between midi file formats.</summary>
	class ConvertFormat
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string [] args)
		{
			foreach(string midiFilename in args)
			{
				try
				{
					// Open midi file
					MidiSequence sequence = MidiSequence.Import(midiFilename);

					// Spit out format info and ask for new format
					int oldFormat = sequence.Format;
					int newFormat = 0;

					if (oldFormat != newFormat)
					{
						// Create the new midi file
						MidiSequence newSequence = MidiSequence.Convert(
							sequence, newFormat, MidiSequence.FormatConversionOptions.CopyTrackToChannel);
			
						// Write out the new converted file
						string newFilename = midiFilename + "." + newFormat + ".mid";
						newSequence.Save(newFilename);

						// Let the user know
						Console.WriteLine("Converted {0}\r\n\tFrom type {1}\r\n\tTo type {2}\r\n\tSaved to {3}",
							midiFilename, oldFormat, newFormat, newFilename);
					} 
					else
					{
						Console.WriteLine("File {0} is already type {1}.", midiFilename, newFormat);
					}
				} 
				catch(Exception exc)
				{
					// Let the user know something went wrong
					Console.WriteLine("Converting {0}\r\n\t{1}", midiFilename, exc.Message);
				}
			}

			Console.WriteLine("");
			Console.WriteLine("Hit enter to exit...");
			Console.ReadLine();
		}
	}
}
