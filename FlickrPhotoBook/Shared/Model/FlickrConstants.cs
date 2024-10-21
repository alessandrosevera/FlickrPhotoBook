using System;
namespace FlickrPhotoBook.Model
{
    public class FlickrConstants
    {
        #region nested classes

        public static class SizeSuffix
        {
            #region properties

            public static Level Thumb => new Level(
                small: new Level.SizeSpecifics("s", 75, true, false),
                medium: new Level.SizeSpecifics("t", 100, false, false),
                large: new Level.SizeSpecifics("q", 150, true, false),
                extraLarge: new Level.SizeSpecifics("q", 150, true, false)
            );

            public static Level Small => new Level(
                small: new Level.SizeSpecifics("m", 240, false, false),
                medium: new Level.SizeSpecifics("n", 320, false, false),
                large: new Level.SizeSpecifics("w", 400, false, false),
                extraLarge: new Level.SizeSpecifics("w", 400, false, false)
            );

            public static Level Default => null;

            public static Level Medium => new Level(
                small: new Level.SizeSpecifics("z", 640, false, false),
                medium: new Level.SizeSpecifics("z", 640, false, false),
                large: new Level.SizeSpecifics("c", 800, false, false),
                extraLarge: new Level.SizeSpecifics("c", 800, false, false)
            );

            public static Level Large => new Level(
                small: new Level.SizeSpecifics("b", 1024, false, false),
                medium: new Level.SizeSpecifics("h", 1600, false, true),
                large: new Level.SizeSpecifics("k", 2048, false, true),
                extraLarge: new Level.SizeSpecifics("k", 2048, false, true)
            );

            public static Level ExtraLarge => new Level(
                small: new Level.SizeSpecifics("3k", 3072, false, true),
                medium: new Level.SizeSpecifics("4k", 4096, false, true),
                large: new Level.SizeSpecifics("5k", 5120, false, true),
                extraLarge: new Level.SizeSpecifics("6k", 6144, false, true)
            );

            public static Level.SizeSpecifics Original => new Level.SizeSpecifics("o", null, null, null);

            #endregion
        }

        public class Level
        {
            #region nested classes

            public class SizeSpecifics
            {
                public string Suffix { get; }
                public int? Size { get; }
                public bool? IsClipped { get; }
                public bool? CanBeRestricted { get; }

                public SizeSpecifics(string suffix, int? size, bool? isClipped, bool? canBeRestricted)
                {
                    Suffix = suffix;
                    Size = size;
                    IsClipped = isClipped;
                    CanBeRestricted = canBeRestricted;
                }
            }

            #endregion

            #region auto-properties

            public SizeSpecifics Small { get; }
            public SizeSpecifics Medium { get; }
            public SizeSpecifics Large { get; }
            public SizeSpecifics ExtraLarge { get; }

            #endregion

            #region ctor(s)

            public Level(SizeSpecifics small, SizeSpecifics medium, SizeSpecifics large, SizeSpecifics extraLarge)
            {
                Small = small;
                Medium = medium;
                Large = large;
                ExtraLarge = extraLarge;
            }

            #endregion
        }

        #endregion
    }
}

