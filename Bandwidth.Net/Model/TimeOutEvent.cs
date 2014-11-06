﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bandwidth.Net.Model
{
    public class TimeOutEvent:EventBase
    {
        protected internal TimeOutEvent(JObject jObject) : base(jObject)
        {
        }
        public override void Execute(Visitor visitor)
        {
            visitor.ProcessEvent(this);
        }
    }
}
