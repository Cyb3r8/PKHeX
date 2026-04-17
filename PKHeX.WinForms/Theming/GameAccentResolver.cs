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
        GameVersion.B => Color.FromArgb(0x22, 0x2A, 0x30),
        GameVersion.W => Color.FromArgb(0xEB, 0xEB, 0xEB),
        GameVersion.B2 => Color.FromArgb(0x2E, 0x6A, 0xA8),
        GameVersion.W2 => Color.FromArgb(0xB3, 0x3A, 0x3A),
        GameVersion.HG => Color.FromArgb(0xC4, 0x65, 0x1D),
        GameVersion.SS => Color.FromArgb(0xB0, 0xA2, 0x4D),
        GameVersion.D => Color.FromArgb(0x2A, 0x5E, 0xAE),
        GameVersion.P => Color.FromArgb(0xCC, 0x4D, 0xA0),
        GameVersion.Pt => Color.FromArgb(0x6A, 0x6E, 0x7E),
        GameVersion.R => Color.FromArgb(0xC8, 0x2A, 0x2A),
        GameVersion.S => Color.FromArgb(0x2A, 0x7A, 0x3A),
        GameVersion.E => Color.FromArgb(0x9A, 0x3C, 0xC8),
        GameVersion.FR => Color.FromArgb(0xD9, 0x54, 0x2D),
        GameVersion.LG => Color.FromArgb(0x4E, 0xA3, 0x4E),
        GameVersion.C => Color.FromArgb(0xE8, 0xBB, 0x2A),
        GameVersion.GD => Color.FromArgb(0xC4, 0x7A, 0x2C),
        GameVersion.SV => Color.FromArgb(0xC0, 0xC0, 0xC0),
        GameVersion.RD => Color.FromArgb(0xC8, 0x2A, 0x2A),
        GameVersion.BU => Color.FromArgb(0x2A, 0x5E, 0xAE),
        GameVersion.GN => Color.FromArgb(0x4E, 0xA3, 0x4E),
        GameVersion.YW => Color.FromArgb(0xE8, 0xBB, 0x2A),
        GameVersion.CXD => Color.FromArgb(0x3C, 0x3C, 0x3C),
        _ => Default,
    };
}
