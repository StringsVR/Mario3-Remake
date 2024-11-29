using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteNumberDisplay : MonoBehaviour
{
    [SerializeField] private List<Sprite> numberSprites; // Assign the digit sprites (0-9) in order via Inspector
    [SerializeField] private GameObject digitPrefab;    // Prefab of an Image component
    [SerializeField] private Transform container;       // Parent Transform for digits
    [SerializeField] private int zeroPadAmount = 2;     // Set the number of zeros to pad to
    [SerializeField] private float digitSpacing = 50f;  // Spacing between digits (adjust to fit your design)

    private List<GameObject> digitImages = new List<GameObject>();

    public void DisplayNumber(int number)
    {
        // Clear previous digits
        foreach (var digitImage in digitImages)
        {
            Destroy(digitImage);
        }
        digitImages.Clear();

        // Get the number as a string and pad it with leading zeros
        string numberString = number.ToString();
        numberString = PadWithZeros(numberString, zeroPadAmount);

        RectTransform ogTransform = digitPrefab.GetComponent<RectTransform>();
        float x = ogTransform.anchoredPosition.x;
        for (int i = 0; i < numberString.Length; i++)
        {
            char digitChar = numberString[i];
            int digit = digitChar - '0'; // Convert char to int
            if (digit < 0 || digit > 9) continue; // Skip non-digit characters

            // Create an Image for the digit
            GameObject digitObject = Instantiate(digitPrefab, container);

            // Set the sprite for the current digit
            digitObject.GetComponent<Image>().sprite = numberSprites[digit];

            // Position the digit based on its index
            RectTransform rectTransform = digitObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(x + (i * digitSpacing), 0);

            // Add to the list of digit images
            digitImages.Add(digitObject);
        }
    }

    private string PadWithZeros(string input, int totalLength)
    {
        return input.PadLeft(totalLength, '0');
    }
}
