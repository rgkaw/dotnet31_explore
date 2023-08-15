using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using mvc.Data;
using mvc.Models;

namespace mvc.Controllers
{
    [Authorize(Policy="User")]
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _db;
        public StoreController(ApplicationDbContext db){
            _db = db;
        }
        public IActionResult Index(string search, int page = 1, int limit =5, string orderBy ="DateCreated", string desc="desc")
        {
            string query="select * from Book where 1=1";
            if(!search.IsNullOrEmpty()){
                    var searchStr="'%"+search+"%'";
                    query+="and (";
                    query+=" Author like "+searchStr;
                    query+=" or Title like "+searchStr;
                    query+=")";
            }
            query+=" order by "+orderBy+" "+desc+" OFFSET 0 ROWS";
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
        public IActionResult BestSeller(){
            return View();
        }
        public IActionResult TopUp(){
            return View();
        } 
        public IActionResult Buy(){
            return View();
        }
        

    }
}