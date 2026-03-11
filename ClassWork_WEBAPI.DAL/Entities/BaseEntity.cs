using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Entities
{
    public interface IBaseEntiy
    {
        int Id { get; set; }
        DateTime CreateDate { get; set; }
    }


    public class BaseEntity : IBaseEntiy
    {
        public int Id { get; set ; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
