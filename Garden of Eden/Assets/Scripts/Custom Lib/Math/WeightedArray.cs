using UnityEngine;

public class WeightedArray
{
    /// <summary>
    /// Method takes a list of floats as an argument
    /// And will return the item number for the list of 
    /// items to be choosen from.
    /// </summary>
    /// <param name="listOfProbs">The list of probabilities</param>
    /// <returns>Returns the element in list to choose</returns>
    public int ChooseElement(float[] listOfProbs ) {
        //Initialize the variable that holds the total of all probabilities
        float total = 0;

        //Iterate through list adding each item to the total probability
        foreach(float item in listOfProbs ) {
            total += item;
        }

        //Pick a random percent and scale it up compared to the total
        //If the total of all probabilities are equal to 1.2, then the
        //Random value will be multiplied by 1.2 to base everything off a 
        //100% scale
        float randomValue = Random.value * total;
        //Loop through list comparing list item to random value
        for(int i = 0; i < listOfProbs.Length; i++ ) {
            //If the random value is less than the list item then return list item
            if(randomValue < listOfProbs[i] ) {
                return i;
            }
            else {
                //Subtract the list items value from the random value to reduce
                //This will reduce the random value by previous value so that
                //The value will go down as it iterates through list
                randomValue -= listOfProbs[i];
            }
        }
        //If no item was returned then return the last item
        return listOfProbs.Length - 1;
    }
}
