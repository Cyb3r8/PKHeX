using System.Drawing;

namespace PKHeX.WinForms.Theming;

public sealed record ThemePalette
{
    public required bool IsDark { get; init; }
    public required Color Surface0 { get; init; }
    public required Color Surface1 { get; init; }
    public required Color Surface2 { get; init; }
    public required Color Border { get; init; }
    public required Color TextPrimary { get; init; }
    public required Color TextMuted { get; init; }
    public required Color TextDisabled { get; init; }
    public required Color Accent { get; init; }
    public required Color AccentHover { get; init; }
    public required Color AccentText { get; init; }
    public required Color Legal { get; init; }
    public required Color Warning { get; init; }
    public required Color Invalid { get; init; }
    public required Color Info { get; init; }

    public Color HoverSurface => Mix(Surface0, Surface2, 0.55f);
    public Color PressedSurface => Mix(Accent, Surface0, 0.85f);
    public Color FocusRing => Accent;
    public Color AccentMuted => Mix(Accent, Surface0, 0.70f);

    public static ThemePalette CreateLight(Color accent) => new()
    {
        IsDark = false,
        Surface0 = Color.FromArgb(0xF5, 0xF6, 0xF8),
        Surface1 = Color.FromArgb(0xFF, 0xFF, 0xFF),
        Surface2 = Color.FromArgb(0xEE, 0xF0, 0xF4),
        Border = Color.FromArgb(0xD8, 0xDB, 0xE3),
        TextPrimary = Color.FromArgb(0x1A, 0x1D, 0x23),
        TextMuted = Color.FromArgb(0x5A, 0x61, 0x70),
        TextDisabled = Color.FromArgb(0xA0, 0xA6, 0xB0),
        Accent = accent,
        AccentHover = Lighten(accent, 0.08f),
        AccentText = Color.White,
        Legal = Color.FromArgb(0x22, 0xC5, 0x5E),
        Warning = Color.FromArgb(0xF5, 0x9E, 0x0B),
        Invalid = Color.FromArgb(0xEF, 0x44, 0x44),
        Info = Color.FromArgb(0x3B, 0x82, 0xF6),
    };

    public static ThemePalette CreateDark(Color accent) => new()
    {
        IsDark = true,
        Surface0 = Color.FromArgb(0x14, 0x16, 0x1B),
        Surface1 = Color.FromArgb(0x1C, 0x1F, 0x26),
        Surface2 = Color.FromArgb(0x26, 0x2A, 0x34),
        Border = Color.FromArgb(0x2A, 0x2E, 0x38),
        TextPrimary = Color.FromArgb(0xE8, 0xEA, 0xF0),
        TextMuted = Color.FromArgb(0x8A, 0x92, 0xA6),
        TextDisabled = Color.FromArgb(0x5A, 0x61, 0x70),
        Accent = accent,
        AccentHover = Lighten(accent, 0.10f),
        AccentText = Color.White,
        Legal = Color.FromArgb(0x22, 0xC5, 0x5E),
        Warning = Color.FromArgb(0xF5, 0x9E, 0x0B),
        Invalid = Color.FromArgb(0xEF, 0x44, 0x44),
        Info = Color.FromArgb(0x3B, 0x82, 0xF6),
    };

    public ThemePalette WithAccent(Color accent) => this with
    {
        Accent = accent,
        AccentHover = IsDark ? Lighten(accent, 0.10f) : Lighten(accent, 0.08f),
    };

    public Color Mix(Color a, Color b, float t)
    {
        t = Clamp01(t);
        var inv = 1f - t;
        return Color.FromArgb(
            (int)((a.A * inv) + (b.A * t)),
            (int)((a.R * inv) + (b.R * t)),
            (int)((a.G * inv) + (b.G * t)),
            (int)((a.B * inv) + (b.B * t)));
    }

    private static Color Lighten(Color c, float amount)
    {
        amount = Clamp01(amount);
        return Color.FromArgb(
            c.A,
            (int)(c.R + ((255 - c.R) * amount)),
            (int)(c.G + ((255 - c.G) * amount)),
            (int)(c.B + ((255 - c.B) * amount)));
    }

    private static float Clamp01(float v) => v < 0f ? 0f : (v > 1f ? 1f : v);
}
