﻿using EmotionsDetection.Services;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmotionsDetection.Controllers
{
    public class ImagesController : Controller
    {
        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return RedirectToAction("Index");
            }

            var apiService = new EmotionApiService(ConfigurationManager.AppSettings["EmotionApiKey"]);
            var resultImages = await apiService.UploadAndDetectEmotions(file);
            
            return View("Results", resultImages);
        }
    }
}