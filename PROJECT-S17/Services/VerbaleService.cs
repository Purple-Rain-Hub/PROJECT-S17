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

        public async Task<IndexViewModel> GetAllVerbali()
        {
            try
            {
                var verbaliList = new IndexViewModel();

                verbaliList.Verbali = await _context.Verbale.OrderBy(v=> v.Idverbale).Include(v=> v.Anagrafica).Include(v=> v.Violazione).ToListAsync();

                return verbaliList;
            }
            catch
            {
                return new IndexViewModel() { Verbali = new List<Verbale>() };
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


        //task per i vari filtri

        public async Task<IndexViewModel> contestabiliFilter()
        {
            try
            {
                var verbali = new IndexViewModel();
                verbali.Verbali = await _context.Verbale.OrderBy(v => v.Idverbale).Include(v => v.Anagrafica).Include(v => v.Violazione).Where(v => v.Constestabile == true).ToListAsync();

                return verbali;
            }
            catch
            {
                return new IndexViewModel() { Verbali = new List<Verbale>() };
            }
        }

        public async Task<IndexViewModel> DecurtazioniFilter()
        {
            try
            {
                var verbali = new IndexViewModel();
                verbali.Verbali = await _context.Verbale.OrderBy(v => v.Idverbale).Include(v => v.Anagrafica).Include(v => v.Violazione).Where(v => v.DecurtamentoPunti >= 3).ToListAsync();

                return verbali;
            }
            catch
            {
                return new IndexViewModel() { Verbali = new List<Verbale>() };
            }
        }

        public async Task<IndexViewModel> ImportoFilter()
        {
            try
            {
                var verbali = new IndexViewModel();
                verbali.Verbali = await _context.Verbale.OrderBy(v => v.Idverbale).Include(v => v.Anagrafica).Include(v => v.Violazione).Where(v => v.Importo >= 200).ToListAsync();
                
                return verbali;
            }
            catch
            {
                return new IndexViewModel() { Verbali = new List<Verbale>() };
            }
        }

        public async Task<TotVerbaliViewModel> GetTotVerbaliAsync()
        {
            try
            {
                var totVerbali = new TotVerbaliViewModel();
                totVerbali.TotVerbali = await _context.Verbale.Include(v=> v.Anagrafica).GroupBy(v=> v.Idanagrafica).Select(g=> new TotVerbali()
                {
                    Nome = g.FirstOrDefault().Anagrafica.Nome,
                    Cognome = g.FirstOrDefault().Anagrafica.Cognome,
                    Indirizzo = g.FirstOrDefault().Anagrafica.Indirizzo,
                    Citta = g.FirstOrDefault().Anagrafica.Citta,
                    VerbaliTotali = g.Select(v=> v.Idverbale).Distinct().Count()
                }).ToListAsync();

                return totVerbali;
            }
            catch
            {
                return new TotVerbaliViewModel() { TotVerbali = new List<TotVerbali>() };
            }
        }

        public async Task<TotDecurtazioniViewModel> GetTotDecurtazioniAsync()
        {
            try
            {
                var totDecurtazioni = new TotDecurtazioniViewModel();
                totDecurtazioni.TotDecurtazioni = await _context.Verbale.Include(v => v.Anagrafica).GroupBy(v => v.Idanagrafica).Select(g => new TotDecurtazioni()
                {
                    Nome = g.FirstOrDefault().Anagrafica.Nome,
                    Cognome = g.FirstOrDefault().Anagrafica.Cognome,
                    Indirizzo = g.FirstOrDefault().Anagrafica.Indirizzo,
                    Citta = g.FirstOrDefault().Anagrafica.Citta,
                    DecurtazioniTotali = g.GroupBy(v=> v.Idverbale).Select(g=> g.FirstOrDefault().DecurtamentoPunti).Sum() //sono fiero di questa espressione lambda
                }).ToListAsync();

                return totDecurtazioni;
            }
            catch
            {
                return new TotDecurtazioniViewModel() { TotDecurtazioni = new List<TotDecurtazioni>() };
            }
        }
    }
}
