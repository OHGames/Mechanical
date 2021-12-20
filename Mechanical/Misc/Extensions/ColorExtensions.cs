/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanical
{
    /// <summary>
    /// Extensions for <see cref="Color"/>
    /// </summary>
    public static class ColorExtensions
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

    }
}
