using VotePreferentiel;

namespace VotePreferentiel.Tests;

public class VoteCounterTests
{
    private readonly VoteCounter _counter = new();

    // Test 1 : Cas nominal
    [Test]
    public async Task CasNominal()
    {
        var votes = CreerVotes(
            ["Alice", "Bilal", "Chloé"],
            ["Alice", "Chloé", "Bilal"],
            ["Bilal", "Chloé", "Alice"],
            ["Chloé", "Bilal", "Alice"],
            ["Chloé", "Alice", "Bilal"]
        );

        var gagnant = _counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Chloé");
    }

    // Test 2 : Un seul candidat
    [Test]
    public async Task UnSeulCandidat()
    {
        var votes = CreerVotes(["Alice"]);

        var gagnant = _counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Alice");
    }

    // Test 3 : Report des préférences
    [Test]
    public async Task ReportPreferences()
    {
        var votes = new VoteModel();

        for (int i = 0; i < 4; i++)
            votes.Votes.Add(new Vote {
                Choices = ["Alice", "Bilal", "Chloé", "Diane"]
            });

        for (int i = 0; i < 3; i++)
            votes.Votes.Add(new Vote {
                Choices = ["Bilal", "Chloé", "Alice", "Diane"]
            });

        for (int i = 0; i < 3; i++)
            votes.Votes.Add(new Vote {
                Choices = ["Chloé", "Alice", "Bilal", "Diane"]
            });

        votes.Votes.Add(new Vote {
            Choices = ["Diane", "Chloé", "Bilal", "Alice"]
        });

        var gagnant = _counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Chloé");
    }

    // Test 4 : Égalité en dernière position
    [Test]
    public async Task EgaliteDernierePosition()
    {
        var votes = CreerVotes(
            ["Alice", "Chloé", "Bilal"],
            ["Bilal", "Chloé", "Alice"],
            ["Chloé", "Alice", "Bilal"],
            ["Chloé", "Bilal", "Alice"]
        );

        var gagnant = _counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Chloé");
    }

    // Test 5 : Bulletin vide
    [Test]
    public async Task BulletinVide()
    {
        var votes = CreerVotes([]);

        await Assert.That(() => _counter.GetWinner(votes))
            .Throws<ArgumentException>();
    }

    // Méthode pour créer facilement des votes
    private VoteModel CreerVotes(params string[][] choix)
    {
        return new VoteModel
        {
            Votes = choix.Select(c => new Vote {
                Choices = c.ToList()
            }).ToList()
        };
    }
}