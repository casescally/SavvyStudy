using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SavvyStudy.Models.ViewModels
{
    public class WordEditViewModel
    {
        public int Id { get; set; }
        public string Untranslated { get; set; }
        public string Translated { get; set; }
        public string Pronunciation { get; set; }
        public string Phrase { get; set; }
        public string Language { get; set; }
    }
}
