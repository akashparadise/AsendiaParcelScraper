namespace AsendiaParcelScraper
{
    public sealed class Logger
    {
        private static readonly object obj = new object();
        private static Logger instance = null;

        private Logger()
        {

        }

        public static Logger GetInstance
        { 
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new Logger();
                        }
                    }
                }
                    
                return instance;
            }
        }
    }
}
