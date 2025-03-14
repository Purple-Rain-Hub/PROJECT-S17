using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PROJECT_S17.Models;

[Table("ANAGRAFICA")]
public partial class Anagrafica
{
    [Key]
    [Column("idanagrafica")]
    public Guid Idanagrafica { get; set; }
    [Display(Name = "Cognome")]
    [StringLength(40)]
    public string Cognome { get; set; } = null!;
    [Display(Name = "Nome")]
    [StringLength(40)]
    public string Nome { get; set; } = null!;

    [StringLength(100)]
    public string Indirizzo { get; set; } = null!;

    [StringLength(50)]
    public string Citta { get; set; } = null!;

    [Column("CAP")]
    [StringLength(5)]
    public string Cap { get; set; } = null!;
    [Display(Name = "Codice Fiscale")]
    [Column("Cod_Fisc")]
    [StringLength(16)]
    public string CodFisc { get; set; } = null!;

    [InverseProperty("Anagrafica")]
    public virtual ICollection<Verbale> Verbale { get; set; } = new List<Verbale>();
}
