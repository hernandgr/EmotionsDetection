using System.Collections.Generic;

namespace EmotionsDetection.Models
{
    public class Document
    {
        public double score { get; set; }
        public string id { get; set; }
    }

    public class Error
    {
        public string id { get; set; }
        public string message { get; set; }
    }

    public class TextAnalysisApiResultModel
    {
        public List<Document> documents { get; set; }
        public List<Error> errors { get; set; }
    }
}