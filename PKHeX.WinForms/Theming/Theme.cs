using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PKHeX.Core;

namespace PKHeX.WinForms.Theming;

public static class Theme
{
    private static ThemePalette _current = ThemePalette.CreateLight(GameAccentResolver.Default);
    private static ThemeSettings _settings = new();
    private static readonly HashSet<Form> _hooked = [];

    public static ThemePalette Current => _current;
    public static ThemeSettings Settings => _settings;

    public static event EventHandler? Changed;

    public static void Initialize(PKHeXSettings pkSettings)
    {
        _settings = pkSettings.Theme;

        if (pkSettings.Startup.DarkMode && _settings.Mode == ThemeMode.System)
        {
            _settings.Mode = ThemeMode.Dark;
            pkSettings.Startup.DarkMode = false;
        }

        ApplyColorMode();
        RebuildPalette();

        ToolStripManager.Renderer = new ThemedToolStripRenderer();

        Application.Idle += OnIdleHookForms;
    }

    public static void SetMode(ThemeMode mode)
    {
        if (_settings.Mode == mode)
            return;
        _settings.Mode = mode;
        ApplyColorMode();
        RebuildPalette();
        RaiseChanged();
    }

    public static void SetAccent(AccentSource source, Color custom)
    {
        _settings.AccentSource = source;
        if (source == AccentSource.Custom)
            _settings.AccentColor = custom;
        RebuildPalette();
        RaiseChanged();
    }

    public static void OnSaveFileLoaded(SaveFile? sav)
    {
        if (_settings.AccentSource != AccentSource.FromSave)
            return;
        var newAccent = GameAccentResolver.Resolve(sav);
        if (newAccent.ToArgb() == _current.Accent.ToArgb())
            return;
        _current = _current.WithAccent(newAccent);
        RaiseChanged();
    }

    public static void Apply(Control root) => ThemeWalker.Apply(root, _current);

    private static void ApplyColorMode()
    {
#pragma warning disable WFO5001
        var mode = _settings.Mode switch
        {
            ThemeMode.Light => SystemColorMode.Classic,
            ThemeMode.Dark => SystemColorMode.Dark,
            _ => SystemColorMode.System,
        };
        Application.SetColorMode(mode);
#pragma warning restore WFO5001
    }

    private static void RebuildPalette()
    {
        var accent = _settings.AccentSource switch
        {
            AccentSource.Custom => _settings.AccentColor,
            AccentSource.FromSave => _current.Accent,
            _ => GameAccentResolver.Default,
        };

        bool dark = _settings.Mode switch
        {
            ThemeMode.Dark => true,
            ThemeMode.Light => false,
            _ => SystemColors.Control.GetBrightness() < 0.5f,
        };

        _current = dark ? ThemePalette.CreateDark(accent) : ThemePalette.CreateLight(accent);
    }

    private static void RaiseChanged()
    {
        foreach (var f in _hooked)
        {
            if (!f.IsDisposed)
                Apply(f);
        }
        Changed?.Invoke(null, EventArgs.Empty);
    }

    private static void OnIdleHookForms(object? sender, EventArgs e)
    {
        // Application.OpenForms spans threads; skip forms on other UI threads.
        foreach (Form f in Application.OpenForms)
        {
            if (f.IsDisposed || f.InvokeRequired)
                continue;
            if (!_hooked.Add(f))
                continue;
            HookForm(f);
        }
    }

    private static void HookForm(Form f)
    {
        try
        {
            if (f.IsHandleCreated)
                Apply(f);
            else
                f.HandleCreated += (_, _) => TryApply(f);
        }
        catch
        {
        }

        f.Disposed += (_, _) => _hooked.Remove(f);
    }

    private static void TryApply(Control c)
    {
        try { Apply(c); }
        catch { }
    }
}
