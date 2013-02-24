using System;
using System.Collections.Generic;
using System.Drawing;
using Wolfpack.Core;

namespace Wolfpack.Contrib.Geckoboard
{
    public interface IColourPicker
    {
        ColourPicker.DisplayColour Next(string label);
    }

    /// <summary>
    /// This class provides random colours for use in charts/graphs etc
    /// </summary>
    public class ColourPicker
    {
        public class DisplayColour
        {
            private Color _colour;

            public static DisplayColour FromKnownColour(Color colour)
            {
                return new DisplayColour
                           {
                               _colour = colour
                           };
            }

            public static DisplayColour FromHexString(string colour)
            {
                try
                {
                    var knownColour = (KnownColor)Enum.Parse(typeof(KnownColor), colour, true);
                    return new DisplayColour
                               {
                                   _colour = Color.FromKnownColor(knownColour)
                               };
                }
                catch
                {
                    
                }
                

                var alpha = Convert.ToInt32("0x" + colour.Substring(0, 2));
                var red = Convert.ToInt32("0x" + colour.Substring(2, 2));
                var green = Convert.ToInt32("0x" + colour.Substring(4, 2));
                var blue = Convert.ToInt32("0x" + colour.Substring(6, 2));

                return FromArgb(alpha, red, green, blue);                
            }

            public static DisplayColour FromArgb(int alpha, int red, int green, int blue)
            {
                return new DisplayColour
                {
                    _colour = Color.FromArgb(alpha, red, green, blue)
                };                
            }

            public override string ToString()
            {
                return string.Format("{0:X2}{1:X2}{2:X2}", _colour.R, _colour.G ,_colour.B);
            }
        }

        protected static IColourPicker _instance;

        static ColourPicker()
        {
            _instance = Container.Resolve<IColourPicker>();
        }

        public static string Next(string label)
        {
            return _instance.Next(label).ToString();
        }
    }

    public class DefaultColourPicker : IColourPicker
    {
        protected static Random _randomiser;
        protected Dictionary<string, ColourPicker.DisplayColour> _cache;
        protected List<string> _remainingFavourites;

        /// <summary>
        /// This is a set of colours reserved for specific labels
        /// </summary>
        public Dictionary<string, string> Reserved { get; set; }

        private List<string> _favourites;
        public List<string> Favourites
        {
            get { return _favourites; }
            set
            {
                _favourites = value;
                _remainingFavourites = new List<string>(_favourites);
            }
        }

        public DefaultColourPicker()
        {
            _randomiser = new Random();          
            _cache = new Dictionary<string, ColourPicker.DisplayColour>();

            Favourites = new List<string>();
            Reserved = new Dictionary<string, string>();
        }

        public ColourPicker.DisplayColour Next(string label)
        {
            ColourPicker.DisplayColour colour;

            // check cache...
            if (_cache.ContainsKey(label))
                return _cache[label];

            // check reserved
            if (Reserved.ContainsKey(label))
            {
                colour = ColourPicker.DisplayColour.FromHexString(Reserved[label]);
                _cache.Add(label, colour);
                return colour;
            }
            
            // use the next favourite
            if (_remainingFavourites.Count > 0)
            {                    
                colour = ColourPicker.DisplayColour.FromHexString(_remainingFavourites[0]);
                _cache.Add(label, colour);
                _remainingFavourites.RemoveAt(0);
                return colour;                
            }

            // random
            colour = ColourPicker.DisplayColour.FromArgb(
                _randomiser.Next(100, 255),
                _randomiser.Next(0, 200),
                _randomiser.Next(0, 200),
                _randomiser.Next(0, 200));
            _cache.Add(label, colour);
            return colour;
        }
    }
}