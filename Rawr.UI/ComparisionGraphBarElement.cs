using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.UI
{
    public class ComparisionGraphBarElement : FrameworkElement
    {
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(System.Collections.Generic.List<float>),
            typeof(ComparisionGraphBarElement), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register("Colors", typeof(System.Collections.Generic.List<Color>),
            typeof(ComparisionGraphBarElement), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily),
            typeof(ComparisionGraphBarElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, 
                FrameworkPropertyMetadataOptions.Inherits, FontFamilyChanged));

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof(FontWeight),
            typeof(ComparisionGraphBarElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, 
                FrameworkPropertyMetadataOptions.Inherits, FontFamilyChanged));
        
        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle),
            typeof(ComparisionGraphBarElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle,
            FrameworkPropertyMetadataOptions.Inherits, FontFamilyChanged));

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double),
            typeof(ComparisionGraphBarElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize,
            FrameworkPropertyMetadataOptions.Inherits));

        IEnumerable<float> values;
        IEnumerable<Color> colors;

        Typeface Typeface;
        static void FontFamilyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs prop)
        {
            ComparisionGraphBarElement self = sender as ComparisionGraphBarElement;
            self.Typeface = new Typeface(self.FontFamily, self.FontStyle, self.FontWeight, FontStretches.Normal);
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set{SetValue(FontFamilyProperty, value);}
        }
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }
        public double MaxScale
        {
            get;
            set;
        }
        public void Init(IEnumerable<float> values, IEnumerable<Color> colors)
        {
            this.values = values;
            this.colors = colors;

            Typeface = new Typeface(FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);            
        }
        public ComparisionGraphBarElement()
        {
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            List<float> vals = new List<float>();
            if (values == null || colors == null) return;

            float total = 0;
            foreach (float v in values)
            {
                vals.Add(v);
                total += v;

            }
            //base.OnRender(drawingContext);
            double left = 0;
            int i = 0;

            Rect all = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            //drawingContext.DrawRectangle(Brushes.Green, null, all);

            double scale = this.ActualWidth / MaxScale;

            Brush fillBrush = this.FindResource("FillBrush") as Brush;
            Pen fillStroke = new Pen(this.FindResource("StrokeBrush") as Brush, 1);
            fillStroke.Freeze();

            // create only one of these and just change the properties
            Point p = new Point();
            foreach (Color c in this.colors)
            {
                Brush brush = new SolidColorBrush(c);
                brush.Freeze();
                Rect rect = new Rect(left * scale, 0, Math.Abs(vals[i]) * scale, this.ActualHeight);
                Geometry clipGeo = new RectangleGeometry(rect);
                
                drawingContext.PushClip(clipGeo);


                FormattedText ftxt = new FormattedText(
                       vals[i].ToString(), System.Globalization.CultureInfo.CurrentUICulture,
                        System.Windows.FlowDirection.LeftToRight,
                        this.Typeface, FontSize, Brushes.Black);

                p.X = left * scale + (vals[i] * scale - ftxt.Width) / 2;
                p.Y = (this.ActualHeight - ftxt.Height) / 2;

                drawingContext.DrawText(ftxt, p);

                drawingContext.Pop();


                // TODO: may be faster to push this once and iterate over rects
                drawingContext.PushOpacity(0.5);
                drawingContext.DrawGeometry(brush, null, clipGeo);
                drawingContext.Pop();

                drawingContext.DrawGeometry(fillBrush, fillStroke, clipGeo);                                
                

                left += vals[i];
                i++;
            }            


            // TODO: would be faster to generate this only as needed
            if (totalTxt == null)
            {

                totalTxt = new FormattedText(total.ToString(), System.Globalization.CultureInfo.CurrentUICulture,
                        System.Windows.FlowDirection.LeftToRight,
                        this.Typeface, FontSize, Brushes.Black);
            }

            p.X = left * scale  + 4;
            p.Y = (this.ActualHeight - totalTxt.Height) / 2;
            drawingContext.DrawText(totalTxt, p);

        }

        FormattedText totalTxt;
    }
}
