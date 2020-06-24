using EmotionPlatzi.Data;
using EmotionPlatzi.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Controllers
{
    public class EmoUploaderController : Controller
    {
        string serverFolderPath;
        EmotionHelper emotionHelper;
        string key;
        EmotionPlatziDataContext db = new EmotionPlatziDataContext();

        public EmoUploaderController()
        {
            serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            key = ConfigurationManager.AppSettings["EMOTION_KEY"];
            emotionHelper = new EmotionHelper(key);
        }

        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //public async ActionResult Index(HttpPostedFileBase file)
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {
            //if (file != null && file.ContentLength > 0)
            //{ }
            if (file?.ContentLength > 0)
            {
                var pictureName = Guid.NewGuid().ToString();
                pictureName += Path.GetExtension(file.FileName);

                var route = Server.MapPath(serverFolderPath);
                route += "/" + pictureName;

                file.SaveAs(route);

                var emoPicture = await emotionHelper.DetectAndExtractFacesAsync(file.InputStream);

                emoPicture.Name = file.FileName;
                //emoPicture.Path = serverFolderPath + "/" + pictureName;
                emoPicture.Path = $"{serverFolderPath}/{pictureName}";

                db.EmoPictures.Add(emoPicture);
                await db.SaveChangesAsync();

                return RedirectToAction("Details", "EmoPictures", new { Id = emoPicture.Id });
            }
            return View();
        }
    }
}