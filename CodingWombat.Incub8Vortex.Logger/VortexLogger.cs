using System;
using System.Collections.Generic;
using CodingWombat.Incub8Vortex.Logger.LogEventDtos;
using Microsoft.Extensions.Logging;

namespace CodingWombat.Incub8Vortex.Logger
{
    public class VortexLogger : ILogger
    {
        private readonly string _name;
        private readonly VortexLoggerConfiguration _config;
        private readonly VortexLogEventProcessor _processor;
        
        public VortexLogger(string name, VortexLoggerConfiguration config)
        {
            _name = name;
            _config = config;
            _processor = new VortexLogEventProcessor(_config);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (_config.EventId != 0 && _config.EventId != eventId.Id)
            {
                return;
            }

            var attributes = new Dictionary<string, object>();
            attributes["Source"] = _name;
            attributes["LogLevel"] = logLevel.ToString();
            attributes["Event"] = eventId;
            attributes["Exception"] = exception;
            attributes["State"] = formatter(state, exception);
            var eventDto = new LogEventDto { Attributes = attributes};

            _processor.EnqueueEvent(eventDto);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _config.LogLevel && _config.LogLevel != LogLevel.None;
            
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}