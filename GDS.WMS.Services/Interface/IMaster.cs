using System;
using System.Reflection.Emit;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using EStudio.Framework;

namespace GDS.WMS.Services.Interface
{
    public interface IMaster
    {
        BaseResponse Run(string type);
    }
}
