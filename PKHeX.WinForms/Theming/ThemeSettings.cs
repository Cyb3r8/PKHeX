using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using PKHeX.Core;

namespace PKHeX.WinForms.Theming;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public sealed class ThemeSettings
{
    [LocalizedDescription("Light, Dark, or System palette selection.")]
    public ThemeMode Mode { get; set; } = ThemeMode.System;

    [LocalizedDescription("Where the accent color is sourced from.")]
    public AccentSource AccentSource { get; set; } = AccentSource.FromSave;

    [LocalizedDescription("Custom accent color used when AccentSource is Custom.")]
    public Color AccentColor { get; set; } = Color.FromArgb(0x6C, 0x5C, 0xE7);

    [LocalizedDescription("Draw a 2 pixel accent underline beneath the top header band.")]
    public bool MenuAccentUnderline { get; set; } = true;

    [LocalizedDescription("Use a gradient type backdrop behind the drag-out sprite.")]
    public bool HeroDragoutCard { get; set; } = true;
}
