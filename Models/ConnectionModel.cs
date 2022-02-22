using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDynamicDbApp.Models
{
    [Serializable]
    public class ConnectionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public string Description { get; set; }
    }
}
