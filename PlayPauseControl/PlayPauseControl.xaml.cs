using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PlayPauseControl
{
    /// <summary>
    /// Interaction logic for PlayPauseControl.xaml
    /// </summary>
    public partial class PlayPauseControl : UserControl
    {
        private IPlayButtonState _state;
        public Brush Color { get; set; } = new SolidColorBrush(Colors.Black);

        public bool IsPause
        {
            get { return (bool)GetValue(IsPauseProperty); }
            set { SetValue(IsPauseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPause.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPauseProperty =
            DependencyProperty.Register("IsPause", typeof(bool), typeof(PlayPauseControl), new FrameworkPropertyMetadata { BindsTwoWayByDefault = true, DefaultValue = true });



        public PlayPauseControl()
        {
            InitializeComponent();

            _state = new PauseState(this);
            DataContext = this;
        }

        private void SetState(IPlayButtonState newState)
        {
            _state = newState;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Toggle();
        }
        private void Toggle()
        {
            LeftLineInnerBottomPoint.BeginAnimation(LineSegment.PointProperty, _state.LeftLineInnerBottomPointAnimation);
            LeftLineInnerUpperPoint.BeginAnimation(LineSegment.PointProperty, _state.LeftLineInnerUpperPointAnimation);

            RightLineOuterUpperPoint.BeginAnimation(PathFigure.StartPointProperty, _state.SecondLineStartPointAnimation);
            RightLineOuterBottomPoint.BeginAnimation(LineSegment.PointProperty, _state.SecondLineFirstPointAnimation);
            RightLineInnerBottomPoint.BeginAnimation(LineSegment.PointProperty, _state.SecondLineSecondPointAnimation);
            RightLineInnerUpperPoint.BeginAnimation(LineSegment.PointProperty, _state.SecondLineThirdPointAnimation);

            _state.Toggle();
            IsPause = !IsPause;
        }

        private interface IPlayButtonState
        {
            void Toggle();
            PointAnimation LeftLineInnerBottomPointAnimation { get; }
            PointAnimation LeftLineInnerUpperPointAnimation { get; }
            PointAnimation SecondLineStartPointAnimation { get; }
            PointAnimation SecondLineFirstPointAnimation { get; }
            PointAnimation SecondLineSecondPointAnimation { get; }
            PointAnimation SecondLineThirdPointAnimation { get; }
        }
        private class PlayState : IPlayButtonState
        {
            public PointAnimation LeftLineInnerBottomPointAnimation { get; } = new PointAnimation
            {
                To = new Point(8, 22),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation LeftLineInnerUpperPointAnimation { get; } = new PointAnimation
            {
                To = new Point(8, 8),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineStartPointAnimation { get; } = new PointAnimation
            {
                To = new Point(15, 15),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineFirstPointAnimation { get; } = new PointAnimation
            {
                To = new Point(15, 15),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineSecondPointAnimation { get; } = new PointAnimation
            {
                To = new Point(8, 22),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineThirdPointAnimation { get; set; } = new PointAnimation
            {
                To = new Point(8, 8),
                Duration = TimeSpan.FromMilliseconds(200)
            };

            private readonly PlayPauseControl _controlInstance;
            private readonly PauseState _pauseState;

            public PlayState(PlayPauseControl controlInstance, PauseState pauseState)
            {
                _controlInstance = controlInstance;
                _pauseState = pauseState;
            }

            public void Toggle()
            {
                _controlInstance.SetState(_pauseState);
            }
        }
        private class PauseState : IPlayButtonState
        {
            public PointAnimation LeftLineInnerBottomPointAnimation { get; } = new PointAnimation
            {
                To = new Point(5, 30),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation LeftLineInnerUpperPointAnimation { get; } = new PointAnimation
            {
                To = new Point(5, 0),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineStartPointAnimation { get; } = new PointAnimation
            {
                To = new Point(15, 0),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineFirstPointAnimation { get; } = new PointAnimation
            {
                To = new Point(15, 30),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineSecondPointAnimation { get; } = new PointAnimation
            {
                To = new Point(10, 30),
                Duration = TimeSpan.FromMilliseconds(200)
            };
            public PointAnimation SecondLineThirdPointAnimation { get; set; } = new PointAnimation
            {
                To = new Point(10, 0),
                Duration = TimeSpan.FromMilliseconds(200)
            };

            private readonly PlayPauseControl _controlInstance;

            public PauseState(PlayPauseControl controlInstance)
            {
                _controlInstance = controlInstance;
            }

            public void Toggle()
            {
                _controlInstance.SetState(new PlayState(_controlInstance, this));
            }
        }
    }
}
