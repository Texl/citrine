using System;
using Citrine.Scene;

namespace Citrine.Application
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            using (var application = new Application("citrine", Scene3.RenderScene, 64984))
            {
                application.Run();
            }
        }
    }
}
