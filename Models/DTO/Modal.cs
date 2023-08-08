using System;

namespace mvc.Models.DTO
{
    public class Modal
    {
        public Modal(Guid id, string Controller, string Action){
            Guid=id;
            ActionUrl+=Controller+"/";
            ActionUrl+=Action+"/";
        }
        public Guid Guid {get;set;}
        public string ActionUrl {get;set;} ="";

    }
}
