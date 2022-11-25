
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadisticaApp.Models
{
    public class Dato
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
