using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SwitchControl
{
    /// <summary>
    /// Interaction logic for Switch.xaml
    /// </summary>
    public partial class SwitchControl : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(bool), typeof(SwitchControl), new FrameworkPropertyMetadata { DefaultValue = false, BindsTwoWayByDefault = true });

        public bool Value
        {
            get
            {
                return (bool)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public SwitchControl()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Value = !Value;
        }
    }
}
