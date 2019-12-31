using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public class RecognizeResultModel
    {
        public UInt64 log_id { get; set; }

        public int words_result_num { get; set; }

        public List<WordsResultModel> words_result { get; set; }
    }
}
