using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EmotionsDetection.Models;
using EmotionsDetection.Services;

namespace EmotionsDetection.Controllers
{
    public class TextController : Controller
    {
        // GET: Text
        public ActionResult Index()
        {
            return View(new TextModel());
        }

        [HttpPost]
        public async Task<ActionResult> Index(TextModel model)
        {
            var apiService = new TextAnalyticsApiService(ConfigurationManager.AppSettings["TextAnalyticsApiKey"]);
            model.Score = await apiService.Analyze(model.Text);
            ModelState.Clear();

            return View(model);
        }
    }
}