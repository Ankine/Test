using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardsLib
{
    public delegate void LastCardDrawnHandler(Deak currentDeck);
    public class Deak : ICloneable
    {
	in test2 asdf,h as,djfhasdf
        public event LastCardDrawnHandler LastCardDrawn;
        private Cards cards = new Cards();
        private Random rnd1 = new Random();
        private int myval;
        private int my Val1;
        private int newvalue;

        //Додатковий конструктор для клонування колоди
        private Deak(Cards newCards)
        {
            cards = newCards;
        }
        //Глибоке еопіювання
        public object Clone()
        {
            Deak newDeak = new Deak(cards.Clone() as Cards);
            return newDeak;
        }
        //Конструктор з встановленням туза старшим
        public Deak(bool isAceHigh)
            : this()
        {
            Card.isAceHigh = isAceHigh;
        }
        //Конструктор з встановленням козирю
        public Deak(bool useTrumps, Suit trump)
            : this()
        {
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }
        public Deak(bool useTrumps, int trump)
            : this()
        {
            Card.useTrumps = useTrumps;
            Card.trump = (Suit)trump;
        }
        //Конструктор з старшим тузом і козирьом
        public Deak(bool isAceHigh, bool useTrumps, Suit trump)
            : this()
        {
            Card.isAceHigh = isAceHigh;
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }
        //Стандартний конструктор
        public Deak()
        {
            for (int suitval = 0; suitval < 4; suitval++)
            {
                for (int rankval = 1; rankval < 14; rankval++)
                {
                    cards.Add(new Card((Suit)suitval, (Rank)rankval));
                }
            }
        }
        public Card GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= cards.Count)
            {
                LastCardDrawn(this);
                Card buffercard = cards[cardNum];
                // cards.Remove(buffercard);
                return buffercard;

            }
            else
            {
                throw new CardOutOfRangeException(cards.Clone() as Cards);
            }
        }
        public Card GetRandomCard()
        {
            int randomval = rnd1.Next(cards.Count);
            Card buferCard = new Card(cards[randomval].Suitcard, cards[randomval].Rankcard);
            cards.Remove(cards[randomval]);
            return buferCard;
        }
        public void Shuffle()
        {
            Cards cardsbufer = new Cards();
            for (int i = 0; i < 51; i++)
            {
                cardsbufer.Add(this.GetRandomCard());
            }
            cards = cardsbufer;
        }
        public override string ToString()
        {
            string deakstr = "";
            for (int i = 0; i < cards.Count; i++)
            {
                deakstr = string.Format("{0}\n{1}", deakstr, cards[i]);
            }
            return deakstr;
        }
    }

    public class CardOutOfRangeException : Exception
    {
        private Cards deckContents;
        public Cards DeckContents
        {
            get
            {
                return deckContents;
            }
        }
        public CardOutOfRangeException(Cards sourceDeckContents)
            : base("There are only 52 cards in deck.")
        {
            deckContents = sourceDeckContents;
        }

    }
}