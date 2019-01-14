using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWheel : MonoBehaviour {

	public static readonly float CARD_WHEEL_RADIUS = 3.5f;
	public static readonly int CARD_ANGLE_INCREMENT = 10;
	public static readonly float CARD_HIGHLIGHT_HEIGHT = 0.3f;
	public static readonly int HIGHLIGHT_ANGLE_BORDER = 8;

	public static readonly float ANIMATION_SPEED = 0.5f;
	public static readonly float THROW_SWIPE_SENSITIVITY = 50f;
	public static readonly float THROW_SWIPE_BORDER = 200f;

    public Vector2 startPos;
    public Vector2 direction;
    public bool wasMoved;

    public float MaxDoubleTapTime = 0.4f;
    int TapCount;
    float NewTime;

    private float currentRotationAngle = 0;
	private Vector3 originalWheelPosition;

	private int currentHighlightIndex = 0; // Current card index to highlight
	private Vector2 firstPressPos, secondPressPos, currentSwipe; // for swipe detection

	public GameObject cardPrefab; // The card prefab
    public Player player;

    private float cardDepth;

	private List<GameObject> cardUIObjects; // All cards from the player object as gameObjects in wheel
    private List<ColorCard> addedCards;

    // Use this for initialization 
    // Called when the scene is loaded
	void Awake () {
        cardUIObjects = new List<GameObject>();
        addedCards = new List<ColorCard>();

		cardDepth = 0.005f;
		originalWheelPosition = gameObject.transform.position;

		RotateWheel ();
        TapCount = 0;
        wasMoved = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			Touch t = Input.GetTouch (0);


            // Handle finger movements based on TouchPhase
            switch (t.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = t.position;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // SWIPE FOR SCROLLING WHEEL 
                    // Determine direction by comparing the current touch position with the initial one
                    direction = t.position - startPos;

                    gameObject.transform.Rotate(0f, 0f, -t.deltaPosition.x / 10);

                    if (gameObject.transform.rotation.z < 0f)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    if (gameObject.transform.rotation.eulerAngles.z > (cardUIObjects.Count - 1) * CARD_ANGLE_INCREMENT)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, (cardUIObjects.Count - 1) * CARD_ANGLE_INCREMENT);
                    }

                    currentRotationAngle = gameObject.transform.rotation.eulerAngles.z;
                    MoveCardWheelZ();

                    wasMoved = true;
                    break;

                case TouchPhase.Ended:
                    if (!wasMoved)
                    {
                        TapCount += 1;

                        if (TapCount == 1)
                        {
                            NewTime = Time.time + MaxDoubleTapTime;
                        }
                        else if (TapCount == 2 && Time.time <= NewTime)
                        {
                            // Double tap detected
                            // Get current selected card object
                            ColorCard currentSelectedCard = addedCards[currentHighlightIndex];

                            // Check match with game rules
                            bool cardsMatch = ColorGame.CheckCardMatch(player.GetTopCard(), currentSelectedCard);

                            if (cardsMatch && player.HasTurn() && !player.MustDrawBeforePlaying())
                            {
                                // Send played card to server
                                player.PlayCard(currentSelectedCard);

                                // Remove the card from the wheel
                                RemoveCard(currentSelectedCard);

                                // Remove playing permission
                                player.SetTurn(false);
                            }
                            TapCount = 0;
                        }
                    }                  
                    else
                        TapCount = 0;

                    wasMoved = false;
                    break;
            }

            
            
            if (Time.time > NewTime)
            {
                TapCount = 0;
            }           
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            // Double tap detected
            // Get current selected card object
            ColorCard currentSelectedCard = addedCards[currentHighlightIndex];

            // Check match with game rules
            bool cardsMatch = ColorGame.CheckCardMatch(player.GetTopCard(), currentSelectedCard);

            if (cardsMatch && player.HasTurn() && !player.MustDrawBeforePlaying())
            {
                // Send played card to server
                player.PlayCard(currentSelectedCard);

                // Remove the card from the wheel
                RemoveCard(currentSelectedCard);

                // Remove playing permission
                player.SetTurn(false);
            }
        } else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRotationAngle -= CARD_ANGLE_INCREMENT;
            MoveCardWheelZ();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentRotationAngle += CARD_ANGLE_INCREMENT;
            MoveCardWheelZ();
        }
        else if (Input.GetKeyDown(KeyCode.D) && player.MustDrawBeforePlaying() && player.HasTurn())
        {
            player.DrawCards();
            MoveCardWheelZ();
        }

        if (cardUIObjects.Count != 0)
        {
            // Check card to highlight
            HighlightActiveCard();
        }
    }


	/// <summary>
	/// Realingns the cards on the wheel after a card has been removed/played or added.
	/// </summary>
	private void RealingnCards() {
		gameObject.transform.rotation = Quaternion.Euler (0, 0, 0); // Reset rotation to make positioning easier
		for(int i = 0; i < cardUIObjects.Count; i++) {
			cardUIObjects [i].transform.position = GetCardPosition (i);
			cardUIObjects [i].transform.rotation = Quaternion.Euler (0, 0, -i * CARD_ANGLE_INCREMENT);
		}

		if (currentRotationAngle > (cardUIObjects.Count - 1)*CARD_ANGLE_INCREMENT) {
			currentRotationAngle = (cardUIObjects.Count - 1) * CARD_ANGLE_INCREMENT;
		}

		gameObject.transform.rotation = Quaternion.Euler (0, 0, currentRotationAngle); // Set rotation to original value
	}
		
	/// <summary>
	/// Adjusts the cardwheel gameobject according to its current rotation angle.
	/// Detail: Because the cards are all added with some Z offset (cardDepth) you have to move the
	/// cards depth to make them appear with the same size when you rotate the wheel.
	/// </summary>
	private void MoveCardWheelZ() {
		float moveFactor = currentRotationAngle / CARD_ANGLE_INCREMENT;

		gameObject.transform.position = originalWheelPosition;
		gameObject.transform.Translate (0, 0, moveFactor * cardDepth);
	}

	/// <summary>
	/// Highlights a card if it is within the specified HIGHLIGHT_ANGLE_BOARDER.
	/// </summary>
	private void HighlightActiveCard() {
		if (currentHighlightIndex >= 0 && currentHighlightIndex < cardUIObjects.Count)
			DeHighlightCard (currentHighlightIndex);

		currentHighlightIndex = (int)((currentRotationAngle + HIGHLIGHT_ANGLE_BORDER) / CARD_ANGLE_INCREMENT); // get index to highlight (based on rotation angle)

		if (currentRotationAngle >= currentHighlightIndex * CARD_ANGLE_INCREMENT - HIGHLIGHT_ANGLE_BORDER &&
		    currentRotationAngle <= currentHighlightIndex * CARD_ANGLE_INCREMENT + HIGHLIGHT_ANGLE_BORDER) {
			HighlightCard (currentHighlightIndex); // highlight the card if within bounds
		} else {
			currentHighlightIndex = -1; // reset index so we know no card is active
		}
	}

	/// <summary>
	/// Rotates the card wheel so that it is centered.
	/// </summary>
	public void RotateWheel() {
		if (cardUIObjects.Count > 1) {
			transform.rotation = Quaternion.Euler (0, 0, (cardUIObjects.Count - 1)*CARD_ANGLE_INCREMENT/2);
			currentRotationAngle = gameObject.transform.rotation.eulerAngles.z;
			MoveCardWheelZ ();
		}
	}


	/// <summary>
	/// Adds the given card to the card wheel.
	/// </summary>
	/// <param name="card">Card.</param>
	public void AddCard(ColorCard card) {

		GameObject newCard = Instantiate (cardPrefab, GetCardPosition (cardUIObjects.Count), Quaternion.Euler (0, 0, -cardUIObjects.Count*CARD_ANGLE_INCREMENT));
		newCard.transform.parent = gameObject.transform;
		CardSpriteLoader ctl = newCard.GetComponent<CardSpriteLoader> ();
		ctl.SetSprite(card.type, card.color, card.value);

		cardUIObjects.Add(newCard);
        addedCards.Add(card);

        RealingnCards();
	}

    public void RemoveCard(ColorCard card)
    {
        int index = addedCards.IndexOf(card);
        // Remove gameobject from wheel
        GameObject go = cardUIObjects[index];
        if (go != null)
        {
            Destroy(go); // Maybe do some cool animated stuff here.
        }
                
        // Remove from our lists
        cardUIObjects.RemoveAt(index);
        addedCards.RemoveAt(index);

        RealingnCards();
    }

	/// <summary>
	/// Returns the card position for the given index in the wheel.
	/// </summary>
	/// <returns>The card position.</returns>
	/// <param name="idx">Index in the wheel.</param>
	public Vector3 GetCardPosition(int idx) {
		float x = Mathf.Sin (Mathf.Deg2Rad * idx * CARD_ANGLE_INCREMENT) * CARD_WHEEL_RADIUS + gameObject.transform.position.x;
		float y = Mathf.Cos (Mathf.Deg2Rad * idx * CARD_ANGLE_INCREMENT) * CARD_WHEEL_RADIUS + gameObject.transform.position.y; 
		float z = gameObject.transform.position.z - cardDepth * idx;

		return new Vector3 (x, y, z);
	}

    /// <summary>
    /// Highlights the card on the specified index.
    /// </summary>
    /// <param name="_cardIndex">Card index to highlight.</param>
    public void HighlightCard(int _cardIndex)
    {
        Vector2 newCardPos = GetHighlightedCardPosition(CARD_ANGLE_INCREMENT * _cardIndex); // Calculate new position for card
        Vector3 newCardPos3 = new Vector3(newCardPos.x, newCardPos.y, cardUIObjects[_cardIndex].transform.position.z); // Convert to Vector3

        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation to make positioning easier
        cardUIObjects[_cardIndex].transform.position = newCardPos3; // Set new position of card
        gameObject.transform.rotation = Quaternion.Euler(0, 0, currentRotationAngle); // Set rotation to original value
    }
		
	/// <summary>
	/// Calculates the x,y position for the highlighted state for a given angle.
	/// </summary>
	/// <returns>The highlighted card position.</returns>
	/// <param name="_cardAngle">Card angle.</param>
	public Vector2 GetHighlightedCardPosition(int _cardAngle) {
		float x = Mathf.Sin (Mathf.Deg2Rad * _cardAngle) * (CARD_WHEEL_RADIUS + CARD_HIGHLIGHT_HEIGHT) + gameObject.transform.position.x;
		float y = Mathf.Cos (Mathf.Deg2Rad * _cardAngle) * (CARD_WHEEL_RADIUS + CARD_HIGHLIGHT_HEIGHT) + gameObject.transform.position.y; 

		return new Vector2 (x, y);
	}	

	/// <summary>
	/// Calculates the x,y position for the normal state for a given angle.
	/// </summary>
	/// <returns>The normal card position.</returns>
	/// <param name="_cardAngle">Card angle.</param>
	public Vector2 GetNormalCardPosition(int _cardAngle) {
		float x = Mathf.Sin (Mathf.Deg2Rad * _cardAngle) * CARD_WHEEL_RADIUS + gameObject.transform.position.x;
		float y = Mathf.Cos (Mathf.Deg2Rad * _cardAngle) * CARD_WHEEL_RADIUS + gameObject.transform.position.y; 

		return new Vector2 (x, y);
	}

	/// <summary>
	/// Dehighlights the card at the given index (sets it back to normal state).
	/// </summary>
	/// <param name="_cardIndex">Card index.</param>
	public void DeHighlightCard(int _cardIndex) {
		Vector2 newCardPos = GetNormalCardPosition(CARD_ANGLE_INCREMENT*_cardIndex); // Calculate new position for card
		Vector3 newCardPos3 = new Vector3(newCardPos.x, newCardPos.y, cardUIObjects[_cardIndex].transform.position.z); // Convert to Vector3

		gameObject.transform.rotation = Quaternion.Euler (0, 0, 0); // Reset rotation to make positioning easier
		cardUIObjects [_cardIndex].transform.position = newCardPos3; // Set new position of card
		gameObject.transform.rotation = Quaternion.Euler (0, 0, currentRotationAngle); // Set rotation to original value
	}
}
