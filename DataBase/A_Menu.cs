//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Menu
    {
        public A_Menu()
        {
            this.A_Menu1 = new HashSet<A_Menu>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string Rank { get; set; }
        public string Icon { get; set; }
    
        public virtual ICollection<A_Menu> A_Menu1 { get; set; }
        public virtual A_Menu A_Menu2 { get; set; }
    }
}
