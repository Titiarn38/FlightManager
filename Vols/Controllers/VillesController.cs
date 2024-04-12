using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Vols.Models;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace Vols.Controllers
{
    public class VillesController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "8g71zZg6AivLuFnUhFW9TuM55J2KPQcpD63A0tBO",
            BasePath = "https://mspgroup2-a447a-default-rtdb.europe-west1.firebasedatabase.app"
        };

        IFirebaseClient? Client;
        // GET: VillesController
        public ActionResult Index()
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Villes");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Ville>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Ville>(((JProperty)item).Value.ToString()));
                }
            }
            return View(list);
        }

        // GET: VillesController/Details/5
        public ActionResult Details(string id)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Villes/" + id);
            Ville? vil = JsonConvert.DeserializeObject<Ville>(response.Body);

            var list = new List<Vol>();
            FirebaseResponse res = Client.Get("Vols");
            dynamic? data1 = JsonConvert.DeserializeObject<dynamic>(res.Body);

            if (data1 != null)
            {
                foreach (var item in data1)
                {
                    Vol vol = JsonConvert.DeserializeObject<Vol>(((JProperty)item).Value.ToString());

                    // Vérifier si la ville actuelle est soit la ville de départ ou d'arrivée du vol
                    if (vol.VilleDepart == vil.Nom || vol.VilleArrivee == vil.Nom)
                    {
                        list.Add(vol);
                    }
                }
            }

            vil.Vols = list;
            return View(vil);
        }


        // GET: VillesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VillesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ville vil)
        {
            try
            {
                Client = new FireSharp.FirebaseClient(config);
                PushResponse response = Client.Push("Villes/", vil);
                vil.IdVille = response.Result.name;
                SetResponse setresponse = Client.Set("Villes/" + vil.IdVille, vil);

                if (setresponse.StatusCode == System.Net.HttpStatusCode.OK)
                    ModelState.AddModelError(string.Empty, "OK");
                else
                    ModelState.AddModelError(string.Empty, "KO!");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VillesController/Edit/5
        public ActionResult Edit(string id)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Villes/" + id);
            Ville vil = JsonConvert.DeserializeObject<Ville>(response.Body);
            return View(vil);
        }

        // POST: VillesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ville vil)
        {
            Client = new FireSharp.FirebaseClient(config);
            SetResponse response = Client.Set("Villes/" + vil.IdVille, vil);
            return RedirectToAction("Index");
        }

        // GET: VillesController/Delete/5
        public ActionResult Delete(string id)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Villes/" + id);
            Ville vil = JsonConvert.DeserializeObject<Ville>(response.Body);
            return View(vil);
        }

        // POST: VillesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Ville vil)
        {
            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Delete("Villes/" + id);
            return RedirectToAction("Index");
        }
        private List<Vol> GetListVols()
        {
            var list = new List<Vol>();

            Client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = Client.Get("Vols");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Vol>(((JProperty)item).Value.ToString()));
                }
            }
            return list.Where(v => v.IdVille == data.IdVille).ToList();
        }
    }
}
