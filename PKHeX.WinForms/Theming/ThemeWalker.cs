using System.Drawing;
using System.Windows.Forms;

namespace PKHeX.WinForms.Theming;

internal static class ThemeWalker
{
    public const string OptOutTag = "notheme";

    public static void Apply(Control root, ThemePalette p)
    {
        if (root is null)
            return;
        ApplyToControl(root, p);

        if (root is Form f && f.ContextMenuStrip is { } ctx)
            StyleContextMenuStrip(ctx, p);

        foreach (Control child in root.Controls)
            Apply(child, p);
    }

    private static void ApplyToControl(Control c, ThemePalette p)
    {
        if (c.Tag is string tag && tag.Contains(OptOutTag))
            return;

        switch (c)
        {
            case Form form:
                form.BackColor = p.Surface0;
                form.ForeColor = p.TextPrimary;
                break;

            case MenuStrip ms:
                ms.BackColor = p.Surface0;
                ms.ForeColor = p.TextPrimary;
                foreach (ToolStripItem i in ms.Items)
                    StyleMenuItem(i, p);
                break;

            case StatusStrip ss:
                ss.BackColor = p.Surface1;
                ss.ForeColor = p.TextPrimary;
                break;

            case ToolStrip ts:
                ts.BackColor = p.Surface0;
                ts.ForeColor = p.TextPrimary;
                foreach (ToolStripItem i in ts.Items)
                    StyleMenuItem(i, p);
                break;

            case TabControl tc:
                tc.BackColor = p.Surface1;
                tc.ForeColor = p.TextPrimary;
                foreach (TabPage tp in tc.TabPages)
                {
                    tp.UseVisualStyleBackColor = false;
                    tp.BackColor = p.Surface1;
                    tp.ForeColor = p.TextPrimary;
                }
                break;

            case GroupBox gb:
                gb.BackColor = Color.Transparent;
                gb.ForeColor = p.TextMuted;
                break;

            case Button btn:
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = p.Border;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.MouseOverBackColor = p.Surface2;
                btn.FlatAppearance.MouseDownBackColor = p.Mix(p.Accent, p.Surface0, 0.85f);
                btn.BackColor = p.Surface1;
                btn.ForeColor = p.TextPrimary;
                btn.UseVisualStyleBackColor = false;
                break;

            case CheckBox cb:
                cb.BackColor = Color.Transparent;
                cb.ForeColor = p.TextPrimary;
                cb.FlatStyle = FlatStyle.Flat;
                cb.FlatAppearance.BorderColor = p.Border;
                cb.FlatAppearance.CheckedBackColor = p.Accent;
                break;

            case RadioButton rb:
                rb.BackColor = Color.Transparent;
                rb.ForeColor = p.TextPrimary;
                rb.FlatStyle = FlatStyle.Flat;
                rb.FlatAppearance.BorderColor = p.Border;
                rb.FlatAppearance.CheckedBackColor = p.Accent;
                break;

            case LinkLabel ll:
                ll.BackColor = Color.Transparent;
                ll.LinkColor = p.Accent;
                ll.ActiveLinkColor = p.AccentHover;
                ll.VisitedLinkColor = p.Accent;
                ll.ForeColor = p.TextPrimary;
                break;

            case Label lbl:
                lbl.BackColor = Color.Transparent;
                lbl.ForeColor = p.TextPrimary;
                break;

            case TextBox tb:
                tb.BackColor = p.Surface1;
                tb.ForeColor = p.TextPrimary;
                tb.BorderStyle = BorderStyle.FixedSingle;
                break;

            case MaskedTextBox mtb:
                mtb.BackColor = p.Surface1;
                mtb.ForeColor = p.TextPrimary;
                mtb.BorderStyle = BorderStyle.FixedSingle;
                break;

            case NumericUpDown nud:
                nud.BackColor = p.Surface1;
                nud.ForeColor = p.TextPrimary;
                nud.BorderStyle = BorderStyle.FixedSingle;
                break;

            case ComboBox cbx:
                cbx.BackColor = p.Surface1;
                cbx.ForeColor = p.TextPrimary;
                cbx.FlatStyle = FlatStyle.Flat;
                break;

            case ListBox lb:
                lb.BackColor = p.Surface1;
                lb.ForeColor = p.TextPrimary;
                lb.BorderStyle = BorderStyle.FixedSingle;
                break;

            case ListView lv:
                lv.BackColor = p.Surface1;
                lv.ForeColor = p.TextPrimary;
                lv.BorderStyle = BorderStyle.FixedSingle;
                break;

            case TreeView tv:
                tv.BackColor = p.Surface1;
                tv.ForeColor = p.TextPrimary;
                tv.BorderStyle = BorderStyle.FixedSingle;
                tv.LineColor = p.Border;
                break;

            case DataGridView dgv:
                StyleDataGridView(dgv, p);
                break;

            case PropertyGrid pg:
                pg.BackColor = p.Surface0;
                pg.ViewBackColor = p.Surface1;
                pg.ViewForeColor = p.TextPrimary;
                pg.ViewBorderColor = p.Border;
                pg.CategoryForeColor = p.TextMuted;
                pg.CategorySplitterColor = p.Border;
                pg.LineColor = p.Border;
                pg.HelpBackColor = p.Surface1;
                pg.HelpForeColor = p.TextPrimary;
                pg.HelpBorderColor = p.Border;
                pg.CommandsBackColor = p.Surface1;
                pg.CommandsForeColor = p.TextPrimary;
                pg.CommandsBorderColor = p.Border;
                pg.DisabledItemForeColor = p.TextDisabled;
                pg.SelectedItemWithFocusBackColor = p.Accent;
                pg.SelectedItemWithFocusForeColor = p.AccentText;
                break;

            case SplitContainer sc:
                sc.BackColor = p.Border;
                sc.Panel1.BackColor = p.Surface0;
                sc.Panel2.BackColor = p.Surface0;
                break;

            case ProgressBar pb:
                pb.BackColor = p.Surface2;
                pb.ForeColor = p.Accent;
                break;

            case TrackBar tb:
                tb.BackColor = p.Surface0;
                tb.ForeColor = p.Accent;
                break;

            case ScrollBar sb:
                sb.BackColor = p.Surface2;
                sb.ForeColor = p.TextPrimary;
                break;

            case PictureBox pic:
                if (pic.BackColor != Color.Transparent)
                    pic.BackColor = p.Surface0;
                break;

            case Panel pan:
                if (pan.BackColor == SystemColors.Control || pan.BackColor == Color.White)
                    pan.BackColor = p.Surface0;
                pan.ForeColor = p.TextPrimary;
                break;

            case UserControl uc:
                uc.BackColor = p.Surface0;
                uc.ForeColor = p.TextPrimary;
                break;
        }

        if (c.ContextMenuStrip is { } attached)
            StyleContextMenuStrip(attached, p);
    }

