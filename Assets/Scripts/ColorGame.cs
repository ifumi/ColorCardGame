using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ColorGame
{

    private static Stack<ColorCard> gameBoardStack;

    private static int currentPlayerIdx; // Index of current player in list
    private static int currentDrawCount; // Count to draw for the next eligible player (sum of PLUS cards played before drawing)
    private static bool reverse;

    private static bool gameOver = false;
    private static int winnerIdx = -1;

    private static bool isReady = false;

    static ColorGame()
    {
        gameBoardStack = new Stack<ColorCard>();
    }

    public static bool IsReady()
    {
        return isReady;
    }

    public static int GetCurrentPlayerIdx()
    {
        return currentPlayerIdx;
    }

    public static ColorCard PeekGameBoardStack()
    {
        return gameBoardStack.Peek();
    }


    public static bool IsGameOver()
    {
        return gameOver;
    }

    /// <summary>
    /// Checks if the current player can play (i.e. has a matching card)
    /// </summary>
    /// <returns><c>true</c>, if player can play was currented, <c>false</c> otherwise.</returns>
    public static bool PlayerCanPlay(Player player)
    {
        List<ColorCard> cards = player.GetCards();
        foreach (ColorCard card in cards)
        {
            if (CheckCardMatch(player.GetTopCard(), card))
                return true;
        }
        return false;
    }

    public static bool CheckCardMatch(ColorCard currentTopCard, ColorCard toCHeck)
    {

        switch (currentTopCard.type)
        {
            case ColorCard.Type.STANDARD:
                if (toCHeck.type == ColorCard.Type.WILD ||
                    toCHeck.type == ColorCard.Type.WILD4)
                    return true;

                if (toCHeck.type == ColorCard.Type.SKIP ||
                    toCHeck.type == ColorCard.Type.REVERSE ||
                    toCHeck.type == ColorCard.Type.DRAW2)
                {
                    if (currentTopCard.color == toCHeck.color)
                        return true;
                }

                if (toCHeck.type == ColorCard.Type.STANDARD &&
                    (currentTopCard.color == toCHeck.color || currentTopCard.value == toCHeck.value))
                    return true;

                break;

            case ColorCard.Type.DRAW2:
                if (toCHeck.type == ColorCard.Type.DRAW2 ||
                    toCHeck.type == ColorCard.Type.WILD ||
                    toCHeck.type == ColorCard.Type.WILD4)
                    return true;

                if ((toCHeck.type == ColorCard.Type.SKIP ||
                    toCHeck.type == ColorCard.Type.REVERSE ||
                    toCHeck.type == ColorCard.Type.STANDARD) &&
                    currentTopCard.color == toCHeck.color)
                    return true;

                break;

            case ColorCard.Type.SKIP:
                if (toCHeck.type == ColorCard.Type.WILD ||
                    toCHeck.type == ColorCard.Type.WILD4 ||
                    toCHeck.type == ColorCard.Type.SKIP)
                {
                    return true;
                }
                else
                {
                    if (currentTopCard.color == toCHeck.color)
                        return true;
                }
                break;

            case ColorCard.Type.REVERSE:
                if (toCHeck.type == ColorCard.Type.WILD ||
                    toCHeck.type == ColorCard.Type.WILD4 ||
                    toCHeck.type == ColorCard.Type.REVERSE)
                {
                    return true;
                }
                else
                {
                    if (toCHeck.color == currentTopCard.color)
                        return true;
                }
                break;

            case ColorCard.Type.WILD:
                if (toCHeck.type == ColorCard.Type.WILD ||
                    toCHeck.type == ColorCard.Type.WILD4)
                {
                    return true;
                }
                else
                {
                    if (currentTopCard.color == toCHeck.color)
                        return true;
                }

                break;

            case ColorCard.Type.WILD4:
                if (toCHeck.type == ColorCard.Type.WILD)
                    return false;
                if (toCHeck.type == ColorCard.Type.WILD4)
                {
                    return true;
                }
                else
                {
                    if (currentTopCard.color == toCHeck.color)
                        return true;
                }

                break;
        }

        return false;
    }

    /// <summary>
    /// Draws the number of cards stored in currentDrawCount (incremented by PLUS cards)
    /// for the current player.
    /// </summary>
    public static void DrawCardsForPlayer(PlayerConnection player, int drawCount)
    {
        while (drawCount >= 0)
        {
            player.CmdGetCard();
            drawCount--;
        }
    }

    public static ColorCard.Color GetMostOccuringColorForPlayer(Player p)
    {
        int green = 0;
        int yellow = 0;
        int red = 0;
        int blue = 0;

        foreach (ColorCard card in p.GetCards())
        {
            switch (card.color)
            {
                case ColorCard.Color.BLUE:
                    blue++;
                    break;
                case ColorCard.Color.RED:
                    red++;
                    break;
                case ColorCard.Color.GREEN:
                    green++;
                    break;
                case ColorCard.Color.YELLOW:
                    yellow++;
                    break;
            }
        }

        int max = new int[] { green, yellow, red, blue }.Max();

        if (max == green)
            return ColorCard.Color.GREEN;
        if (max == yellow)
            return ColorCard.Color.YELLOW;
        if (max == red)
            return ColorCard.Color.RED;
        if (max == blue)
            return ColorCard.Color.BLUE;

        return ColorCard.Color.NONE;
    }

    public static int GetWinnerIndex()
    {
        return winnerIdx;
    }


}
