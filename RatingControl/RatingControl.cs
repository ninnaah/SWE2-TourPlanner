using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RatingControl
{

    public partial class Rating : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Rating),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RatingChanged)));


        private static readonly int _maxStars = 5;

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                if (value < 0)
                    SetValue(ValueProperty, 0);

                else if (value > _maxStars)
                    SetValue(ValueProperty, _maxStars);

                else
                    SetValue(ValueProperty, value);
            }
        }

        private static void RatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Rating rating = sender as Rating;
            int newValue = (int)e.NewValue;
            UIElementCollection childs = ((Grid)(rating.Content)).Children;

            ToggleButton button;

            for (int i = 0; i < newValue; i++)
            {
                button = childs[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = true;
            }

            for (int i = newValue; i < childs.Count; i++)
            {
                button = childs[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = false;
            }

        }

        private void ClickEventHandler(object sender, RoutedEventArgs args)
        {
            ToggleButton button = sender as ToggleButton;
            int newValue = int.Parse(button.Tag.ToString());
            Value = newValue;
            
        }

        public Rating()
        {
            InitializeComponent();
        }
    }
}