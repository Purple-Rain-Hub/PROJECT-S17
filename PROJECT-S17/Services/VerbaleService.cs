using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROJECT_S17.Data;
using PROJECT_S17.Models;
using PROJECT_S17.ViewModels;

namespace PROJECT_S17.Services
{
    public class VerbaleService
    {
        private readonly S17DbContext _context;
        public VerbaleService(S17DbContext context)
        {
            _context = context;
        }

        private async Task<bool> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Violazione>> GetViolazioni()
        {
            try
            {
                var violazioniList = await _context.Violazione.ToListAsync();

                return violazioniList;
            }
            catch (Exception ex)
            {
                var exception = ex.Message;
                return new List<Violazione>();
            }
        }

        public async Task<bool> SaveVerbaleAsync(AddVerbaleViewModel model)
        {
            var IsItnew = await _context.Anagrafica.Where(a => a.CodFisc == model.Verbale.Anagrafica.CodFisc).FirstOrDefaultAsync();

            if(IsItnew == null)
            {
                IsItnew = new Anagrafica()
                {
                    Idanagrafica = Guid.NewGuid(),
                    Nome = model.Verbale.Anagrafica.Nome,
                    Cognome = model.Verbale.Anagrafica.Cognome,
                    Indirizzo = model.Verbale.Anagrafica.Indirizzo,
                    Citta = model.Verbale.Anagrafica.Citta,
                    Cap = model.Verbale.Anagrafica.Cap,
                    CodFisc = model.Verbale.Anagrafica.CodFisc
                };

                _context.Anagrafica.Add(IsItnew);
            }

            var maxIdVerbale = await _context.Verbale.OrderByDescending(v => v.Idverbale).Select(v => v.Idverbale).FirstOrDefaultAsync();

            var newViolazione = new Verbale()
            {
                Idverbale = maxIdVerbale +1,
                Idanagrafica = IsItnew.Idanagrafica,
                Idviolazione = model.Verbale.Violazione.Idviolazione,
                Constestabile = model.Verbale.Constestabile,
                DataTrascrizioneVerbale = model.Verbale.DataTrascrizioneVerbale,
                DataViolazione = model.Verbale.DataViolazione,
                Importo = model.Verbale.Importo,
                DecurtamentoPunti = model.Verbale.DecurtamentoPunti,
                IndirizzoViolazione = model.Verbale.IndirizzoViolazione,
                NominativoAgente = model.Verbale.NominativoAgente
            };

            _context.Verbale.Add(newViolazione);

            return await SaveAsync();
        }
    }
}
