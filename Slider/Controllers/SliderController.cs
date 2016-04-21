using Slider.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Slider.Controllers
{
    public class SliderController : Controller
    {
        // GET: Slider
        public ActionResult Index()
        {
            using (SliderContext db=new SliderContext())
            {
                return View(db.gallery.ToList());
            }
            //return View();
        }


        public ActionResult AddImage()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase ImagePath)
        {
            if (ImagePath != null)
            {
                Image img = Image.FromStream(ImagePath.InputStream);
                if ((img.Width != 800) || (img.Height != 356))
                {
                    ModelState.AddModelError("", "Image resolution must be 800 x 356 pixels");
                    return View();
                }
                string pic = Path.GetFileName(ImagePath.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), pic);
                ImagePath.SaveAs(path);
                using (SliderContext db=new SliderContext())
                {
                    Gallery gallery = new Gallery
                    {
                        ImagePath = "~/Content/images/" + pic
                    };
                    db.gallery.Add(gallery);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteImages()
        {
            using (SliderContext db =new SliderContext())
            {
                return View(db.gallery.ToList());
            }
        }

        [HttpPost]
        public ActionResult DeleteImages(IEnumerable<int> ImagesIds)
        {
            using (SliderContext db = new SliderContext())
            {
                foreach(var id in ImagesIds)
                {
                    var image = db.gallery.Single(s => s.GalleryID == id);
                    string imgPath = Server.MapPath(image.ImagePath);
                    db.gallery.Remove(image);
                    if (System.IO.File.Exists(imgPath))
                        System.IO.File.Delete(imgPath);
                }
                db.SaveChanges();
            }
            return RedirectToAction("DeleteImages");
        }
    }
}