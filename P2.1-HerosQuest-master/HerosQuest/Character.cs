using System;

namespace HerosQuest
{
    public class Character
    {
        static Random rng = new Random();

        public string _Name;
        public int _Health;
        public int _MaxHealth;
        public int _Energy;
        public int _MaxEnergy;
        public int _EnergyPotions;
        public int _HealthPotions;
        public string _CharacterType;
        public int _NumberOfArrows;
        public int _MaxNumberOfArrows;
        public int _Rage;
        public int _MaxRage;
        public Character _Allied;

        public Character(string pType, string pName)
        {
            _Name = pName;
            _CharacterType = pType;
            switch (_CharacterType)
            {
                case "ranger":
                    _MaxHealth = rng.Next(10, 15);
                    _MaxEnergy = rng.Next(4, 9);
                    _EnergyPotions = rng.Next(1, 4);
                    _HealthPotions = rng.Next(1, 4);
                    _MaxNumberOfArrows = rng.Next(4, 9);
                    break;
                case "mage":
                    _MaxHealth = rng.Next(8, 11);
                    _MaxEnergy = rng.Next(6, 13);
                    _EnergyPotions = rng.Next(1, 3);
                    _HealthPotions = rng.Next(2, 4);
                    break;
                case "barbarian":
                    _MaxHealth = rng.Next(14, 19);
                    _MaxEnergy = rng.Next(8, 13);
                    _EnergyPotions = rng.Next(2, 4);
                    _HealthPotions = rng.Next(1, 3);
                    _MaxRage = rng.Next(4, 9);
                    break;
            }

            _Rage = 0;
            _Health = _MaxHealth;
            _Energy = _MaxEnergy;
            _NumberOfArrows = _MaxNumberOfArrows;
        }

