namespace VotePreferentiel;

public class VoteCounter : InterfaceVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel)
    {
        return voteModel.Votes
            .SelectMany(v => v.Choices)
            .ToHashSet();
    }

    public List<string> GetAllFirstChoices(VoteModel voteModel)
    {
        return voteModel.Votes
            .Select(v => v.Choices.First())
            .ToList();
    }

    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices)
    {
        return firstChoices
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public string[] GetCandidatesWithLeastVotes(
        IDictionary<string, int> firstChoiceCounts)
    {
        int minimum = firstChoiceCounts.Values.Min();

        return firstChoiceCounts
            .Where(c => c.Value == minimum)
            .Select(c => c.Key)
            .ToArray();
    }

    public string GetWinner(VoteModel voteModel)
    {
        if (voteModel.Votes.Count == 0 ||
            voteModel.Votes.Any(v => v.Choices.Count == 0))
        {
            throw new ArgumentException("Un bulletin est vide.");
        }

        var candidats = GetCandidates(voteModel);

        while (candidats.Count > 1)
        {
            var premiersChoix = voteModel.Votes
                .Select(v => v.Choices.FirstOrDefault(c => candidats.Contains(c)))
                .Where(c => c != null)
                .Select(c => c!)
                .ToList();

            var compte = CountFirstChoices(premiersChoix);

            // Ajouter les candidats qui ont zéro voix
            foreach (var candidat in candidats)
            {
                if (!compte.ContainsKey(candidat))
                    compte[candidat] = 0;
            }

            var elimines = GetCandidatesWithLeastVotes(compte);

            // Si tous les candidats sont à égalité, aucun gagnant unique
            if (elimines.Length == candidats.Count)
                throw new InvalidOperationException("Égalité entre tous les candidats.");

            foreach (var candidat in elimines)
                candidats.Remove(candidat);
        }

        return candidats.Single();
    }
}