using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ResultsPerPageModel
    {
        public static class GlobalVariables
        {
            public static int ResultsPerPage
            {
                get
                {
                    return 3;
                }
            }
        }
    }
}
