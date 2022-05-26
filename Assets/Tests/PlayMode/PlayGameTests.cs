using GameFifteen;
using GameFifteen.GameLogic;
using NUnit.Framework;
using UnityEngine;

public class PlayGameTests
{
    Game CreateGame(int size)
    {
        Game game = new Game(size);
        game.Start();

        int digit = 0;

        for (int i = 0; i < 15; i++)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Animation>();
            Chip chip = gameObject.AddComponent<Chip>();
            chip.ID = ++digit;

            game.SetAt(i / size, i % size, chip);
        }

        return game;
    }
    
    [Test]
    public void PlayGameTestsSimplePasses()
    {
        Game game = CreateGame(4);

        Assert.AreEqual(1, game.GetAt(0, 0).ID);
        Assert.AreEqual(6, game.GetAt(1, 1).ID);
        Assert.AreEqual(11, game.GetAt(2, 2).ID);
        Assert.AreEqual(null, game.GetAt(3, 3));
    }

    [Test]
    public void PlayGameMoveCheck()
    {
        Game game = CreateGame(4);

        Chip chip = game.GetAt(0, 0);
        Chip chip2 = game.GetAt(2, 2);
        game.SetAt(2, 2, chip);
        game.SetAt(0, 0, chip2);

        Assert.AreNotEqual(null, chip);
        Assert.AreEqual(chip2.ID, game.GetAt(0, 0).ID);
        Assert.AreEqual(chip.ID, game.GetAt(2, 2).ID);
    }


    [Test]
    public void OnBoardTest()
    {
        Game game = CreateGame(4);

        Assert.AreEqual(true, game.IsOnOneLine(new Vector2Int(2, 3)));
        Assert.AreEqual(false, game.IsOnOneLine(new Vector2Int(1, 1)));
    }
}
