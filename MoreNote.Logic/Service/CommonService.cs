using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service
{
   public  class CommonService
    {
        public  void parsePageAndSort(int pageNumber,int pageSize,string sortField,bool isAsc,out int skipNum,out string sortFieldR)
        {
            skipNum = (pageNumber - 1) * pageSize;
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "UpdatedTime";
            }
            if (!isAsc)
            {
                sortFieldR = "-" + sortField;

            }
            else
            {
                sortFieldR = sortField;
            }
        }
    }
}
