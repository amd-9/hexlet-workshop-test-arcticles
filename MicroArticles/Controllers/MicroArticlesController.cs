using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MicroArticles.Models;
using MicroArticles.Providers;
using System.IO;

namespace MicroArticles.Controllers
{
    public class MicroArticlesController : Controller
    {
        private readonly MicroArticlesContext _context;
        private readonly IArticleFileProvider _articleFileProvider;

        public MicroArticlesController(MicroArticlesContext context, IArticleFileProvider articleFileProvider)
        {
            _context = context;
            _articleFileProvider = articleFileProvider;           
        }

        // GET: MicroArticles
        public async Task<IActionResult> Index()
        {
            return View(await _context.MicroArticle.ToListAsync());
        }

        // GET: MicroArticles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var microArticle = await _context.MicroArticle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (microArticle == null)
            {
                return NotFound();
            }

            return View(microArticle);
        }

        // GET: MicroArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MicroArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Body,ImageAddress")] MicroArticle microArticle)
        {
            if (ModelState.IsValid)
            {
                //Download file
                var uploadedFile = await _articleFileProvider.SaveFileFromUriAsync(microArticle.ImageAddress);
                
                if(string.IsNullOrEmpty(uploadedFile))
                    return View(microArticle);

                microArticle.ImageFileName = uploadedFile;
                microArticle.Id = Guid.NewGuid();
                _context.Add(microArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(microArticle);
        }

        // GET: MicroArticles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var microArticle = await _context.MicroArticle.FindAsync(id);
            if (microArticle == null)
            {
                return NotFound();
            }
            return View(microArticle);
        }

        // POST: MicroArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Body,ImageAddress")] MicroArticle microArticle)
        {
            if (id != microArticle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(microArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MicroArticleExists(microArticle.Id))
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
            return View(microArticle);
        }

        // GET: MicroArticles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var microArticle = await _context.MicroArticle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (microArticle == null)
            {
                return NotFound();
            }

            return View(microArticle);
        }

        // POST: MicroArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var microArticle = await _context.MicroArticle.FindAsync(id);
            _context.MicroArticle.Remove(microArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MicroArticleExists(Guid id)
        {
            return _context.MicroArticle.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return Content("Filename is empty");
            }
            
            var fileContent = await _articleFileProvider.GetFileByNameAsync(fileName);

            if (fileContent == null)
                return NotFound();

            return File(fileContent, "image/jpg");
        }
    }
}
