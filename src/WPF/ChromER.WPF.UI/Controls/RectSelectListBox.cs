using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChromER.WPF.UI
{
    internal class RectSelectListBox : ListBox
    {
        #region Private Fields

        private Canvas _canvas;
        private Rectangle _rectangleShape;
        private Point _initPos;
        private bool _isRectSelected;

        #endregion

        #region Static Constructor

        static RectSelectListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectSelectListBox),
                new FrameworkPropertyMetadata(typeof(RectSelectListBox)));
        }

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvas = GetTemplateChild("PART_Canvas") as Canvas;

            _rectangleShape = new Rectangle()
            {
                Fill = new SolidColorBrush(Color.FromArgb(80, 0, 128, 255)),
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 128, 255)),
                Visibility = Visibility.Collapsed
            };

            _canvas.Children.Add(_rectangleShape);
        }

        #endregion

        #region Protected Methods

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            _initPos = e.GetPosition(_canvas);
            _isRectSelected = true;
            _rectangleShape.Visibility = Visibility.Visible;
            Mouse.Capture(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isRectSelected)
            {
                var pos = e.GetPosition(_canvas);

                var width = Math.Abs(pos.X - _initPos.X);
                var height = Math.Abs(pos.Y - _initPos.Y);

                var left = Math.Min(pos.X, _initPos.X);
                var top = Math.Min(pos.Y, _initPos.Y);

                _rectangleShape.Width = width;
                _rectangleShape.Height = height;
                Canvas.SetLeft(_rectangleShape, left);
                Canvas.SetTop(_rectangleShape, top);


                SelectItems(new RectangleGeometry(new Rect(new Point(left, top), new Size(width, height))));
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            _rectangleShape.Width = 0;
            _rectangleShape.Height = 0;

            _isRectSelected = false;
            _rectangleShape.Visibility = Visibility.Collapsed;
            Mouse.Capture(null);
        }

        #endregion

        #region Private Methods

        private void SelectItems(RectangleGeometry rectangleGeometry)
        {
            foreach (var item in Items)
            {
                var listBoxItem = ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                var pos = listBoxItem.TranslatePoint(new Point(), _canvas);

                pos = new Point(pos.X + 10, pos.Y + 10);

                var itemGeometry =
                    new RectangleGeometry(new Rect(pos, new Size(listBoxItem.ActualWidth - 20, listBoxItem.ActualHeight - 20)));

                var detail = itemGeometry.FillContainsWithDetail(rectangleGeometry);

                if (detail == IntersectionDetail.FullyContains ||
                    detail == IntersectionDetail.Intersects ||
                    detail == IntersectionDetail.FullyInside)
                {
                    listBoxItem.IsSelected = true;
                }
                else
                {
                    listBoxItem.IsSelected = false;
                }
            }
        }

        #endregion
    }
}