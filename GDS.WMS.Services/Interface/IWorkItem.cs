﻿using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using EStudio.Framework;

namespace GDS.WMS.Services.Interface
{
    public interface IWorkItem
    {
        BaseResponse Run();
    }
}
