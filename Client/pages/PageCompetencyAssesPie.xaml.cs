using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageCompetencyAssesPie.xaml
    /// </summary>
    public partial class PageCompetencyAssesPie : Page
    {
        public PageCompetencyAssesPie()
        {
            InitializeComponent();
            Loaded += PageCompetencyAssesPie_Loaded;

        }

        private void PageCompetencyAssesPie_Loaded(object sender, RoutedEventArgs e)
        {
            List<PieSlice> slices = new List<PieSlice>
            {
                new PieSlice() { Value = 0.4, FillColor = ScottPlot.Colors.Pink, Label = "Pink" },
                new PieSlice() { Value = 0.3, FillColor = ScottPlot.Colors.LightBlue, Label = "LightBlue" },
                new PieSlice() { Value = 0.1, FillColor = ScottPlot.Colors.LightGreen, Label = "LightGreen" },
                new PieSlice() { Value = 0.2, FillColor = ScottPlot.Color.FromHex("#FAFAD2"), Label = "LightGoldenrodYellow" },
            };

            myPlot.Plot.HideGrid();
            myPlot.Plot.Axes.SquareUnits();
            
            var pie = myPlot.Plot.Add.Pie(slices);
            pie.DonutFraction = .28;
            myPlot.Plot.Legend.IsVisible = false;
            var line1 = myPlot.Plot.Add.Circle(0, 0, 0.52);
            line1.LineColor = ScottPlot.Colors.Black;
            line1.LinePattern = LinePattern.DenselyDashed;
            var line2 = myPlot.Plot.Add.Circle(0, 0, 0.76);
            line2.LineColor = ScottPlot.Colors.Black;
            line2.LinePattern = LinePattern.DenselyDashed;
            double yy = Math.Sin(2 * Math.PI * -0.4);
            double xx = Math.Cos(2 * Math.PI * -0.4);
            myPlot.Plot.Add.Line(0, 0, xx, yy);
            myPlot.Plot.Axes.SetLimitsY(-1.1, 1.1);
            myPlot.Plot.Axes.SetLimitsX(-1, 1);
            myPlot.Plot.Layout.Frameless();
            myPlot.Refresh();
        }
    }
}
