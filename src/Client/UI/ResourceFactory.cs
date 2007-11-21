using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Yad.Properties.Common;
using System.IO;

namespace Yad.UI {
    class ResourceFactory {

        private static Dictionary<String, Image> pictures = new Dictionary<string, Image>();
        private static Dictionary<String, Image> animations = new Dictionary<string, Image>();

        public static Image GetAnimation(String name) {
            Image im;
            if (animations.TryGetValue(name, out im)) {
                return im;
            } else {
                try {
                    im = TryGetImage(Path.Combine(Settings.Default.Animations, name + ".png"));
                    animations.Add(name, im);
                    return im;
                } catch (FileNotFoundException ex) {
                    throw new NotImplementedException("brak pliku animacji dla " + name + ": " + ex.Message);
                }
            }
            return null;
        }

        
        public static Image GetPicture(String name) {
            Image im;
            if (pictures.TryGetValue(name, out im)) {
                return im;
            } else {
                try {
                    im = TryGetImage(Path.Combine(Settings.Default.Pictures, name + ".png"));
                    pictures.Add(name, im);
                    return im;
                } catch (FileNotFoundException ex) {
                    throw new NotImplementedException("brak pliku grafiki dla " + name + ": " + ex.Message);
                }
            }
            return null;
        }

        private static Image TryGetImage(String path) {
            return Bitmap.FromFile(path);
        }

    }
}
