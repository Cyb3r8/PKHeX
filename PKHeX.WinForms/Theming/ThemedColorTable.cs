using System.Drawing;
using System.Windows.Forms;

namespace PKHeX.WinForms.Theming;

internal sealed class ThemedColorTable : ProfessionalColorTable
{
    private static ThemePalette P => Theme.Current;

    public override Color MenuStripGradientBegin => P.Surface0;
    public override Color MenuStripGradientEnd => P.Surface0;
    public override Color ToolStripGradientBegin => P.Surface0;
    public override Color ToolStripGradientMiddle => P.Surface0;
    public override Color ToolStripGradientEnd => P.Surface0;

    public override Color MenuItemSelected => P.Surface2;
    public override Color MenuItemSelectedGradientBegin => P.Surface2;
    public override Color MenuItemSelectedGradientEnd => P.Surface2;
    public override Color MenuItemBorder => P.Surface2;

    public override Color MenuItemPressedGradientBegin => Mix(P.Accent, P.Surface0, 0.85f);
    public override Color MenuItemPressedGradientMiddle => Mix(P.Accent, P.Surface0, 0.85f);
    public override Color MenuItemPressedGradientEnd => Mix(P.Accent, P.Surface0, 0.85f);

    public override Color MenuBorder => P.Border;
    public override Color ToolStripBorder => P.Border;
    public override Color SeparatorDark => P.Border;
    public override Color SeparatorLight => P.Border;

    public override Color ImageMarginGradientBegin => P.Surface1;
    public override Color ImageMarginGradientMiddle => P.Surface1;
    public override Color ImageMarginGradientEnd => P.Surface1;
    public override Color ImageMarginRevealedGradientBegin => P.Surface1;
    public override Color ImageMarginRevealedGradientMiddle => P.Surface1;
    public override Color ImageMarginRevealedGradientEnd => P.Surface1;

    public override Color ToolStripDropDownBackground => P.Surface1;
    public override Color ButtonSelectedHighlight => P.Accent;
    public override Color ButtonSelectedBorder => P.Accent;
    public override Color CheckBackground => Mix(P.Accent, P.Surface0, 0.7f);
    public override Color CheckSelectedBackground => P.Accent;
    public override Color CheckPressedBackground => P.Accent;

    public override Color GripDark => P.Border;
    public override Color GripLight => P.Border;

    public override Color RaftingContainerGradientBegin => P.Surface1;
    public override Color RaftingContainerGradientEnd => P.Surface1;

    public override Color StatusStripGradientBegin => P.Surface1;
    public override Color StatusStripGradientEnd => P.Surface1;

    private static Color Mix(Color a, Color b, float t) => Theme.Current.Mix(a, b, t);
}
