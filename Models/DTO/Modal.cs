using System;

namespace mvc.Models.DTO
{
    public class Modal
    {
        public Modal(Guid id, string cnt, string act){
            Guid=id;
            Action = act;
            Controller = cnt;
            ActionUrl+=cnt+"/";
            ActionUrl+=act+"/";
        }
        public Guid Guid {get;set;}
        public string ActionUrl {get;set;} ="";
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";

    }
}
