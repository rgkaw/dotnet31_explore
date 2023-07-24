using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using mvc.Data;
using mvc.Models;
using mvc.Models.DTO;

namespace mvc.Controllers
{
    [Authorize]
    public class AttachmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AttachmentController(ApplicationDbContext db){
            _db=db;
        }
        [HttpGet]
        public IActionResult Index(){
            System.Console.WriteLine("List View doank");
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult List(int page =1, int limit=5, string search = ""){
            System.Console.WriteLine("List View Lengkap");
            if(!_db.Attachment.Any(x=>
                x.Filename.Contains(search)
            )){
                return View(new PaginateAttachment{
                    Pagination = new Pagination(),
                    Attachment = new List<Attachment>()
                });
            }
            if(!_db.Attachment.Any()){
                return View();
            }
            page= (page>=1? page:1);
            List<Attachment> Attachment =  _db.Attachment
                .Skip((page-1)*limit)
                .Take(limit)
                .ToList();
            int totalItem = _db.Attachment
                .Count();
            System.Console.WriteLine(totalItem);
            int totalPage = (int) Math.Ceiling((double)totalItem/(double)limit);
            return View(new PaginateAttachment{
                Attachment = Attachment,
                Pagination = new Pagination{
                    Page = page,
                    TotalPage = totalPage,
                    Limit=limit,
                    Search=search,
                    ItemCount=totalItem,
                    Path = "List"
                }
            });
        }
        [HttpGet]
        public IActionResult Upload(){

            return PartialView("_UploadAttachment");
        }
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> FormFile){
            try{
                if(!ModelState.IsValid){
                    return PartialView("UploadAttachment");
                }
                // System.Console.WriteLine("UPLOAD TJOYYY");
                // string subPath ="files"; // Your code goes here
                // if(!System.IO.Directory.Exists(subPath)){
                //     System.IO.Directory.CreateDirectory(subPath);
                // }
                // Console.WriteLine(FormFile.Length);
                // Attachment atc = new Attachment{
                //     Filename = FormFile.FileName,
                //     Size = FormFile.Length,
                //     Location = subPath,
                // };
                // string filePath = subPath+"/"+atc.Guid.ToString();
                // using (var stream = System.IO.File.Create(filePath)){
                //     await FormFile.CopyToAsync(stream);
                // }
                // System.Console.WriteLine(filePath);
                // _db.Attachment.Add(atc);
                // await _db.SaveChangesAsync();
                string subPath ="files";
                Console.WriteLine(FormFile.Count);
                foreach(IFormFile file in FormFile){
                    if(file.Length>0){
                        Attachment atc = new Attachment{
                            Filename = file.FileName,
                            Size = file.Length,
                            Location = subPath,
                        };
                        string filePath = subPath+"/"+atc.Guid.ToString();
                        using (var stream = System.IO.File.Create(filePath)){
                            await file.CopyToAsync(stream);
                        }
                        System.Console.WriteLine(filePath);
                        _db.Attachment.Add(atc);
                        await _db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("List");
            }catch(Exception e){
                Console.WriteLine(e);
                return RedirectToAction("List");
            }


        }
        [HttpGet]
        public  IActionResult Download(string guid) {
            try {
                string reqFile = "files/" + guid.ToLower();
                byte[] fileBytes = System.IO.File.ReadAllBytes(reqFile);
                Attachment file = _db.Attachment.FirstOrDefault(x => x.Guid == Guid.Parse(guid));
                return File(fileBytes,System.Net.Mime.MediaTypeNames.Application.Octet,file.Filename);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string guid)
        {
            Attachment model = await Task.Run(() => _db.Attachment.FirstOrDefault(m => m.Guid == Guid.Parse(guid)));
            if (model == null)
            {
                return RedirectToAction("List");
            }
            return PartialView("_DeleteAttachment", model);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string guid)
        {
            Attachment model = await Task.Run(() => _db.Attachment.FirstOrDefault(m => m.Guid == Guid.Parse(guid)));
            if (model == null)
            {
                return RedirectToAction("List");
            }
            string reqFile = "files/" + guid.ToLower();
            System.IO.File.Delete(reqFile);
            _db.Attachment.Remove(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
    
}