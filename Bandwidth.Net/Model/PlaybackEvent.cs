﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bandwidth.Net.Model
{
    public class PlaybackEvent:EventBase
    {
        protected internal PlaybackEvent(JObject jObject) : base(jObject)
        {
        }
        public override void Execute(Visitor visitor)
        {
            visitor.ProcessEvent(this);
        }
    }
}