using System.Collections.Generic;
using mvc.Data;
using mvc.Models.Event;
using System.Linq;

namespace mvc.Utils
{
    
    public class TypeGetter
    {
        private readonly ApplicationDbContext _db;
        public TypeGetter (ApplicationDbContext db)
        {
            _db=db;
        }

        public List<EventType> TypeList() {
            return _db.EventTypes.ToList();
            }
    }
}