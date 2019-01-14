using System.Collections.Generic;

public static class ColorCardStack
{

    private static Stack<ColorCard> stack;

    static ColorCardStack()
    {
        BuildStack();
    }

    public static int GetCardsLeft()
    {
        return stack.Count;
    }

    /// <summary>
    /// Draws a card from the stack.
    /// </summary>
    /// <returns>The drawn card.</returns>
    public static ColorCard DrawCard()
    {
        return stack.Pop();
    }

    /// <summary>
    /// Draws the card hand. Returns a list of 7 cards.
    /// </summary>
    /// <returns>The card hand.</returns>
    public static List<ColorCard> DrawCardHand()
    {
        List<ColorCard> hand = new List<ColorCard>();
        for (int i = 0; i < 7; i++)
        {
            hand.Add(stack.Pop());
        }
        return hand;
    }

    /// <summary>
    /// Reshuffles the stack with the given cards on the gameboard.
    /// This method should be called when the stack is empty but the game
    /// is still not over and players have to draw more cards.
    /// 
    /// It adds the already played cards from the gameboard to the stack and
    /// reshuffles it.
    /// </summary>
    /// <param name="gameBoard">The cards from the gameboard.</param>
    public static void ReshuffleStack(Stack<ColorCard> gameBoard)
    {
        List<ColorCard> gb = new List<ColorCard>(gameBoard);

        // Add remaining cards from the stack
        while (stack.Count != 0)
        {
            gb.Add(stack.Pop());
        }

        // Shuffle cards and put them back to the stack
        Shuffle(gb);
        stack = new Stack<ColorCard>(gb);
    }

    /// <summary>
    /// Builds the stack of cards. Adds all cards to the stack and shuffles it.
    /// </summary>
    private static void BuildStack()
    {
        // Sorted list of cards which will then be shuffled
        List<ColorCard> sortedCards = new List<ColorCard>();

        // Add 2x 0-9 of each color
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                ColorCard r = new ColorCard(ColorCard.Type.STANDARD, ColorCard.Color.RED, i);
                ColorCard g = new ColorCard(ColorCard.Type.STANDARD, ColorCard.Color.GREEN, i);
                ColorCard b = new ColorCard(ColorCard.Type.STANDARD, ColorCard.Color.BLUE, i);
                ColorCard y = new ColorCard(ColorCard.Type.STANDARD, ColorCard.Color.YELLOW, i);
                sortedCards.Add(r);
                sortedCards.Add(g);
                sortedCards.Add(b);
                sortedCards.Add(y);
            }
        }

        // Add draw two / reverse / skip cards --- TWO OF EACH COLOR
        for (int i = 0; i < 2; i++)
        {
            // Draw two
            ColorCard rDT = new ColorCard(ColorCard.Type.DRAW2, ColorCard.Color.RED, -1);
            ColorCard gDT = new ColorCard(ColorCard.Type.DRAW2, ColorCard.Color.GREEN, -1);
            ColorCard bDT = new ColorCard(ColorCard.Type.DRAW2, ColorCard.Color.BLUE, -1);
            ColorCard yDT = new ColorCard(ColorCard.Type.DRAW2, ColorCard.Color.YELLOW, -1);

            // Reverse
            ColorCard rRV = new ColorCard(ColorCard.Type.REVERSE, ColorCard.Color.RED, -1);
            ColorCard gRV = new ColorCard(ColorCard.Type.REVERSE, ColorCard.Color.GREEN, -1);
            ColorCard bRV = new ColorCard(ColorCard.Type.REVERSE, ColorCard.Color.BLUE, -1);
            ColorCard yRV = new ColorCard(ColorCard.Type.REVERSE, ColorCard.Color.YELLOW, -1);

            // Skip
            ColorCard rSP = new ColorCard(ColorCard.Type.SKIP, ColorCard.Color.RED, -1);
            ColorCard gSP = new ColorCard(ColorCard.Type.SKIP, ColorCard.Color.GREEN, -1);
            ColorCard bSP = new ColorCard(ColorCard.Type.SKIP, ColorCard.Color.BLUE, -1);
            ColorCard ySP = new ColorCard(ColorCard.Type.SKIP, ColorCard.Color.YELLOW, -1);

            sortedCards.Add(rDT);
            sortedCards.Add(gDT);
            sortedCards.Add(bDT);
            sortedCards.Add(yDT);

            sortedCards.Add(rRV);
            sortedCards.Add(gRV);
            sortedCards.Add(bRV);
            sortedCards.Add(yRV);

            sortedCards.Add(rSP);
            sortedCards.Add(gSP);
            sortedCards.Add(bSP);
            sortedCards.Add(ySP);
        }

        
        // Add wild & wild4 cards
        for (int i = 0; i < 4; i++)
        {
            ColorCard wild = new ColorCard(ColorCard.Type.WILD, ColorCard.Color.NONE, -1);
            ColorCard wild4 = new ColorCard(ColorCard.Type.WILD4, ColorCard.Color.NONE, -1);

            sortedCards.Add(wild);
            sortedCards.Add(wild4);
        }
        
        // Shuffle cards
        Shuffle(sortedCards);

        // Covert/Store to stack
        stack = new Stack<ColorCard>(sortedCards);
    }

    private static System.Random rng = new System.Random(); // Used for shuffling

    /// <summary>
    /// Shuffle the specified list.
    /// </summary>
    /// <param name="list">List.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
