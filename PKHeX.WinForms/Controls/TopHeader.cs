using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PKHeX.WinForms.Theming;

namespace PKHeX.WinForms.Controls;

public sealed class TopHeader : Panel
{
    public string Wordmark { get; set; } = "PKHeX";
    public int LogoInset { get; set; } = 8;

    public TopHeader()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.UserPaint
                 | ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.ResizeRedraw, true);
        DoubleBuffered = true;
        BackColor = Theme.Current.Surface0;
        Dock = DockStyle.Top;
        Height = 44;
        Theme.Changed += (_, _) =>
        {
            BackColor = Theme.Current.Surface0;
            Invalidate();
        };
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var palette = Theme.Current;

        using var bg = new SolidBrush(palette.Surface0);
        g.FillRectangle(bg, ClientRectangle);

        int y = (Height - 18) / 2;
        var mark = new Rectangle(LogoInset, y, 18, 18);
        using (var path = RoundedRect(mark, 4))
        using (var accent = new LinearGradientBrush(mark, palette.Accent, palette.AccentHover, 45f))
            g.FillPath(accent, path);

        using var font = new Font(Font.FontFamily, 10f, FontStyle.Bold);
        var textSize = TextRenderer.MeasureText(g, Wordmark, font);
        var textBounds = new Rectangle(mark.Right + 6, (Height - textSize.Height) / 2, textSize.Width + 4, textSize.Height);
        TextRenderer.DrawText(g, Wordmark, font, textBounds, palette.TextPrimary,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        if (Theme.Settings.MenuAccentUnderline)
        {
            using var pen = new Pen(palette.Accent, 2f);
            g.DrawLine(pen, 0, Height - 1, Width, Height - 1);
        }
    }

    private static GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        var path = new GraphicsPath();
        int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
