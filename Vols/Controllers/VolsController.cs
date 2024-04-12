using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Vols.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Vols.Controllers
{
    public class VolsController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "8g71zZg6AivLuFnUhFW9TuM55J2KPQcpD63A0tBO",
            BasePath = "https://mspgroup2-a447a-default-rtdb.europe-west1.firebasedatabase.app"
        };

        IFirebaseClient? Client;
        // GET: VolsController
        public ActionResult Index()
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Vols");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Vol>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Vol>(((JProperty)item).Value.ToString()));
                }
            }
            return View(list);
        }

        // GET: VolsController/Details/5
        public ActionResult Details(string id)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Vols/" + id);
            Vol? vol = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(vol);
        }

        // GET: VolsController/Create
        public ActionResult Create()
        {
            ViewData["IdVille"] = new SelectList(GetListVilles(), "IdVille", "Nom");
            return View();
        }

        // POST: VolsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vol vol)
        {
            try
            {
                Client = new FireSharp.FirebaseClient(config);
                PushResponse response = Client.Push("Vols/", vol);
                vol.IdVol = response.Result.name;
                SetResponse setResponse = Client.Set("Vols/" + vol.IdVol, vol);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    ModelState.AddModelError(string.Empty, "OK");
                else
                    ModelState.AddModelError(string.Empty, "KO !");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Index");
        }

        // GET: VolsController/Edit/5
        public ActionResult Edit(string id)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Vols/" + id);
            Vol vol = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(vol);
        }

        // POST: VolsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vol vol)
        {
            Client = new FireSharp.FirebaseClient(config);
            SetResponse response = Client.Set("Vols/" + vol.IdVol, vol);
            return RedirectToAction("Index");
        }

        // GET: VolsController/Delete/5
        public ActionResult Delete(string id)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Vols/" + id);
            Vol vol = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(vol);
        }

        // POST: VolsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Vol vol)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Delete("Vols/" + id);
            return RedirectToAction("Index");
        }
        private List<Ville> GetListVilles()
        {
            var list = new List<Ville>();

            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Villes");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Ville>(((JProperty)item).Value.ToString()));
                }
            }
            return list;
        }
    }
}
