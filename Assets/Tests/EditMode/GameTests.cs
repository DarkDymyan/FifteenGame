using GameFifteen.GameLogic;
using NUnit.Framework;

public class GameTests
{
    [Test]
    public void StartTest()
    {
        int size = 4;
        Game game = new Game(size);
        game.Start();

        Assert.AreEqual(null, game.GetAt(3,3));
    }
}
