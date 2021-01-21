using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Data.Entities
{
    //Hangi tablonun  primary key inin hangi type inda oldugunu bilemedigim için
    public abstract class EntityBase<Tkey>
    {
        public Tkey Id { get; set; }
    }
}
