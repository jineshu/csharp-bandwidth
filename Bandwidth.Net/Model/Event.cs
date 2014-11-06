using System;

namespace Bandwidth.Net.Model
{
    public interface Event
    {
        void Execute(Visitor v);
        String GetProperty(String property);
        void SetProperty(String name, String value);
        EventType GetEventType();
    }
}
