using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionScript
{
    public string QuestionText;
    public List<string> AnswersList;
    public int CorrectAnswer;
    public string FriendsAdvice;
    public Difficulty Difficulty;

    public QuestionScript(string questionText, List<string> answersList, int correctAnswer, string friendsAdvice, Difficulty difficulty)
    {
        QuestionText = questionText;
        AnswersList = answersList;
        CorrectAnswer = correctAnswer;
        FriendsAdvice = friendsAdvice;
        Difficulty = difficulty;
    }   
}

public enum Difficulty
{
    VERY_EASY,
    EASY,
    MEDIUM,
    HARD,
    VERY_HARD,
    EXTREMELY_HARD,
    IMPOSSIBLE
}
