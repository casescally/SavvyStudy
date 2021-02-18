using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SavvyStudy.Models
{
    public class WordUntranslatedPracticeViewModel
    {
        public int Id { get; set; }
        public string Untranslated { get; set; }
        public string Translated { get; set; }
        public string Pronunciation { get; set; }
        public string Language { get; set; }
        public List<Word> NextWords { get; set; }
    }
}
