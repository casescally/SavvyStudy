using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SavvyStudy.Models;

namespace SavvyStudy.Controllers
{
    public class WordController : Controller
    {
        // GET: Word
        public ActionResult Index()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Untranslated, Translated, Pronunciation, Phrase, Language FROM Words";

                    var reader = cmd.ExecuteReader();
                    var words = new List<Word>();

                    while(reader.Read())
                    {
                        words.Add(new Word()
                        { 
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Untranslated = reader.GetString(reader.GetOrdinal("Untranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("Translated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("Pronunciation")),
                            Phrase = reader.GetInt32(reader.GetOrdinal("Phrase")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                            });
                    }
                    reader.Close();
return View();

                }
            }

            
        }

        // GET: Word/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Word/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Word/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Word/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Word/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Word/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Word/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}