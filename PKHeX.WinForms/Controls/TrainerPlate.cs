using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.WinForms.Theming;

namespace PKHeX.WinForms.Controls;

public sealed class TrainerPlate : Panel, IThemedControl
{
    private string _game = string.Empty;
    private string _ot = string.Empty;
    private string _ids = string.Empty;
    private string _language = string.Empty;
    private Color _accent = Color.Gray;
    private bool _hasSave;

    public TrainerPlate()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.UserPaint
                 | ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.ResizeRedraw, true);
        DoubleBuffered = true;
        BackColor = Theme.Current.Surface1;
        Dock = DockStyle.Top;
        Height = LogicalToDeviceUnits(32);
        AccessibleRole = AccessibleRole.StatusBar;
        AccessibleName = "Trainer info";
    }

    public void ApplyTheme(ThemePalette palette)
    {
        BackColor = palette.Surface1;
        Invalidate();
    }

    public void SetSave(SaveFile? sav)
    {
        _hasSave = sav is not null;
        if (sav is null)
        {
            _game = _ot = _ids = _language = string.Empty;
            AccessibleDescription = "No save loaded";
            Invalidate();
            return;
        }

        _game = sav.Version.ToString();
        _ot = string.IsNullOrWhiteSpace(sav.OT) ? "?" : sav.OT;
        _ids = $"TID: {sav.DisplayTID}  SID: {sav.DisplaySID}";
        _language = ((LanguageID)sav.Language).ToString();
        _accent = GameAccentResolver.Resolve(sav.Version);
        AccessibleDescription = $"{_game} · {_ot} · {_ids} · {_language}";

        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var palette = Theme.Current;

        using var bg = new SolidBrush(palette.Surface1);
        g.FillRectangle(bg, ClientRectangle);

        int x = LogicalToDeviceUnits(12);
        int centerY = Height / 2;

        if (!_hasSave)
        {
            using var muted = new Font(Font, FontStyle.Italic);
            TextRenderer.DrawText(g, "No save loaded · Open a file to begin", muted,
                new Rectangle(x, 0, Width - x, Height), palette.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            using var divider = new Pen(palette.Border, 1f);
            g.DrawLine(divider, 0, Height - 1, Width, Height - 1);
            return;
        }

        using var badgeFont = new Font(Font.FontFamily, 8f, FontStyle.Bold);
        var badgeTextSize = TextRenderer.MeasureText(g, _game, badgeFont);
        var badge = new Rectangle(x, centerY - LogicalToDeviceUnits(10), badgeTextSize.Width + LogicalToDeviceUnits(14), LogicalToDeviceUnits(20));
        using (var path = Pill(badge))
        using (var brush = new SolidBrush(_accent))
            g.FillPath(brush, path);
        TextRenderer.DrawText(g, _game, badgeFont, badge, Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        x = badge.Right + LogicalToDeviceUnits(10);

        using var otFont = new Font(Font.FontFamily, 9f, FontStyle.Bold);
        x = DrawLine(g, _ot, otFont, x, centerY, palette.TextPrimary) + LogicalToDeviceUnits(8);

        int dotSize = LogicalToDeviceUnits(3);
        using var dot = new SolidBrush(palette.TextMuted);
        g.FillEllipse(dot, x, centerY - (dotSize / 2), dotSize, dotSize);
        x += LogicalToDeviceUnits(10);

        x = DrawLine(g, _ids, Font, x, centerY, palette.TextMuted) + LogicalToDeviceUnits(8);
        g.FillEllipse(dot, x, centerY - (dotSize / 2), dotSize, dotSize);
        x += LogicalToDeviceUnits(10);

        DrawLine(g, _language, Font, x, centerY, palette.TextMuted);

        using var border = new Pen(palette.Border, 1f);
        g.DrawLine(border, 0, Height - 1, Width, Height - 1);
    }

    private static int DrawLine(Graphics g, string text, Font font, int x, int centerY, Color color)
    {
        var size = TextRenderer.MeasureText(g, text, font);
        var r = new Rectangle(x, centerY - (size.Height / 2), size.Width + 4, size.Height);
        TextRenderer.DrawText(g, text, font, r, color,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        return r.Right;
    }

    private static GraphicsPath Pill(Rectangle r)
    {
        var path = new GraphicsPath();
        int d = Math.Min(r.Height, r.Width);
        path.AddArc(r.X, r.Y, d, d, 90, 180);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 180);
        path.CloseFigure();
        return path;
    }
}
