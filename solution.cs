using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{

    class Error
    {
        public bool isError { get; set; }
        public string error { get; set; }
    }

    class Command
    {
        public string[] arr = new string[3] {
            "Unrecognized command",
            "Invalid Command",
            "Directory already exists"
        };
        public string command { get; set; }
        public Error error { get; set; }
        public Command(string _command)
        {
            command = _command;
            error = new Error();
        }

        public Command() { }

        public Command Validate()
        {
            //Validate of command
            if (command != null)
            {
                //Validate is spliteable
                string commandNoParameters = command.Split(' ')[0];

                switch (commandNoParameters)
                {
                    case "quit":
                        return new CommandQuit(command);
                    case "pwd":
                        return new CommandCurrentDir(command);
                    case "ls":
                        return new CommandListContents(command);
                    case "mkdir":
                        return new CommandMakeDir(command);
                    case "cd":
                        return new CommandChangeDir(command);
                    case "touch":
                        return new CommandCreateDir(command);
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual Error validateParameters()
        {
            return error;
        }

        public virtual bool Execute()
        {
            return true;
        }
    }



    class CommandQuit : Command
    {
        public CommandQuit(string _command) : base(_command)
        {
        }

        //No params
        public override Error validateParameters()
        {
            var commandNoParameters = command.Split(' ');
            if (commandNoParameters.Length > 1)
            {
                error.isError = false;
                error.error = arr[1].ToString();
                return error;
            }
            error.isError = true;
            return error;
        }

        //Exit
        public override bool Execute()
        {
            return false;
        }
    }



    class CommandListContents : Command
    {

        public CommandListContents(string _command) : base(_command)
        {
        }

        //Two params -r or onlye one
        public override Error validateParameters()
        {
            var commandNoParameters = command.Split(' ');
            if (commandNoParameters.Length > 2)
            {
                error.isError = false;
                error.error = arr[1].ToString();
                return error;
            }
            else
            {
                if (commandNoParameters.Length == 2)
                {
                    if (!commandNoParameters[1].Equals("-r"))
                    {
                        error.isError = false;
                        error.error = arr[1].ToString();
                        return error;
                    }
                }
            }
            error.isError = true;
            return error;
        }

        //Recursive files and folders
        private void recursiveFilesFolders(string[] files, string[] folders)
        {
            foreach (string file in files)
            {
                System.Console.WriteLine(file);
            }

            foreach (string folder in folders)
            {
                System.Console.WriteLine(folder);
                var newfiles = Directory.GetFiles(folder);
                var newfolders = Directory.GetDirectories(folder);
                recursiveFilesFolders(newfiles, newfolders);
            }
        }

        //Get files and folders
        public override bool Execute()
        {

            var files = Directory.GetFiles(Directory.GetCurrentDirectory());
            var folders = Directory.GetDirectories(Directory.GetCurrentDirectory());

            var commandNoParameters = command.Split(' ');
            if (commandNoParameters.Length > 1)
            {

                //-r
                recursiveFilesFolders(files, folders);

            }
            else
            {
                //no param

                foreach (string file in files)
                {
                    System.Console.WriteLine(file);
                }

                foreach (string folder in folders)
                {
                    System.Console.WriteLine(folder);
                }
            }
            return true;
        }
    }


    class CommandCurrentDir : Command
    {

        public CommandCurrentDir(string _command) : base(_command)
        {
        }

        //Only one param
        public override Error validateParameters()
        {
            var commandNoParameters = command.Split(' ');
            if (commandNoParameters.Length > 1)
            {
                error.isError = false;
                error.error = arr[1].ToString();
                return error;
            }
            error.isError = true;
            return error;
        }

        //Get current dir
        public override bool Execute()
        {
            System.Console.WriteLine(Directory.GetCurrentDirectory());
            return true;
        }
    }




    class CommandMakeDir : Command
    {
        public CommandMakeDir(string _command) : base(_command)
        {
        }

        //Two params and second one < 100
        public override Error validateParameters()
        {

            var commandNoParameters = command.Split(' ');
            if (commandNoParameters.Length != 2 )
            {
                error.isError = false;
                error.error = arr[1].ToString();
                return error;
            }
            else
            {
                if (commandNoParameters[1].ToString().Length > 100)
                {
                    error.isError = false;
                    error.error = arr[1].ToString();
                    return error;
                }
            }
            error.isError = true;
            return error;
        }

        //Create directory
        public override bool Execute()
        {
            var commandNoParameters = command.Split(' ');
            string fileToCreate = Directory.GetCurrentDirectory() + "\\" + commandNoParameters[1].ToString();
            if (Directory.Exists(fileToCreate))
            {
                System.Console.WriteLine(arr[2].ToString());
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(fileToCreate);
            }
            return true;

        }
    }

    class CommandChangeDir : Command
    {
        public CommandChangeDir(string _command) : base(_command)
        {
        }

        //Two params
        public override Error validateParameters()
        {
            var commandNoParameters = command.Split(' ');
            if (commandNoParameters.Length != 2)
            {
                error.isError = false;
                error.error = arr[1].ToString();
                return error;
            }
            error.isError = true;
            return error;
        }

        //Go to directory in param or to parent
        public override bool Execute()
        {
            var commandNoParameters = command.Split(' ');
            string fileToCreate = Directory.GetCurrentDirectory() + "\\" + commandNoParameters[1].ToString();
            if (!Directory.Exists(fileToCreate))
            {
                System.Console.WriteLine("Directory not found");
            }
            else
            {
                //No time
                if(commandNoParameters[1].ToString().Equals("..")){
                    Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).ToString());
                }else{
                    Directory.SetCurrentDirectory(fileToCreate);
                }
                System.Console.WriteLine(Directory.GetCurrentDirectory());
            }

            return true;
        }
    }


    class CommandCreateDir : Command
    {

        public CommandCreateDir(string _command) : base(_command)
        {
        }

        //Two params and second max 100
        public override Error validateParameters()
        {
            var commandNoParameters = command.Split(' ');
            if (commandNoParameters.Length != 2)
            {
                error.isError = false;
                error.error = arr[1].ToString();
                return error;
            }
            else
            {
                if (commandNoParameters.Length == 2)
                {
                    if (commandNoParameters[1].ToString().Length > 100)
                    {
                        error.isError = false;
                        error.error = arr[1].ToString();
                        return error;
                    }
                }
            }
            error.isError = true;
            return error;
        }

        //Create file
        public override bool Execute()
        {
            var commandNoParameters = command.Split(' ');
            using (FileStream fs = File.Create(commandNoParameters[1]))
            {
            }
            return true;
        }
    }



    class Solution
    {

        private static string getKeyboardCommand()
        {
            return System.Console.ReadLine();
        }



        static void Main(string[] args)
        {

            string[] arr = new string[1] {
            "Unrecognized command"

        };

            bool procesing = true;
            while (procesing)
            {

                try
                {
                    //Read the entry command
                    string command = getKeyboardCommand();

                    //Validate command
                    Command cmd = new Command(command);
                    var commandSpecific = cmd.Validate();
                    if (commandSpecific == null)
                    {
                        System.Console.WriteLine(arr[0].ToString());
                    }
                    else
                    {
                        //Validate parameters for a valid command
                        Error result = commandSpecific.validateParameters();
                        if (!result.isError)
                        {
                            //Error
                            System.Console.WriteLine(result.error);
                        }
                        else
                        {
                            //Execute command
                            procesing = commandSpecific.Execute();
                        }
                    }

                   
                }
                catch (Exception e)
                {
                    //Generic error
                    System.Console.WriteLine(e);
                }


            }



        }
    }
}
