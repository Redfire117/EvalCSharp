using VotePreferentiel;

namespace VotePreferentiel.Worker;

public class Worker : BackgroundService
{
    private readonly VoteProvider _voteProvider;
    private readonly InterfaceVoteCounter _voteCounter;

    public Worker(VoteProvider voteProvider, InterfaceVoteCounter voteCounter)
    {
        _voteProvider = voteProvider;
        _voteCounter = voteCounter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var votes = await _voteProvider.GetVotes();

        Console.WriteLine($"Nombre de votes : {votes.Votes.Count}");

        string gagnant = _voteCounter.GetWinner(votes);

        Console.WriteLine($"Le gagnant est : {gagnant}");
    }
}