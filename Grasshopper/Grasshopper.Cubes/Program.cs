using Grasshopper.Cubes.Game;

namespace Grasshopper.Cubes
{
	class Program
	{
		static void Main(string[] args)
		{
			using(var game = new GameCore())
			using(var form = new GameForm(game.Title, 1280, 720))
			using(var app = new AppCore(form, game))
			{
				app.Initalize();
				game.Initialize();
				
				app.Run();
			}
		}
	}
}
