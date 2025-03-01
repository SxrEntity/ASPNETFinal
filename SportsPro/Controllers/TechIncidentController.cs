using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers{
    
    public class TechIncidentController : Controller{

        private readonly SportsProContext _context;

        public TechIncidentController(SportsProContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(){
            int? id = HttpContext.Session.GetInt32("techID") ?? 0;
            Technician technician;
            if(id == 0 || id == null){
                technician = new Technician();
            }
            else{
                technician = await _context.Technicians.FindAsync(id);
            }
            return View(technician);
        }

        [HttpPost]
        public IActionResult List(Technician technician){
            HttpContext.Session.SetInt32("techID", technician.TechnicianID);

            if(technician.TechnicianID == 0){
                TempData["message"] = "You must select a technician.";
                return RedirectToAction("Get");
            }
            else{
                return RedirectToAction("List", new {id = technician.TechnicianID});
            }
        }

        [HttpGet]
        public async Task<IActionResult> List(int id){
            var model = new TechIncidentViewModel{
                Technician = await _context.Technicians.FindAsync(id),

                Incidents = await _context.Incidents
                .Include("Customer")
                .Include("Product")
                .OrderBy(i => i.DateOpened)
                .Where(i => i.TechnicianID == id)
                .Where(i => i.DateClosed == null)
                .ToListAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id){
            var model = new TechIncidentViewModel{
                Technician = await _context.Technicians.FindAsync(id),

                Incident = await _context.Incidents.FindAsync(id)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IncidentViewModel model){
            Incident i = await _context.Incidents.FindAsync(model.Incident.IncidentID);
            i.Description = model.Incident.Description;
            i.DateClosed = model.Incident.DateClosed;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(i);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(i.IncidentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["message"] = i.Title + " was edited.";
                return RedirectToAction(nameof(Index));
            }

            int techID = HttpContext.Session.GetInt32("techID") ?? 0;
            return RedirectToAction("List", new {id = techID});

        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.IncidentID == id);
        }
    }
}