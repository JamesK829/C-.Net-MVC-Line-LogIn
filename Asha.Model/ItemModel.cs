using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asha.Model
{
    public class ItemModel
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string LINK { get; set; }
        public string PARENT_ID { get; set; }
        public Nullable<int> SORT { get; set; }
        public string STATUS { get; set; }
        public virtual List<ItemModel> CHILDS { get; set; }
    }

    public class FuncModel
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string LINK { get; set; }
        public string PARENT_ID { get; set; }
        public Nullable<int> SORT { get; set; }
        public string STATUS { get; set; }
        public virtual List<ChildItemModel> CHILDS { get; set; }
    }

    public class ChildItemModel
    {
        public string ROLE_ID { get; set; }
        public string FUNC_ID { get; set; }
        public string ACCESS_ID { get; set; }
        public string LINK { get; set; }
        //public List<FSITC_ROLE_FUNC> List { get; set; }
        //public List<FSITC_FUNC> ParentList { get; set; }
    }
}
