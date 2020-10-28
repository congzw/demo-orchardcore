namespace NbSites.Core
{
    public class StartupOrder
    {
        public int AppMax = 2000;
        public int App = 1500;
        public int AppMin = 1001;


        public int BaseMax = 1000;
        public int Base = 500;
        public int BaseMin = 1;
        
        public int BeforeAllModulesLoad = -10000;
        public int AfterAllModulesLoad = 10000;
        
        public static StartupOrder Instance = new StartupOrder();
    }
}
