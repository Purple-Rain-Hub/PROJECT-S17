using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PROJECT_S17.Models;

[PrimaryKey("Idverbale", "Idviolazione", "Idanagrafica")]
[Table("VERBALE")]
public partial class Verbale
{
    [Key]
    [Column("idverbale")]
    public int Idverbale { get; set; }
    [Display(Name = "Data violazione")]
    [Column(TypeName = "datetime")]
    public DateTime DataViolazione { get; set; }
    [Display(Name = "Indirizzo violazione")]
    [StringLength(100)]
    public string IndirizzoViolazione { get; set; } = null!;
    [Display(Name = "Nominativo agente")]
    [Column("Nominativo_Agente")]
    [StringLength(40)]
    public string NominativoAgente { get; set; } = null!;
    [Display(Name = "Data trascrizione verbale")]
    [Column(TypeName = "datetime")]
    public DateTime DataTrascrizioneVerbale { get; set; }
    [Display(Name = "Importo dovuto")]
    [Column(TypeName = "money")]
    public decimal Importo { get; set; }
    [Display(Name = "Punti Decurtati")]
    public int? DecurtamentoPunti { get; set; }

    [Key]
    [Column("idanagrafica")]
    public Guid Idanagrafica { get; set; }

    [Key]
    [Column("idviolazione")]
    public Guid Idviolazione { get; set; }
    [Display(Name = "Contestabile?")]
    public bool Constestabile { get; set; }

    [ForeignKey("Idanagrafica")]
    [InverseProperty("Verbale")]
    public virtual Anagrafica Anagrafica { get; set; } = null!;

    [ForeignKey("Idviolazione")]
    [InverseProperty("Verbale")]
    public virtual Violazione Violazione { get; set; } = null!;
}
