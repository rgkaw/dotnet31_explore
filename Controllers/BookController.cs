using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace mvc.Controllers
{
    [Authorize]
    public class BookController : Controller
    {

        private readonly ApplicationDbContext _db;
        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet("api/v1/getbook")]
        public IActionResult getId(int id) {
            if (!_db.Book.Any(x => x.Id == id))
            {
                return NotFound("Not Found");
            }
            List<Book> book = new List<Book>
                {
                    _db.Book.FirstOrDefault(x => x.Id == id)

                };
            return Ok(book);
        }
        // GET: id
        public IActionResult Index([FromQuery(Name = "id")] int? id, [FromQuery(Name = "name")] string name, [FromQuery(Name = "author")] string author, int page = 1, int limit =5)
        {
            HttpContext.Request.Headers.Add("Authorization", Request.Cookies["accessToken"]);

            if (id != null)
            {
                if (!_db.Book.Any(x => x.Id == id))
                {
                    return NotFound("Not Found");
                }
                List<Book> book = new List<Book>
                {
                    _db.Book.FirstOrDefault(x => x.Id == id)

                };
                PaginateBook pb = new PaginateBook
                {
                    Pagination = new Pagination
                    {
                        Page = page,
                        Limit = limit,
                        ItemCount = book.Count(),
                        TotalPage = 1
                    }
                };
                return View(pb);
            }
            List<Book> books = _db.Book
                .Skip((page -1)*limit)
                .Take(limit)
                .ToList();
            var totalItem = _db.Book.Count();
            var totalPages = (int)Math.Ceiling((double)totalItem / (double)limit);
            PaginateBook pbs = new PaginateBook
            {
                Pagination = new Pagination { Page = page, Limit = limit, ItemCount = totalItem, TotalPage=totalPages },
                Book = books
            };
            return View(pbs);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Author,Price,Stock")] Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Book.Add(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Book model = await Task.Run(() => _db.Book.FirstOrDefault(m => m.Id == id));
            if (model == null)
            {
                return NotFound();
            }
            return View(model);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var book = await _db.Book.FindAsync(id);
            _db.Book.Remove(book);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var book = await _db.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Author, Name, Price, Stock")] Book book)
        {
            System.Console.WriteLine(book);
            if (id != book.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    book.DateModified=DateTime.Now;
                    _db.Update(book);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        private bool BookExists(int? id)
        {
            return _db.Book.Any(e => e.Id == id);
        }


    
        [HttpGet]
        public IActionResult Download(){
            if(!_db.Book.Any()){
                return RedirectToAction("Index");
            }
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                List<Book> books = _db.Book.ToList();
                string filename= "Table_List";
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(filename);
                if(!books.Any()){
                    ViewData.Add("ErrorDownload","Table is Empty! nothing to download");
                    RedirectToAction("Index");
                }
                int i=1;
                foreach(var prop in books.First().GetType().GetProperties()){
                    worksheet.Cells[1,i].Value=prop.Name;
                    i++;
                }
                for(i = 2; i<books.Count;i++)
                {
                    PropertyInfo[] prop = books[i].GetType().GetProperties();
                    int j=1;
                    foreach(var val in prop){
                        worksheet.Cells[i,j].Value=val.GetValue(books[i],null).ToString();
                        j++;
                    }
                }
                MemoryStream stream = new MemoryStream();
                excelPackage.SaveAs(stream);
                stream.Position=0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",filename+".xlsx");

            }
        }

        public IActionResult Price(){
            return View(
                _db.Book
                    .OrderByDescending(x=>x.Price)
                    .Take(5)
                    .ToList()
                );
        }
    }
}