using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TwoDecks;

class Program
{
    static void Main(string[] args)
    {
        
        var deck = new Deck()
            .Shuffle()
            .Take(16);

        var grouped =
            from card in deck
            group card by card.Suit into suitGroup //разделение последовательностей на группы
            orderby suitGroup.Key descending
            select suitGroup;

        foreach (var group in grouped)
        {
            Console.WriteLine($@"Group: {group.Key}
             Count: {group.Count()}
             Minimum: {group.Min()}
             Maximun: {group.Max()}");
        }

    }

}
class Card : IComparable<Card>

{
    public int CompareTo(Card other)
    {
        return new CardComparerByValue().Compare(this, other);
    }
    public Values Value { get; private set; }
    public Suits Suit { get; private set; }

    public Card(Values value, Suits suit)
    {
        this.Suit = suit;
        this.Value = value;
    }
    public string Name
    {
        get { return $"{Value} of {Suit}"; }
    }

    public override string ToString()
    {
        return Name;
    }

}
class Deck : ObservableCollection<Card>
{
    private static Random random = new Random();

    public Deck()
    {
        Reset();
    }

    public Card Deal(int index)
    {
        Card cardToDeal = base[index];
        RemoveAt(index);
        return cardToDeal;
    }

    public void Reset()
    {
        Clear();
        for (int suit = 0; suit <= 3; suit++)
            for (int value = 1; value <= 13; value++)
                Add(new Card((Values)value, (Suits)suit));
    }

    public Deck Shuffle()
    {
        List<Card> copy = new List<Card>(this);
        Clear();
        while (copy.Count > 0)
        {
            int index = random.Next(copy.Count);
            Card card = copy[index];
            copy.RemoveAt(index);
            Add(card);
        }
        return this;
    }

    public void Sort()
    {
        List<Card> sortedCards = new List<Card>(this);
        sortedCards.Sort(new CardComparerByValue());
        Clear();
        foreach (Card card in sortedCards)
        {
            Add(card);
        }
    }
}
class CardComparerByValue : IComparer<Card>
{
    public int Compare(Card x, Card y)
    {
        if (x.Suit < y.Suit)
            return -1;
        if (x.Suit > y.Suit)
            return 1;
        if (x.Value < y.Value)
            return -1;
        if (x.Value > y.Value)
            return 1;
        return 0;
    }
}
enum Suits
{
    Diamonds,
    Clubs,
    Hearts,
    Spades,
}
enum Values
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
}


