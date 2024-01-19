using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryStudy
{
    public class Class1
    {
        // calculation

        public static int calc(int modCredits, int weeks, int modClassHours)
        {
            // self study hours calculation
            int selfStudyHours = (modCredits * 10 / weeks) - modClassHours;
            return selfStudyHours;
        }
    }
}
