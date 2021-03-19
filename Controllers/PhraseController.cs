﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SavvyStudy.Models;

namespace SavvyStudy.Controllers
{
    public class PhraseController : Controller
    {
                private readonly IConfiguration _config;
        public PhraseController(IConfiguration config)
        {
            _config = config;
        }


        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }


        // GET: Phrase
        public ActionResult Index()
        {
            return View();
        }

        // GET: Phrase/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Phrase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Phrase/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Phrase phrase)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Phrases (Untranslated, Translated, Pronunciation, Language)
                                            OUTPUT INSERTED.Id
                                            VALUES (@untranslated, @translated, @pronunciation, @language)";

                        cmd.Parameters.Add(new SqlParameter("@untranslated", phrase.Untranslated));
                        cmd.Parameters.Add(new SqlParameter("@translated", phrase.Translated));
                        cmd.Parameters.Add(new SqlParameter("@pronunciation", phrase.Pronunciation));
                        cmd.Parameters.Add(new SqlParameter("@language", phrase.Language));

                        var id = (int)cmd.ExecuteScalar();

                        phrase.Id = id;

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Phrase/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Phrase/Edit/5
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

        // GET: Phrase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Phrase/Delete/5
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