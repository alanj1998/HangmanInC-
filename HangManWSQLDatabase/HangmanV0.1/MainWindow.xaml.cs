/*#########################################################
 *## Hangman V0.1                                        ##
 *## Created by Alan Jachimczak                          ##
 *##                                                     ##
 *## In this version the game utilises:                  ##
 *## 1) 14 words from Words Database                     ##
 *## 2) Written confirmation on amount of goes left      ##
 *## 3) IF/FOR/IF system of analyzing data               ##
 *##                                                     ##
 *## 24-02-2017 Update                                   ##
 *## 1) App fully converted to WPF style                 ##
 *## 2) New method included to repeat                    ##
 *##                                                     ##
 *## Copyright(c) by Alan Jachimczak                     ##
 *## This work can only be used for educational purposes ##
 *######################################################### 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace HangmanV0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Static strings used throughout all methods!
        static string word, hiddenWord;
        static char[] wordSeparated, hiddenLetters;
        static string wordInMinus;
        static char guessedLetter;
        static int wrongLetterCount;
        const int MAXIMUM_WRONG = 5;
        static List<char> usedLetters = new List<char>();

        //App starts here
        public MainWindow()
        {
            //Initializing the App
            InitializeComponent();

            //Initializing the variables
            word = Word_Picker().ToUpper();
            hiddenWord = new string('-', word.Count());
            wordSeparated = word.ToCharArray();
            hiddenLetters = hiddenWord.ToCharArray();
            wordInMinus = new string(hiddenLetters);
            wrongLetterCount = 0;

            //Input
            MessageBox.Show("Welcome to my Game! Have Fun!", "Hangman V0.1", MessageBoxButton.OK, MessageBoxImage.Information);

            //Game Setup
            hiddenWordBox.Text = wordInMinus;
        }

        //When the button is clicked
        private void check_Click(object sender, RoutedEventArgs e)
        {
            if (!wordSeparated.SequenceEqual(hiddenLetters) && wrongLetterCount != MAXIMUM_WRONG)
            {
                if (letterBox.Text == "" || letterBox.Text.Length > 1)
                {
                    MessageBox.Show("Enter one letter only!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    letterBox.Text = "";
                    Thread.Sleep(100);
                }

                guessedLetter = char.Parse(letterBox.Text.ToUpper().Trim());

                if (char.IsNumber(guessedLetter) || char.IsSymbol(guessedLetter) || char.IsPunctuation(guessedLetter))
                {
                    MessageBox.Show("Only letters allowed!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    letterBox.Text = "";
                    Thread.Sleep(100);
                }

                if (wordSeparated.Contains(guessedLetter) && !usedLetters.Contains(guessedLetter))
                {
                    if (wordSeparated.Contains(guessedLetter) && !usedLetters.Contains(guessedLetter))
                    {
                        for (int i = 0; i < wordSeparated.Count(); i++)
                        {
                            if (wordSeparated[i] == guessedLetter)
                            {
                                hiddenLetters[i] = guessedLetter;

                            }
                            else
                                continue;
                        }
                        usedLetters.Add(guessedLetter);
                        wordInMinus = new string(hiddenLetters);
                        hiddenWordBox.Text = wordInMinus;
                        letterBox.Text = "";
                    }
                    MessageBox.Show("Well Done ! You get the letter " + guessedLetter + " right!", "Correct Letter", MessageBoxButton.OK);
                    Thread.Sleep(100);
                }
                else if (usedLetters.Contains(guessedLetter))
                {
                    MessageBox.Show("You already used " + guessedLetter + " . Use a differnet letter!", "Already Used!", MessageBoxButton.OK);
                    Thread.Sleep(100);
                    letterBox.Text = "";
                }
                else
                {
                    wrongLetterCount++;
                    MessageBox.Show("Wrong Letter! " + (MAXIMUM_WRONG - wrongLetterCount) + " goes left!", "Wrong Letter", MessageBoxButton.OK);
                    Thread.Sleep(100);
                    usedLetters.Add(guessedLetter);
                    letterBox.Text = "";
                }
            }

            if (wrongLetterCount != MAXIMUM_WRONG && wordSeparated.SequenceEqual(hiddenLetters))
            {
                MessageBox.Show("Well Done! You Won!", "Winner!", MessageBoxButton.OK);
                Repeat();
            }
            else if (wrongLetterCount == MAXIMUM_WRONG)
            {
                MessageBox.Show("Sorry you lost!\nThe word was " + word + ".", "Lost!", MessageBoxButton.OK);
                Repeat();
            }
        }
        
        /*
         *  This method connects to the SQL Server Database and collects all the words.
         *  After that, it calls the randomiser method which provides it with a random number.
         *  That number acts as an index to the list of words and therefore picks a word.
         */
        static string Word_Picker()
        {
            //Vars
            string word;
            List<string> data = new List<string>();
            int number, amountOfWords;

            //Establishing Connection with Database and getting all the words
            Database_Connection conn = new Database_Connection();
            data = conn.Data_Gathering();
            amountOfWords = data.Count;
            number = Number_Generator(amountOfWords);

            //Assiging Word
            word = data[number];

            return word;
        }

        /*
         * This method uses a random method to generate a number
         */
        static int Number_Generator(int wordAmount)
        {
            //Vars
            int number;

            //Getting a number
            Random rdn = new Random();
            number = rdn.Next(1, wordAmount);

            return number;
        }

        /*
         * The method displays a message box to the user asking to repeat.
         * If the user chooses yes, the app is restarted
         * If the user chooses no, a message box is displayed and the programme quits
         */
        static void Repeat()
        {
            var result = MessageBox.Show("Would you like to play again?", "Play Again?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Thank you for playing the game!\nCredits:\nMade by Alan Jachimczak\nCopyrighted (C) by Alan Jachimczak", "Thank you for playing", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Environment.Exit(0);
            }
        }
    }
}
