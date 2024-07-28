using Blazorise.Charts;

namespace IGB.Models
{
    public class LineChartData<TItem>
    {
        public LineChart<TItem> LineChart { get; set; }
        public string[] Labels { get; set; }
        public LineChartDataset<TItem> Dataset { get; set; }
    }
}
