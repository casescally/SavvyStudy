﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SavvyStudy.Models;
using Microsoft.Extensions.Configuration;

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

        [HttpGet]
public ActionResult Search(string search)
{
if (!String.IsNullOrEmpty(search))
        {

                

                            using(SqlConnection conn = Connection)
            {



                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {

cmd.Parameters.Add(new SqlParameter("@search", search));
                    cmd.CommandText = "SELECT w.Id, w.Untranslated, w.Translated, w.Pronunciation, w.Language FROM Words w WHERE w.Translated = @search";

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
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                            };
                        words.Add(word);
                    }
                    reader.Close();

               return View(words);
}
                        

                    }
      } else return View();
}













                

        // GET: Word
        public ActionResult Index()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Untranslated, Translated, Pronunciation, Language FROM Words";

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

cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.CommandText = @"
                                      SELECT w.Id,
                                        w.Untranslated,
                                        w.Translated,
                                        w.Pronunciation,
                                        w.Language
                                      FROM Words w
                                      WHERE w.Id = @id
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
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                    }
                    reader.Close();
                    return View(word);
                }
            }
        }


        public RedirectToActionResult WordPractice()

        {
                        Random rnd = new Random();
                int caseSwitch  = rnd.Next(1, 3);

                       Word randomword = GetRandomWord();
           int id = randomword.Id;


            switch (caseSwitch)
      {
        case 1:


                    return RedirectToAction("WordUntranslatedTypedPractice", new { id = id });
                    break;
        case 2:
                    return RedirectToAction("WordTranslatedTypedPractice", new { id = id });
              break;

        default:
                    return null;
      }
        }


        // GET: Word/WordUntranslatedTypedPractice/5
        public ActionResult WordUntranslatedTypedPractice(int id)

        {


            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.CommandText = @"
                                      SELECT w.Id,
                                        w.Untranslated,
                                        w.Translated,
                                        w.Pronunciation,
                                        w.Language
                                      FROM Words w
                                      WHERE w.Id = @id
                                      ";
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    WordUntranslatedPracticeViewModel wordVM = null;


                    while (reader.Read())
                    {
                        wordVM = new WordUntranslatedPracticeViewModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Untranslated = reader.GetString(reader.GetOrdinal("Untranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("Translated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("Pronunciation")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };


            wordVM.NextWords = GetNextWords(id); //retrieves from database


                    }
                    reader.Close();
                    return View(wordVM);
                }
            }
        }


                // POST: Word/WordUntranslatedTypedPractice/
                [HttpPost]
        public ActionResult WordUntranslatedTypedPractice(int id, IFormCollection collection)
        {
            string untranslatedGuess = collection["untranslatedGuess"];


            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {

cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.CommandText = @"
                                      SELECT w.Id,
                                        w.Untranslated,
                                        w.Translated,
                                        w.Pronunciation,
                                        w.Language
                                      FROM Words w
                                      WHERE w.Id = @id
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
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                    }
                    reader.Close();
                    
                    if (word.Untranslated == untranslatedGuess)
                    {
                        return View("WordUntranslatedTypedPracticeCorrect", word);
                    } else
                    {
                        return View("WordUntranslatedTypedPracticeIncorrect", word);
                    }
                    
                    
                }
            }
        }



        // GET: Word/WordTranslatedTypedPractice/5
        public ActionResult WordTranslatedTypedPractice(int id)

        {

            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {

cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.CommandText = @"
                                      SELECT w.Id,
                                        w.Untranslated,
                                        w.Translated,
                                        w.Pronunciation,
                                        w.Language
                                      FROM Words w
                                      WHERE w.Id = @id
                                      ";
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    WordUntranslatedPracticeViewModel wordVM = null;

                    while (reader.Read())
                    {
                        wordVM = new WordUntranslatedPracticeViewModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Untranslated = reader.GetString(reader.GetOrdinal("Untranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("Translated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("Pronunciation")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };


                    wordVM.NextWords = GetNextWords(id); //retrieves from database


                    }
                    reader.Close();
                    return View(wordVM);
                }
            }
        }



                // POST: Word/TranslatedTypedPractice/
                [HttpPost]
        public ActionResult WordTranslatedTypedPractice(int id, IFormCollection collection)
        {
            string translatedGuess = collection["translatedGuess"];


            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {

cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.CommandText = @"
                                      SELECT w.Id,
                                        w.Untranslated,
                                        w.Translated,
                                        w.Pronunciation,
                                        w.Language
                                      FROM Words w
                                      WHERE w.Id = @id
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
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                    }
                    reader.Close();
                    
                    if (word.Translated == translatedGuess)
                    {
                        return View("WordTranslatedTypedPracticeCorrect", word);
                    } else
                    {
                        return View("WordTranslatedTypedPracticeIncorrect", word);
                    }
                    
                    
                }
            }
        }

        private List<Word> GetNextWords(int currentWordId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.Parameters.Add(new SqlParameter("@CurrentWordId", currentWordId));

                            cmd.CommandText = @"
                                      SELECT w.Id AS NextWordId,
                                        w.Untranslated AS NextWordUntranslated,
                                        w.Translated AS NextWordTranslated,
                                        w.Pronunciation AS NextWordPronunciation,
                                        w.Language AS NextWordLanguage
                                        FROM Words w
                                      WHERE w.Id != @CurrentWordId
                                      ";

                    var reader = cmd.ExecuteReader();

                    var nextWords = new List<Word>();

                    while (reader.Read())
                    {
                        var nextWord = new Word()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("NextWordId")),
                            Untranslated = reader.GetString(reader.GetOrdinal("NextWordUntranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("NextWordTranslated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("NextWordPronunciation")),
                            Language = reader.GetString(reader.GetOrdinal("NextWordLanguage"))
                        };

                        nextWords.Add(nextWord);

                    }
                    reader.Close();
                    return nextWords;
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
        public ActionResult Create(Word word, Phrase phrase)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Words (Untranslated, Translated, Pronunciation, Language)
                                            OUTPUT INSERTED.Id
                                            VALUES (@untranslated, @translated, @pronunciation, @language)";

                        cmd.Parameters.Add(new SqlParameter("@untranslated", word.Untranslated));
                        cmd.Parameters.Add(new SqlParameter("@translated", word.Translated));
                        cmd.Parameters.Add(new SqlParameter("@pronunciation", word.Pronunciation));
                        cmd.Parameters.Add(new SqlParameter("@language", word.Language));

                        var id = (int)cmd.ExecuteScalar();

                        word.Id = id;

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Word/Edit/5
        public ActionResult Edit(int id)
        {
            var word = GetWordById(id);

            return View(word);
        }

        // POST: Word/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Word word)
{
                try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"UPDATE Words
                                            SET Untranslated = @untranslated, Translated = @translated, Pronunciation = @pronunciation, Language = @language
                                            WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@untranslated", word.Untranslated));
                        cmd.Parameters.Add(new SqlParameter("@translated", word.Translated));
                        cmd.Parameters.Add(new SqlParameter("@pronunciation", word.Pronunciation));
                        cmd.Parameters.Add(new SqlParameter("@language", word.Language));
                        cmd.Parameters.Add(new SqlParameter("@id", word.Id));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)

                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }

            catch (Exception)

            {
                if (!WordExists(id))

                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            bool WordExists(int id)

        {

            using (SqlConnection conn = Connection)

            {

                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())

                {

                    cmd.CommandText = @"Select Id, Untranslated, Translated, Pronunciation, Language
                                        FROM Words
                                        Where Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();

                } 
            }
        }
    }

        // GET: Word/Delete/5
        public ActionResult Delete(int id)
        {
            var word = GetWordById(id);
            return View(word);
        }

        // POST: Word/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWord([FromRoute] int id)
        {
            try
            {
                using(SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM Words WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }





                private Word GetRandomWord()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "USE SavvyStudy SELECT TOP 1 * FROM Words ORDER BY NEWID();";


                    var reader = cmd.ExecuteReader();
                    Word word = null;

                    if (reader.Read())
                    {
                        word = new Word()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Untranslated = reader.GetString(reader.GetOrdinal("Untranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("Translated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("Pronunciation")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                    }
                    reader.Close();
                    return word;
                }
            }
        }


                private Word GetWordById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Untranslated, Translated, Pronunciation, Language FROM Words WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    var reader = cmd.ExecuteReader();
                    Word word = null;

                    if (reader.Read())
                    {
                        word = new Word()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Untranslated = reader.GetString(reader.GetOrdinal("Untranslated")),
                            Translated = reader.GetString(reader.GetOrdinal("Translated")),
                            Pronunciation = reader.GetString(reader.GetOrdinal("Pronunciation")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                    }
                    reader.Close();
                    return word;
                }
            }
        }

    }
}