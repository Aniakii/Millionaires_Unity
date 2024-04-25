using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class GameScript : MonoBehaviour
{
    public Text questionText;
    public Text answerA;
    public Text answerB;
    public Text answerC;
    public Text answerD;

    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Button buttonD;

    public Button button5050;
    public Button buttonCall;

    public GameObject EndGameScreen;
    public Text EndGameText;
    public Text WonPrizeText;
    public LevelsScript LevelsScript;
    public TimerScript Timer;
    public GameObject CallingScreen;


    private readonly Color buttonColor = new Color(220f / 255f, 165f / 255f, 205f / 255f);
    private readonly Color activeColor = new Color(196f / 255f, 25f / 255f, 127f / 255f);

    private readonly System.Random rnd = new System.Random();

    private List<Question> QuestionsBase = new List<Question>(Base.QuestionsBase);
    private readonly List<int> Prizes = new List<int> { 500, 1000, 2000, 5000, 10000, 20000, 50000, 75000, 150000, 250000, 500000, 1000000 };

    private int CurrentQuestionNumber = 0;
    private Question CurrentQuestion;

    public int currentPrize = 0;
    private int prizeToWon = 0;

    private bool answersEnabled = false;
    private bool spaceEnabled = false;

    private bool is5050Avaible = true;
    private bool isFriendsAdviceAvaible = true;

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && spaceEnabled)
        {
            spaceEnabled = false;
            nextQuestion();
            answersEnabled = true;
        }

        if(Mathf.FloorToInt(Timer.timer) == 0)
        {
            gameOver(false);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }


    public void getQuestion()
    {

        drawQuestion();
        if (CurrentQuestion != null)
        {
            questionText.text = CurrentQuestion.QuestionText;
            answerA.text = CurrentQuestion.AnswersList[0];
            answerB.text = CurrentQuestion.AnswersList[1];
            answerC.text = CurrentQuestion.AnswersList[2];
            answerD.text = CurrentQuestion.AnswersList[3];
        }
        answersEnabled = true;
        LevelsScript.UpdateColor(CurrentQuestionNumber, true);

    }

    private void drawQuestion()
    {
        List<Question> filteredQuestions = new List<Question>();

        if (CurrentQuestionNumber >= 0 && CurrentQuestionNumber <= 1)
        {
            filteredQuestions = QuestionsBase.Where(q => q.Difficulty == DifficultyLevel.VERY_EASY).ToList();
        }
        else if (CurrentQuestionNumber >= 2 && CurrentQuestionNumber <= 3)
        {
            filteredQuestions = QuestionsBase.Where(q => q.Difficulty == DifficultyLevel.EASY).ToList();
        }
        else if (CurrentQuestionNumber >= 4 && CurrentQuestionNumber <= 5)
        {
            filteredQuestions = QuestionsBase.Where(q => q.Difficulty == DifficultyLevel.MEDIUM).ToList();
        }
        else if (CurrentQuestionNumber >= 6 && CurrentQuestionNumber <= 7)
        {
            filteredQuestions = QuestionsBase.Where(q => q.Difficulty == DifficultyLevel.HARD).ToList();
        }
        else if (CurrentQuestionNumber >= 8 && CurrentQuestionNumber <= 9)
        {
            filteredQuestions = QuestionsBase.Where(q => q.Difficulty == DifficultyLevel.VERY_HARD).ToList();
        }
        else if (CurrentQuestionNumber >= 10 && CurrentQuestionNumber <= 11)
        {
            filteredQuestions = QuestionsBase.Where(q => q.Difficulty == DifficultyLevel.EXTREMELY_HARD).ToList();
        }
        else if (CurrentQuestionNumber == 12)
        {
            filteredQuestions = QuestionsBase.Where(q => q.Difficulty == DifficultyLevel.IMPOSSIBLE).ToList();
        }
        else
        {
            Debug.LogError("Invalid question number or difficulty range.");
            return;
        }

        if (filteredQuestions.Count == 0)
        {
            Debug.LogError("No questions available for the chosen difficulty.");
            return;
        }

        int randomIndex = rnd.Next(0, filteredQuestions.Count);
        CurrentQuestion = filteredQuestions[randomIndex];
        QuestionsBase.Remove(CurrentQuestion);

        
    }

    private void updatePrize()
    {
        
            currentPrize = Prizes[CurrentQuestionNumber];

            if (CurrentQuestionNumber == 1 || CurrentQuestionNumber == 6)
            {
                prizeToWon = currentPrize;
            }
        
    }

    public void loadGame()
    {
       getQuestion();
    }

    public void restartGame()
    {
        CurrentQuestionNumber = 0;
        CurrentQuestion = null;
        prizeToWon = 0;
        currentPrize = 0;
        answersEnabled = false;
        spaceEnabled = false;
        QuestionsBase = new List<Question>(Base.QuestionsBase);
        buttonA.image.color = buttonColor;
        buttonB.image.color = buttonColor;
        buttonC.image.color = buttonColor;
        buttonD.image.color = buttonColor;

        for(int i = 0; i<12;i++)
        {
            LevelsScript.UpdateColor(i, false);
        }

        Timer.restartTimer();
        is5050Avaible = true;
        isFriendsAdviceAvaible = true;
        button5050.image.color = activeColor;
        buttonCall.image.color = activeColor;
        loadGame();
    }

    public void answerPressed(int answerNumber)
    {
        if (!answersEnabled)
        {
            return;
        }
        answersEnabled = false;
        if (CurrentQuestion.CorrectAnswer == answerNumber)
        {

            switch (answerNumber)
            {
                case 0:
                    
                    buttonA.image.color = Color.green;
                    break;
                case 1:
                    buttonB.image.color = Color.green;
                    break;
                case 2:
                    buttonC.image.color = Color.green;
                    break;
                case 3:
                    buttonD.image.color = Color.green;
                    break;
                default:
                    break;
            }

            updatePrize();
            if (CurrentQuestionNumber == 11)
            {
                EndGameScreen.SetActive(true);
                EndGameText.text = "Gratulacje, uda�o Ci si� wygra� g��wn� nagrod�!";
                WonPrizeText.text = $"Wygrana:  MILION Z�OTYCH";
                answersEnabled = false;
            } else
            {
                spaceEnabled = true;
            }
            
        }
        else
        {
            switch (answerNumber)
            {
                case 0:
                    if (answerA.text == "")
                        return;
                    buttonA.image.color = Color.red;
                    break;
                case 1:
                    if (answerB.text == "")
                        return;
                    buttonB.image.color = Color.red;
                    break;
                case 2:
                    if (answerC.text == "")
                        return;
                    buttonC.image.color = Color.red;
                    break;
                case 3:
                    if (answerD.text == "")
                        return;
                    buttonD.image.color = Color.red;
                    break;
                default:
                    break;
            }

            gameOver(false);

        }
    }

    public void use5050()
    {
        if (!is5050Avaible)
        {
            return;
        }
        List<int> answers = new List<int> { 0, 1, 2, 3 };
        answers.Remove(CurrentQuestion.CorrectAnswer);

        int firstIndex = rnd.Next(0, answers.Count);
        int firstNumber = answers[firstIndex];

        answers.RemoveAt(firstIndex);

        int secondNumber = answers[rnd.Next(0, answers.Count)];

        Debug.Log(firstNumber);
        Debug.Log(secondNumber);


        if (firstNumber == 0 || secondNumber == 0)
        {
            answerA.text = "";

        } 
        if (firstNumber == 1 || secondNumber == 1)
        {
            answerB.text = "";
        } 
        if (firstNumber == 2 || secondNumber == 2) 
        {
            answerC.text = "";
        } 
        if (firstNumber == 3 || secondNumber == 3)
        {
            answerD.text = "";
        }

        is5050Avaible = false;
        button5050.image.color = Color.gray;

    }

    public void useFriendsAdvice()
    {
        if(!isFriendsAdviceAvaible)
        {
            return;
        }

        CallingScreen.SetActive(true);
        Text hintText = CallingScreen.GetComponentInChildren<Text>();
        hintText.text = CurrentQuestion.FriendsAdvice;

        isFriendsAdviceAvaible = false;
        buttonCall.image.color = Color.gray;
    }

    

    public void gameOver(bool isWin)
    {
        EndGameScreen.SetActive(true);

        EndGameText.text = isWin ? "Gratulacje, uda�o Ci si� wygra� cz�ciow� nagrod�" : "Przykro mi, ale nie uda�o Ci si� przej�� gry do ko�ca";
        int prize = isWin ? currentPrize : prizeToWon;
        WonPrizeText.text = $"Wygrana: {prize} z�";
        answersEnabled = false;
        Timer.isGameStarted = false;
    }

    public void nextQuestion()
    {
        if (CurrentQuestionNumber < 12)
        {
            CurrentQuestionNumber++;
            getQuestion();
            buttonA.image.color = buttonColor;
            buttonB.image.color = buttonColor;
            buttonC.image.color = buttonColor;
            buttonD.image.color = buttonColor;
           
           
        }
    }
    
}


