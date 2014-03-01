namespace Grasshopper.Cubes
{
	class Program
	{
		static void Main(string[] args)
		{
			using(var game = new GameCore())
			using(var form = new GameForm(game.Title, 1024, 768))
			using(var app = new AppCore(form, game))
			{
				app.Initalize();
				app.Run();
			}
		}
	}
}
