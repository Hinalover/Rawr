using System;
using System.Collections.Generic;
#if !SILVERLIGHT
using System.Printing;
#endif
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
#if SILVERLIGHT
using System.Windows.Printing;
#endif

namespace Rawr.UI
{
    public static class UIUtilities
    {
#if !SILVERLIGHT
        public static void Print(this Panel panel, PrintDialog printDialog)
        {
           bool? proceed = true;

           if (printDialog == null)
           {
              printDialog = new PrintDialog();
              proceed = printDialog.ShowDialog();
           }

           if (proceed == true)
           {
              PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

              if (capabilities.PageImageableArea != null)
              {
                 // scale the area to the smaller of width zoom or height zoom
                 double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth/panel.ActualWidth,
                                         capabilities.PageImageableArea.ExtentHeight/panel.ActualHeight);

                 // remember current values so we can restore later, then rescale
                 Transform oldTransform = panel.LayoutTransform;
                 Size oldSize = new Size(panel.ActualWidth, panel.ActualHeight);

                 panel.LayoutTransform = new ScaleTransform(scale, scale);
                 Size newSize = new Size(capabilities.PageImageableArea.ExtentWidth,
                                         capabilities.PageImageableArea.ExtentHeight);
                 panel.Measure(newSize);

                 // might re-flow depending on the needs of the new size
                 panel.Arrange(
                    new Rect(
                       new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight),
                       newSize));

                 // do the work
                 printDialog.PrintVisual(panel, "Print Results");

                 // back to where we were
                 panel.LayoutTransform = oldTransform;
                 panel.Measure(oldSize);
                 panel.Arrange(new Rect(new Point(0, 0), oldSize));
              }
           }
        }
#endif
    }
}