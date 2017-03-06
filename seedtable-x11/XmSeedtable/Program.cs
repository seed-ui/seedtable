namespace XmSeedtable
{
    class Program
    {
        static void Main(string[] args) {
            //System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(System.Console.Out));
            var app = new TonNurako.ApplicationContext();
            app.Name = "SeedTable";
            app.FallbackResource.Add("*fontList", "-misc-fixed-medium-r-normal--14-*-*-*-*-*-*-*:");
            app.FallbackResource.Add("*geometry", "+100+100");
            app.FallbackResource.Add("*title", "SeedTable");

            TonNurako.Application.Run(app, new SeedTableX11());
        }
    }
}
