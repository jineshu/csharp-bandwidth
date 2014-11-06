using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public enum EventType
    {
        INCOMINGCALL,
        ANSWER,
        SPEAK,
        PLAYBACK,
        GATHER,
        HANGUP,
        DTMF,
        REJECT,
        RECORDING,
        SMS,
        TIMEOUT,
        UNKNOWN
    }
}
