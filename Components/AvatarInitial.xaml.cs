using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PereMaria.GestorHotel.Components;

public partial class AvatarInitial : UserControl
{
    private static readonly Color[] AvatarColors =
    [
        Color.FromRgb(0x6A, 0x4C, 0x93), // Purple
        Color.FromRgb(0x3D, 0x5A, 0x80), // Blue
        Color.FromRgb(0x29, 0x8C, 0x76), // Teal
        Color.FromRgb(0xE0, 0x7A, 0x5F), // Coral
        Color.FromRgb(0xF4, 0xA2, 0x61), // Orange
        Color.FromRgb(0x81, 0xB2, 0x9A), // Sage
        Color.FromRgb(0x9C, 0x27, 0xB0), // Deep Purple
        Color.FromRgb(0x00, 0x96, 0x88), // Teal Dark
        Color.FromRgb(0xFF, 0x57, 0x22), // Deep Orange
        Color.FromRgb(0x60, 0x7D, 0x8B)  // Blue Grey
    ];

    private static readonly Random RandomGenerator = new();

    #region Dependency Properties

    public static readonly DependencyProperty InitialProperty =
        DependencyProperty.Register(
            nameof(Initial),
            typeof(string),
            typeof(AvatarInitial),
            new PropertyMetadata("A", OnInitialChanged));

    public string Initial
    {
        get => (string)GetValue(InitialProperty);
        set => SetValue(InitialProperty, value);
    }

    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register(
            nameof(Size),
            typeof(double),
            typeof(AvatarInitial),
            new PropertyMetadata(40.0, OnSizeChanged));

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly DependencyProperty UseRandomColorProperty =
        DependencyProperty.Register(
            nameof(UseRandomColor),
            typeof(bool),
            typeof(AvatarInitial),
            new PropertyMetadata(true));

    public bool UseRandomColor
    {
        get => (bool)GetValue(UseRandomColorProperty);
        set => SetValue(UseRandomColorProperty, value);
    }

    #endregion

    public AvatarInitial()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateInitial();
        UpdateSize();

        if (UseRandomColor)
        {
            ApplyRandomColor();
        }
    }

    private static void OnInitialChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AvatarInitial avatar)
        {
            avatar.UpdateInitial();
        }
    }

    private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AvatarInitial avatar)
        {
            avatar.UpdateSize();
        }
    }

    private void UpdateInitial()
    {
        if (InitialText != null && !string.IsNullOrEmpty(Initial))
        {
            InitialText.Text = Initial[0].ToString().ToUpper();
        }
    }

    private void UpdateSize()
    {
        if (RootGrid != null)
        {
            RootGrid.Width = Size;
            RootGrid.Height = Size;

            if (InitialText != null)
            {
                InitialText.FontSize = Size * 0.45;
            }
        }
    }

    private void ApplyRandomColor()
    {
        if (BackgroundEllipse != null)
        {
            var color = GetColorFromInitial(Initial);
            BackgroundEllipse.Fill = new SolidColorBrush(color);
        }
    }


    private static Color GetColorFromInitial(string initial)
    {
        if (string.IsNullOrEmpty(initial))
        {
            return AvatarColors[0];
        }

        int index = char.ToUpper(initial[0]) % AvatarColors.Length;
        return AvatarColors[index];
    }
}
