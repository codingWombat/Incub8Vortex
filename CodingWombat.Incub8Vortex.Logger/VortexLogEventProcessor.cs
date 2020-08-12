using System;
using System.Collections.Concurrent;
using System.Threading;
using CodingWombat.Incub8Vortex.Logger.LogEventDtos;

namespace CodingWombat.Incub8Vortex.Logger
{
    public class VortexLogEventProcessor : IDisposable
    {
        private const int _maxQueuedMessages = 512;
        private readonly BlockingCollection<LogEventDto> _eventQueue = new BlockingCollection<LogEventDto>(_maxQueuedMessages);
        private readonly Thread _sendingThread;
        private readonly VortexLoggerConfiguration _configuration;
        private readonly VortexLoggingClient _vortexClient;
        
        public VortexLogEventProcessor(VortexLoggerConfiguration configuration)
        {
            _configuration = configuration;
            _vortexClient = new VortexLoggingClient(_configuration);
            _sendingThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "Event logger sending thread"
            };
            _sendingThread.Start();
        }

        public virtual void EnqueueEvent(LogEventDto eventDto)
        {
            if (!_eventQueue.IsAddingCompleted)
            {
                try
                {
                    _eventQueue.Add(eventDto);
                    return;
                }
                catch (InvalidOperationException) { }
            }

            SendEvent(eventDto);
        }

        private void SendEvent(LogEventDto eventDto)
        {
            _vortexClient.SendEventAsync(eventDto);
        }
        
        private void ProcessLogQueue()
        {
            try
            {
                foreach (var eventDto in _eventQueue.GetConsumingEnumerable())
                {
                    SendEvent(eventDto);
                }
            }
            catch
            {
                try
                {
                    _eventQueue.CompleteAdding();
                }
                catch { }
            }
        }
        
        public void Dispose()
        {
            _eventQueue.CompleteAdding();

            try
            {
                _sendingThread.Join(5000); 
            }
            catch (ThreadStateException) { }
        }
    }
}