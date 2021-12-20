/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Mechanical
{
    /// <summary>
    /// The console allows the execution of commands from the game. To make a new command, put a <see cref="ConsoleCommandAttribute"/> on a static function with a single parameter of a <see cref="string"/>[].
    /// <para>
    /// Dont worry, no RCE here (unlike Log4J) 😏
    /// </para>
    /// </summary>
    public static class Console
    {

        /// <summary>
        /// The amount of time in seconds a key needs to be held in order for it to repeat.
        /// </summary>
        public const float HOLD_TIME = 1f;

        /// <summary>
        /// If the console is open.
        /// </summary>
        public static bool IsOpen { get; set; }

        /// <summary>
        /// The window of the console.
        /// </summary>
        public static ConsoleWindow Window { get; set; }

        /// <summary>
        /// A dictionary of commands.
        /// 
        /// <para>
        /// The key is the name of the command, while the value is the <see cref="MethodInfo"/> that will be used to invoke the command function.
        /// </para>
        /// </summary>
        public static Dictionary<string, MethodInfo> Commands { get; private set; } = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// The list of command names and their help descriptions.
        /// 
        /// <para>
        /// The key is the command name, while the value is the help text.
        /// </para>
        /// </summary>
        public static Dictionary<string, string> CommandHelps { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// A list of names for commands.
        /// </summary>
        public static List<string> CommandNames { get; private set; } = new List<string>();

        /// <summary>
        /// The text to display when there is an invalid command.
        /// </summary>
        public const string HelpText = @"Looks like that command does not exist! Use the `help` command to see the help for that command, or `listCommands` to see a list of all commands.";

        /// <summary>
        /// A list of lines that are in the output and the line colors.
        /// </summary>
        public static List<ConsoleLine> Output { get; private set; } = new List<ConsoleLine>();

        /// <summary>
        /// The text that is currently in the input window.
        /// </summary>
        public static string CurrentInput { get; private set; } = "";

        /// <summary>
        /// The position of the caret in the input window.
        /// </summary>
        public static int caretPosition = 0;

        /// <summary>
        /// If the caps lock is on.
        /// </summary>
        private static bool capsLockOn = false;

        /// <summary>
        /// The list of commands previously run.
        /// </summary>
        private static List<string> commandHistory = new List<string>();

        /// <summary>
        /// The index of the <see cref="commandHistory"/>. Set to -1 when typing a new command.
        /// </summary>
        private static int commandHistoryIndex = -1;

        /// <summary>
        /// If the shift key is held.
        /// </summary>
        private static bool IsShiftDown { get => MechKeyboard.IsKeyDown(Keys.LeftShift) || MechKeyboard.IsKeyDown(Keys.RightShift); }

        /// <summary>
        /// How many times a line is repeated in the console.
        /// </summary>
        private static Dictionary<string, int> OutputRepeats = new Dictionary<string, int>();

        public static void Initialize()
        {
            Window = new ConsoleWindow();
        }

        public static void LoadContent(ContentManager content)
        {
            Window.LoadContent(content);
        }

        public static void Update(float deltaTime)
        {
            Keys[] keys = MechKeyboard.GetPressedKeys();

            for (int i = 0; i < keys.Length; i++)
            {
                Keys key = keys[i];

                //todo: insert text at carat

                float time = MechKeyboard.GetTimeHeld(key);

                if (time == 0 || time > HOLD_TIME)
                {
                    bool dontMoveCaret = true;

                    #region Keys
                    switch (key)
                    {
                        case Keys.Back:
                            if (caretPosition - 1 < 0)
                            {
                                break;
                            }
                            CurrentInput = CurrentInput.Remove(caretPosition - 1, 1);
                            caretPosition--;
                            dontMoveCaret = true;
                            break;
                        case Keys.Tab:
                            CurrentInput = CurrentInput.Insert(caretPosition, "    ");
                            dontMoveCaret = true;
                            caretPosition += 4;
                            break;
                        case Keys.Enter:
                            if (CurrentInput != "")
                            {
                                RunCommand(CurrentInput);
                                caretPosition = 0;
                                CurrentInput = "";
                                dontMoveCaret = true;
                            }
                            break;
                        case Keys.CapsLock:
                            capsLockOn = !capsLockOn;
                            dontMoveCaret = true;
                            break;
                        case Keys.Escape:
                        case Keys.OemTilde:
                            // set false because console will only update if open.
                            //IsOpen = false;
                            break;
                        case Keys.Space:
                            CurrentInput = CurrentInput.Insert(caretPosition, " ");
                            dontMoveCaret = false;
                            break;
                        // move caret.
                        case Keys.Left:
                            caretPosition--;
                            if (caretPosition < 0)
                            {
                                caretPosition = 0;
                            }
                            dontMoveCaret = true;
                            break;
                        case Keys.Right:
                            caretPosition++;
                            if (caretPosition > CurrentInput.Length + 1)
                            {
                                caretPosition = CurrentInput.Length + 1;
                            }
                            dontMoveCaret = true;
                            break;
                        // cycle through history.
                        case Keys.PageUp:
                        case Keys.Up:
                            if (commandHistory.Count > 0)
                            {
                                commandHistoryIndex++;
                                CurrentInput = commandHistory[commandHistoryIndex];
                                dontMoveCaret = true;
                                caretPosition = CurrentInput.Length;
                            }
                            break;
                        // cycle through history.
                        case Keys.PageDown:
                        case Keys.Down:
                            if (commandHistory.Count > 0)
                            {
                                commandHistoryIndex--;
                                if (commandHistoryIndex < 0)
                                {
                                    commandHistoryIndex = -1;
                                    CurrentInput = "";
                                }
                                CurrentInput = commandHistory[commandHistoryIndex];
                                dontMoveCaret = true;
                                caretPosition = CurrentInput.Length;
                            }
                            break;
                        case Keys.Delete:
                        case Keys.OemClear:
                            CurrentInput = "";
                            caretPosition = 0;
                            dontMoveCaret = true;
                            break;
                        // thanks: https://stackoverflow.com/questions/11917126/xna-keys-number-1
                        // lol
                        case Keys.D0:
                        case Keys.NumPad0:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput = CurrentInput.Insert(caretPosition, ")");
                            else
                                CurrentInput = CurrentInput.Insert(caretPosition, "0");
                            caretPosition++;
                            break;
                        case Keys.D1:
                        case Keys.NumPad1:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput = CurrentInput.Insert(caretPosition, "!");
                            else
                                CurrentInput = CurrentInput.Insert(caretPosition, "1");
                            caretPosition++;
                            break;
                        case Keys.D2:
                        case Keys.NumPad2:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "@";
                            else
                                CurrentInput += "2";
                            caretPosition++;
                            break;
                        case Keys.D3:
                        case Keys.NumPad3:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "#";
                            else
                                CurrentInput += "3";
                            caretPosition++;
                            break;
                        case Keys.D4:
                        case Keys.NumPad4:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "$";
                            else
                                CurrentInput += "4";
                            caretPosition++;
                            break;
                        case Keys.D5:
                        case Keys.NumPad5:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "%";
                            else
                                CurrentInput += "5";
                            caretPosition++;
                            break;
                        case Keys.D6:
                        case Keys.NumPad6:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "^";
                            else
                                CurrentInput += "6";
                            caretPosition++;
                            break;
                        case Keys.D7:
                        case Keys.NumPad7:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "&";
                            else
                                CurrentInput += "7";
                            caretPosition++;
                            break;
                        case Keys.D8:
                        case Keys.NumPad8:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "*";
                            else
                                CurrentInput += "8";
                            caretPosition++;
                            break;
                        case Keys.D9:
                        case Keys.NumPad9:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "(";
                            else
                                CurrentInput += "9";
                            caretPosition++;
                            break;
                        case Keys.F1:
                        case Keys.F2:
                        case Keys.F3:
                        case Keys.F4:
                        case Keys.F5:
                        case Keys.F6:
                        case Keys.F7:
                        case Keys.F8:
                        case Keys.F9:
                        case Keys.F10:
                        case Keys.F11:
                        case Keys.F12:
                        case Keys.F13:
                        case Keys.F14:
                        case Keys.F15:
                        case Keys.F16:
                        case Keys.F17:
                        case Keys.F18:
                        case Keys.F19:
                        case Keys.F20:
                        case Keys.F21:
                        case Keys.F22:
                        case Keys.F23:
                        case Keys.F24:
                            break;
                        case Keys.OemPlus:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "+";
                            else
                                CurrentInput += "=";
                            caretPosition++;
                            break;
                        case Keys.OemComma:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "<";
                            else
                                CurrentInput += ",";
                            caretPosition++;
                            break;
                        case Keys.OemMinus:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "_";
                            else
                                CurrentInput += "-";
                            caretPosition++;
                            break;
                        case Keys.OemPeriod:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += ">";
                            else
                                CurrentInput += ".";
                            caretPosition++;
                            break;
                        case Keys.OemQuestion:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "?";
                            else
                                CurrentInput += "/";
                            caretPosition++;
                            break;
                        case Keys.OemSemicolon:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += ":";
                            else
                                CurrentInput += ";";
                            caretPosition++;
                            break;
                        case Keys.OemOpenBrackets:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "{";
                            else
                                CurrentInput += "[";
                            caretPosition++;
                            break;
                        case Keys.OemPipe:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "|";
                            else
                                CurrentInput += "\\";
                            caretPosition++;
                            break;
                        case Keys.OemCloseBrackets:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "}";
                            else
                                CurrentInput += "]";
                            caretPosition++;
                            break;
                        case Keys.OemQuotes:
                            if (IsShiftDown || capsLockOn)
                                CurrentInput += "\"";
                            else
                                CurrentInput += "\'";
                            caretPosition++;
                            break;
                        default:

                            if (key != Keys.LeftControl && key != Keys.RightControl && key != Keys.RightAlt && key != Keys.LeftAlt && key != Keys.LeftWindows && key != Keys.RightWindows && key != Keys.RightShift && key != Keys.LeftShift)
                            {
                                dontMoveCaret = false;
                                // key not specified
                                if (IsShiftDown)
                                    CurrentInput = CurrentInput.Insert(caretPosition, key.ToString());
                                else
                                    CurrentInput = CurrentInput.Insert(caretPosition, key.ToString().ToLower());
                                //caretPosition++;
                            }

                            break;
                    }
                    #endregion

                    if (!dontMoveCaret)
                    {
                        caretPosition++;
                    }

                }

            }

            Window.Update(deltaTime);
        }

        public static void Draw()
        {
            Window.Draw();
        }

        /// <summary>
        /// Scan the assemblies for commands and add them to the list.
        /// </summary>
        /// <param name="gameAssembly">The assembly of the game.</param>
        public static void GetCommands(Assembly gameAssembly)
        {
            // get game types.
            Type[] gameTypes = gameAssembly.GetTypes();
            // get engine types.
            Type[] engineTypes = Assembly.GetExecutingAssembly().GetTypes();

            // combine types.
            Type[] types = new Type[gameTypes.Length + engineTypes.Length];
            for (int i = 0; i < gameTypes.Length; i++)
            {
                types[i] = gameTypes[i];
            }
            for (int j = 0; j < engineTypes.Length; j++)
            {
                types[j + gameTypes.Length] = engineTypes[j];
            }

            // loop through types and find functions.
            for (int i = 0; i < types.Length; i++)
            {
                MethodInfo[] functions = types[i].GetMethods(BindingFlags.Static | BindingFlags.Public);

                // loop theough functions
                for (int j = 0; j < functions.Length; j++)
                {
                    ConsoleCommandAttribute[] attrs = (ConsoleCommandAttribute[])functions[j].GetCustomAttributes<ConsoleCommandAttribute>();

                    for (int k = 0; k < attrs.Length; k++)
                    {
                        ConsoleCommandAttribute a = attrs[k];
                        if (a != null)
                        {
                            Commands.Add(a.Name, functions[j]);
                            CommandNames.Add(a.Name);
                            CommandHelps.Add(a.Name, a.Usage);
                        }
                    }
                }
            }

        } 

        /// <summary>
        /// Prints a message to the console.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="type">The type of message.</param>
        public static void Log(string message, ConsoleMessageType type = ConsoleMessageType.Message)
        {
            string[] lines = message.Split('\n');
            // get color
            Color c = Color.Black;

            switch (type)
            {
                case ConsoleMessageType.Message:
                    c = Color.White;
                    break;
                case ConsoleMessageType.Output:
                    c = Color.Aqua;
                    break;
                case ConsoleMessageType.Warning:
                    c = Color.Yellow;
                    break;
                case ConsoleMessageType.Error:
                    c = Color.Tomato;
                    break;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];
                Output.Add(new ConsoleLine(l, c));
            }
        }

        /// <summary>
        /// Prints a message to the console.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="color">The color of the text.</param>
        public static void Log(string message, Color color)
        {
            string[] lines = message.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];
                Output.Add(new ConsoleLine(l, color));
            }
        }

        /// <summary>
        /// Get a command based off its name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <returns>The method info of the command.</returns>
        public static MethodInfo GetCommand(string name)
        {
            if (!CommandNames.Contains(name))
            {
                Log($"The commnand, {name}, does not exist!", ConsoleMessageType.Error);
                return null;
            }
            else
            {
                return Commands[name];
            }
        }

        /// <summary>
        /// Run a console command.
        /// </summary>
        /// <param name="command">The whole string in the input window.</param>
        public static void RunCommand(string command)
        {
            string[] commandInfo = command.SplitArgs();
            string name = commandInfo[0];

            // make new array with one less count.
            string[] args = new string[commandInfo.Length - 1];

            for (int i = 0; i < commandInfo.Length - 1; i++)
            {
                args[i] = commandInfo[i + 1];
            }

            MethodInfo info = GetCommand(name);

            if (info != null)
            {
                try
                {
                    Log(command, ConsoleMessageType.Message);
                    info.Invoke(null, new object[] { args });
                }
                catch (Exception e)
                {
                    Log($"=== A FATAL ERROR HAS OCCURRED WHEN TRYING TO RUN COMMAND. ({name}) ===", ConsoleMessageType.Error);
                    Log($"The stack trace is as follows: \n {e.StackTrace}", ConsoleMessageType.Error);
                    Log("==============================================================", ConsoleMessageType.Error);
                }
            }

        }

        #region Commands
        /// <summary>
        /// This command clears the console window.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        [ConsoleCommand("clear", "This command clears the console window. No paremeters needed.")]
        public static void ClearConsole(string[] arguments)
        {
            NeedsNoArguments(arguments);
            Output.Clear();
        }

        /// <summary>
        /// This command quits the game.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        [ConsoleCommand("quit", "This command will quit the game immediately. No paremeters needed.")]
        [ConsoleCommand("exit", "This command will exit the game immediately. No paremeters needed.")]
        public static void QuitGame(string[] arguments)
        {
            NeedsNoArguments(arguments);
            Engine.Instance.Exit();
        }

        /// <summary>
        /// Prints the text to the console.
        /// </summary>
        /// <param name="arguments">The aguments.</param>
        [ConsoleCommand("echo", "This command will print the text to the console.\nArguments: \n    string: the message to print\n    color: the color to use = White")]
        public static void Echo(string[] arguments)
        {
            if (arguments.Length <= 0 || arguments.Length > 2)
            {
                Log("There are too many or not enough arguments! Use the `help` command to see the needed arguments.", ConsoleMessageType.Error);
                return;
            }

            // only name
            Color c = Color.White;
            if (arguments.Length == 2)
            {
                c = arguments[1].GetColorByName();
            }

            Log(arguments[0], c);
        }

        /// <summary>
        /// Closes the console.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        [ConsoleCommand("close", "Closes the console.")]
        public static void CloseConsole(string[] arguments)
        {
            NeedsNoArguments(arguments);
            IsOpen = false;
        }

        /// <summary>
        /// The help command.
        /// </summary>
        /// <param name="arguments"></param>
        [ConsoleCommand("help", "The help command prints the help data for a command. Leave out argument to list all commands. \nArguments: \n    string: The command.")]
        public static void HelpCommand(string[] arguments)
        {
            if (arguments.Length > 1) Log("Too many arguments!", ConsoleMessageType.Error);

            if (arguments.Length == 0)
            {
                string commands = "commands: ";
                // print all args
                for (int i = 0; i < CommandNames.Count - 1; i++)
                {
                    commands += CommandNames[i] + ", ";
                }
                commands += CommandNames[CommandNames.Count - 1];
                Log(commands, ConsoleMessageType.Output);
            }
            else
            {
                if (!CommandHelps.ContainsKey(arguments[0]))
                {
                    HelpCommand(new string[] { });
                    return;
                }
                else
                {
                    Log($"Listing help for {arguments[0]}: {CommandHelps[arguments[0]]}", ConsoleMessageType.Output);
                }
            }
        }

        /// <summary>
        /// Returns true and logs if there are arguments when the command needs none.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>Returns true if there are arguments when the command needs none.</returns>
        public static bool NeedsNoArguments(string[] arguments)
        {
            if (arguments.Length != 0)
            {
                Log("No arguments are needed for this command.", ConsoleMessageType.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Toggles the console.
        /// </summary>
        public static void Toggle()
        {
            IsOpen = !IsOpen;
            Engine.Instance.Paused = IsOpen;
            CurrentInput = "";
            caretPosition = 0;
        }

    }

    /// <summary>
    /// This represents a line of the console.
    /// </summary>
    public struct ConsoleLine
    {
        /// <summary>
        /// The text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The color.
        /// </summary>
        public Color Color { get; set; }

        public ConsoleLine(string text, Color color)
        {
            Text = text;
            Color = color;
        }
    }

}
