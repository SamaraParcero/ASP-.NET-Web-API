
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace APICatalog.Logging
{
    public class CustomerLogger : ILogger
    {
        
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration loggerConfig)
        {
            this.loggerName = name;
            this.loggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
           return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensage = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state,exception)}";

            WriteLogInFile(mensage);
        }

        private void WriteLogInFile(string mensage)
        {
            string filePath = @"G:\ProjetosGithub\log.txt";

            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                try
                {
                    streamWriter.WriteLine(mensage);
                    streamWriter.Close();
                }catch(Exception )
                {
                    throw;
                }
            }
        }
    }
}
