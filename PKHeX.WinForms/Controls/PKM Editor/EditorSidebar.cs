using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PKHeX.WinForms.Theming;

namespace PKHeX.WinForms.Controls;

public sealed class EditorSidebar : Control
{
    private readonly List<Item> _items = [];
    private int _selectedIndex = -1;
    private int _hoverIndex = -1;

    [DefaultValue(40)]
    public int ItemHeight { get; set; } = 40;

    [DefaultValue(7)]
    public int PipSize { get; set; } = 7;

    public event EventHandler? SelectedIndexChanged;

    public EditorSidebar()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
            | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Current.Surface0;
        ForeColor = Theme.Current.TextPrimary;
        TabStop = true;
    }

    [Browsable(false)]
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (value < -1 || value >= _items.Count || _selectedIndex == value)
                return;
            _selectedIndex = value;
            Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void AddItem(string name, string text, Color pip)
        => InsertItem(_items.Count, name, text, pip);

    public void InsertItem(int index, string name, string text, Color pip)
    {
        _items.Insert(index, new Item(name, text, pip));
        if (_selectedIndex == -1)
            _selectedIndex = 0;
        else if (index <= _selectedIndex)
            _selectedIndex++;
        Invalidate();
    }

    public void RemoveItem(string name)
    {
        int i = IndexOf(name);
        if (i < 0)
            return;
        _items.RemoveAt(i);
        if (_items.Count == 0)
            _selectedIndex = -1;
        else if (i < _selectedIndex)
            _selectedIndex--;
        else if (i == _selectedIndex)
            _selectedIndex = Math.Min(_selectedIndex, _items.Count - 1);
        Invalidate();
    }

    public void SelectByName(string name)
    {
        var i = IndexOf(name);
        if (i >= 0)
            SelectedIndex = i;
    }

    public int IndexOf(string name)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Name == name)
                return i;
        }
        return -1;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Theme.Changed += OnThemeChanged;
        BackColor = Theme.Current.Surface0;
        ForeColor = Theme.Current.TextPrimary;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            Theme.Changed -= OnThemeChanged;
        base.Dispose(disposing);
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        BackColor = Theme.Current.Surface0;
        ForeColor = Theme.Current.TextPrimary;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int idx = HitTest(e.Y);
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

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left)
            return;
        int idx = HitTest(e.Y);
        if (idx < 0)
            return;
        Focus();
        SelectedIndex = idx;
    }

    protected override bool IsInputKey(Keys keyData) => keyData is Keys.Up or Keys.Down;

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (_items.Count == 0)
            return;
        switch (e.KeyCode)
        {
            case Keys.Down:
                SelectedIndex = (_selectedIndex + 1) % _items.Count;
                e.Handled = true;
                break;
            case Keys.Up:
                SelectedIndex = (_selectedIndex - 1 + _items.Count) % _items.Count;
                e.Handled = true;
                break;
        }
    }

    private int HitTest(int y)
    {
        if (ItemHeight <= 0)
            return -1;
        int i = y / ItemHeight;
        return (uint)i < (uint)_items.Count ? i : -1;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var p = Theme.Current;
        var g = e.Graphics;
        using (var bg = new SolidBrush(p.Surface0))
            g.FillRectangle(bg, ClientRectangle);

        for (int i = 0; i < _items.Count; i++)
            DrawItem(g, i, p);
    }

    private void DrawItem(Graphics g, int index, ThemePalette p)
    {
        var rect = new Rectangle(0, index * ItemHeight, Width, ItemHeight);
        bool selected = index == _selectedIndex;
        bool hover = index == _hoverIndex;

        var fill = selected ? p.Surface2
            : hover ? p.Mix(p.Surface0, p.Surface2, 0.55f)
            : p.Surface0;
        using (var bg = new SolidBrush(fill))
            g.FillRectangle(bg, rect);

        if (selected)
        {
            using var accent = new SolidBrush(p.Accent);
            g.FillRectangle(accent, new Rectangle(rect.X, rect.Y + 4, 3, rect.Height - 8));
        }

        var item = _items[index];
        DrawPip(g, rect, item.Pip, selected);
        DrawLabel(g, rect, item.Text, p, selected);
    }

    private void DrawPip(Graphics g, Rectangle rect, Color pip, bool selected)
    {
        var prev = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int x = rect.X + 12;
        int y = rect.Y + ((rect.Height - PipSize) / 2);
        var dot = new Rectangle(x, y, PipSize, PipSize);

        using (var brush = new SolidBrush(pip))
            g.FillEllipse(brush, dot);
        if (!selected)
        {
            using var ring = new Pen(Color.FromArgb(120, pip), 1f);
            g.DrawEllipse(ring, new Rectangle(dot.X - 1, dot.Y - 1, dot.Width + 1, dot.Height + 1));
        }

        g.SmoothingMode = prev;
    }

    private void DrawLabel(Graphics g, Rectangle rect, string text, ThemePalette p, bool selected)
    {
        int textX = rect.X + 12 + PipSize + 8;
        var textRect = new Rectangle(textX, rect.Y, rect.Right - textX - 4, rect.Height);

        using var flags = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap,
        };
        using var brush = new SolidBrush(selected ? p.TextPrimary : p.TextMuted);
        if (!selected)
        {
            g.DrawString(text, Font, brush, textRect, flags);
            return;
        }
        using var bold = new Font(Font, FontStyle.Bold);
        g.DrawString(text, bold, brush, textRect, flags);
    }

    private sealed record Item(string Name, string Text, Color Pip);
}
