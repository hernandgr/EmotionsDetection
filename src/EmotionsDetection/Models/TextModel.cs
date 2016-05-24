using System.ComponentModel.DataAnnotations;

namespace EmotionsDetection.Models
{
    public class TextModel
    {
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public double? Score { get; set; }
    }
}