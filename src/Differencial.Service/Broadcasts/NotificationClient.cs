using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Service
{
    public class NotificationClient
    {
        public NotificationClient()
        {
            Groups = new HashSet<string>();
        }
        public int IdOperador { get; set; }
        public string Name { get; set; }
        public virtual ICollection<string> Groups { get; set; }
    }
}
