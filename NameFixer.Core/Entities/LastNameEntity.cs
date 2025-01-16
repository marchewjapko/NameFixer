namespace NameFixer.Core.Entities;

public sealed record LastNameEntity(string LastName, int OccurenceRate)
{
    public bool Equals(LastNameEntity? other)
    {
        if (other is null) return false;

        return LastName == other.LastName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LastName);
    }
}