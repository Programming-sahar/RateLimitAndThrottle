namespace ThrottleLiveSearch.Services;

public interface IStaticSearchService
{
    List<string> GetSearchList();
}

public class StaticSearchService : IStaticSearchService
{
    private static readonly List<string> _items = new()
    {
        "apple", "aparat", "banana", "orange", "grape", "watermelon", "cherry", "mango", "pineapple", "kiwi"
    };

    public List<string> GetSearchList() => _items;
}
