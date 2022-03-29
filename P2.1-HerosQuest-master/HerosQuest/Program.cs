using System;

namespace HerosQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            string playAgain = "yes";
            while (playAgain == "yes") // this loop gives users the opportunity to play again
            {
                Character ranger, mage, barbarian;
                SetupCharacters(out ranger, out mage, out barbarian);

                Character winner = null;
                while (winner == null)
                {
                    /* Turn order is always ranger (the quickest), then mage, then barbarian (the slowest).
                     * Before each character takes their turn a check is performed to see if they are still alive.
                     * After each character takes their turn there is a check to see if the other two characters are still alive or not.
                     * if they are not then the character who just too their turn is assigned to the winner Character reference, and the
                     * continue keywork skips ahead to the loop condition, breaking the loop.
                     */
                    if (ranger._Health > 0)
                    {
                        TakeRangerTurn(ranger, mage, barbarian);
                        if (mage._Health <= 0 && barbarian._Health <= 0)
                        {
                            winner = ranger;
                            continue;
                        }
                    }

                    if (mage._Health > 0)
                    {
                        TakeMageTurn(ranger, mage, barbarian);
                        if (ranger._Health <= 0 && barbarian._Health <= 0)
                        {
                            winner = mage;
                            continue;
                        }
                    }

                    if (barbarian._Health > 0)
                    {
                        TakeBarbarianTurn(ranger, mage, barbarian);
                        if (ranger._Health <= 0 && mage._Health <= 0)
                        {
                            winner = barbarian;
                            continue;
                        }
                    }
                }; // winner == null loop end

                Console.WriteLine("Congratulations " + winner._Name + ", you win!");
                do
                {
                    Console.WriteLine("Would you like to play again? Enter yes or no");
                    playAgain = Console.ReadLine();
                } while (playAgain != "yes" || playAgain != "no");

            } // play again loop end

            Console.WriteLine("Thank you for playing Hero's Quest");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        } // Main method end

        /// <summary>
        /// Prints welcome message and sets up the three heroes by asking the user to input names for each
        /// </summary>
        /// <param name="ranger">Ranger character to be set up</param>Output
        /// <param name="mage">Mage character to be set up</param>
        /// <param name="barbarian">Barbarian character to be set up</param>
        private static void SetupCharacters(out Character ranger, out Character mage, out Character barbarian)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Welcome to hero's quest! - A quest for heroes!");
            Console.WriteLine("Player 1 - You are the Ranger! Name your character!");
            string name = Console.ReadLine();
            ranger = new Character("ranger", name);
            Console.WriteLine("Welcome " + ranger._Name + ", the fastest ranger in all of Hullidian!");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Player 2 - You are the Mage! Name your character!");
            name = Console.ReadLine();
            mage = new Character("mage", name);
            Console.WriteLine("Welcome " + mage._Name + ", the wisest mage in all of Hullidian!");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Player 3 - You are the Barbarian! Name your character!");
            name = Console.ReadLine();
            barbarian = new Character("barbarian", name);
            Console.WriteLine("Welcome " + barbarian._Name + ", the strongest barbarian in all of Hullidian!");
        }

        /// <summary>
        /// Takes a turn for the ranger character, against the mage and barbarian opponents
        /// </summary>
        /// <param name="ranger">Ranger character taking turn</param>
        /// <param name="mage">Mage character opponent</param>
        /// <param name="barbarian">Barbarian character opponent</param>
        private static void TakeRangerTurn(Character ranger, Character mage, Character barbarian)
        {
            string rangerMenu = "1. Rest.\r\n" +
                                "2. Take potion of healing.\r\n" +
                                "3. Take potion of vitality.\r\n" +
                                "4. Fire arrow at " + mage._Name + " the mage.\r\n" +
                                "5. Fire arrow at " + barbarian._Name + " the barbarian.\r\n" +
                                "6. Collect arrows.\r\n";

            bool successfulAction = false;

            // output the currect game state
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            ranger.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            mage.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            barbarian.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            do // this loop repeats until the player takes a successful action
            {
                Console.WriteLine(ranger._Name + " it is your turn!");

                int selection = GetNumberInRange(rangerMenu, 1, 6);

                switch (selection)
                {
                    case 1:
                        ranger.Rest();
                        successfulAction = true; // the rest action is always successful
                        break;
                    case 2:
                        successfulAction = ranger.TakeHealthPotion();
                        break;
                    case 3:
                        successfulAction = ranger.TakeEnergyPotion();
                        break;
                    case 4:
                        successfulAction = ranger.FireBow(mage);
                        break;
                    case 5:
                        successfulAction = ranger.FireBow(barbarian);
                        break;
                    case 6:
                        successfulAction = ranger.PickUpArrows();
                        break;
                }
            } while (!successfulAction);

            if (ranger._Allied != null)
            {
                Console.WriteLine("You are no longer allied with " + ranger._Allied._Name);
                ranger._Allied = null;
            }
        }

        /// <summary>
        /// Takes a turn for the mage character, against the ranger and barbarian opponents
        /// </summary>
        /// <param name="ranger">Ranger character opponent</param>
        /// <param name="mage">Mage character taking turn</param>
        /// <param name="barbarian">Barbarian character opponent</param>
        private static void TakeMageTurn(Character ranger, Character mage, Character barbarian)
        {

            string mageMenu = "1. Rest.\r\n" +
                                            "2. Take potion of healing.\r\n" +
                                            "3. Take potion of vitality.\r\n" +
                                            "4. Throw fireball at " + ranger._Name + " the ranger.\r\n" +
                                            "5. Throw fireball at " + barbarian._Name + " the barbarian.\r\n" +
                                            "6. Cast healing spell on yourself.\r\n" +
                                            "7. Cast healing spell on " + ranger._Name + " the ranger.\r\n" +
                                            "   You will become allies for " + ranger._Name + "'s next turn.\r\n" +
                                            "8. Cast healing spell on " + barbarian._Name + " the barbarian.\r\n" +
                                            "   You will become allies for " + barbarian._Name + "'s next turn.\r\n";

            bool successfulAction = false;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            ranger.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            mage.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            barbarian.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            do
            {
                Console.WriteLine(mage._Name + " it is your turn!");

                int selection = GetNumberInRange(mageMenu, 1, 8);

                switch (selection)
                {
                    case 1:
                        mage.Rest();
                        successfulAction = true;
                        break;
                    case 2:
                        successfulAction = mage.TakeHealthPotion();
                        break;
                    case 3:
                        successfulAction = mage.TakeEnergyPotion();
                        break;
                    case 4:
                        successfulAction = mage.ThrowFireball(ranger);
                        break;
                    case 5:
                        successfulAction = mage.ThrowFireball(barbarian);
                        break;
                    case 6:
                        successfulAction = mage.HealPlayer(mage);
                        break;
                    case 7:
                        successfulAction = mage.HealPlayer(ranger);
                        break;
                    case 8:
                        successfulAction = mage.HealPlayer(barbarian);
                        break;
                }
            } while (!successfulAction);

        }

        /// <summary>
        /// Takes a turn for the barbarian character, against the ranger and mage opponents
        /// </summary>
        /// <param name="ranger">Ranger character opponent</param>
        /// <param name="mage">Mage character opponent</param>
        /// <param name="barbarian">Barbarian character taking turn</param>
        private static void TakeBarbarianTurn(Character ranger, Character mage, Character barbarian)
        {
            string barbarianMenu = "1. Rest.\r\n" +
                                            "2. Take potion of healing.\r\n" +
                                            "3. Take potion of vitality.\r\n" +
                                            "4. Swing axe at " + ranger._Name + " the ranger.\r\n" +
                                            "5. Swing axe at " + mage._Name + " the mage.\r\n";
            bool successfulAction = false;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            ranger.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            mage.OutputState();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            barbarian.OutputState();

            do
            {
                Console.WriteLine(barbarian._Name + " it is your turn!");

                int selection = GetNumberInRange(barbarianMenu, 1, 5);

                switch (selection)
                {
                    case 1:
                        barbarian.Rest();
                        successfulAction = true;
                        break;
                    case 2:
                        successfulAction = barbarian.TakeHealthPotion();
                        break;
                    case 3:
                        successfulAction = barbarian.TakeEnergyPotion();
                        break;
                    case 4:
                        successfulAction = barbarian.SwingAxe(ranger);
                        break;
                    case 5:
                        successfulAction = barbarian.SwingAxe(mage);
                        break;
                }
            } while (!successfulAction);

            if (barbarian._Allied != null)
            {
                Console.WriteLine("You are no longer allied with " + barbarian._Allied._Name);
                barbarian._Allied = null;
            }
        }

        /// <summary>
        /// Gets number from the user within the specified range.
        /// </summary>
        /// <param name="pPrompt">prompt to display to the user.</param>
        /// <param name="pMin">Inclusive minimum value that could be returned.</param>
        /// <param name="pMax">Inclusive maximum value that could be returned.</param>
        /// <returns></returns>
        private static int GetNumberInRange(string pPrompt, int pMin, int pMax)
        {
            int result = 0;
            do
            {
                Console.WriteLine(pPrompt);
                Console.WriteLine("Please enter a selection between " + pMin + " and " + pMax + " inclusive");
                string input = Console.ReadLine();

                try
                {
                    result = int.Parse(input);
                }
                catch
                {
                    Console.WriteLine(input + " is not a number.");
                    continue;
                }

                if (result >= pMin && result <= pMax)
                {
                    return result;
                }
            } while (true);
        }
    }
}
