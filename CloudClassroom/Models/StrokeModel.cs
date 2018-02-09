using System.Collections.Generic;

namespace CloudClassroom.Models
{
    public class StrokeModel
    {
        public string ColorString { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public IList<PointModel> Points { get; set; }
    }
}
