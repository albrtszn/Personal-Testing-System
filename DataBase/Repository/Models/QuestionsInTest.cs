using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Repository.Models
{
    public class QuestionsInTest
    {
        [Column("idTest")]
        public int IdTest { get; set; }
        [Column("idQuestion")]
        public int IdQuestion { get; set; }
    }
}
