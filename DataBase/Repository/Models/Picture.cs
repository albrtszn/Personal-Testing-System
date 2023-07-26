using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Picture")]
public partial class Picture
{
    [Key]
    [Column("id")]
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [Column("image")]
    public byte[]? Image { get; set; }

    [InverseProperty("IdPictureNavigation")]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [InverseProperty("IdPictureNavigation")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
