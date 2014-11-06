using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public interface Visitor
    {
        void ProcessEvent(IncomingCallEvent eventObj);
        void ProcessEvent(AnswerEvent eventObj);
        void ProcessEvent(Event eventObj);
        void ProcessEvent(SpeakEvent eventObj);
        void ProcessEvent(PlaybackEvent eventObj);
        void ProcessEvent(GatherEvent eventObj);
        void ProcessEvent(HangupEvent eventObj);
        void ProcessEvent(DtmfEvent eventObj);
        void ProcessEvent(RejectEvent eventObj);
        void ProcessEvent(RecordingEvent eventObj);
        void ProcessEvent(SmsEvent eventObj);
        void ProcessEvent(TimeOutEvent eventObj);
    }
}
