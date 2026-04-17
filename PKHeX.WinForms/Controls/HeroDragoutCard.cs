using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PKHeX.WinForms.Theming;

namespace PKHeX.WinForms.Controls;

public sealed class HeroDragoutCard : Panel
{
    private Color _primary = Color.FromArgb(40, 120, 220);
    private Color _secondary = Color.Empty;

    public int CornerRadius { get; set; } = 8;

    public HeroDragoutCard()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.UserPaint
                 | ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.ResizeRedraw
                 | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        Theme.Changed += (_, _) => Invalidate();
    }

    public void SetTypes(Color primary, Color? secondary = null)
    {
        _primary = primary;
        _secondary = secondary ?? Color.Empty;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var r = ClientRectangle;
        if (r.Width <= 2 || r.Height <= 2)
            return;
        r.Inflate(-1, -1);

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var palette = Theme.Current;

        using var path = CreateRounded(r, CornerRadius);

        using (var surface = new SolidBrush(palette.Surface1))
            g.FillPath(surface, path);

        var tintAlpha = palette.IsDark ? 80 : 55;
        var a = Color.FromArgb(tintAlpha, _primary);
        var b = _secondary.IsEmpty
            ? Color.FromArgb(0, _primary)
            : Color.FromArgb(tintAlpha, _secondary);

        using (var tint = new LinearGradientBrush(r, a, b, 45f))
        {
            var clip = g.Clip;
            g.SetClip(path);
            g.FillRectangle(tint, r);
            g.Clip = clip;
        }

        using var pen = new Pen(palette.Border, 1f);
        g.DrawPath(pen, path);

        base.OnPaint(e);
    }

    private static GraphicsPath CreateRounded(Rectangle r, int radius)
    {
        var path = new GraphicsPath();
        if (radius <= 0 || r.Width <= 0 || r.Height <= 0)
        {
            path.AddRectangle(r);
            return path;
        }
        int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
