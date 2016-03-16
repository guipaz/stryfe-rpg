using System;

namespace StryfeRPG
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = Stryfe.Instance.Setup())
                game.Run();
        }
    }
#endif
}
