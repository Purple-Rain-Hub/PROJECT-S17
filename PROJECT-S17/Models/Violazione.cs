using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PROJECT_S17.Models;

[Table("VIOLAZIONE")]
public partial class Violazione
{
    [Key]
    [Column("idviolazione")]
    public Guid Idviolazione { get; set; }

    [Column("descrizione")]
    [StringLength(300)]
    public string Descrizione { get; set; } = null!;

    [InverseProperty("Violazione")]
    public virtual ICollection<Verbale> Verbale { get; set; } = new List<Verbale>();
}
