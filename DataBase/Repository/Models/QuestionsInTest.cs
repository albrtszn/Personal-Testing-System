using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Repository.Models
{
    [PrimaryKey(nameof(IdTest), nameof(IdQuestion))]
    public class QuestionsInTest
    {
        //[Key]
        [Column("idTest")]
        [ForeignKey("Test")]
        public int IdTest { get; set; }
        //[Key]
        [Column("idQuestion")]
        [ForeignKey("Question")]
        public int IdQuestion { get; set; }

        //[InverseProperty("Tests")]
        public virtual Test? Test { get; set; }
        //[InverseProperty("Questions")]
        public virtual Question? Question { get; set; }
    }
}
