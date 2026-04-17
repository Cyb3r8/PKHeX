using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PKHeX.Drawing.PokeSprite;
using PKHeX.WinForms.Theming;

namespace PKHeX.WinForms.Controls;

/// <summary>
/// Aligns tabs to the left side of the control with text displayed horizontally.
/// </summary>
public class VerticalTabControl : TabControl
{
    public VerticalTabControl()
    {
        Alignment = TabAlignment.Right;
        DrawMode = TabDrawMode.OwnerDrawFixed;
        SizeMode = TabSizeMode.Fixed;
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        var index = e.Index;
        if ((uint)index >= TabPages.Count)
            return;
        var bounds = GetTabRect(index);

        var graphics = e.Graphics;
        DrawBackground(e, bounds, graphics);

        using var flags = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };
        using var text = new SolidBrush(ForeColor);
        graphics.DrawString(TabPages[index].Text, Font, text, bounds, flags);
        base.OnDrawItem(e);
    }

    protected static void DrawBackground(DrawItemEventArgs e, Rectangle bounds, Graphics graphics)
    {
        var palette = Theme.Current;
        if (e.State != DrawItemState.Selected)
        {
            using var bg = new SolidBrush(palette.Surface1);
            graphics.FillRectangle(bg, bounds);
            return;
        }

        using var brush = new LinearGradientBrush(bounds, palette.Surface2, palette.Surface1, 90f);
        graphics.FillRectangle(brush, bounds);
    }

    protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
    {
        base.ScaleControl(factor, specified);
        ItemSize = new((int)(ItemSize.Width * factor.Width), (int)(ItemSize.Height * factor.Height));
    }
}

/// <summary>
/// Specialized <see cref="VerticalTabControl"/> for displaying a <see cref="PKHeX.Core.PKM"/> editor tabs.
/// </summary>
public sealed class VerticalTabControlEntityEditor : VerticalTabControl
{
    private static readonly Color[] SelectedTags =
    [
        ContestColor.Cool,    // Main
        ContestColor.Beauty,  // Met
        ContestColor.Cute,    // Stats
        ContestColor.Clever,  // Moves
        ContestColor.Tough,   // Cosmetic
        Color.RosyBrown,      // OT
    ];

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        var index = e.Index;
        if ((uint)index >= TabPages.Count)
            return;
        var bounds = GetTabRect(index);

        var graphics = e.Graphics;
        var palette = Theme.Current;
        DrawBackground(e, bounds, graphics);

        if (e.State == DrawItemState.Selected)
        {
            var pipColor = index < SelectedTags.Length ? SelectedTags[index] : palette.Accent;
            using var pipBrush = new SolidBrush(pipColor);
            var pip = new Rectangle(bounds.X, bounds.Y, bounds.Width / 8, bounds.Height);
            graphics.FillRectangle(pipBrush, pip);

            using var accent = new SolidBrush(palette.Accent);
            graphics.FillRectangle(accent, new Rectangle(bounds.Right - 2, bounds.Y + 2, 2, bounds.Height - 4));

            bounds = bounds with { Width = bounds.Width - pip.Width, X = bounds.X + pip.Width };
        }

        using var flags = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };
        using var text = new SolidBrush(ForeColor);
        graphics.DrawString(TabPages[index].Text, Font, text, bounds, flags);
    }
}
