/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Mechanical
{
    /// <summary>
    /// Extensions for <see cref="Color"/>
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// A list of colors and their names.
        /// </summary>
        public static Dictionary<string, Color> Colors = new Dictionary<string, Color>()
        {
            { "transparent",    Color.Transparent    },
            { "aliceblue",      Color.AliceBlue      },
            { "antiquewhite",   Color.AntiqueWhite   },
            { "aqua",           Color.Aqua           },
            { "aquamarine",     Color.Aquamarine     },
            { "azure",          Color.Azure          },
            { "beige",          Color.Beige          },
            { "bisque",         Color.Bisque         },
            { "black",          Color.Black          },
            { "blanchedalmond", Color.BlanchedAlmond },
            { "blue",           Color.Blue           },
            { "blueviolet",     Color.BlueViolet     },
            { "brown",          Color.Brown          },
            { "burlywood",      Color.BurlyWood      },
            { "cadetblue",      Color.CadetBlue      },
            { "chartreuse",     Color.Chartreuse     },
            { "chocolate",      Color.Chocolate      },
            { "coral",          Color.Coral          },
            { "cornflowerblue", Color.CornflowerBlue },
            { "cornsilk",       Color.Cornsilk       },
            { "crimson",        Color.Crimson        },
            { "cyan",           Color.Cyan           },
            { "darkblue",       Color.DarkBlue       },
            { "darkcyan",       Color.DarkCyan       },
            { "darkgoldenrod",  Color.DarkGoldenrod  },
            { "darkgray",       Color.DarkGray       },
            { "darkgreen",      Color.DarkGreen      },
            { "darkkhaki",      Color.DarkKhaki      },
            { "darkmagenta",    Color.DarkMagenta    },
            { "darkolivegreen", Color.DarkOliveGreen },
            { "darkorange",     Color.DarkOrange     },
            { "darkorchid",     Color.DarkOrchid     },
            { "darkred",        Color.DarkRed        },
            { "darksalmon",     Color.DarkSalmon     },
            { "darkseagreen",   Color.DarkSeaGreen   },
            { "darkslateblue",  Color.DarkSlateBlue  },
            { "darkslategray",  Color.DarkSlateGray  },
            { "darkturquoise",  Color.DarkTurquoise  },
            { "darkviolet",     Color.DarkViolet     },
            { "deeppink",       Color.DeepPink       },
            { "deepskyblue",    Color.DeepSkyBlue    },
            { "dimgray",        Color.DimGray        },
            { "dodgerblue",     Color.DodgerBlue     },
            { "firebrick",      Color.Firebrick      },
            { "floralwhite",    Color.FloralWhite    },
            { "forestgreen",    Color.ForestGreen    },
            { "fuchsia",        Color.Fuchsia        },
            { "gainsboro",      Color.Gainsboro      },
            { "ghostwhite",     Color.GhostWhite     },
            { "gold",           Color.Gold           },
            { "goldenrod",      Color.Goldenrod      },
            { "gray",           Color.Gray           }, // i give up on formatting this.
            { "green", Color.Green },
            { "greenyellow", Color.GreenYellow },
            { "honeydew", Color.Honeydew },
            { "hotpink", Color.HotPink },
            { "indianred", Color.IndianRed },
            { "indigo", Color.Indigo },
            { "ivory", Color.Ivory },
            { "khaki", Color.Khaki },
            { "lavender", Color.Lavender },
            { "lavenderblush", Color.LavenderBlush },
            { "lawngreen", Color.LawnGreen },
            { "lemonchiffon", Color.LemonChiffon },
            { "lightblue", Color.LightBlue },
            { "lightcoral", Color.LightCoral },
            { "lightcyan", Color.LightCyan },
            { "lightgoldenrodyellow", Color.LightGoldenrodYellow },
            { "lightgray", Color.LightGray },
            { "lightgreen", Color.LightGreen },
            { "lightpink", Color.LightPink },
            { "lightsalmon", Color.LightSalmon },
            { "lightseagreen", Color.LightSeaGreen },
            { "lightskyblue", Color.LightSkyBlue },
            { "lightslategray", Color.LightSlateGray },
            { "lightsteelblue", Color.LightSteelBlue },
            { "lightyellow", Color.LightYellow },
            { "lime", Color.Lime },
            { "limegreen", Color.LimeGreen },
            { "linen", Color.Linen },
            { "magenta", Color.Magenta },
            { "maroon", Color.Maroon },
            { "mediumaquamarine", Color.MediumAquamarine },
            { "mediumblue", Color.MediumBlue },
            { "mediumorchid", Color.MediumOrchid },
            { "mediumpurple", Color.MediumPurple },
            { "mediumseagreen", Color.MediumSeaGreen },
            { "mediumslateblue", Color.MediumSlateBlue },
            { "mediumspringgreen", Color.MediumSpringGreen },
            { "mediumturquoise", Color.MediumTurquoise },
            { "mediumvioletred", Color.MediumVioletRed },
            { "midnightblue", Color.MidnightBlue },
            { "mintcream", Color.MintCream },
            { "mistyrose", Color.MistyRose },
            { "moccasin", Color.Moccasin },
            { "monogameorange", Color.MonoGameOrange },
            { "navajowhite", Color.NavajoWhite },
            { "navy", Color.Navy },
            { "oldlace", Color.OldLace },
            { "olive", Color.Olive },
            { "olivedrab", Color.OliveDrab },
            { "orange", Color.Orange },
            { "orangered", Color.OrangeRed },
            { "orchid", Color.Orchid },
            { "palegoldenrod", Color.PaleGoldenrod },
            { "palegreen", Color.PaleGreen },
            { "paleturquoise", Color.PaleTurquoise },
            { "palevioletred", Color.PaleVioletRed },
            { "papayawhip", Color.PapayaWhip },
            { "peachpuff", Color.PeachPuff },
            { "peru", Color.Peru },
            { "pink", Color.Pink },
            { "plum", Color.Plum },
            { "powderblue", Color.PowderBlue },
            { "purple", Color.Purple },
            { "red", Color.Red },
            { "rosybrown", Color.RosyBrown },
            { "royalblue", Color.RoyalBlue },
            { "saddlebrown", Color.SaddleBrown },
            { "salmon", Color.Salmon },
            { "sandybrown", Color.SandyBrown },
            { "seagreen", Color.SeaGreen },
            { "seashell", Color.SeaShell },
            { "sienna", Color.Sienna },
            { "silver", Color.Silver },
            { "skyblue", Color.SkyBlue },
            { "slateblue", Color.SlateBlue },
            { "slategray", Color.SlateGray },
            { "snow", Color.Snow },
            { "springgreen", Color.SpringGreen },
            { "steelblue", Color.SteelBlue },
            { "tan", Color.Tan },
            { "teal", Color.Teal },
            { "thistle", Color.Thistle },
            { "tomato", Color.Tomato },
            { "turquoise", Color.Turquoise },
            { "violet", Color.Violet },
            { "wheat", Color.Wheat },
            { "white", Color.White },
            { "whitesmoke", Color.WhiteSmoke },
            { "yellow", Color.Yellow },
            { "yellowgreen", Color.YellowGreen }
        };

        /// <summary>
        /// Get a <see cref="Color"/> based on its name.
        /// </summary>
        /// <param name="name">The name of the color.</param>
        /// <returns>A color.</returns>
        public static Color GetColorByName(this string name)
        {
            return Colors[name.ToLower()];
        }

        /// <summary>
        /// Returns a random color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color GetRandomColor()
        {
            // we use Colors.Count because max is exclusive.
            return Colors.GetValue(new Random().Next(0, Colors.Count));
        }

        /// <summary>
        /// Returns a random color name.
        /// </summary>
        /// <returns>A random color name.</returns>
        public static string GetRandomColorName()
        {
            // we use Colors.Count because max is exclusive.
            return Colors.GetKey(new Random().Next(0, Colors.Count));
        }

        /// <summary>
        /// Turns a hex code into a color.
        /// </summary>
        /// <param name="hex">The hex color in RGBA format.</param>
        /// <returns>A new <see cref="Color"/> from the hex.</returns>
        public static Color FromHexRGBA(string hex)
        {
            // flip the last 2 digits to make it RGBA -> ARGB.
            // remove #
            hex = hex.Remove(0, 1);
            // convert alpha chars to strings.
            string alpha = $"{hex[hex.Length - 2]}{hex[hex.Length - 1]}";
            // remove last 2 alpha digits
            // do same thing twice because when it removes the last one, the length changes so we remove the last one again.
            hex = hex.Remove(hex.Length - 1, 1);
            hex = hex.Remove(hex.Length - 1, 1);
            // put alpha onto begining.
            hex = alpha + hex;
            // convert to system color
            // https://stackoverflow.com/a/2109938
            System.Drawing.Color c = System.Drawing.Color.FromArgb(Convert.ToInt32(hex, 16));
            // return color.
            return new Color(c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// Turns a hex code into a color.
        /// </summary>
        /// <param name="hex">The hex color in ARGB format.</param>
        /// <returns>A new <see cref="Color"/> from the hex.</returns>
        public static Color FromHex(string hex)
        {
            // convert to system color
            System.Drawing.Color c = System.Drawing.Color.FromArgb(Convert.ToInt32(hex));
            // return color.
            return new Color(c.R, c.G, c.B, c.A);
        }

    }
}
