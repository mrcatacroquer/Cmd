﻿using System.Collections;
using System.Diagnostics;

namespace Codice.CmdRunner
{
    public class CmdRunner
    {

        private static CodiceCmdRunner runner = new CodiceCmdRunner();

        public static void InitConsole(IConsoleWriter writer)
        {
            runner.InitConsole(writer);
        }

        private static bool bSetBotMode = false;
        public static void SetBotWorkingMode()
        {
            if (bSetBotMode)
                return;

            // not use file communication,
            // in linux crash the test
            runner.DontTimeout();
            runner.WorkWithoutFileCommunication();
            bSetBotMode = true;
        }

        public static void TerminateShell()
        {
            try
            {
                if (runner.mCmdProc != null)
                {
                    runner.mCmdProc.StandardInput.WriteLine("exit");
                }
            }
            catch (System.IO.IOException e)
            {
            }

            if (runner.mCmdProc != null)
            {
                runner.mCmdProc.WaitForExit();
                runner.mCmdProc.Close();
                runner.mCmdProc = null;
            }
        }

        public static Process Run(string cmd, string workingdir)
        {
            return runner.InternalRun(cmd, workingdir, true);
        }

        public static void ExecuteCommand(string command, string path)
        {
            string output, error;

            if (runner.InternalExecuteCommand(command, path, null, out output, out error, true) != 0)
            {
                throw new System.Exception(output);
            }
        }

        public static int ExecuteCommandWithInput(string command, string path, string input)
        {
            string output, error;
            return runner.InternalExecuteCommand(command, path, input, out output, out error, true);
        }

        public static int ExecuteCommandWithResult(string command, string path)
        {
            string output, error;
            return runner.InternalExecuteCommand(command, path, null, out output, out error, true);
        }

        public static int ExecuteCommandWithResult(string command, string path, out string output, out string error, bool bUseCmShell)
        {
            return runner.InternalExecuteCommand(command, path, null, out output, out error, bUseCmShell);
        }

        public static string ExecuteCommandWithStringResult(string command, string path)
        {
            return ExecuteCommandWithStringResult(command, path, true);
        }

        public static string ExecuteCommandWithStringResult(string command, string path, bool bUseShell)
        {
            string output, error;
            runner.InternalExecuteCommand(command, path, null, out output, out error, bUseShell);
            return output;
        }

        public static void SetEnvironmentVariables(Hashtable envVars)
        {
            runner.SetEnvironmentVariables(envVars);
        }

        public static void ExecuteCommandWithoutOutput(string command, string path)
        {
            if (runner.InternalExecuteCommand(command, path) != 0)
            {
                throw new System.Exception("Bad internal execution");
            }

        }

        public static int RunAndWait(string cmd, string workingdir, out string output, out string error)
        {
            return runner.RunAndWait(cmd, workingdir, out output, out error);
        }
    }
}