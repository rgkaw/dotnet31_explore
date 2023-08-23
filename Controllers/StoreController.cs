using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using mvc.Data;
using mvc.Models;

namespace mvc.Controllers
{
    [Authorize(Policy="User")]
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        public StoreController(ApplicationDbContext db,IConfiguration configuration){
            _config=configuration;
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
            List<Book> OBs = _db.OwnedBooks.Where(x=> x.User.Username==HttpContext.User.Identity.Name).Select(x=>x.Book).ToList();
            PaginateBook pbs = new PaginateBook
            {
                Pagination = new Pagination { Page = page, Limit = limit, ItemCount = totalItem, TotalPage=totalPages, Search=search },
                Book = books.ToList(),
                OwnedBooks = OBs
            };
            return View(pbs);
        }
        public IActionResult BestSeller(){
            return View();
        }
        public IActionResult TopUp(){
            return View();
        } 
        public IActionResult Buy(Book book){
            User u = _db.User.FirstOrDefault(x=> x.Username==HttpContext.User.Identity.Name);
            Book b = _db.Book.FirstOrDefault(x=> x.Id==book.Id);
            OwnedBook ob = new OwnedBook{User = u, Book = b};
            InsertBuy(ob);
            u.balance -= b.Price;
            
            return RedirectToAction("Index");
        }

        private bool InsertBuy(OwnedBook obj){
            using (SqlConnection conn = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DockerConnection"))){
                SqlDataAdapter adapter = new SqlDataAdapter("", conn)
                {
                    InsertCommand = new SqlCommand("BuyBook", conn)
                };
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@RowCount", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.ReturnValue;
                adapter.InsertCommand.Parameters.Add("@Guid", SqlDbType.UniqueIdentifier, 16, "Guid");
                adapter.InsertCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 460, "Username");
                adapter.InsertCommand.Parameters.Add("@BookId", SqlDbType.Int, 4, "BookId");
                
                parameter = adapter.InsertCommand.Parameters.Add("@Identity", SqlDbType.NVarChar , 0,"Guid");
                parameter.Direction = ParameterDirection.Output;
                DataTable OwnedBooks = new DataTable();
                adapter.Fill(OwnedBooks);

                DataRow OwnedBookRow = OwnedBooks.NewRow();
                OwnedBookRow["Guid"]= obj.Guid;
                OwnedBookRow["Username"]=obj.User.Username;
                OwnedBookRow["BookId"]=obj.Book.Id;

                OwnedBooks.Rows.Add(OwnedBookRow);
                adapter.Update(OwnedBooks);

                int rowCount = (int) adapter.InsertCommand.Parameters["@RowCount"].Value;
                Console.WriteLine("ReturnValue: {0}", rowCount.ToString());
                Console.WriteLine("All Rows:");
                foreach (DataRow row in OwnedBooks.Rows)
                {
                        Console.WriteLine("  {0} : {1} : {2}", row[0], row[1], row[2]);
                }
            }
            
            
            return false;
        }
        

    }
}