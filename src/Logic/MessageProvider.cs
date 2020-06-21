using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public static class MessageProvider
    {
        public static readonly Random Random = new Random();
        public static TwoTimeMessage DefaultCelebrationMessage = new TwoTimeMessage("This is a very special 2-time. My creators have no idea how I'm sending this particular message. HAPPY 2-TIME!");

        public static TwoTimeMessage GetMessageForDate(DateTime date)
        {
            return new TwoTimeMessage("Happy 2-time, bro! :)");
        }

        //public static bool IsSpecialTwoTimeDate(this DateTime date)
        //{

        //}

        //private static readonly List<TwoTimeMessage> _twoTimeMessages = new List<TwoTimeMessage>
        //{
        //    new TwoTimeMessage("Happy 2-time, bro.")
        //};
    }
}
