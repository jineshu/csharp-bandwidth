using System;
using Newtonsoft.Json.Linq;

namespace Bandwidth.Net.Model
{
    public class EventBase:ModelBase,Event
    {
        protected EventType EventTypeObj;

        public static Event CreateEventFromString(String jsonString)
        {
            Event eventObj;
            JObject jObject = JObject.Parse(jsonString);

            JToken jToken;
            jObject.TryGetValue("eventType",out jToken);

            if (jToken == null) 
                return null;

            EventType thisEventType;
            Enum.TryParse(jToken.ToString(), true, out thisEventType);

            switch (thisEventType)
            {
                case EventType.INCOMINGCALL:
                    eventObj = new IncomingCallEvent(jObject);
                    break;

                case EventType.ANSWER:
                    eventObj = new AnswerEvent(jObject);
                    break;

                case EventType.SPEAK:
                    eventObj = new SpeakEvent(jObject);
                    break;

                case EventType.PLAYBACK:
                    eventObj = new PlaybackEvent(jObject);
                    break;

                case EventType.GATHER:
                    eventObj = new GatherEvent(jObject);
                    break;

                case EventType.HANGUP:
                    eventObj = new HangupEvent(jObject);
                    break;

                case EventType.DTMF:
                    eventObj = new DtmfEvent(jObject);
                    break;

                case EventType.REJECT:
                    eventObj = new RejectEvent(jObject);
                    break;

                case EventType.RECORDING:
                    eventObj = new RecordingEvent(jObject);
                    break;

                case EventType.SMS:
                    eventObj = new SmsEvent(jObject);
                    break;

                case EventType.TIMEOUT:
                    eventObj = new TimeOutEvent(jObject);
                    break;

                default:
                    eventObj = new EventBase(jObject);
                    break;
            }
            return eventObj;
        }

        public virtual void Execute(Visitor visitor)
        {
            visitor.ProcessEvent(this);
        }

        protected EventBase(JObject jObject)
        {
            UpdateProperties(jObject);
            JToken jToken;
            jObject.TryGetValue("eventType",out jToken);
            if (jToken != null) 
                Enum.TryParse(jToken.ToString(), true, out EventTypeObj);
        }

        public DateTime GetTime()
        {
            long time = GetPropertyAsLong("time");
            return new DateTime(time);
        }

        public object GetData()
        {
            return GetProperty("data");
        }

        public string GetProperty(string property)
        {
            return GetPropertyAsString(property);
        }

        public void SetProperty(string name, string value)
        {
            PutProperty(name,value);
        }

        public EventType GetEventType()
        {
            return EventTypeObj;
        }

        public override string ToString()
        {
            return "Event{" +
                "id='" + GetId() + '\'' +
                ", time=" + GetTime() +
                ", data=" + GetData() +
                '}';
        }
    }
}
