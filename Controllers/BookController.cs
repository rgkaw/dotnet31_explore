using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public IActionResult Index(string search, int page = 1, int limit =5, string orderBy ="DateCreated", string desc="")
        {
            string query="select * from Book where 1=1";
            if(!search.IsNullOrEmpty()){
                    var searchStr="'%"+search+"%'";
                    query+="and (";
                    query+=" Author like "+searchStr;
                    query+=" or Title like "+searchStr;
                    query+=")";
            }
            query+=" order by "+orderBy+" "+desc+"OFFSET 0 ROWS";
            Console.WriteLine(query);
            IQueryable<Book> books = _db.Book.FromSqlRaw(query);

            var totalItem = books.Count();
            var totalPages = (int)Math.Ceiling((double)totalItem / (double)limit);
            books = books
                .Skip((page - 1) * limit)
                .Take(limit);
            
            PaginateBook pbs = new PaginateBook
            {
                Pagination = new Pagination { Page = page, Limit = limit, ItemCount = totalItem, TotalPage=totalPages, Search=search },
                Book = books.ToList()
            };
            return View(pbs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateBook");
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title,Author,Price,Stock")] Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Book.Add(book);
                await _db.SaveChangesAsync();
                ViewBag.Message = "Sucess or Failure Message";
                ModelState.Clear();
                return RedirectToAction(nameof(Index));

            }
            
            return PartialView("_CreateBook", book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Book model = await Task.Run(() => _db.Book.FirstOrDefault(m => m.Id == id));
            if (model == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteBook", model);

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
                return View("List");
            }
            return PartialView(book);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id, Author, Title, Price, Stock")] Book book)
        {
            System.Console.WriteLine(book);
            if (!_db.Book.Any(x=>x.Id==book.Id))
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
        public IActionResult Download(string search, int page, int limit)
        {
            if (!_db.Book.Any()){
                return RedirectToAction("Index");
            }
            IQueryable<Book> ibooks = _db.Book.AsQueryable();
            if ((!search.IsNullOrEmpty()) || (!string.IsNullOrWhiteSpace(search)))
            {
                ibooks = ibooks.Where(x=>
                x.Title == search ||
                x.Author == search);
            }
            if (page<=0)
            {
                page = 1;
            }
            if (limit > 0)
            {
                ibooks = ibooks
                .Skip((page - 1) * limit)
                .Take(limit);
            }
            else
            {
                limit = ibooks.Count();
            }
            var totalItem = ibooks.Count();
            var totalPages = (int)Math.Ceiling((double)totalItem / (double)limit);
            List < Book > books = ibooks.ToList();
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                Console.WriteLine(books.Count());
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
                    Console.WriteLine(i);
                }
                for(i = 0; i<books.Count;i++)
                {
                    PropertyInfo[] prop = books[i].GetType().GetProperties();
                    int j=1;
                    foreach(var val in prop){
                        worksheet.Cells[i+2,j].Value=val.GetValue(books[i],null).ToString();
                        j++;
                        Console.WriteLine(i.ToString()+j.ToString());
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