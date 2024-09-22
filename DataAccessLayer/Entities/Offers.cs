using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Offers
    {



        public int offerId { get; set; }
        public string? taskId { get; set; }
        public string? TranslatorName { get; set; }
        public string? deadline { get; set; }
        public string? description { get; set; }
        public string? price { get; set; }
    }
}
