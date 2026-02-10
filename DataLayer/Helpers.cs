using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataLayer.Helpers
{
    public static class PatientFields
    {
        public static class FirstName 
        {
            public const int MaxLength = 256;
        }
    }

    public static class MessageFields
    {
        public static class Title
        {
            public const int MaxLength = 256;

        }

        public static class Body
        {
            public const int MaxLength = 4096;
        }
    }
}
