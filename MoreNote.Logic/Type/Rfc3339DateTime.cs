using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace leanote.Common.Type
{
    public class Rfc3339DateTime
    {
        public DateTime thisTime;

        public Rfc3339DateTime()
        {
            thisTime=DateTime.Now;
        }


        public Rfc3339DateTime(DateTime thisTime)
        {
            this.thisTime = thisTime;
        }

        public  string ToRfc3339String()
        {
            return thisTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
        }
        public  string ToDataTimeString()
        {
            return thisTime.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override  string ToString()
        {
            return this.ToRfc3339String();
            //return base.ToString();
        }
    }
}