        public bool TakeEnergyPotion()
        {
            // error string used to detect is anything is wrong
            string error = "";

            if (_EnergyPotions < 1)
            {
                error += "You do not have any energy potions left.\r\n";
            }

            if (_Energy < 1)
            {
                error += "You need at least 1 energy to use a potion.\r\n";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            _Energy--;
            _Energy += _MaxEnergy / 2;
            Console.WriteLine("You take an energy potion.");
            Console.WriteLine("Your energy is now " + _Energy);
            return true;
        }

        public bool TakeHealthPotion()
        {
            // error string used to detect is anything is wrong
            string error = "";

            if (_HealthPotions < 1)
            {
                error += "You do not have any health potions left.\r\n";
            }

            if (_Energy < 1)
            {
                error += "You need at least 1 energy to use a potion.\r\n";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            _Energy--;
            _Health += _MaxHealth / 2;
            Console.WriteLine("You take a health potion.");
            Console.WriteLine("Your health is now " + _Health);
            return true;
        }

        public void Rest()
        {
            int energy = 3 + rng.Next(4);
            int health = 3 + rng.Next(4);
            _Energy += (energy);
            _Health += (health);

            Console.WriteLine("You are well rested.");
            Console.WriteLine("Your energy has increased by {0} to {1} / {2}.", energy, _Energy, _MaxEnergy);
            Console.WriteLine("Your health has increased by {0} to {1} / {2}.", health, _Health, _MaxHealth);
        }

        public bool FireBow(Character pTarget)
        {
            // should never happen, so throw an exception
            if (_CharacterType != "ranger")
            {
                throw new Exception(_CharacterType + " cannot use a bow.");
            }

            // error string used to detect is anything is wrong
            string error = "";

            if (_NumberOfArrows == 0)
            {
                error += "You have no arrows to fire!\r\n";
            }

            if (_Energy < 1)
            {
                error += "You need at least 1 energy to use a bow.\r\n";
            }

            if (pTarget == _Allied)
            {
                error += "You cannot attack " + pTarget + " because you are allied with them this turn.";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            _NumberOfArrows--;
            _Energy--;
            Console.WriteLine(_Name + " releases a deadly arrow towards the " + pTarget._Name);

            int roll = rng.Next(1, 21);

            if (roll < 4)
            {
                Console.WriteLine("The arrow misses " + pTarget._Name + " completely!");
            }
            else if (roll < 7)
            {
                Console.WriteLine("The arrow grazes " + pTarget._Name + "'s limb dealing 1 damage.");
                pTarget._Health -= 1;
            }
            else if (roll < 13)
            {
                Console.WriteLine("The arrow hits " + pTarget._Name + "'s  torso dealing 2 damage!");
                Console.WriteLine("You regain 1 energy.");
                pTarget._Health -= 2;
                _Energy++;
            }
            else
            {
                Console.WriteLine("The arrow hits " + pTarget._Name + "'s  head dealing 3 damage!");
                Console.WriteLine("You regain 2 energy.");
                _Energy += 2;
                pTarget._Health -= 3;
            }

            _Allied = null;
            return true;
        }

        public bool PickUpArrows()
        {
            // should never happen, so throw an exception
            if (_CharacterType != "ranger")
            {
                throw new Exception(_CharacterType + " cannot pick up arrows.");
            }

            // error string used to detect is anything is wrong
            string error = "";

            if (_Energy < 1)
            {
                error += "You need at least 1 energy to pick up arrows.";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            _Energy--;

            int minimumArrows = Math.Min(2, _MaxNumberOfArrows - _NumberOfArrows);
            int arrowsCollected = rng.Next(minimumArrows, _MaxNumberOfArrows - _NumberOfArrows);
            _NumberOfArrows += arrowsCollected;
            Console.WriteLine(_Name + " the ranger picked up " + arrowsCollected + " and now has " + _NumberOfArrows + "/" + _MaxNumberOfArrows);
            return true;
        }

        public bool SwingAxe(Character pTarget)
        {
            // should never happen, so throw an exception
            if (_CharacterType != "barbarian")
            {
                throw new Exception(_CharacterType + " cannot use an axe.");
            }

            // error string used to detect is anything is wrong
            string error = "";

            if (pTarget._Health <= 0)
            {
                error += pTarget._Name + " is already dead.";
            }

            if (_Energy <= 0)
            {
                error += "You need at least 1 energy to swing your axe.";
            }

            if (pTarget == _Allied)
            {
                error += "You cannot attack " + pTarget._Name + " because you are allied with them this turn.";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true

            Console.WriteLine(_Name + " swings his mighty axe at " + pTarget._Name);
            int damageMultiplier = 1;
            if (_Rage >= _MaxRage)
            {
                Console.WriteLine("RAGING STRIKE - THIS STRIKE WILL DEAL DOUBLE DAMAGE!");
                damageMultiplier = 2;
                _Rage = 0;
            }

            _Energy--;

            int roll = rng.Next(1, 21);
            if (roll < 4)
            {
                Console.WriteLine("The axe misses " + pTarget._Name + " completely!");
                _Rage += 4;
            }
            else if (roll < 9)
            {

                Console.WriteLine("The axe grazes " + pTarget._Name + "'s leg dealing " + (damageMultiplier * 2) + " damage!");
                pTarget._Health -= (damageMultiplier * 2);
                _Rage += 3;
            }
            else if (roll < 17)
            {

                Console.WriteLine("The axe crashed into " + pTarget._Name + "'s torso dealing " + (damageMultiplier * 3) + " damage.");
                pTarget._Health -= (damageMultiplier * 3);
                _Rage += 2;
            }
            else
            {
                _Rage += 1;
                Console.WriteLine("The axe smashes into " + pTarget._Name + "'s head dealing " + (damageMultiplier * 4) + " damage.");
                pTarget._Health -= (damageMultiplier * 4);
            }

            Console.WriteLine("Your rage increases to " + _Rage);
            return true;
        }

        public bool ThrowFireball(Character pTarget)
        {
            // should never happen, so throw an exception
            if (_CharacterType != "mage")
            {
                throw new Exception(_CharacterType + " cannot throw a fireball.");
            }

            // error string used to detect is anything is wrong
            string error = "";

            if (pTarget._Health <= 0)
            {
                error += pTarget._Name + " is already dead.";
            }

            if (_Energy <= 0)
            {
                error += "You need at least 1 energy to throw a fireball.";
            }

            if (pTarget == _Allied)
            {
                error += "You cannot attack " + pTarget + " because you are allied with them this turn.";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            int roll = rng.Next(1, 21);

            if (roll == 3)
            {
                Console.WriteLine("The fireball misses " + pTarget._Name + " completely!");
            }
            else if (roll < 7)
            {
                Console.WriteLine("The fireball grazes " + pTarget._Name + "'s limb dealing 1 damage.");
                pTarget._Health -= 1;
            }
            else if (roll < 17)
            {
                Console.WriteLine("The fireball hits " + pTarget._Name + "'s  torso dealing 2 damage!");
                _Energy++;
            }
            else
            {
                Console.WriteLine("The fireball hits " + pTarget._Name + "'s  head dealing 3 damage!");
                pTarget._Health -= 3;
            }

            _Allied = null;
            return true;
        }

        public bool HealPlayer(Character pTarget)
        {
            // should never happen, so throw an exception
            if (_CharacterType != "mage")
            {
                throw new Exception(_CharacterType + " cannot heal a player.");
            }

            // error string used to detect is anything is wrong
            string error = "";

            if (pTarget._Health <= 0)
            {
                error += pTarget._Name + " is already dead.";
            }

            if (_Energy <= 0)
            {
                error += "You need at least 1 energy to heal a player.";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            pTarget._Health += rng.Next(3, 7);
            if (pTarget != this)
            {
                pTarget._Allied = this;
            }
            return true;
        }

        /// <summary>
        /// Outputs the state of this instance of character to the console
        /// </summary>
        public void OutputState()
        {
            string output = _Name + " the " + _CharacterType + ":" + _Health + "/" + _MaxHealth + " Health. " + _Energy + "/" + _MaxEnergy + " Energy. ";
            if (_CharacterType == "ranger")
            {
                output += _NumberOfArrows + "/" + _MaxNumberOfArrows + " arrows.";
            }
            else if (_CharacterType == "barbarian")
            {
                output += _Rage + "/" + _MaxRage + " rage.";
            }

            output += "\r\n" + _Name + " has " + _HealthPotions + " health potions and " + _EnergyPotions + " energy potions.\r\n";

            if (_Allied != null)
            {
                output += _Name + " is currently allied with " + _Allied._Name;
            }

            Console.WriteLine(output);
        }
    }
}
