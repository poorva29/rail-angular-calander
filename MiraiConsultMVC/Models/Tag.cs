using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Tags
    {
        private int tagid;
        private string tagname;
        // constructor
        public Tags()
        {
        }
        public string TagName { get { return tagname; } set { tagname = value; } }
        public int TagId { get { return tagid; } set { tagid = value; } }
    }
}