using System;
using System.Drawing;
using System.Windows.Forms;
using PKHeX.WinForms.Theming;

namespace PKHeX.WinForms.Controls;

public class VerticalTabControl : TabControl, IThemedControl
{
    private int _hoverIndex = -1;

    public VerticalTabControl()
    {
        Alignment = TabAlignment.Right;
        DrawMode = TabDrawMode.OwnerDrawFixed;
        SizeMode = TabSizeMode.Fixed;
        DoubleBuffered = true;
    }

    public void ApplyTheme(ThemePalette palette)
    {
        BackColor = palette.Surface0;
        ForeColor = palette.TextPrimary;
        foreach (TabPage tp in TabPages)
        {
            tp.UseVisualStyleBackColor = false;
            tp.BackColor = palette.Surface0;
            tp.ForeColor = palette.TextPrimary;
        }
        Invalidate();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        ApplyTheme(Theme.Current);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int idx = HitTest(e.Location);
        if (idx == _hoverIndex)
            return;
        _hoverIndex = idx;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_hoverIndex == -1)
            return;
        _hoverIndex = -1;
        Invalidate();
    }

    private int HitTest(Point p)
    {
        for (int i = 0; i < TabPages.Count; i++)
        {
            if (GetTabRect(i).Contains(p))
                return i;
        }
        return -1;
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        var index = e.Index;
        if ((uint)index >= TabPages.Count)
            return;

        var bounds = GetTabRect(index);
        var p = Theme.Current;
        var g = e.Graphics;

        bool selected = (e.State & DrawItemState.Selected) != 0;
        bool hover = _hoverIndex == index;

        Color fill = selected ? p.Surface2
            : hover ? p.HoverSurface
            : p.Surface0;

        using (var bg = new SolidBrush(fill))
            g.FillRectangle(bg, bounds);

        if (selected)
        {
            using var accent = new SolidBrush(p.Accent);
            g.FillRectangle(accent, new Rectangle(bounds.X, bounds.Y + 4, 3, bounds.Height - 8));
        }

        var textRect = new Rectangle(bounds.X + 14, bounds.Y, bounds.Width - 18, bounds.Height);
        using var flags = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap,
        };
        using var textBrush = new SolidBrush(selected ? p.TextPrimary : p.TextMuted);
        var text = TabPages[index].Text;
        if (!selected)
        {
            g.DrawString(text, Font, textBrush, textRect, flags);
        }
        else
        {
            using var bold = new Font(Font, FontStyle.Bold);
            g.DrawString(text, bold, textBrush, textRect, flags);
        }

        base.OnDrawItem(e);
    }

    protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
    {
        base.ScaleControl(factor, specified);
        ItemSize = new((int)(ItemSize.Width * factor.Width), (int)(ItemSize.Height * factor.Height));
    }
}
