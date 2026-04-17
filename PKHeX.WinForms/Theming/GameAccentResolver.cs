using System.Drawing;
using PKHeX.Core;

namespace PKHeX.WinForms.Theming;

public static class GameAccentResolver
{
    public static readonly Color Default = Color.FromArgb(0x6C, 0x5C, 0xE7);

    public static Color Resolve(SaveFile? sav) => sav is null ? Default : Resolve(sav.Version);

    public static Color Resolve(GameVersion version) => version switch
    {
        GameVersion.SL => Color.FromArgb(0xD0, 0x30, 0x31),
        GameVersion.VL => Color.FromArgb(0x7A, 0x3F, 0xBF),
        GameVersion.ZA => Color.FromArgb(0x00, 0xBF, 0xA6),
        GameVersion.SW => Color.FromArgb(0x00, 0xA5, 0xE0),
        GameVersion.SH => Color.FromArgb(0xE5, 0x20, 0x2B),
        GameVersion.BD => Color.FromArgb(0xF1, 0x8C, 0xBF),
        GameVersion.SP => Color.FromArgb(0x4B, 0x89, 0xDC),
        GameVersion.PLA => Color.FromArgb(0x6B, 0xC7, 0xC7),
        GameVersion.SN => Color.FromArgb(0xE8, 0x8D, 0x2A),
        GameVersion.MN => Color.FromArgb(0x4E, 0x54, 0xD0),
        GameVersion.US => Color.FromArgb(0xE8, 0x6E, 0x22),
        GameVersion.UM => Color.FromArgb(0x6C, 0x4A, 0xD0),
        GameVersion.GP => Color.FromArgb(0xF3, 0xB0, 0x3C),
        GameVersion.GE => Color.FromArgb(0x4A, 0xB0, 0x72),
        GameVersion.X => Color.FromArgb(0x3A, 0x58, 0xC0),
        GameVersion.Y => Color.FromArgb(0xD0, 0x3C, 0x3C),
        GameVersion.OR => Color.FromArgb(0xA0, 0x20, 0x20),
        GameVersion.AS => Color.FromArgb(0x3A, 0x58, 0xC0),
        _ => Default,
    };
}
