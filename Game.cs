using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsLib
{
    public class Game
    {
        private int currentCard;
        private Deak playDeak;
        private Player[] players;
        private Cards discardedCards;
        public Game()
        {
            currentCard = 0;
            playDeak = new Deak(true);
            playDeak.LastCardDrawn += new LastCardDrawnHandler(Reshuffle);
            playDeak.Shuffle();
            discardedCards = new Cards();
        }
        private void Reshuffle(Deak currentDeak)
        {
            Console.WriteLine("Disarded cards reshuffle into deak");
            currentDeak.Shuffle();
            discardedCards.Clear();
            currentCard = 0;
        }
        public void SetPlayers(Player[] newPlayers)
        {
            if (newPlayers.Length > 7)
                throw new ArgumentException("A maximum of 7 players may play this game");

            if (newPlayers.Length < 2)
                throw new ArgumentException("A minimum of 2 players may play this game");
            players = newPlayers;
        }
        private void DealHands()
        {
            for (int p = 0; p < players.Length; p++)
            {
                for (int c = 0; c < 7; c++)
                {
                    players[p].PlayHand.Add(playDeak.GetCard(currentCard++));
                }
            }
        }
        public int PlayGame()
        {
            //Проводити гру якшо існують гравці
            if (players == null)
                return -1;
            //Перша роздача карт на руки
            DealHands();
            //Ініціалізація змінних
            bool GameWon = false;
            int currentPlayer;
            Card playCard = playDeak.GetCard(currentCard++);
            discardedCards.Add(playCard);
            //головний цико гри
            do
            {
                //прохід по всім гравцям
                for (currentPlayer = 0; currentPlayer < players.Length; currentPlayer++)
                {
                    //виведення гравця і його карт
                    Console.WriteLine("{0}'s turn.", players[currentPlayer].Name);
                    Console.WriteLine("Current hand:");
                    foreach (Card card in players[currentPlayer].PlayHand)
                    {
                        Console.WriteLine(card);
                    }
                    Console.WriteLine("Card in play: {0}", playCard);
                    //Запрошення гравцю взяти карту
                    bool inputOK = false;
                    do
                    {
                        Console.WriteLine("Press T to take card in play or D to draw");
                        string input = Console.ReadLine();
                        if (input.ToLower() == "t")
                        {
                            //Додавання карти на руки гравцю з столу
                            Console.WriteLine("Drawn: {0}", playCard);
                            if (discardedCards.Contains(playCard))
                            {
                                discardedCards.Remove(playCard);
                            }
                            players[currentPlayer].PlayHand.Add(playCard);
                            inputOK = true;
                        }
                        if (input.ToLower() == "d")
                        {
                            Card newCard;
                            bool cardIsAvailable;
                            do
                            {
                                newCard = playDeak.GetCard(currentCard++);
                                cardIsAvailable = !discardedCards.Contains(newCard);
                                if (cardIsAvailable)
                                {
                                    foreach (Player testPlayer in players)
                                    {
                                        if (testPlayer.PlayHand.Contains(newCard))
                                        {
                                            cardIsAvailable = false;
                                            break;
                                        }
                                    }
                                }
                            } while (!cardIsAvailable);

                            //Додавання нової карти на руки гравцю
                            Console.WriteLine("Drawn: {0}", newCard);
                            players[currentPlayer].PlayHand.Add(newCard);
                            inputOK = true;

                        }

                    } while (inputOK == false);
                    //Відображення нової розкоадки кард з нумерацією
                    Console.WriteLine("New hand:");
                    for (int i = 0; i < players[currentPlayer].PlayHand.Count; i++)
                    {
                        Console.WriteLine("{0}: {1}", i + 1, players[currentPlayer].PlayHand[i]);
                    }
                    //Запрошення гравця відкинути якусь карту
                    inputOK = false;
                    int choice = -1;
                    do
                    {
                        Console.WriteLine("Choose card to discard:");
                        string input = Console.ReadLine();
                        try
                        {
                            //Спроба перетворення
                            choice = Convert.ToInt32(input);
                            if ((choice > 0) && (choice <= 8))
                                inputOK = true;
                        }
                        catch
                        {
                            //Ігнорувати
                        }
                    } while (inputOK == false);

                    //Перенесення ссилки на видалену карту в PlayCard (викладування карти на стіл)
                    //забрати карту в гравця і помістити в відкинуті карти

                    playCard = players[currentPlayer].PlayHand[choice - 1];
                    players[currentPlayer].PlayHand.RemoveAt(choice - 1);
                    discardedCards.Add(playCard);
                    Console.WriteLine("Discarding: {0}", playCard);
                    Console.WriteLine();

                    //Перевірка перемоги поточного гравця

                    GameWon = players[currentPlayer].HasWon();
                    if (GameWon == true) break;
                }

            } while (GameWon == false);
            return currentPlayer;
        }

    }
}
