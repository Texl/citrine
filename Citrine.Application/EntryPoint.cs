using System;
using Citrine.Scene;

namespace Citrine.Application
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            using (var application = new Application("citrine", Scene2.BuildScene().RenderScene))
            {
                application.Run();
            }
        }
    }
}
