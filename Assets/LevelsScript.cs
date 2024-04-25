using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsScript : MonoBehaviour
{

    public GameObject[] levels;

    private readonly Color inactiveColor = new Color(175f / 255f, 119f / 255f, 201f / 255f);

    private readonly Color activeColor = new Color(196f / 255f, 25f / 255f, 127f / 255f);

    public void UpdateColor(int levelNumber, bool active)
    {
        
        if (levelNumber >= 0 && levelNumber <= levels.Length-1)
        {
            
            GameObject level = levels[levelNumber]; 
            Image image = level.GetComponentInChildren<Image>();
            image.color = active ? activeColor : inactiveColor;


            if (levelNumber > 0 && active)
            {
                GameObject levelBefore = levels[levelNumber - 1];
                Image imageBefore = levelBefore.GetComponentInChildren<Image>();
                imageBefore.color = inactiveColor;
            }
            
    
        }
        else
        {
            Debug.LogWarning("Numer poziomu jest poza zakresem.");
        }
    }

    
}
