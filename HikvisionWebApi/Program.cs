using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hikvision
{
	public static class Program
	{
		public static void Main( string[] args )
		{
			CreateHostBuilder( args ).Build().Run();
		}

		private static IHostBuilder CreateHostBuilder( string[] args ) =>
			Host.CreateDefaultBuilder( args )
				.ConfigureLogging( logging =>
				 {
					 logging.ClearProviders();
					 logging.AddDebug();
				 } )
				.ConfigureWebHostDefaults( webBuilder =>
				 {
					 webBuilder.UseStartup<Startup>();
				 } );
	}
}