class Question
{
    public string QuestionText;
    public List<string> AnswersList;
    public int CorrectAnswer;
    public string FriendsAdvice;
    public DifficultyLevel Difficulty;

    public Question(string questionText, List<string> answersList, int correctAnswer, string friendsAdvice, DifficultyLevel difficulty)
    {
        QuestionText = questionText;
        AnswersList = answersList;
        CorrectAnswer = correctAnswer;
        FriendsAdvice = friendsAdvice;
        Difficulty = difficulty;
    }
}

enum DifficultyLevel
{
    VERY_EASY,
    EASY,
    MEDIUM,
    HARD,
    VERY_HARD,
    EXTREMELY_HARD,
    IMPOSSIBLE
}

class Base
{
    public static List<Question> QuestionsBase = new List<Question>
    {
    new Question("Ile wynosi suma k�t�w w tr�jk�cie?", new List<string> { "A) 180 stopni", "B) 90 stopni", "C) 360 stopni", "D) 270 stopni" }, 0, "Podstawowa w�asno�� geometrii p�askiej.", DifficultyLevel.VERY_EASY),
    new Question("Jakiego koloru jest banan?", new List<string> { "A) Zielony", "B) Niebieski", "C) ��ty", "D) Czerwony" }, 2, "Dojrzewaj�c, zmienia kolor.", DifficultyLevel.VERY_EASY),
    new Question("Kt�ry miesi�c w roku ma 28 lub 29 dni?", new List<string> { "A) Luty", "B) Marzec", "C) Kwiecie�", "D) Maj" }, 0, "Najkr�tszy miesi�c w roku.", DifficultyLevel.VERY_EASY),
    new Question("Jak nazywa si� najd�u�sza rzeka w Polsce?", new List<string> { "A) Wis�a", "B) Warta", "C) Odra", "D) Bug" }, 0, "Przep�ywa przez Warszaw�.", DifficultyLevel.VERY_EASY),
    new Question("Ile n�g ma paj�k?", new List<string> { "A) 4", "B) 6", "C) 8", "D) 10" }, 2, "Standardowa liczba dla przedstawicieli tej klasy zwierz�t.", DifficultyLevel.VERY_EASY),
    new Question("Kto napisa� 'Pan Tadeusz'?", new List<string> { "A) Juliusz S�owacki", "B) Adam Mickiewicz", "C) Boles�aw Prus", "D) Henryk Sienkiewicz" }, 1, "Najwybitniejszy przedstawiciel romantyzmu w Polsce.", DifficultyLevel.EASY),
    new Question("W kt�rym roku Polska przyst�pi�a do Unii Europejskiej?", new List<string> { "A) 1999", "B) 2004", "C) 2007", "D) 2009" }, 1, "Rozszerzenie UE, kt�re w��czy�o 10 nowych pa�stw.", DifficultyLevel.EASY),
    new Question("Jakiego koloru jest szmaragd?", new List<string> { "A) Niebieski", "B) Zielony", "C) Czerwony", "D) ��ty" }, 1, "Kamie� szlachetny tego koloru jest bardzo ceniony.", DifficultyLevel.EASY),
    new Question("Co mierzy barometr?", new List<string> { "A) Wilgotno�� powietrza", "B) Ci�nienie atmosferyczne", "C) Temperatur�", "D) Pr�dko�� wiatru" }, 1, "Nazwa pochodzi od greckiego s�owa oznaczaj�cego 'ci�ar'.", DifficultyLevel.EASY),
    new Question("Kt�ra planeta jest znana jako 'Czerwona Planeta'?", new List<string> { "A) Merkury", "B) Wenus", "C) Mars", "D) Jowisz" }, 2, "Jej kolor wynika z obecno�ci tlenku �elaza.", DifficultyLevel.EASY),
    new Question("Kto jest autorem teorii wzgl�dno�ci?", new List<string> { "A) Isaac Newton", "B) Albert Einstein", "C) Nikola Tesla", "D) Stephen Hawking" }, 1, "Niemiecko-ameryka�ski fizyk teoretyczny, kt�ry opublikowa� teori� w 1905 roku.", DifficultyLevel.MEDIUM),
    new Question("W kt�rym wieku rozpocz�a si� Rewolucja Francuska?", new List<string> { "A) XVII", "B) XVIII", "C) XIX", "D) XX" }, 1, "Wybuch�a w 1789 roku.", DifficultyLevel.MEDIUM),
    new Question("Jakie miasto jest stolic� Australii?", new List<string> { "A) Sydney", "B) Melbourne", "C) Canberra", "D) Perth" }, 2, "Nie jest to najwi�ksze miasto w kraju.", DifficultyLevel.MEDIUM),
    new Question("Ile element�w ma okresowa uk�ad pierwiastk�w chemicznych?", new List<string> { "A) 92", "B) 118", "C) 150", "D) 128" }, 1, "Liczba ta wzros�a wraz z odkryciami naukowymi.", DifficultyLevel.MEDIUM),
    new Question("Kt�ra z tych bitew nie by�a stoczona podczas II wojny �wiatowej?", new List<string> { "A) Bitwa o Midway", "B) Bitwa pod Wiedniem", "C) Bitwa o Stalingrad", "D) Bitwa o Angli�" }, 1, "Mia�a miejsce w 1683 roku, d�ugo przed II wojn� �wiatow�.", DifficultyLevel.MEDIUM),
    new Question("Kto by� pierwszym prezydentem Stan�w Zjednoczonych?", new List<string> { "A) Thomas Jefferson", "B) George Washington", "C) Abraham Lincoln", "D) John Adams" }, 1, "Sprawowa� urz�d w latach 1789-1797.", DifficultyLevel.HARD),
    new Question("Jaki jest chemiczny symbol dla o�owiu?", new List<string> { "A) Pb", "B) Pt", "C) Pd", "D) Po" }, 0, "Pochodzi od �aci�skiej nazwy plumbum.", DifficultyLevel.HARD),
    new Question("Jakie pa�stwo historyczne by�o znane jako 'Z�ote Orda'?", new List<string> { "A) Imperium Mongolskie", "B) Imperium Osma�skie", "C) Chanat Krymski", "D) Rosja Kijowska" }, 0, "Mongolskie imperium rozci�gaj�ce si� od wschodniej Europy po Azj�.", DifficultyLevel.HARD),
    new Question("W kt�rym roku wynaleziono drukark� 3D?", new List<string> { "A) 1984", "B) 1974", "C) 1992", "D) 2001" }, 0, "Technologia ta zosta�a opatentowana przez Chucka Hulla.", DifficultyLevel.HARD),
    new Question("Kto jest uznawany za 'ojca komputer�w'?", new List<string> { "A) Charles Babbage", "B) Alan Turing", "C) John von Neumann", "D) Bill Gates" }, 0, "Projektowa� maszyny analityczne w XIX wieku.", DifficultyLevel.HARD),
    new Question("W jakim j�zyku pierwotnie napisano 'Bosk� Komedi�'?", new List<string> { "A) �acina", "B) W�oski", "C) Grecki", "D) Staroangielski" }, 1, "Dante Alighieri u�y� j�zyka volgare, aby jego dzie�o by�o dost�pne szerszemu gronu odbiorc�w.", DifficultyLevel.VERY_HARD),
    new Question("Co odkry� James Watson wraz z Francisem Crickiem?", new List<string> { "A) Penicylin�", "B) Struktur� DNA", "C) Promieniotw�rczo��", "D) Przeciwcia�a" }, 1, "Odkrycie to mia�o miejsce w 1953 roku.", DifficultyLevel.VERY_HARD),
    new Question("Jaka jest najtwardsza substancja wyst�puj�ca w naturze?", new List<string> { "A) Diament", "B) Grafen", "C) Borazon", "D) Wurtzyt boronu nitrydu" }, 0, "Skala Mohsa u�ywa tej substancji jako punktu odniesienia.", DifficultyLevel.VERY_HARD),
    new Question("Ile trwa� najd�u�szy mecz tenisowy w historii?", new List<string> { "A) 8 godzin i 11 minut", "B) 11 godzin i 5 minut", "C) 6 godzin i 36 minut", "D) 5 godzin i 53 minuty" }, 1, "Mecz odby� si� podczas Wimbledonu.", DifficultyLevel.VERY_HARD),
    new Question("Kt�ry z tych element�w by� pierwszym syntetycznie wyprodukowanym przez cz�owieka?", new List<string> { "A) Technet", "B) Pluton", "C) Rad", "D) Uran" }, 0, "Element ten zosta� po raz pierwszy otrzymany w 1937 roku.", DifficultyLevel.VERY_HARD),
    new Question("Jaki jest sk�adnik aktywny w jadzie w�a koralowego?", new List<string> { "A) Hemotoksyna", "B) Neurotoksyna", "C) Kardiotoksyna", "D) Cyjanek" }, 1, "Substancja ta atakuje uk�ad nerwowy.", DifficultyLevel.IMPOSSIBLE),
    new Question("Kto jest autorem teorii 'homo ludens' dotycz�cej roli zabawy w kulturze i spo�ecze�stwie?", new List<string> { "A) Johan Huizinga", "B) Jean Piaget", "C) Lev Vygotsky", "D) Sigmund Freud" }, 0, "Holenderski historyk, kt�ry napisa� ksi��k� o tej samej nazwie.", DifficultyLevel.IMPOSSIBLE),
    new Question("W kt�rym roku odby�a si� Konferencja w Wansee, na kt�rej zaplanowano 'ostateczne rozwi�zanie kwestii �ydowskiej'?", new List<string> { "A) 1942", "B) 1938", "C) 1945", "D) 1933" }, 0, "Spotkanie wysokiego szczebla nazistowskich urz�dnik�w.", DifficultyLevel.IMPOSSIBLE),
    new Question("Jak nazywa si� najwi�kszy znany prehistoryczny ptak?", new List<string> { "A) Argentavis", "B) Pteranodon", "C) Quetzalcoatlus", "D) Pelagornis" }, 0, "�y� oko�o 6 milion�w lat temu w Argentynie.", DifficultyLevel.IMPOSSIBLE),
    new Question("Ile symfonii skomponowa� Ludwig van Beethoven?", new List<string> { "A) 5", "B) 9", "C) 12", "D) 7" }, 1, "Jego dziewi�ta i ostatnia symfonia zawiera s�ynny 'Hymn do rado�ci'.", DifficultyLevel.IMPOSSIBLE),
    new Question("Jaki jest najrzadszy izotop wodoru?", new List<string> { "A) Prot", "B) Deuter", "C) Tryt", "D) Kwart" }, 2, "Ten izotop jest radioaktywny i wyst�puje w naturze tylko w �ladowych ilo�ciach.", DifficultyLevel.EXTREMELY_HARD),
    new Question("Kto opracowa� teori� 'czarnej dziury'?", new List<string> { "A) Stephen Hawking", "B) Albert Einstein", "C) Karl Schwarzschild", "D) Roger Penrose" }, 2, "Niemiecki fizyk, kt�ry jako pierwszy rozwi�za� r�wnania pola Einsteina dla czarnej dziury.", DifficultyLevel.EXTREMELY_HARD),
    new Question("Kt�ra z poni�szych struktur nie jest cz�ci� m�zgu cz�owieka?", new List<string> { "A) Hipokamp", "B) P�at czo�owy", "C) Szyszynka", "D) Kr�gos�up" }, 3, "To cz�� uk�adu szkieletowego, a nie m�zgu.", DifficultyLevel.EXTREMELY_HARD),
    new Question("Kt�ra cywilizacja jako pierwsza u�ywa�a systemu pisma klinowego?", new List<string> { "A) Egipska", "B) Sumer", "C) Maj�w", "D) Indus" }, 1, "Staro�ytna cywilizacja mezopotamska, znana z rozwoju pisma klinowego oko�o 3400 r. p.n.e.", DifficultyLevel.EXTREMELY_HARD),
    new Question("Kt�ry z tych j�zyk�w jest uwa�any za j�zyk izolowany, nie maj�cy udowodnionych pokrewie�stw z innymi j�zykami?", new List<string> { "A) Baskijski", "B) Fi�ski", "C) W�gierski", "D) Alba�ski" }, 0, "J�zyk ten jest u�ywany w regionie Bask�w, w p�nocnej Hiszpanii i po�udniowo-zachodniej Francji.", DifficultyLevel.EXTREMELY_HARD)

    };
}
