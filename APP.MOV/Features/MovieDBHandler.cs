using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.APP.Features;
using APP.MOV.Domain;

namespace APP.MOV.Features
{
    public abstract class MovieDBHandler : Handler
    {
        protected readonly MovieDB _db;
        protected MovieDBHandler(MovieDB db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }
    }
}
