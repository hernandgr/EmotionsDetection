using EmotionsDetection.Models;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EmotionsDetection.Services
{
    public class EmotionApiService
    {
        private readonly EmotionServiceClient _emotionServiceClient;
        
        public EmotionApiService(string subscriptionKey)
        {
            _emotionServiceClient = new EmotionServiceClient(subscriptionKey);
        }

        public async Task<List<EmotionModel>> UploadAndDetectEmotions(HttpPostedFileBase file)
        {
            var emotions = await DetectEmotions(file.InputStream);
            var faces = GetFaces(file.InputStream, emotions.Select(x => x.FaceRectangle));

            return faces.Select((t, i) => new EmotionModel()
            {
                Face = GetImageBase64String(faces.ElementAt(i)),
                EmotionsJson = JsonConvert.SerializeObject(emotions.ElementAt(i).Scores, Formatting.Indented)
            }).ToList();
        }

        private List<byte[]> GetFaces(Stream inputStream, IEnumerable<Microsoft.ProjectOxford.Common.Rectangle> faceRectangles)
        {
            var faces = new List<byte[]>();

            var src = Image.FromStream(inputStream) as Bitmap;

            foreach (var rectangle in faceRectangles)
            {
                var bitmap = src.Clone(new System.Drawing.Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height), src.PixelFormat);
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    faces.Add(ms.ToArray());
                }
            }

            return faces;
        }

        private async Task<IEnumerable<Emotion>> DetectEmotions(Stream imageStream)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    imageStream.CopyTo(stream);
                    stream.Position = 0;

                    var emotions = await _emotionServiceClient.RecognizeAsync(stream);
                    return emotions;
                }
            }
            catch (Exception)
            {
                return Enumerable.Empty<Emotion>();
            }
        }

        private string GetImageBase64String(byte[] imageBytes)
        {
            return "data:image/png;base64, " + Convert.ToBase64String(imageBytes);
        }
    }
}