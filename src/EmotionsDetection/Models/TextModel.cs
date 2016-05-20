using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmotionsDetection.Models
{
    public class TextModel
    {
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public double Score { get; set; }
    }
}