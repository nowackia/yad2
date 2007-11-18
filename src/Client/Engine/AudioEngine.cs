using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Yad.Properties.Client;
using Yad.Utilities.Common;
using Yad.Log.Common;
using System.Threading;

namespace Yad.Engine.Client {
	public static class AudioEngine {
		public enum MusicType : int { Fight = 0, Lose = 1, Win = 2, Peace = 3 };

		static int musicTypesCount;
		static int[] indices;
		static List<String>[] music;


		public static void Init() {
			musicTypesCount = Enum.GetValues(typeof(MusicType)).Length;
			music = new List<String>[musicTypesCount];
			indices = new int[musicTypesCount];
			for (int i = 0; i < musicTypesCount; i++) {
				music[i] = new List<String>();
			}
			DirectoryInfo di;
			di = new DirectoryInfo(Settings.Default.MusicFight);
			foreach (FileInfo fi in di.GetFiles()) {
				music[(int)MusicType.Fight].Add(fi.FullName);
			}
			di = new DirectoryInfo(Settings.Default.MusicLose);
			foreach (FileInfo fi in di.GetFiles()) {
					music[(int)MusicType.Lose].Add(fi.FullName);
			}
			di = new DirectoryInfo(Settings.Default.MusicPeace);
			foreach (FileInfo fi in di.GetFiles()) {
					music[(int)MusicType.Peace].Add(fi.FullName);
			}
			di = new DirectoryInfo(Settings.Default.MusicWin);
			foreach (FileInfo fi in di.GetFiles()) {
					music[(int)MusicType.Win].Add(fi.FullName);
			}
			for (int i = 0; i < musicTypesCount; i++) {
				indices[i] = 0;
			}
		}

		public static bool PlayNext(MusicType mt) {
			List<String> tracks = music[(short)mt];
			if (tracks.Count == 0) {
				return false;
			}

			int idx = indices[(int)mt] % tracks.Count;
			//play
			return true;
		}

		public static bool PlayRandom(MusicType mt) {
			List<String> tracks = music[(int)mt];
			if (tracks.Count == 0) {
				return false;
			}

			int idx = indices[(int)mt] = Randomizer.Next(tracks.Count);
			//play
			return true;
		}

		static void PlayTrack(object filename) {
			string s = filename as string;
			//play
		}
	}
}
