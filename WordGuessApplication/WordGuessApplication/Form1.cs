using System.Collections;
using System.Text;
using System.Linq;
using System.Diagnostics.Contracts;

namespace WordGuessApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // For random generation of objects
        Random rand;
        // Array of words to guess
        string[] words = {"Keyboard", "Computer", "Technology", "Programming",
                          "Algorithm", "Debugging", "Internet", "Monitor"};

        string word; // Current word being guessed
        int word_order; // Index of the current word in the words array

        public void load_word()
        {
            if (word_order < words.Length) // Check if there are more words to load
            {
                word = words[word_order].ToUpper();
                int word_length = word.Length; 
                StringBuilder askedWord = new StringBuilder(word_length); 
                int m_asks = word_length / 2;

                // List of all indices of the word
                List<int> indices = Enumerable.Range(0, word.Length).ToList();

                // Shuffle the indices
                ShuffleList(indices);

                // Select the first 'masks' number of indices to mask letters
                List<int> letter_masks = indices.Take(m_asks).ToList();

                // Construct the masked (hidden) word
                for (int i = 0; i < word.Length; i++)
                {
                    if (letter_masks.Contains(i))
                    {
                        askedWord.Append("?"); // Mask (hide) character
                    }
                    else
                    {
                        askedWord.Append(word[i]); // Make it visible
                    }
                }
                // Display the hidden word 
                lb_asked_word.Text = askedWord.ToString();
            }
            else
            {
                refresh_words();
                load_word();
            }
        }

        public void refresh_words()
        {
            // Shuffle the array of words
            ShuffleArray(words);
            word_order = 0; // Reset word order
        }

        // Function to randomize an array of words  
        private void ShuffleArray(string[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1); // Get a random index between 0 and i
                // Swap the words at index i and j
                string temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        // Function to shuffle a list of integer
        private void ShuffleList(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1); // Get a random index between 0 and i
                // Swap elements
                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rand = new Random();
            word_order = 0; // Initialize word order
            refresh_words(); // Shuffle words before starting
            load_word();// Load the first word
        }

        private void tb_guess_Click(object sender, EventArgs e)
        {
            // Check if the current guess is correct
            if (tb_answer.Text.ToUpper() == word) // Exact match check
            {
                lb_asked_word.Text = word; // Show the full word if guessed correctly
                MessageBox.Show("Correct guess!"); 
                word_order++; // Move to the next word
                load_word(); // Load a new word 
                wrong_guessed_list.Items.Clear(); // Clear the list of wrong guesses
                tb_answer.Text = ""; // Clear the answer 
            }
            else if (word.Contains(tb_answer.Text.ToUpper())) // Partial match check using Contains
            {
                MessageBox.Show("Your guess is partially correct, but not the full word.");
                wrong_guessed_list.Items.Add(tb_answer.Text); // Add to wrong guess list 
            }
            else
            {
                // Add the wrong guess to the list
                wrong_guessed_list.Items.Add(tb_answer.Text); // Mark guess as incorrect
                MessageBox.Show("Wrong guess!\nTry Again");
            }

            tb_answer.Text = ""; // Clear answer after each guess
        }
    }
}