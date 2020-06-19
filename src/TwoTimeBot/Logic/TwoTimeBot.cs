using Logic.OutputClients;
using System;
using System.Data;
using System.Net.Http;

namespace Logic
{
    public interface ITwoTimeBot
    {
        void Start();
        void Stop();
    }

    public class TwoTimeBot : ITwoTimeBot
    {
        private IOutputClient _outputClient;

        public TwoTimeBot(IOutputClient outputClient)
        {
            _outputClient = outputClient;
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
