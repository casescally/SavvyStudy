using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SavvyStudy.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace SavvyStudy.Controllers
{
    public class WordController : Controller
    {
        private readonly IConfiguration _config;
        public WordController(IConfiguration config)
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
                        var word = new Word()
                        { 
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Untranslated = reader.GetString(reader.GetOrdinal("Untranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("Translated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("Pronunciation")),
                            Phrase = reader.GetInt32(reader.GetOrdinal("Phrase")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                            };
                        words.Add(word);
                    }
                    reader.Close();
                    return View(words);
                }
            }
        }

        // GET: Word/Details/5
        public ActionResult Details(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                      SELECT W.Id,
                                        w.Untranslated,
                                        w.Translated,
                                        w.Pronunciation,
                                        w.Phrase,
                                        w.Language
                                      FROM Words w
                                      ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Word word = null;
                    while (reader.Read())
                    {
                        word = new Word
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Untranslated = reader.GetString(reader.GetOrdinal("Untranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("Translated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("Pronunciation")),
                            Phrase = reader.GetInt32(reader.GetOrdinal("Phrase")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                    }
                    reader.Close();
                    return View(word);
                }
            }
        }

        // GET: Word/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Word/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Word word)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Words (Untranslated, Translated, Pronunciation, Phrase, Language)
                                            OUTPUT INSERTED.Id
                                            VALUES (@untranslated, @translated, @pronunciation, @phrase, @language)";
                        cmd.Parameters.Add(new SqlParameter("@untranslated", word.Untranslated));
                        cmd.Parameters.Add(new SqlParameter("@translated", word.Translated));
                        cmd.Parameters.Add(new SqlParameter("@pronunciation", word.Pronunciation));
                        cmd.Parameters.Add(new SqlParameter("@phrase", word.Phrase));
                        cmd.Parameters.Add(new SqlParameter("@language", word.Language));

                        var id = (int)cmd.ExecuteScalar();
                        word.Id = id;

                        return RedirectToAction(nameof(Index));
                    }
                }
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