    private static void StyleMenuItem(ToolStripItem item, ThemePalette p)
    {
        item.ForeColor = p.TextPrimary;
        switch (item)
        {
            case ToolStripMenuItem tsmi:
                tsmi.BackColor = Color.Transparent;
                foreach (ToolStripItem sub in tsmi.DropDownItems)
                    StyleMenuItem(sub, p);
                if (tsmi.DropDown is { } dd)
                {
                    dd.BackColor = p.Surface1;
                    dd.ForeColor = p.TextPrimary;
                }
                break;
            case ToolStripComboBox cb:
                cb.BackColor = p.Surface1;
                cb.ForeColor = p.TextPrimary;
                break;
            case ToolStripTextBox tb:
                tb.BackColor = p.Surface1;
                tb.ForeColor = p.TextPrimary;
                break;
        }
    }

    private static void StyleContextMenuStrip(ContextMenuStrip ctx, ThemePalette p)
    {
        ctx.BackColor = p.Surface1;
        ctx.ForeColor = p.TextPrimary;
        foreach (ToolStripItem i in ctx.Items)
            StyleMenuItem(i, p);
    }

    private static void StyleDataGridView(DataGridView dgv, ThemePalette p)
    {
        dgv.EnableHeadersVisualStyles = false;
        dgv.BackgroundColor = p.Surface1;
        dgv.GridColor = p.Border;
        dgv.BorderStyle = BorderStyle.FixedSingle;
        dgv.ForeColor = p.TextPrimary;
        dgv.DefaultCellStyle.BackColor = p.Surface1;
        dgv.DefaultCellStyle.ForeColor = p.TextPrimary;
        dgv.DefaultCellStyle.SelectionBackColor = p.Accent;
        dgv.DefaultCellStyle.SelectionForeColor = p.AccentText;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = p.Surface2;
        dgv.AlternatingRowsDefaultCellStyle.ForeColor = p.TextPrimary;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = p.Surface0;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = p.TextPrimary;
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = p.Surface2;
        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = p.TextPrimary;
        dgv.RowHeadersDefaultCellStyle.BackColor = p.Surface0;
        dgv.RowHeadersDefaultCellStyle.ForeColor = p.TextMuted;
        dgv.RowHeadersDefaultCellStyle.SelectionBackColor = p.Surface2;
        dgv.RowHeadersDefaultCellStyle.SelectionForeColor = p.TextPrimary;
    }
}
