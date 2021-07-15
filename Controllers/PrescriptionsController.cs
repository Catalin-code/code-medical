using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeMedical.Data;
using CodeMedical.Models;

namespace CodeMedical.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly MedicalOfficeContext _context;

        public PrescriptionsController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index(DateTime startDate, DateTime endDate)
        {
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;

            var medicalOfficeContext = from p in _context.Prescriptions
                                       select p;

            if (!(startDate == DateTime.MinValue) && !(endDate == DateTime.MinValue))
            {
                medicalOfficeContext = medicalOfficeContext
                    .Where(p => p.IssueDate <= endDate &&
                                p.IssueDate >= startDate);
            }

            return View(await medicalOfficeContext.ToListAsync());
        }

        // GET: Prescriptions/PrescriptionsReport
        public async Task<IActionResult> PrescriptionsReport(DateTime startDate, DateTime endDate)
        {
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;

            var patients = _context.Patients
                .Include(p => p.Prescriptions
                .Where(p => p.IssueDate >= startDate &&
                            p.IssueDate <= endDate))
                .AsNoTracking()
                .ToListAsync();

            return View(await patients);
        }

        // GET: Prescriptions/DrugsReport
        public async Task<IActionResult> DrugsReport(DateTime startDate, DateTime endDate)
        {
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;

            var prescriptions = await _context.Prescriptions
                .Where(p => p.IssueDate >= startDate &&
                            p.IssueDate <= endDate)
                .Include(pr => pr.PrescriptedDrugInfos)
                .ThenInclude(prd => prd.Drug)
                .AsNoTracking()
                .ToListAsync();

            Dictionary<string, int> drugsAndQuantities = new Dictionary<string, int>();

            var drugs = _context.Drugs;

            foreach (var drug in drugs)
            {
                var quantity = 0;
                foreach (var prescription in prescriptions)
                {
                    quantity += prescription.PrescriptedDrugInfos.Where(x => x.Drug.Name == drug.Name).Select(x => x.Quantity).Sum();
                }
                drugsAndQuantities.Add(drug.Name, quantity);
            }
            ViewData["DrugsAndQuantities"] = drugsAndQuantities;

            return View(prescriptions);
        }

        // GET: Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptedDrugInfos).ThenInclude(pr => pr.Drug)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // GET: Prescriptions/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID");
            var patientNames = _context.Patients
                .Select(p => new
                {
                    Text = p.LastName + " " + p.FirstName,
                    Value = p.ID
                }).ToList();

            ViewBag.PatientNames = new SelectList(patientNames, "Value", "Text");
            return View();
        }

        // POST: Prescriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PatientID,Series,Number,IssueDate")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID", prescription.PatientID);
            return View(prescription);
        }

        // GET: Prescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID", prescription.PatientID);
            return View(prescription);
        }

        // POST: Prescriptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PatientID,Series,Number,IssueDate")] Prescription prescription)
        {
            if (id != prescription.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionExists(prescription.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID", prescription.PatientID);
            return View(prescription);
        }

        // GET: Prescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.ID == id);
        }
    }
}
